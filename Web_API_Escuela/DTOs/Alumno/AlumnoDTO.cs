using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Alumno
{
    public class AlumnoDTO
    {
        public int IdAlumno { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Curp { get; set; }
        public string Matricula { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Direccion { get; set; }
        public string NombreTutor { get; set; }
        public string NumeroTutor { get; set; }
        public bool Estado { get; set; }
    }
}
