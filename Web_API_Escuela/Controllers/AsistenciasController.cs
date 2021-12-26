using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Asistencia;
using Web_API_Escuela.DTOs.Materia;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Authorize]
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
            var fechaGuardada = await context.AsistenciaCabeceras.FirstOrDefaultAsync(x => x.Fecha == asistenciasCreacionDTO.Fecha && x.IdPeriodo == asistenciasCreacionDTO.IdPeriodo && x.IdMateria == asistenciasCreacionDTO.IdMateria);

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
                    IdAlumno = item.IdAlumno,
                    Asistencia = item.Asistencia == 0 ? 3 : item.Asistencia
                };

                context.Add(detalle);
            }

            await context.SaveChangesAsync();

            return NoContent();

        }


        [HttpGet("obtenerAsistencias/{idMateria:int}/{idPeriodo:int}/{idGrupo:int}/{desde}/{hasta}")]
        public async Task<ActionResult<AsistenciasTablaDTO>> ObtenerAsistencias([FromRoute] int idMateria,
            int idPeriodo,
            int idGrupo,
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
                    .Include(x => x.Alumno)
                    .Where(x => x.IdCabecera == item.IdCabecera && x.Alumno.IdGrupo == idGrupo)//Filtro por grupo, para checar que aun exista en este grupo.
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
                x.IdAlumno
            }).ToList();

            //Asistencia por alumno
            List<AsistenciasDTO> asistenciasList = new();

            foreach (var item in asistenciasALumno)
            {
                //asistencias Alumno
                List<AsistenciaFechaDTO> asistenciasAlumnoFechasList = new();

                //contador de asistencias
                int asistencias = 0;
                int retardos = 0;
                int faltas = 0;

                foreach (var asistenciaFecha in item)
                {
                    if (asistenciaFecha.Asistencia == 1)
                    {
                        asistencias++;
                    }

                    if (asistenciaFecha.Asistencia == 2)
                    {
                        retardos++;
                    }

                    if (asistenciaFecha.Asistencia == 3)
                    {
                        faltas++;
                    }


                    asistenciasAlumnoFechasList.Add(new AsistenciaFechaDTO()
                    {
                        Asistencia = asistenciaFecha.Asistencia,
                        Fecha = asistenciaFecha.AsistenciaCabecera.Fecha
                    });
                }

                //Consulto los datos del alumno
                var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == item.Key.IdAlumno);
                if (alumno == null)
                {
                    BadRequest($"Alumno con id = {item.Key.IdAlumno} no encontrado.");
                }

                asistenciasList.Add(new AsistenciasDTO()
                {
                    Nombre = $"{alumno.ApellidoPaterno} {alumno.ApellidoMaterno} {alumno.Nombre}",
                    Matricula = alumno.Matricula,
                    AsistenciasTotal = asistencias,
                    RetardosTotal = retardos,
                    FaltasTotal = faltas,
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
