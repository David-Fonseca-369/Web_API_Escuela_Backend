using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class AsistenciaCabecera
    {
        public int IdCabecera { get; set; }
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        public Periodo Periodo { get; set; }
        public Materia Materia { get; set; }

    }
}
