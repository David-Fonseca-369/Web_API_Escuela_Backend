using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class AsistenciaDetalleMap : IEntityTypeConfiguration<AsistenciaDetalle>
    {
        public void Configure(EntityTypeBuilder<AsistenciaDetalle> builder)
        {
            builder.ToTable("asistenciaDetalle").HasKey(x => x.IdDetalle);

            builder.HasOne(x => x.AsistenciaCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
        }
    }
}
