using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Asistencia;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Route("api/asistencias")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AsistenciasController(ApplicationDbContext context)
        {
            this.context = context;
        }


        //POST : api/asistencias/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] AsistenciasCreacionDTO asistenciasCreacionDTO)
        {

            //1 = asistencia
            //2 = retardo
            //3 = falta
            //Validar que la asistencia no se encuentre registrada


            AsistenciaCabecera asistenciaCabecera = new()
            {
                IdMateria = asistenciasCreacionDTO.IdMateria,
                IdPeriodo = asistenciasCreacionDTO.IdPeriodo,
                Fecha = asistenciasCreacionDTO.Fecha,
            };

            context.Add(asistenciaCabecera);

            await context.SaveChangesAsync();

            var idCabecera = asistenciaCabecera.IdCabecera;

            foreach (var item in asistenciasCreacionDTO.Detalles)
            {
                AsistenciaDetalle detalle = new()
                {
                    IdCabecera = idCabecera,
                    Nombre = item.Nombre,
                    Matricula = item.Matricula,
                    Asistencia = item.Asistencia == 0 ? 3 : item.Asistencia
                };

                context.Add(detalle);
            }

            await context.SaveChangesAsync();

            return NoContent();

        }
    }
}
