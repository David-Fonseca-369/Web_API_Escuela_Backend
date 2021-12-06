using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var fechaGuardada = await context.AsistenciaCabeceras.FirstOrDefaultAsync(x => x.Fecha == asistenciasCreacionDTO.Fecha);

            if (fechaGuardada != null)
            {
                return BadRequest("Esta fecha ya ha sido registrada.");
            }


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


        [HttpGet("obtenerAsistencias/{idMateria:int}/{idPeriodo:int}/{desde}/{hasta}")]
        public async Task<ActionResult<AsistenciasTablaDTO>> ObtenerAsistencias([FromRoute] int idMateria,
            int idPeriodo, 
            DateTime desde,
            DateTime hasta)
        {
            //Traigo consulta general
            var asistenciasCabecera = await context.AsistenciaCabeceras.Where(x => x.Fecha >= desde && x.Fecha <= hasta
            && x.IdMateria == idMateria
            && x.IdPeriodo == idPeriodo).ToListAsync();

            int totalAsistencias = asistenciasCabecera.Count;

            if (asistenciasCabecera == null)
            {
                return NoContent();
            }

            //Lista de todos los detalles de la consulta
            List<AsistenciaDetalle> asistenciasDetallesConsulta = new List<AsistenciaDetalle>();

            //Lista de fechas
            List<DateTime> fechas = new List<DateTime>();



            //Traigo las constulta de los detalles general
            foreach (var item in asistenciasCabecera)
            {
                //Lista de alumnos por fecha
                var asistenciaDetalle = await context.AsistenciaDetalles
                    .Include(x => x.AsistenciaCabecera)
                    .Where(x => x.IdCabecera == item.IdCabecera)
                    .ToListAsync();

                if (asistenciaDetalle != null)
                {
                    asistenciasDetallesConsulta.AddRange(asistenciaDetalle);
                }

                //Agrego fechas de asistencia
                fechas.Add(item.Fecha);
            }

            //Agrupo la asitencia por alumno
            var asistenciasALumno = asistenciasDetallesConsulta.GroupBy(x => new
            {
                x.Nombre,
                x.Matricula,
            }).ToList();

            //Asistencia por alumno
            List<AsistenciasDTO> asistenciasList = new List<AsistenciasDTO>();

            foreach (var item in asistenciasALumno)
            {
                //asistencias Alumno
                List<AsistenciaFechaDTO> asistenciasAlumnoFechasList = new List<AsistenciaFechaDTO>();

                foreach (var asistenciaFecha in item)
                {


                    asistenciasAlumnoFechasList.Add(new AsistenciaFechaDTO()
                    {
                        Asistencia = asistenciaFecha.Asistencia,
                        Fecha = asistenciaFecha.AsistenciaCabecera.Fecha
                    });


                }

                asistenciasList.Add(new AsistenciasDTO()
                {
                    Nombre = item.Key.Nombre,
                    Matricula = item.Key.Matricula,
                    Asistencias = asistenciasAlumnoFechasList,
                });

            }

            var asistenciasTabla = new AsistenciasTablaDTO()
            {
                Fechas = fechas.OrderBy(x => x.TimeOfDay).ToList(), //ordeno lista por fecha
                Asistencias = asistenciasList,
                TotalAsistenciasFila = totalAsistencias
            };

            
            return asistenciasTabla;
        }
    }
}
