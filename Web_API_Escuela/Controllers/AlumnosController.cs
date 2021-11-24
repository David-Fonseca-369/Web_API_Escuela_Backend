using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Alumno;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AlumnosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET: api/alumnos/todos
        [HttpGet("Todos")]
        public async Task<ActionResult<List<AlumnoDTO>>> Todos()
        {
            var alumnos = await context.Alumnos.Include(x => x.Grupo).ToListAsync();

            return alumnos.Select(x => new AlumnoDTO
            {
                IdAlumno = x.IdAlumno,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Curp = x.Curp,
                Matricula = x.Matricula,
                Correo = x.Correo,
                Telefono = x.Telefono,
                FechaNacimiento = x.FechaNacimiento,
                Genero = x.Genero,
                Direccion = x.Direccion,
                NombreTutor = x.NombreTutor,
                NumeroTutor = x.NumeroTutor,
                Estado = x.Estado
            }).ToList();
        }


        //GET: api/alumnos/TodosPaginacion
        [HttpGet("TodosPaginacion")]
        public async Task<ActionResult<List<AlumnoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Alumnos.Include(x => x.Grupo).AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var alumnos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return alumnos.Select(x => new AlumnoDTO
            {
                IdAlumno = x.IdAlumno,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                Nombre = x.Nombre,
                ApellidoPaterno = x.ApellidoPaterno,
                ApellidoMaterno = x.ApellidoMaterno,
                Curp = x.Curp,
                Matricula = x.Matricula,
                Correo = x.Correo,
                Telefono = x.Telefono,
                FechaNacimiento = x.FechaNacimiento,
                Genero = x.Genero,
                Direccion = x.Direccion,
                NombreTutor = x.NombreTutor,
                NumeroTutor = x.NumeroTutor,
                Estado = x.Estado
            }).ToList();
        }


        //GET: api/alumnos/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AlumnoDTO>> Get(int id)
        {
            var alumno = await context.Alumnos.Include(x => x.Grupo).FirstOrDefaultAsync(x => x.IdAlumno == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return new AlumnoDTO
            {
                IdAlumno = alumno.IdAlumno,
                IdGrupo = alumno.IdGrupo,
                NombreGrupo = alumno.Grupo.Nombre,
                Nombre = alumno.Nombre,
                ApellidoPaterno = alumno.ApellidoPaterno,
                ApellidoMaterno = alumno.ApellidoMaterno,
                Curp = alumno.Curp,
                Matricula = alumno.Matricula,
                Correo = alumno.Correo,
                Telefono = alumno.Telefono,
                FechaNacimiento = alumno.FechaNacimiento,
                Genero = alumno.Genero,
                Direccion = alumno.Direccion,
                NombreTutor = alumno.NombreTutor,
                NumeroTutor = alumno.NumeroTutor,
                Estado = alumno.Estado
            };
        }

        //crear 
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] AlumnoCreacionDTO alumnoCreacionDTO)
        {
            //Verificar si existe correo  curp
            var correo = alumnoCreacionDTO.Correo.ToLower();
            var curp = alumnoCreacionDTO.Curp.ToLower();

            if (await context.Alumnos.AnyAsync(x => x.Correo == correo))
            {
                return BadRequest("El correo ya existe.");
            }

            if (await context.Alumnos.AnyAsync(x => x.Curp == curp))
            {
                return BadRequest("La CURP ya existe.");
            }

            Helpers.Helpers.CrearPasswordHash(alumnoCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Alumno alumno = new()
            {
                IdGrupo = alumnoCreacionDTO.IdGrupo,
                Nombre = alumnoCreacionDTO.Nombre,
                ApellidoPaterno = alumnoCreacionDTO.ApellidoPaterno,
                ApellidoMaterno = alumnoCreacionDTO.ApellidoMaterno,
                Curp = alumnoCreacionDTO.Curp.ToUpper(),
                Matricula = alumnoCreacionDTO.Matricula,
                Correo = alumnoCreacionDTO.Correo.ToLower(),
                Telefono = alumnoCreacionDTO.Telefono,
                FechaNacimiento = alumnoCreacionDTO.FechaNacimiento,
                Genero = alumnoCreacionDTO.Genero,
                Direccion = alumnoCreacionDTO.Direccion,
                NombreTutor = alumnoCreacionDTO.NombreTutor,
                NumeroTutor = alumnoCreacionDTO.NumeroTutor,
                Password_hash = passwordHash,
                Password_salt = passwordSalt,
                Estado = true
            };

            context.Add(alumno);

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT : api/alumnos/Editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] AlumnoActualizacionDTO alumnoActualizacionDTO)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == id);

            if (alumno == null)
            {
                return NotFound();
            }

            var correo = alumnoActualizacionDTO.Correo.ToLower();
            var curp = alumnoActualizacionDTO.Curp.ToUpper();

            if (correo != alumno.Correo)//Verificar que el correo nuevo no se repita con otro registro
            {
                if (await context.Alumnos.AnyAsync(x => x.Correo == correo))
                {
                    return BadRequest("El correo ya existe.");
                }
            }

            if (curp != alumno.Curp)//Verificar que la curp nueva, no se repita con otro alumno
            {
                if (await context.Alumnos.AnyAsync(x => x.Curp == curp))
                {
                    return BadRequest("La CURP ya existe.");
                }
            }

            alumno.Nombre = alumnoActualizacionDTO.Nombre;
            alumno.ApellidoPaterno = alumnoActualizacionDTO.ApellidoPaterno;
            alumno.ApellidoMaterno = alumnoActualizacionDTO.ApellidoMaterno;
            alumno.Curp = alumnoActualizacionDTO.Curp.ToUpper();
            alumno.Matricula = alumnoActualizacionDTO.Matricula;
            alumno.Correo = alumnoActualizacionDTO.Correo.ToLower();
            alumno.Telefono = alumnoActualizacionDTO.Telefono;
            alumno.FechaNacimiento = alumnoActualizacionDTO.FechaNacimiento;
            alumno.Genero = alumnoActualizacionDTO.Genero;
            alumno.Direccion = alumnoActualizacionDTO.Direccion;
            alumno.NombreTutor = alumnoActualizacionDTO.NombreTutor;
            alumno.NumeroTutor = alumnoActualizacionDTO.NumeroTutor;

            //Coproba si se modificó la contraseña
            if (!string.IsNullOrEmpty(alumnoActualizacionDTO.Password))
            {
                Helpers.Helpers.CrearPasswordHash(alumnoActualizacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                alumno.Password_hash = passwordHash;
                alumno.Password_salt = passwordSalt;

            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/alumnos/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == id);
            if (alumno == null)
            {
                return NotFound();
            }

            alumno.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/alumnos/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.IdAlumno == id);
            if (alumno == null)
            {
                return NotFound();
            }

            alumno.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }


    }
}
