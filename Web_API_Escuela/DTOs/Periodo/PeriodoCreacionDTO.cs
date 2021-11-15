using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Periodo
{
    public class PeriodoCreacionDTO
    {
        [Required(ErrorMessage = "Se requiere el Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Se requiere la fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage ="Se requiere la fecha de termino")]
        public DateTime FechaFin { get; set; }
    }
}
