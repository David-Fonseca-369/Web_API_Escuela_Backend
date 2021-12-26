using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Grupo;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Authorize]
    [Route("api/grupos")]
    [ApiController]

    public class GruposController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GruposController( ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //GET: api/grupos/todos
        [HttpGet("todos")]
        public async Task<ActionResult<List<GrupoDTO>>> Todos()
        {
            var grupos = await context.Grupos.ToListAsync();
            return mapper.Map<List<GrupoDTO>>(grupos);
        }

        //GET: api/grupos/TodosPaginacion
        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<GrupoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Grupos.AsQueryable();

            //Cuenta los registros y los expone en cabecera 
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            //Paginación
            var grupos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<GrupoDTO>>(grupos);

        }
        
        //GET: api/grupos/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GrupoDTO>>Get(int id)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            return mapper.Map<GrupoDTO>(grupo);
        }

        //POST: api/grupos/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] GrupoCreacionDTO grupoCreacionDTO)
        {
            var grupo = mapper.Map<Grupo>(grupoCreacionDTO);
            context.Add(grupo);

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/grupos/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] GrupoCreacionDTO grupoCreacionDTO)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            //Mapeo grupoCreacionDTO y despues almacenamos ese mapeo en el mismo grupo.
            grupo = mapper.Map(grupoCreacionDTO, grupo);
            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/grupos/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            grupo.Estado = false;
            
            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/grupos/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.IdGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            grupo.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
