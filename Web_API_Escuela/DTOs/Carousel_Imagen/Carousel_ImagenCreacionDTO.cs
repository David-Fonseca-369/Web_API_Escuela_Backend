using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Carousel_Imagen
{
    public class Carousel_ImagenCreacionDTO
    {
        [Required]
        public IFormFile Imagen { get; set; }
    }
}
