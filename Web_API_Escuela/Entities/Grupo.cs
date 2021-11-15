using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Grupo
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int IdGrupo { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
