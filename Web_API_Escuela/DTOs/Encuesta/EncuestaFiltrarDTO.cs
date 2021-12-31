using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Encuesta
{
    public class EncuestaFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public  PaginacionDTO PaginacionDTO
        {
            get { return new PaginacionDTO() { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina }; }
        }
        //Filtrar por
        public int IdEncuesta { get; set; }
    }
}
