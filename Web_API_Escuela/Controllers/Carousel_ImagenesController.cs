using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Carousel_Imagen;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/carousel_imagenes")]
    [ApiController]
    public class Carousel_ImagenesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "imagenes_carousel";


        public Carousel_ImagenesController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }



        //GET: api/carousel_imagenes/todos
        [HttpGet("todos")]
        public async Task<ActionResult<List<Carousel_ImagenDTO>>> Todos()
        {
            var carousel_imagenes = await context.Carousel_Imagenes.ToListAsync();

            return mapper.Map<List<Carousel_ImagenDTO>>(carousel_imagenes);
        }

        //POST : api/carousel_imagenes
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] Carousel_ImagenCreacionDTO carousel)
        {
            string rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, carousel.Imagen);

            Carousel_Imagen carousel_Imagen = new()
            {
               Nombre = carousel.Imagen.FileName,
               Ruta = rutaArchivo
            };

            context.Add(carousel_Imagen);

            await context.SaveChangesAsync();

            return NoContent();
        }

        //Delete api/carousel_imagenes/eliminar
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult>Eliminar(int id)
        {
            var carousel_imagen = await context.Carousel_Imagenes.FirstOrDefaultAsync(x => x.Id == id);

            if (carousel_imagen == null)
            {
                return NotFound();
            }

            context.Remove(carousel_imagen);

            await context.SaveChangesAsync();

            await almacenadorArchivos.BorrarArchivo(carousel_imagen.Ruta, contenedor);

            return NoContent();

        }
    }
}
