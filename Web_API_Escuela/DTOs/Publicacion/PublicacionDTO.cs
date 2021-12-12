using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Publicacion
{
    public class PublicacionDTO
    {
        public int IdPublicacion { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
