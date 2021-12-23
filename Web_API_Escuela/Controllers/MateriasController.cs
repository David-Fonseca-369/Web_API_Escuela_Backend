using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Materia;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/materias")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public MateriasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //api/materias/todos
        [HttpGet("todos")]
        public async Task<ActionResult<List<MateriaDTO>>> Todos()
        {
            var materias = await context.Materias.Include(x => x.Grupo).ToListAsync();

            return materias.Select(x => new MateriaDTO
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0, //si es diferente de nulo que pnga el id, si no que ponga 0.
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();

        }

        //GET: api/materias/todosPaginacion
        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<MateriaDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Materias.Include(x => x.Grupo).AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var materias = await queryable.Paginar(paginacionDTO).ToListAsync();

            return materias.Select(x => new MateriaDTO
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0,
                Nombre = x.Nombre,
                Estado = x.Estado

            }).ToList();

        }

        //GET: api/materias/disponiblesPaginacion
        [HttpGet("disponiblesPaginacion")]
        public async Task<ActionResult<List<MateriaDTO>>> DisponiblesPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {

            var materiasDisponibles = await context.Materias.Include(x => x.Grupo).Where(x => x.IdDocente == null && x.Estado == true).ToListAsync();

            int cantidad = materiasDisponibles.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = materiasDisponibles.AsQueryable();

            var materiasDisponiblesPaginadas = queryable.Paginar(paginacionDTO).ToList();


            return materiasDisponiblesPaginadas.Select(x => new MateriaDTO
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0,
                Nombre = x.Nombre,
                Estado = x.Estado

            }).ToList();
        }

        //GET: api/materias/asignadasPaginacion/{idDocente}
        [HttpGet("asignadasPaginacion/{idDocente:int}")]
        public async Task<ActionResult<List<MateriaDTO>>> AsignadasPaginacion([FromQuery] PaginacionDTO paginacionDTO, int idDocente)
        {
            var materiasAsignadas = await context.Materias.Include(x => x.Grupo).Where(x => x.IdDocente == idDocente).ToListAsync();
            int cantidad = materiasAsignadas.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = materiasAsignadas.AsQueryable();

            var materiasAsignadasPaginadas = queryable.Paginar(paginacionDTO).ToList();

            return materiasAsignadasPaginadas.Select(x => new MateriaDTO
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0,
                Nombre = x.Nombre,
                Estado = x.Estado

            }).ToList();
        }

        //GET: api/materias/asignadasTodas/{idDocente}
        [HttpGet("asignadasTodas/{idDocente:int}")]
        public async Task<ActionResult<List<MateriaDTO>>> AsignadasTodas(int idDocente)
        {
            var materiasAsignadas = await context.Materias.Include(x => x.Grupo).Where(x => x.IdDocente == idDocente).ToListAsync();

            return materiasAsignadas.Select(x => new MateriaDTO
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0,
                Nombre = x.Nombre,
                Estado = x.Estado

            }).ToList();
        }

        //GET : api/materias/grupo
        [HttpGet("grupo/{idGrupo:int}")]
        public async Task<ActionResult<List<MateriaGrupoDTO>>> Grupo([FromRoute] int idGrupo)
        {
            var materias = await context.Materias.Include(x => x.Grupo).Include(x => x.Docente).Where(x => x.IdGrupo == idGrupo).ToListAsync();

            return materias.Select(x => new MateriaGrupoDTO()
            {
                IdMateria = x.IdMateria,
                IdGrupo = x.IdGrupo,
                NombreGrupo = x.Grupo.Nombre,
                IdDocente = x.IdDocente != null ? Convert.ToInt32(x.IdDocente) : 0,
                NombreDocente = x.IdDocente != null ? $"{x.Docente.Nombre} {x.Docente.ApellidoPaterno} {x.Docente.ApellidoMaterno}" : "No Asignada",
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();
        }

        //GET: api/materias/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MateriaDTO>> Get(int id)
        {
            var materia = await context.Materias.Include(x => x.Grupo).FirstOrDefaultAsync(x => x.IdMateria == id);

            if (materia == null)
            {
                return NotFound();
            }

            MateriaDTO materiaDTO = new MateriaDTO
            {
                IdMateria = materia.IdMateria,
                IdGrupo = materia.IdGrupo,
                NombreGrupo = materia.Grupo.Nombre,
                IdDocente = materia.IdDocente != null ? Convert.ToInt32(materia.IdDocente) : 0,
                Nombre = materia.Nombre,
                Estado = materia.Estado
            };

            return materiaDTO;
        }


        //POST: api/materias/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] MateriaCreacionDTO materiaCreacionDTO)
        {
            //Comprobamos si ya existe el nombre
            if (await MateriaExiste(materiaCreacionDTO.Nombre))
            {
                return BadRequest("Ya hay una materia registrada con el mismo nombre.");
            }

            Materia materia = new()
            {
                IdGrupo = materiaCreacionDTO.IdGrupo,
                Nombre = materiaCreacionDTO.Nombre,
                IdDocente = null, //al crear no lleva un docente, y si lo obtengo desde le modelo el 0 no es valido por no tener realcion y arorojara un error.
                Estado = materiaCreacionDTO.Estado
            };

            context.Add(materia);

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/materias/editar 
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] MateriaCreacionDTO materiaCreacionDTO)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.IdMateria == id);
            if (materia == null)
            {
                return NotFound();
            }

            if (materia.Nombre != materiaCreacionDTO.Nombre)
            {
                //Comprobamos si ya existe el nombre
                if (await MateriaExiste(materiaCreacionDTO.Nombre))
                {
                    return BadRequest("Ya hay una materia registrada con el mismo nombre.");
                }
            }

            materia.IdGrupo = materiaCreacionDTO.IdGrupo;
            materia.Nombre = materiaCreacionDTO.Nombre;
            materia.Estado = materiaCreacionDTO.Estado;

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/materias/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.IdMateria == id);

            if (materia == null)
            {
                return NotFound();
            }

            materia.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT: api/materias/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.IdMateria == id);

            if (materia == null)
            {
                return NotFound();
            }

            materia.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT: api/materias/asignar/{idDocente}/{idMateria}
        [HttpPut("asignar/{idDocente:int}/{idMateria:int}")]
        public async Task<ActionResult> Asignar(int idDocente, int idMateria)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.IdMateria == idMateria);

            if (materia == null)
            {
                return NotFound();
            }

            //Verificar que la materia no esté asignada
            if (materia.IdDocente > 0)
            {
                return BadRequest("La materia ya ha sido asignadaa otro docente.");
            }

            materia.IdDocente = idDocente;

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/materias/quitar/{idMateria}
        [HttpPut("quitar/{id:int}")]
        public async Task<ActionResult> Quitar(int id)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.IdMateria == id);

            if (materia == null)
            {
                return NotFound();
            }

            materia.IdDocente = null;

            await context.SaveChangesAsync();

            return NoContent();
        }



        //Comprobar materia
        private async Task<bool> MateriaExiste(string nombre)
        {
            return await context.Materias.AnyAsync(x => x.Nombre == nombre);
        }

    }
}
