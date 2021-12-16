using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class GaleriaMap : IEntityTypeConfiguration<Galeria>
    {
        public void Configure(EntityTypeBuilder<Galeria> builder)
        {
            builder.ToTable("galeria")
                .HasKey(x => x.Id);
        }
    }
}
