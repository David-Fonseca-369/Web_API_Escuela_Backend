using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Encuesta
{
    public class EncuestaCreacionDTO
    {
        [Required]
        public int IdAlumno { get; set; }
        [Required]
        public string EstadoCivil { get; set; }
        [Required]
        public string Nacionalidad { get; set; }
        public string Idiomas { get; set; }
        [Required]
        public string TipoSangre { get; set; }
        [Required]
        public string SeguroSocial { get; set; }
        [Required]
        public string Grado { get; set; }
        [Required]
        public string Grupo { get; set; }
        [Required]
        public string Semestre { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }


        [Required]
        public string NombreTutor { get; set; }
        [Required]
        public string Parentesco { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Ine { get; set; }
        [Required]
        public string Curp { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public string EstadoCivilTutor { get; set; }
        [Required]
        public string Ocupacion { get; set; }
        [Required]
        public string Estudios { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        public string Domicilio { get; set; }
        [Required]
        public IFormFile Archivo { get; set; }

    }
}
