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
        public int AsistenciasTotal { get; set; }
        public int RetardosTotal { get; set; }
        public int FaltasTotal { get; set; }
        public List<AsistenciaFechaDTO> Asistencias { get; set; }
    }

    
}
