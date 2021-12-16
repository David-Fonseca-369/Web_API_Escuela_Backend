﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.DTOs.Login
{
    public class RespuestaAutenticacionDTO
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
