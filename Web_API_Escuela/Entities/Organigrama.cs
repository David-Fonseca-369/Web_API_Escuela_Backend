using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Organigrama
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Ruta { get; set; }
    }
}
