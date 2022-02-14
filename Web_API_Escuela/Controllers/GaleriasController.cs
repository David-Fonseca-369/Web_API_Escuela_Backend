using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Galeria;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/galerias")]
    [ApiController]
    public class GaleriasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "imagenes_galeria";

        public GaleriasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        //GET : api/galerias/todos 
        [HttpGet("todos")]
        public async Task<ActionResult<List<GaleriaDTO>>> Todos()
        {
            var fotos_galeria = await context.Galerias.OrderByDescending(x => x.Id).ToListAsync();

            return mapper.Map<List<GaleriaDTO>>(fotos_galeria);
        }

        //GET api/galeria/imagenesTodas
        [HttpGet("imagenesTodas")]
        public async Task<ActionResult<List<ImagenesGaleriaDTO>>> ImagenesTodas()
        {
            var imagenesGaleria = await context.Galerias.OrderByDescending( x=> x.Id).ToListAsync();

            return imagenesGaleria.Select(x => new ImagenesGaleriaDTO { Ruta = x.Ruta }).ToList();
        }

        [Authorize]
        //POST : api/galerias
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] GaleriaCreacionDTO galeriaCreacionDTO)
        {
            string rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, galeriaCreacionDTO.Imagen);

            Galeria galeria = new()
            {
                Nombre = galeriaCreacionDTO.Imagen.FileName,
                Ruta = rutaArchivo
            };

            context.Add(galeria);

            await context.SaveChangesAsync();

            return NoContent();
        }


        //Delete api/galerias/eliminar
        [Authorize]
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var galeria_imagen = await context.Galerias.FirstOrDefaultAsync(x => x.Id == id);

            if (galeria_imagen == null)
            {
                return NotFound();
            }

            context.Remove(galeria_imagen);

            await context.SaveChangesAsync();

            await almacenadorArchivos.BorrarArchivo(galeria_imagen.Ruta, contenedor);

            return NoContent();
        }
    }
}
