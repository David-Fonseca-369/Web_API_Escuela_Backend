using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class Carousel_ImagenMap : IEntityTypeConfiguration<Carousel_Imagen>
    {
        public void Configure(EntityTypeBuilder<Carousel_Imagen> builder)
        {
            builder.ToTable("carousel_imagen")
                .HasKey(x => x.Id);
        }
    }
}
