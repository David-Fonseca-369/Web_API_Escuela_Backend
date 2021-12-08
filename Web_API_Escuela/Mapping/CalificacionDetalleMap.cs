using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class CalificacionDetalleMap : IEntityTypeConfiguration<CalificacionDetalle>
    {
        public void Configure(EntityTypeBuilder<CalificacionDetalle> builder)
        {
            builder.ToTable("calificacionDetalle")
                .HasKey(x => x.IdDetalle);

            builder.HasOne(x => x.CalificacionCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
            builder.HasOne(x => x.Alumno).WithMany().HasForeignKey(x => x.IdAlumno);
        }
    }
}
