using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class PublicacionMap : IEntityTypeConfiguration<Publicacion>
    {
        public void Configure(EntityTypeBuilder<Publicacion> builder)
        {
            builder.ToTable("publicacion")
                .HasKey(x => x.IdPublicacion);


            builder.HasOne(x => x.Materia).WithMany().HasForeignKey(x => x.IdMateria);
            builder.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo);
        }
    }
}
