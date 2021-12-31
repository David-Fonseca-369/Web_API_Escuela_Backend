using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Encuesta
{
    public class IndiceEncuestaDTO
    {
        public int IdEncuesta { get; set; }
        public string NombreAlumno { get; set; }
        public string Matricula{ get; set; }
        public string RutaArchivo { get; set; }
    }
}
