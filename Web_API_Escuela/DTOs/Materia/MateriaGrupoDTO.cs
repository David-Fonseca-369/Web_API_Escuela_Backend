using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Materia
{
    public class MateriaGrupoDTO
    {
        public int IdMateria { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public int IdDocente { get; set; }
        public string NombreDocente { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

    }
}
