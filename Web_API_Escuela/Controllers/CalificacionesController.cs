using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Calificacion;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Route("api/calificaciones")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CalificacionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //POST : api/calificaciones/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] CalificacionesCreacionDTO calificacionesCreacionDTO)
        {
            //Validar si ya ha sido creada esa calificacion 
            var existe = await context.CalificacionCabeceras.Where(x => x.IdMateria == calificacionesCreacionDTO.IdMateria
            && x.IdPeriodo == calificacionesCreacionDTO.IdPeriodo && x.IdEvaluacion == calificacionesCreacionDTO.IdEvaluacion).FirstOrDefaultAsync();

            if (existe != null)
            {
                return BadRequest("Ya has guardado calificaciones para esta materia anteriormente.");
            }

            CalificacionCabecera calificacionCabecera = new()
            {
                IdMateria = calificacionesCreacionDTO.IdMateria,
                IdPeriodo = calificacionesCreacionDTO.IdPeriodo,
                IdEvaluacion = calificacionesCreacionDTO.IdEvaluacion
            };

            context.Add(calificacionCabecera);

            await context.SaveChangesAsync();

            var idCabecera = calificacionCabecera.IdCabecera;

            foreach (var item in calificacionesCreacionDTO.Detalles)
            {
                CalificacionDetalle calificacionDetalle = new()
                {
                    IdCabecera = idCabecera,
                    IdAlumno = item.IdAlumno,
                    Calificacion = item.Calificacion
                };

                context.Add(calificacionDetalle);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


    }
}
