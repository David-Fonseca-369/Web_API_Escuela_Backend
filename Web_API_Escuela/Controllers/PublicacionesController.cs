using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Publicacion;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Authorize]
    [Route("api/publicaciones")]
    [ApiController]
    public class PublicacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "archivosPublicaciones";

        public PublicacionesController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        //POST : api/publicaciones/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromForm] PublicacionCreacionDTO publicacionCreacionDTO)
        {
            Publicacion publicacion = new()
            {
                IdMateria = publicacionCreacionDTO.IdMateria,
                IdPeriodo = publicacionCreacionDTO.IdPeriodo,
                Nombre = publicacionCreacionDTO.Nombre,
                FechaEntrega = publicacionCreacionDTO.FechaEntrega != null ? Convert.ToDateTime(publicacionCreacionDTO.FechaEntrega) : null,
                Descripcion = publicacionCreacionDTO.Descripcion,
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            context.Add(publicacion);

            await context.SaveChangesAsync();

            //Verifico que vengan archivos
            if (publicacionCreacionDTO.Archivos == null)
            {
                return NoContent();
            }

            var idPublicacion = publicacion.IdPublicacion;

            //Guardar archivos recibidos
            foreach (var item in publicacionCreacionDTO.Archivos)
            {

                string rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, item);


                Archivo archivo = new()
                {
                    IdPublicacion = idPublicacion,
                    RutaArchivo = rutaArchivo,
                    Nombre = item.FileName
                };

                context.Add(archivo);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        //GET : api/publicaciones/TodosPaginacion
        [HttpGet("todosPaginacion/{idMateria:int}/{idPeriodo:int}")]
        public async Task<ActionResult<List<PublicacionDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO, [FromRoute] int idMateria, int idPeriodo)
        {
            var publicaciones = await context.Publicaciones.Where(x => x.IdMateria == idMateria && x.IdPeriodo == idPeriodo && x.Estado == true).ToListAsync();

            int cantidad = publicaciones.Count;

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = publicaciones.AsQueryable();

            var publicacionesPaginado = queryable.Paginar(paginacionDTO).ToList();

            return publicacionesPaginado.Select(x => new PublicacionDTO()
            {
                IdPublicacion = x.IdPublicacion,
                Nombre = x.Nombre,
                FechaCreacion = x.FechaCreacion
            }).ToList();

        }


        //GET : api/publicaciones/{idPublicacion}
        [HttpGet("{idPublicacion:int}")]
        public async Task<ActionResult<PublicacionDetallesDTO>> Get([FromRoute] int idPublicacion)
        {
            var publicacion = await context.Publicaciones.FirstOrDefaultAsync(x => x.IdPublicacion == idPublicacion);

            if (publicacion == null)
            {
                return NotFound();
            }

            List<ArchivoDTO> archivosList = new();

            //consulto los archivos
            var archivos = await context.Archivos.Where(x => x.IdPublicacion == idPublicacion).ToListAsync();

            if (archivos != null)
            {
                archivosList = archivos.Select(x => new ArchivoDTO()
                {
                    Nombre = x.Nombre,
                    RutaArchivo = x.RutaArchivo
                }).ToList();
            }

            PublicacionDetallesDTO publicacionDetallesDTO = new()
            {
                Nombre = publicacion.Nombre,
                FechaEntrega = publicacion.FechaEntrega,
                Descripcion = publicacion.Descripcion,
                Archivos = archivosList
            };

            return publicacionDetallesDTO;
        }

        //GET : api/publicaciones/materia/{idMateria}/{idPeriodo}
        [HttpGet("materia/{idMateria:int}/{idPeriodo:int}")]
        public async Task<ActionResult<List<PublicacionDetallesDTO>>> Materia([FromRoute] int idMateria, int idPeriodo)
        {
            var publicaciones = await context.Publicaciones.Where(x => x.IdMateria == idMateria && x.IdPeriodo == idPeriodo && x.Estado == true).ToListAsync();

            List<PublicacionDetallesDTO> publicacionDetalles = new();


            foreach (var publicacion in publicaciones)
            {
                List<ArchivoDTO> archivosList = new();


                var archivos = await context.Archivos.Where(x => x.IdPublicacion == publicacion.IdPublicacion).ToListAsync();

                foreach (var archivo in archivos)
                {
                    archivosList.Add(new ArchivoDTO { Nombre = archivo.Nombre, RutaArchivo = archivo.RutaArchivo });
                }

                publicacionDetalles.Add(new PublicacionDetallesDTO { Nombre = publicacion.Nombre, FechaEntrega = publicacion.FechaEntrega, Descripcion = publicacion.Descripcion, Archivos = archivosList });

            }

            return publicacionDetalles;

        }

        //DELETE : api/publicaciones/Eliminar
        [HttpDelete("eliminar/{idPublicacion:int}")]
        public async Task<ActionResult> Eliminar([FromRoute] int idPublicacion)
        {
            var publicacion = await context.Publicaciones.FirstOrDefaultAsync(x => x.IdPublicacion == idPublicacion);

            if (publicacion == null)
            {
                return NotFound();
            }



            publicacion.Estado = false;

            await context.SaveChangesAsync();

            //eliminar documentos relacionados y archivos
            var archivos = await context.Archivos.Where(x => x.IdPublicacion == publicacion.IdPublicacion).ToListAsync();

            if (archivos != null)
            {
                foreach (var archivo in archivos)
                {
                    await almacenadorArchivos.BorrarArchivo(archivo.RutaArchivo, contenedor);
                }

            }

            return NoContent();
        }
    }
}
