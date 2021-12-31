using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Encuesta;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Authorize]
    [Route("api/encuestas")]
    [ApiController]
    public class EncuestasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "archivosEncuestas";

        public EncuestasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        //POST : api/encuestas/crear
        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaEncuestaDTO>> Crear([FromForm] EncuestaCreacionDTO encuestaCreacionDTO)
        {
            //Validar si existe el alumno
            if (await context.Alumnos.AnyAsync(x => x.IdAlumno == encuestaCreacionDTO.IdAlumno))
            {
                string rutaArchivo = await almacenadorArchivos.GuardarArchivo(contenedor, encuestaCreacionDTO.Archivo);

                Encuesta encuesta = new()
                {
                    IdAlumno = encuestaCreacionDTO.IdAlumno,
                    EstadoCivil = encuestaCreacionDTO.EstadoCivil,
                    Nacionalidad = encuestaCreacionDTO.Nacionalidad.ToUpper(),
                    Idiomas = encuestaCreacionDTO.Idiomas != null ? encuestaCreacionDTO.Idiomas.ToUpper() : null, //porque puede venir nulo
                    TipoSangre = encuestaCreacionDTO.TipoSangre.ToUpper(),
                    SeguroSocial = encuestaCreacionDTO.SeguroSocial.ToUpper(),
                    Grado = encuestaCreacionDTO.Grado,
                    Grupo = encuestaCreacionDTO.Grupo,
                    Semestre = encuestaCreacionDTO.Semestre,
                    Facebook = encuestaCreacionDTO.Facebook != null ? encuestaCreacionDTO.Facebook.ToUpper() : null,
                    Twitter = encuestaCreacionDTO.Twitter != null ? encuestaCreacionDTO.Twitter.ToUpper() : null,

                    NombreTutor = encuestaCreacionDTO.NombreTutor.ToUpper(),
                    Parentesco = encuestaCreacionDTO.Parentesco,
                    FechaNacimiento = encuestaCreacionDTO.FechaNacimiento,
                    Ine = encuestaCreacionDTO.Ine.ToUpper(),
                    Curp = encuestaCreacionDTO.Curp.ToUpper(),
                    Genero = encuestaCreacionDTO.Genero,
                    EstadoCivilTutor = encuestaCreacionDTO.EstadoCivilTutor,
                    Ocupacion = encuestaCreacionDTO.Ocupacion.ToUpper(),
                    Estudios = encuestaCreacionDTO.Estudios,
                    Telefono = encuestaCreacionDTO.Telefono,
                    Celular = encuestaCreacionDTO.Celular,
                    Correo = encuestaCreacionDTO.Correo,
                    Domicilio = encuestaCreacionDTO.Domicilio.ToUpper(),
                    RutaArchivo = rutaArchivo
                };

                context.Add(encuesta);

                await context.SaveChangesAsync();

                return new RespuestaEncuestaDTO() { IdEncuesta = encuesta.IdEncuesta };

            }
            else
            {
                return BadRequest("El Alumno no existe en la base de datos.");
            }
        }

        //GET: api/encuestas/todasPaginacion
        [HttpGet("todasPaginacion")]
        public async Task<ActionResult<List<IndiceEncuestaDTO>>> TodasPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var encuestas = await context.Encuestas.Include(x => x.Alumno).OrderByDescending(x => x.IdEncuesta).ToListAsync();

            int cantidad = encuestas.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = encuestas.AsQueryable();

            var encuestasPaginado = queryable.Paginar(paginacionDTO).ToList();

            return encuestasPaginado.Select(x => new IndiceEncuestaDTO
            {
                IdEncuesta = x.IdEncuesta,
                NombreAlumno = $"{x.Alumno.Nombre} {x.Alumno.ApellidoPaterno} {x.Alumno.ApellidoMaterno}",
                Matricula = x.Alumno.Matricula,
                RutaArchivo = x.RutaArchivo
            }).ToList();
        }

        //GET: api/filtrarTodas
        [HttpGet("filtrarTodas")]
        public async Task<ActionResult<List<IndiceEncuestaDTO>>> FiltrarTodas([FromQuery] EncuestaFiltrarDTO encuestaFiltrarDTO)
        {
            var encuestasQueryable = context.Encuestas.Include(x => x.Alumno).AsQueryable();

            if (encuestaFiltrarDTO.IdEncuesta > 0)
            {
                encuestasQueryable = encuestasQueryable.Where(x => x.IdEncuesta == encuestaFiltrarDTO.IdEncuesta);
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(encuestasQueryable);
            var encuestas = await encuestasQueryable.Paginar(encuestaFiltrarDTO.PaginacionDTO).ToListAsync();

            return encuestas.Select(x => new IndiceEncuestaDTO
            {
                IdEncuesta = x.IdEncuesta,
                NombreAlumno = $"{x.Alumno.Nombre} {x.Alumno.ApellidoPaterno} {x.Alumno.ApellidoMaterno}",
                Matricula = x.Alumno.Matricula,
                RutaArchivo = x.RutaArchivo
            }).ToList();

        }

        //GET: api/encuestas/comprobante/{idEncuesta}
        [HttpGet("Comprobante/{idEncuesta:int}")]
        public async Task<ActionResult<EncuestaDTO>> Comprobante([FromRoute] int idEncuesta)
        {
            var encuestaRegistrada = await context.Encuestas.Include(x => x.Alumno).FirstOrDefaultAsync(x => x.IdEncuesta == idEncuesta);

            //Calcular edades 
            if (encuestaRegistrada == null)
            {
                return NotFound();
            }

            EncuestaDTO encuestaRespuesta = new()
            {
                IdEncuesta = encuestaRegistrada.IdEncuesta,
                NombreAlumno = $"{encuestaRegistrada.Alumno.Nombre.ToUpper()} {encuestaRegistrada.Alumno.ApellidoPaterno.ToUpper()} {encuestaRegistrada.Alumno.ApellidoMaterno.ToUpper()}",
                CurpAlumno = encuestaRegistrada.Alumno.Curp,
                GeneroAlumno = encuestaRegistrada.Alumno.Genero,
                FechaNacimientoAlumno = encuestaRegistrada.Alumno.FechaNacimiento,
                EdadAlumno = CalcularEdad(encuestaRegistrada.Alumno.FechaNacimiento),
                TelefonoAlumno = encuestaRegistrada.Alumno.Telefono,
                CorreoAlumno = encuestaRegistrada.Alumno.Correo,
                DomicilioAlumno = encuestaRegistrada.Alumno.Direccion,
                EstadoCivil = encuestaRegistrada.EstadoCivil,
                Nacionalidad = encuestaRegistrada.Nacionalidad,
                Idiomas = encuestaRegistrada.Idiomas,
                TipoSangre = encuestaRegistrada.TipoSangre,
                SeguroSocial = encuestaRegistrada.SeguroSocial,
                Grado = encuestaRegistrada.Grado,
                Grupo = encuestaRegistrada.Grupo,
                Semestre = encuestaRegistrada.Semestre,
                Facebook = encuestaRegistrada.Facebook,
                Twitter = encuestaRegistrada.Twitter,

                NombreTutor = encuestaRegistrada.NombreTutor,
                Parentesco = encuestaRegistrada.Parentesco,
                FechaNacimiento = encuestaRegistrada.FechaNacimiento,
                EdadTutor = CalcularEdad(encuestaRegistrada.FechaNacimiento),
                Ine = encuestaRegistrada.Ine,
                Curp = encuestaRegistrada.Curp,
                Genero = encuestaRegistrada.Genero,
                EstadoCivilTutor = encuestaRegistrada.EstadoCivilTutor,
                Ocupacion = encuestaRegistrada.Ocupacion,
                Estudios = encuestaRegistrada.Estudios,
                Telefono = encuestaRegistrada.Telefono,
                Celular = encuestaRegistrada.Celular,
                Correo = encuestaRegistrada.Correo,
                Domicilio = encuestaRegistrada.Domicilio,
            };

            return encuestaRespuesta;
        }

        //DELETE : api/encuestas/eliminar/{id}
        [HttpDelete("eliminar/{idEncuesta:int}")]
        public async Task<ActionResult>Eliminar(int idEncuesta)
        {
            var encuesta = await context.Encuestas.FirstOrDefaultAsync(x => x.IdEncuesta == idEncuesta);

            if (encuesta == null)
            {
                return NotFound();
            }

            context.Remove(encuesta);

            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(encuesta.RutaArchivo, contenedor);

            return NoContent();
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (DateTime.Today < fechaNacimiento.AddYears(edad))
            {
                return edad--;
            }

            return edad;
        }
    }
}
