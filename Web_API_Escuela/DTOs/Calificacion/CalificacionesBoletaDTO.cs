﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Calificacion
{
    public class CalificacionesBoletaDTO
    {
        public string NombreMateria { get; set; }
        public string NombrePeriodo { get; set; }
        public decimal Calificacion { get; set; }
    }
}
