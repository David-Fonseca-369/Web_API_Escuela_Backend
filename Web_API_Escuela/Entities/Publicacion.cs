using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Publicacion
    {
        public int IdPublicacion { get; set; }
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        [Required]
        public bool Estado { get; set; }


        //Relaciones
        public Materia Materia { get; set; }
        public Periodo Periodo { get; set; }
    }
}
