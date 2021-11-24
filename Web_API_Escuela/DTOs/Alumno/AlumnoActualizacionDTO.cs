using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Alumno
{
    public class AlumnoActualizacionDTO
    {
        [Required]
        public int IdGrupo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        [MinLength(18, ErrorMessage = "Se requieren 18 caracteres.")]
        [MaxLength(20)]
        public string Curp { get; set; }
        [Required]
        [MaxLength(20)]
        public string Matricula { get; set; }
        [Required]
        [EmailAddress]
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
        [MaxLength(20)]
        public string NumeroTutor { get; set; }
        public string Password { get; set; }
    }
}
