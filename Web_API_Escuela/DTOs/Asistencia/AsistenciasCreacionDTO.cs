using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Asistencia
{
    public class AsistenciasCreacionDTO
    {
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        //Asistencia Detalles
        [Required]
        public List<DetallesCreacionDTO> Detalles { get; set; }

    }
}
