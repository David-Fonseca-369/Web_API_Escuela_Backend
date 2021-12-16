using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Materia
    {
        public int IdMateria { get; set; }
        [Required (ErrorMessage = "El IdGrupo es requerido.")]
        public int IdGrupo { get; set; }        
        public int? IdDocente { get; set; }
        [Required (ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        
        public Grupo Grupo { get; set; }

        //Poner aquí el docente
        public Usuario Docente { get; set; }

    }
}
