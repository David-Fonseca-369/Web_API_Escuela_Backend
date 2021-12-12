using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Alumno
{
    public class AlumnoCreacionDTO
    {
        [Required]
        public int IdGrupo { get; set; }
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
        [MinLength(18, ErrorMessage = "Se requieren 18 caracteres.")]
        [MaxLength(20)]
        public string Curp { get; set; }
        [Required]
        [MaxLength(50)]
        public string Matricula { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Correo { get; set; }
        [Phone]
        [MaxLength(20)]
        public string Telefono { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string NombreTutor { get; set; }
        [MaxLength(60)]
        public string NumeroTutor { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mínimo 8 caracteres.")]
        public string Password { get; set; }
    }
}
