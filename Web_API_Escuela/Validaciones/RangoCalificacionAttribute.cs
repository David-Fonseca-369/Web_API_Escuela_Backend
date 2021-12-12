using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Escuela.Validaciones
{
    public class RangoCalificacionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            decimal calificacion = Convert.ToDecimal(value);

            if (calificacion > 10)
            {
                return new ValidationResult("La calificación no deben ser mayor a 10.");
            }

            if (calificacion < 0)
            {
                return new ValidationResult("La calificación no debe ser menor a 0.");
            }

            return ValidationResult.Success;
        }
    }
}
