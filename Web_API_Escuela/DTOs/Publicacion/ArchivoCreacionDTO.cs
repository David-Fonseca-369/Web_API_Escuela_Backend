using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Publicacion
{
    public class ArchivoCreacionDTO
    {
        //Recibo archivo
        [Required]
        public IFormFile Archivo { get; set; }
    }
}
