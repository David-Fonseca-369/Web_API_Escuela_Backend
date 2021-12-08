using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Alumno
{
    public class AlumnoCalificacionDTO
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        public decimal Calificacion { get; set; }
    }
}
