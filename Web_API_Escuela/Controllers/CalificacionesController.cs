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
            //var existe = await context.CalificacionCabeceras.Where(x => x.IdMateria == calificacionesCreacionDTO.IdMateria
            //&& x.IdPeriodo == calificacionesCreacionDTO.IdPeriodo).FirstOrDefaultAsync();
            //
            //
            if (await Existe(calificacionesCreacionDTO.IdMateria, calificacionesCreacionDTO.IdPeriodo, calificacionesCreacionDTO.IdEvaluacion))
            {
                return BadRequest("Ya has registrado calificaciones en este parcial.");
            }

            if (calificacionesCreacionDTO.IdEvaluacion == 1) //primer parcial 
            {

                //Es nuevo registro
                CalificacionCabecera calificacionCabecera = new()
                {
                    IdMateria = calificacionesCreacionDTO.IdMateria,
                    IdPeriodo = calificacionesCreacionDTO.IdPeriodo,
                    Evaluacion = calificacionesCreacionDTO.IdEvaluacion
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
                        primerParcial = item.Calificacion,
                        segundoParcial = 0,
                        tercerParcial = 0
                    };

                    context.Add(calificacionDetalle);
                }

                await context.SaveChangesAsync();

                return NoContent();


            }
            else if (calificacionesCreacionDTO.IdEvaluacion == 2)//segundo parcial
            {
                //Consulto cabecera y detalle para editar
                var cabeceraCalificacion = await context.CalificacionCabeceras.FirstOrDefaultAsync(x => x.IdMateria == calificacionesCreacionDTO.IdMateria && x.IdPeriodo == calificacionesCreacionDTO.IdPeriodo && x.Evaluacion == 1);
                if (cabeceraCalificacion == null)
                {
                    return BadRequest("No existe registro de calificaciones de la primera evaluación.");
                }

                cabeceraCalificacion.Evaluacion = 2;

                await context.SaveChangesAsync();

                //Registro las calificaciones
                

                var listaCalificaciones = await context.CalificacionDetalles.Where(x => x.IdCabecera == cabeceraCalificacion.IdCabecera).ToListAsync();

                foreach (var item in calificacionesCreacionDTO.Detalles)
                {
                    var alumnoCalificaciones = await context.CalificacionDetalles.FirstOrDefaultAsync(x => x.IdAlumno == item.IdAlumno && x.IdCabecera == cabeceraCalificacion.IdCabecera); 

                    if (alumnoCalificaciones != null)//si existe
                    {
                        alumnoCalificaciones.segundoParcial = item.Calificacion; //modifico calificacion 2parcial
                    }
                    else //creo alumno
                    {
                        CalificacionDetalle calificacionDetalle = new()
                        {
                            IdCabecera = cabeceraCalificacion.IdCabecera,
                            IdAlumno = item.IdAlumno,
                            primerParcial = 0,
                            segundoParcial = item.Calificacion, //se registra a partir del paracial que llevan.
                            tercerParcial = 0
                        };

                        context.Add(calificacionDetalle);
                    }
                }

                await context.SaveChangesAsync();
            }
            else if (calificacionesCreacionDTO.IdEvaluacion == 3)//tercer parcial
            {
                //Consulto cabecera y detalle para editar
                var cabeceraCalificacion = await context.CalificacionCabeceras.FirstOrDefaultAsync(x => x.IdMateria == calificacionesCreacionDTO.IdMateria && x.IdPeriodo == calificacionesCreacionDTO.IdPeriodo && x.Evaluacion == 2);
                if (cabeceraCalificacion == null)
                {
                    return BadRequest("No existe registro de calificaciones de la segunda evaluación.");
                }

                cabeceraCalificacion.Evaluacion = 3;

                await context.SaveChangesAsync();

                //Registro las calificaciones

                var listaCalificaciones = await context.CalificacionDetalles.Where(x => x.IdCabecera == cabeceraCalificacion.IdCabecera).ToListAsync();

                foreach (var item in calificacionesCreacionDTO.Detalles)
                {
                    var alumnoCalificaciones = await context.CalificacionDetalles.FirstOrDefaultAsync(x => x.IdAlumno == item.IdAlumno && x.IdCabecera == cabeceraCalificacion.IdCabecera);

                    if (alumnoCalificaciones != null)//si existe
                    {
                        alumnoCalificaciones.tercerParcial = item.Calificacion; //modifico calificacion 2parcial
                    }
                    else //creo alumno
                    {
                        CalificacionDetalle calificacionDetalle = new()
                        {
                            IdCabecera = cabeceraCalificacion.IdCabecera,
                            IdAlumno = item.IdAlumno,
                            primerParcial = 0,
                            segundoParcial = 0,
                            tercerParcial = item.Calificacion, //se registra a partir del paracial que llevan.
                        };

                        context.Add(calificacionDetalle);
                    }
                }

                await context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }





            return NoContent();
        }

        private async Task<bool> Existe(int idMatera, int idPeriodo, int idEvaluacion)
        {
            return await context.CalificacionCabeceras.AnyAsync(x =>
            x.IdMateria == idMatera
            && x.IdPeriodo == idPeriodo
            && x.Evaluacion == idEvaluacion);
        }


        //GET : api/calificaciones/Todas
        [HttpGet("calificaciones/{idMateria:int}/{idPeriodo:int}")]
        public async Task<ActionResult<List<CalificacionDTO>>> Calificaciones([FromRoute] int idMateria, int idPeriodo)
        {
            //Evaluaciones
            //primer parcial = 1
            //segundo parcial = 2
            //tercer parcial = 3

            //checar si existe la cabecera     
            var cabeceraCalificaciones = await context.CalificacionCabeceras.FirstOrDefaultAsync(x => x.IdMateria == idMateria && x.IdPeriodo == idPeriodo);

            if (cabeceraCalificaciones != null) //si existe, consulto los promedios de esta
            {
                //Traigo sus calificaciones
                var listaCalificaciones = await context.CalificacionDetalles
                    .Include(x => x.Alumno)
                    .Where(x => x.IdCabecera == cabeceraCalificaciones.IdCabecera)
                    .OrderBy(x => x.Alumno.ApellidoPaterno)
                    .ThenBy(x => x.Alumno.ApellidoMaterno)
                    .ThenBy(x => x.Alumno.Nombre)
                    .ToListAsync();

                return listaCalificaciones.Select(x => new CalificacionDTO()
                {
                    Nombre = $"{x.Alumno.ApellidoPaterno} {x.Alumno.ApellidoMaterno} {x.Alumno.Nombre}",
                    Matricula = x.Alumno.Matricula,
                    PrimerParcial = x.primerParcial,
                    SegundoParcial = x.segundoParcial,
                    TercerParcial = x.tercerParcial
                }).ToList();

            }

            return NoContent();

        }


        //GET: api/calificaciones/evaluacion/{idMateria}/{idPeriodo}/{idEvaluacion}
        [HttpGet("evaluacion/{idMateria}/{idPeriodo}/{idEvaluacion}")]
        public async Task<ActionResult<List<CalificacionesMateriaDTO>>> Evaluacion([FromRoute] int idMateria, int idPeriodo, int idEvaluacion)
        {

            //checar si existe la cabecera     
            var cabeceraCalificaciones = await context.CalificacionCabeceras.FirstOrDefaultAsync(x => x.IdMateria == idMateria && x.IdPeriodo == idPeriodo);

            if (cabeceraCalificaciones != null)
            {
                //Traigo sus calificaciones
                var listaCalificaciones = await context.CalificacionDetalles
                    .Include(x => x.Alumno)
                    .Where(x => x.IdCabecera == cabeceraCalificaciones.IdCabecera)
                    .OrderBy(x => x.Alumno.ApellidoPaterno)
                    .ThenBy(x => x.Alumno.ApellidoMaterno)
                    .ThenBy(x => x.Alumno.Nombre)
                    .ToListAsync();


                if (idEvaluacion == 1) //Primera evaluacion
                {
                    int reprobados = 0;
                    int bajos = 0;
                    int medioBajo = 0;
                    int medio = 0;
                    int medioAlto = 0;
                    int alto = 0;

                    foreach (var item in listaCalificaciones)
                    {
                        switch ((float)item.primerParcial)
                        {
                            case float n when (n <= 5.9):
                                reprobados++;
                                break;

                            case float n when (n > 5.9 && n <= 6.9):
                                bajos++;
                                break;

                            case float n when (n > 6.9 && n <= 7.9):
                                medioBajo++;
                                break;
                            
                            case float n when (n > 7.9 && n <= 8.9):
                                medio++;
                                break;
                            
                            case float n when (n > 8.9 && n <= 9.9):
                                medioAlto++;
                                break;
                            
                            case 10:
                                alto++;
                                break;

                            default:
                                break;
                        }

                    }

                    //LLenar lista
                    List<CalificacionesMateriaDTO> calificacionesMateriaDTO = new ();

                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "0 - 5.9", Value = reprobados });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "6 - 6.9", Value = bajos });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "7 - 7.9", Value = medioBajo });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "8 - 8.9", Value = medio });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "9 - 9.9", Value = medioAlto });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "10", Value = alto });

                    return calificacionesMateriaDTO;
                }
                else if (idEvaluacion == 2) //segunda evaluacion
                {
                    int reprobados = 0;
                    int bajos = 0;
                    int medioBajo = 0;
                    int medio = 0;
                    int medioAlto = 0;
                    int alto = 0;

                    foreach (var item in listaCalificaciones)
                    {
                        switch ((float)item.segundoParcial)
                        {
                            case float n when (n <= 5.9):
                                reprobados++;
                                break;

                            case float n when (n > 5.9 && n <= 6.9):
                                bajos++;
                                break;

                            case float n when (n > 6.9 && n <= 7.9):
                                medioBajo++;
                                break;

                            case float n when (n > 7.9 && n <= 8.9):
                                medio++;
                                break;

                            case float n when (n > 8.9 && n <= 9.9):
                                medioAlto++;
                                break;

                            case 10:
                                alto++;
                                break;

                            default:
                                break;
                        }

                    }

                    //LLenar lista
                    List<CalificacionesMateriaDTO> calificacionesMateriaDTO = new();

                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "0 - 5.9", Value = reprobados });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "6 - 6.9", Value = bajos });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "7 - 7.9", Value = medioBajo });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "8 - 8.9", Value = medio });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "9 - 9.9", Value = medioAlto });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "10", Value = alto });

                    return calificacionesMateriaDTO;

                }
                else if (idEvaluacion == 3) //tercera evaluacion
                {
                    int reprobados = 0;
                    int bajos = 0;
                    int medioBajo = 0;
                    int medio = 0;
                    int medioAlto = 0;
                    int alto = 0;

                    foreach (var item in listaCalificaciones)
                    {
                        switch ((float)item.tercerParcial)
                        {
                            case float n when (n <= 5.9):
                                reprobados++;
                                break;

                            case float n when (n > 5.9 && n <= 6.9):
                                bajos++;
                                break;

                            case float n when (n > 6.9 && n <= 7.9):
                                medioBajo++;
                                break;

                            case float n when (n > 7.9 && n <= 8.9):
                                medio++;
                                break;

                            case float n when (n > 8.9 && n <= 9.9):
                                medioAlto++;
                                break;

                            case 10:
                                alto++;
                                break;

                            default:
                                break;
                        }

                    }

                    //LLenar lista
                    List<CalificacionesMateriaDTO> calificacionesMateriaDTO = new();

                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "0 - 5.9", Value = reprobados });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "6 - 6.9", Value = bajos });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "7 - 7.9", Value = medioBajo });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "8 - 8.9", Value = medio });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "9 - 9.9", Value = medioAlto });
                    calificacionesMateriaDTO.Add(new CalificacionesMateriaDTO { Name = "10", Value = alto });

                    return calificacionesMateriaDTO;

                }

                return NotFound();
            }

            return NoContent();

        }

      

    }
}
