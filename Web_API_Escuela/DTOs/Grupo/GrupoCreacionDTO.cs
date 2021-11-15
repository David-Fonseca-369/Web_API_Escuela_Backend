using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Grupo
{
    public class GrupoCreacionDTO
    {    
        [Required (ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
