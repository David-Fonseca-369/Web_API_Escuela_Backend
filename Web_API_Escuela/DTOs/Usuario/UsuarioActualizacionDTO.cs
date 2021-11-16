using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Usuario
{
    public class UsuarioActualizacionDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
    }
}
