using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class EncuestaMap : IEntityTypeConfiguration<Encuesta>
    {
        public void Configure(EntityTypeBuilder<Encuesta> builder)
        {
            builder.ToTable("encuesta")
                .HasKey(x => x.IdEncuesta);

            builder.HasOne(x => x.Alumno).WithMany().HasForeignKey(x => x.IdAlumno);
        }
    }
}
