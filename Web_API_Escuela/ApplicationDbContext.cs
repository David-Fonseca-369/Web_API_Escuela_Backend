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
            modelBuilder.ApplyConfiguration(new AlumnoMap());

            modelBuilder.ApplyConfiguration(new AsistenciaCabeceraMap());
            modelBuilder.ApplyConfiguration(new AsistenciaDetalleMap());
            modelBuilder.ApplyConfiguration(new EvaluacionMap());

            modelBuilder.ApplyConfiguration(new CalificacionCabeceraMap());
            modelBuilder.ApplyConfiguration(new CalificacionDetalleMap());

            modelBuilder.ApplyConfiguration(new PublicacionMap());
            modelBuilder.ApplyConfiguration(new ArchivoMap());

            modelBuilder.ApplyConfiguration(new Carousel_ImagenMap());

        }

        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }

        public DbSet<AsistenciaCabecera> AsistenciaCabeceras { get; set; }
        public DbSet<AsistenciaDetalle> AsistenciaDetalles { get; set; }

        public DbSet<Evaluacion> Evaluaciones { get; set; }

        public DbSet<CalificacionCabecera> CalificacionCabeceras { get; set; }
        public DbSet<CalificacionDetalle> CalificacionDetalles { get; set; }

        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Archivo> Archivos { get; set; }

        public DbSet<Carousel_Imagen> Carousel_Imagenes { get; set; }
    }
}
