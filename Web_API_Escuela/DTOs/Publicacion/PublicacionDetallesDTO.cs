using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Publicacion
{
    public class PublicacionDetallesDTO
    {
        public string Nombre { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Descripcion { get; set; }

        public List<ArchivoDTO> Archivos { get; set; }

    }
}
