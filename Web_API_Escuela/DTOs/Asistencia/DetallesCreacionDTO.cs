using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Asistencia
{
    public class DetallesCreacionDTO
    {
        [Required]
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        [Required]
        public int Asistencia { get; set; }
    }
}
