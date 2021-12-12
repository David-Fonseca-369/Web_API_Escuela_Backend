using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class Archivo
    {
        public int IdArchivo { get; set; }
        public int IdPublicacion { get; set; }
        public string RutaArchivo { get; set; }
        public string Nombre { get; set; }

        //Relacion
        public Publicacion Publicacion { get; set; }
    }
}
