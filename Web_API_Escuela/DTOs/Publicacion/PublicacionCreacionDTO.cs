using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Publicacion
{
    public class PublicacionCreacionDTO
    {
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Descripcion { get; set; }

        //Detalles archivos
        public List<ArchivoCreacionDTO> Archivos { get; set; }

    }
}
