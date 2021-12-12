using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Validaciones;

namespace Web_API_Escuela.DTOs.Calificacion
{
    public class CalificacionDetalleCreacionDTO
    {
        [Required]
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        [Required]
        [RangoCalificacion]
        public decimal  Calificacion { get; set; }
    }
}
