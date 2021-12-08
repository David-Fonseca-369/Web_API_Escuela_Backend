﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Entities
{
    public class CalificacionDetalle
    {
        public int IdDetalle { get; set; }
        [Required]
        public int IdCabecera { get; set; }
        [Required]
        public int IdAlumno { get; set; }
        [Required]
        public decimal Calificacion { get; set; }

        public CalificacionCabecera CalificacionCabecera { get; set; }
        public Alumno Alumno { get; set; }
    }
}
