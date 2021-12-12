using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Usuario
{
    public class UsuarioCreacionDTO
    {

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(50)]
        public string ApellidoPaterno { get; set; }
        [Required]
        [MaxLength(50)]
        public string ApellidoMaterno { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Correo { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mínimo 8 caracteres.")]
        public string Password { get; set; }
        public bool Estado { get; set; }
    }
}
