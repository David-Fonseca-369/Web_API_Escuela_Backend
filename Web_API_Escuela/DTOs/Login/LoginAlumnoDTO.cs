using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Login
{
    public class LoginAlumnoDTO
    {
        [Required]
        [MinLength(18)]
        public string Curp { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
