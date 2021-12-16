using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Organigrama;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{

    [Route("api/organigramas")]
    [ApiController]
    public class OrganigramasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "imagen_organigrama";

        public OrganigramasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        //GET :  api/organigramas/organigrama
        [HttpGet("organigrama")]
        public async Task<ActionResult<OrganigramaDTO>> Organigrama()
        {
            //Por default solo podrá obtener una imagen cuyo id será el 1.
            var organigrama = await context.Organigramas.FirstOrDefaultAsync(x => x.Id == 1);

            if (organigrama == null)
            {
                return NoContent();
            }

            return mapper.Map<OrganigramaDTO>(organigrama);
        }

        //PUT : api/organigramas/editar
        [HttpPut("editar")]
        public async Task<ActionResult>Editar([FromForm] OrganigramaCreacionDTO organigramaCreacionDTO)
        {
            //Verificar si existe 
            var organigrama = await context.Organigramas.FirstOrDefaultAsync(x => x.Id == 1);

            if (organigrama == null) //Si no existe, se crea.
            {

                string rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, organigramaCreacionDTO.Imagen);

                Organigrama organigrama1 = new()
                {
                    Id = 1,
                    Ruta = rutaArchivo
                };

                context.Add(organigrama1);

                await context.SaveChangesAsync();

                return NoContent();
            }

            //Si existe, pasamos a modificar imagen

            string rutaArchivoActualizada = await almacenadorArchivos.EditarArchivo(contenedor, organigramaCreacionDTO.Imagen, organigrama.Ruta);

            organigrama.Ruta = rutaArchivoActualizada;

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
