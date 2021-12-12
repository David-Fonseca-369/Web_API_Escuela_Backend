using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.DTOs.Carousel_Imagen;
using Web_API_Escuela.DTOs.Grupo;
using Web_API_Escuela.DTOs.Periodo;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //mapeo doble vía con reverseMap //get
            CreateMap<Grupo, GrupoDTO>().ReverseMap();

            //mapeo de GrupoDTO hacia Grupo //post
            //Aquí no aplica reversemap, porque nunca se hará a la inversa.
            CreateMap<GrupoCreacionDTO, Grupo>();


            CreateMap<Periodo, PeriodoDTO>().ReverseMap();
            CreateMap<PeriodoCreacionDTO, Periodo>();

            CreateMap<Carousel_Imagen, Carousel_ImagenDTO>().ReverseMap();
        }
    }
}
