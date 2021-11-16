using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Escuela.Entities;
using Web_API_Escuela.Mapping;

namespace Web_API_Escuela
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GrupoMap());
            modelBuilder.ApplyConfiguration(new PeriodoMap());
            modelBuilder.ApplyConfiguration(new MateriaMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }

        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
