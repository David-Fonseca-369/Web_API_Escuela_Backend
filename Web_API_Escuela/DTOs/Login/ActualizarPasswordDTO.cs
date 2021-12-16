using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Login
{
    public class ActualizarPasswordDTO
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
