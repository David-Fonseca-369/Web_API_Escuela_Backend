using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Materia;
using Web_API_Escuela.Entities;

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

        //paginacion


        //id

        //POST: api/materias/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] MateriaCreacionDTO materiaCreacionDTO)
        {
            Materia materia = new Materia
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

            materia = new Materia
            {
                IdGrupo = materiaCreacionDTO.IdGrupo,
                IdDocente = materiaCreacionDTO.IdDocente,
                Nombre = materiaCreacionDTO.Nombre,
                Estado = materiaCreacionDTO.Estado
            };

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
    }
}
