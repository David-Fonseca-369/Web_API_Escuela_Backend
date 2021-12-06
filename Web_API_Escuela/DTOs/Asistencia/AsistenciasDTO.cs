using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Asistencia
{
    public class AsistenciasDTO
    {
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        public List<AsistenciaFechaDTO> Asistencias { get; set; }
    }

    
}
