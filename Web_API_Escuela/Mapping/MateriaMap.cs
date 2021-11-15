using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;

namespace Web_API_Escuela.Mapping
{
    public class MateriaMap : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> builder)
        {
            builder.ToTable("materia").HasKey(x => x.IdMateria);


           //Es necesario indicar que se tiene una relacion, para poder agregar el tipo de tabla a la que hace referencia nuestra llave foranea
           builder.HasOne(x => x.Grupo).WithMany().HasForeignKey(x => x.IdGrupo);
        }
    }
}
