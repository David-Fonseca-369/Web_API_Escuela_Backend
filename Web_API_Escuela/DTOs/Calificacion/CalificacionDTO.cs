using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Calificacion
{
    public class CalificacionDTO
    {
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        public decimal PrimerParcial { get; set; }
        public decimal SegundoParcial { get; set; }
        public decimal TercerParcial { get; set; }
    }
}
