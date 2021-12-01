using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class AsistenciaDetalle
    {

        public int IdDetalle { get; set; }
        [Required]
        public int IdCabecera { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Matricula { get; set; }
        [Required]
        public int Asistencia { get; set; }

        public AsistenciaCabecera AsistenciaCabecera { get; set; }



    }
}
