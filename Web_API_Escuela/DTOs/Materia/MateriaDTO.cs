using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Materia
{
    public class MateriaDTO
    {
        public int IdMateria { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public int IdDocente { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        
    }
}
