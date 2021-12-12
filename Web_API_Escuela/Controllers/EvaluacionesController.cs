using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Route("api/evaluaciones")]
    [ApiController]
    public class EvaluacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EvaluacionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET : api/evaluaciones/{idMateria}/{idPeriodo}
        [HttpGet("{idMateria:int}/{idPeriodo:int}")]
        public async Task<ActionResult<Evaluacion>> Get([FromRoute] int idMateria, int idPeriodo)
        {
            //Verificar que exista cabecera con calificacion guardada, para saber si en que evaluacion se encuentra
            var cabeceraCalificaciones = await context.CalificacionCabeceras.FirstOrDefaultAsync(x => x.IdMateria == idMateria && x.IdPeriodo == idPeriodo);

            if (cabeceraCalificaciones == null) //no ha registrado calificacion
            {
                return await context.Evaluaciones.FirstOrDefaultAsync(x => x.IdEvaluacion == 1);
            }

            //Ya esta registrada la cabecera, buscar en que evaluacion se encuentra

            if (cabeceraCalificaciones.Evaluacion == 1)
            {
                return await context.Evaluaciones.FirstOrDefaultAsync(x => x.IdEvaluacion == 2);//retrno la opción de segunda evaluación
            }
            else if (cabeceraCalificaciones.Evaluacion == 2)
            {
                return await context.Evaluaciones.FirstOrDefaultAsync(x => x.IdEvaluacion == 3);//retrno la opción de tercera evaluación
            }
            else
            {
                return NoContent();
            }
        }

        
    }
}
