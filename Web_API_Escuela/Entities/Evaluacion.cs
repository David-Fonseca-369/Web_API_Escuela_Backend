using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Evaluacion
    {
        public int IdEvaluacion { get; set; }
        [Required]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
