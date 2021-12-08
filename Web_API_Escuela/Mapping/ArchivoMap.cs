using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class ArchivoMap : IEntityTypeConfiguration<Archivo>
    {
        public void Configure(EntityTypeBuilder<Archivo> builder)
        {
            builder.ToTable("archivo")
                .HasKey(x => x.IdArchivo);

            builder.HasOne(x => x.Publicacion).WithMany().HasForeignKey(x => x.IdPublicacion);
        }
    }
}
