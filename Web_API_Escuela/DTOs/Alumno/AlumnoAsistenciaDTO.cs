using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.DTOs.Alumno
{
    public class AlumnoAsistenciaDTO
    {
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        public int Asistencia { get; set; }
        
    }
}
