using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Materia
{
    public class MateriaCreacionDTO
    {
        
        [Required(ErrorMessage = "El IdGrupo es requerido.")]
        public int IdGrupo { get; set; }
        public int IdDocente { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
