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

        //GET: api/periodos/periodoActual 
        [HttpGet("periodoActual")]
        public async Task<ActionResult<PeriodoDTO>> PeriodoActual()
        {

            DateTime hoy = new DateTime(2021, 09, 09); //Ejemplo
            //DateTime hoy = DateTime.Now;

            var periodo = await context.Periodos.Where(x => hoy >= x.FechaInicio && hoy <= x.FechaFin).FirstOrDefaultAsync();

            if (periodo != null)
            {
                return mapper.Map<PeriodoDTO>(periodo);
            }

            return NoContent();
        }

        //POST: api/periodos/crear 
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] PeriodoCreacionDTO periodoCreacionDTO)
        {
            //Validar que la fecha del periodo no este dentro de otro periodo.
            var resultTuple = await ValidarFechas(periodoCreacionDTO.FechaInicio, periodoCreacionDTO.FechaFin);

            bool fechaEcontrada = resultTuple.Item1;
            string mensaje = resultTuple.Item2;

            if (fechaEcontrada)
            {
                return BadRequest(mensaje);
            }


            var periodo = mapper.Map<Periodo>(periodoCreacionDTO);
            context.Add(periodo);

            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<Tuple<bool, string>> ValidarFechas(DateTime fechaInicioTemp, DateTime fechaFinTemp)
        {
            var periodos = await context.Periodos.ToListAsync();

            if (periodos.Count > 0)
            {
                foreach (var item in periodos)
                {
                    bool periodoUsado = VerificarFechaItem(fechaInicioTemp, fechaFinTemp, item.FechaInicio, item.FechaFin);

                    if (periodoUsado)
                    {
                        return Tuple.Create<bool, string>(true, $"Las fechas indicadas ya se encuentran ocupadas en el periodo {item.Nombre} del {item.FechaInicio:dd/MM/yyyy} - {item.FechaFin:dd/MM/yyyy}.");
                    }
                }

                return Tuple.Create<bool, string>(false, null);

            }

            return Tuple.Create<bool, string>(false, null);
        }

        private bool VerificarFechaItem(DateTime fechaInicioTemp, DateTime fechaFinTemp, DateTime fechaInicio, DateTime fechaFin)
        {

            if (fechaInicioTemp >= fechaInicio && fechaInicioTemp <= fechaFin)
            {
                return true;
            }
            else if (fechaFinTemp >= fechaInicio && fechaFinTemp <= fechaFin)
            {
                return true;
            }

            return false;

        }



    }
}
