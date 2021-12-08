using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class CalificacionCabecera
    {
        public int IdCabecera { get; set; }
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public int IdEvaluacion { get; set; }

        //Relaciones

        public Materia Materia { get; set; }
        public Periodo Periodo { get; set; }
        public Evaluacion Evaluacion { get; set; }


    }
}
