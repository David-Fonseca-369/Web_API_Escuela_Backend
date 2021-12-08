using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Publicacion;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
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
                FechaEntrega = publicacionCreacionDTO.FechaEntrega,
                Descripcion = publicacionCreacionDTO.Descripcion
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

               string  rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, item.Archivo);

                Archivo archivo = new()
                {
                    IdPublicacion = idPublicacion,
                    RutaArchivo = rutaArchivo
                };

                context.Add(archivo);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
