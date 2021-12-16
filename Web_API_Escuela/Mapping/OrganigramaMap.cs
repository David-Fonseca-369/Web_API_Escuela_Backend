using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class OrganigramaMap : IEntityTypeConfiguration<Organigrama>
    {
        public void Configure(EntityTypeBuilder<Organigrama> builder)
        {
            builder.ToTable("organigrama")
                .HasKey(x => x.Id);
        }
    }
}
