using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Controllers
{
    [Route("api/evaluaciones")]
    [ApiController]
    public class EvaluacionesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EvaluacionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET : api/evaluaciones
        [HttpGet]
        public async Task<ActionResult<List<Evaluacion>>> Get()
        {
            return await context.Evaluaciones.Where(x => x.Estado == true).ToListAsync();
        }

    }
}
