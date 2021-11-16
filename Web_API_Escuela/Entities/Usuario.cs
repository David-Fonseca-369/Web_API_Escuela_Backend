using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public byte[] Password_hash { get; set; }
        [Required]
        public byte[] Password_salt { get; set; }
        public bool Estado { get; set; }


    }
}
