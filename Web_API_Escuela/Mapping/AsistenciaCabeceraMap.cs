using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class AsistenciaCabeceraMap : IEntityTypeConfiguration<AsistenciaCabecera>
    {
        public void Configure(EntityTypeBuilder<AsistenciaCabecera> builder)
        {
            builder.ToTable("asistenciaCabecera").HasKey(x => x.IdCabecera);

            builder.HasOne(x => x.Materia).WithMany().HasForeignKey(x => x.IdMateria);
            builder.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo);

        }
    }
}
