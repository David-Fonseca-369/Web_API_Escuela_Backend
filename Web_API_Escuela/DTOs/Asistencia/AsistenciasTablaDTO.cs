using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Asistencia
{
    public class AsistenciasTablaDTO
    {
        public List<DateTime> Fechas { get; set; }
        public List<AsistenciasDTO> Asistencias { get; set; }
        public int TotalAsistenciasFila { get; set; }

    }
}
