using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Calificacion
{
    public class CalificacionesCreacionDTO
    {
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public int IdEvaluacion { get; set; }

        [Required]
        public List<CalificacionDetalleCreacionDTO> Detalles { get; set; }
    }
}
