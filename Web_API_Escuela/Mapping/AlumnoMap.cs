using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class AlumnoMap : IEntityTypeConfiguration<Alumno>

    {
        public void Configure(EntityTypeBuilder<Alumno> builder)
        {
            builder.ToTable("alumno")
                .HasKey(x => x.IdAlumno);

            builder.HasOne(x => x.Grupo).WithMany().HasForeignKey(x => x.IdGrupo);
        }
    }
}
