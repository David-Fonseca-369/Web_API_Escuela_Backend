using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs;
using Web_API_Escuela.DTOs.Periodo;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Helpers;

namespace Web_API_Escuela.Controllers
{
    [Route("api/periodos")]
    [ApiController]
    public class PeriodosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PeriodosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //GET: api/periodos/todos
        [HttpGet("todos")]
        public async Task<ActionResult<List<PeriodoDTO>>> Todos()
        {
            var periodos = await context.Periodos.ToListAsync();
            return mapper.Map<List<PeriodoDTO>>(periodos);
        }

        //GET: api/periodos/todosPaginacion
        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<PeriodoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Periodos.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var periodos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<PeriodoDTO>>(periodos);
        }


        //POST: api/periodos/crear 
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] PeriodoCreacionDTO periodoCreacionDTO)
        {
            var periodo = mapper.Map<Periodo>(periodoCreacionDTO);
            context.Add(periodo);

            await context.SaveChangesAsync();

            return NoContent();
        }

      


    }
}
