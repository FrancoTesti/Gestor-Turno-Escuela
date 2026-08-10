using Microsoft.EntityFrameworkCore;
using GTE.Dominio;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GTE.Data
{
    public class GTEContext : DbContext
    {
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<CursoEscolar> Cursos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Secretario> Secretarios { get; set; }
        public DbSet<Portero> Porteros { get; set; }
        public DbSet<Autorizacion> Autorizaciones { get; set; }
        public DbSet<Retiro> Retiros { get; set; }
        public DbSet<HorarioEspecial> HorariosEspeciales { get; set; }

        public GTEContext(DbContextOptions<GTEContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public GTEContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GestorTurnoEscuelaDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contrasena).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.NombreUsuario).IsUnique();
            });

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.HasKey(e => e.IdAlumno);
                entity.Property(e => e.IdAlumno).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Grado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Curso).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<CursoEscolar>(entity =>
            {
                entity.HasKey(e => e.IdCurso);
                entity.Property(e => e.IdCurso).ValueGeneratedOnAdd();
                entity.Property(e => e.Grado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Curso).IsRequired().HasMaxLength(50);
                entity.Property(e => e.HorarioSalida).IsRequired();
            });

            // Mapeo de Tutor
            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.HasKey(e => e.IdTutor);
                entity.Property(e => e.IdTutor).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Dni).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Parentesco).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TieneRestriccion).IsRequired();

                entity.HasOne(e => e.Usuario)
                      .WithOne()
                      .HasForeignKey<Tutor>("IdUsuario")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.IdPersonal);
                entity.Property(e => e.IdPersonal).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Usuario)
                      .WithOne()
                      .HasForeignKey<Personal>("IdUsuario")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Personal>()
                .HasDiscriminator<string>("TipoPersonal")
                .HasValue<Secretario>("Secretario")
                .HasValue<Portero>("Portero");

            modelBuilder.Entity<Secretario>(entity =>
            {
                entity.Property(e => e.NivelAccesoSistema).IsRequired();
            });

            modelBuilder.Entity<Portero>(entity =>
            {
                entity.Property(e => e.PuertaAsignada).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Autorizacion>(entity =>
            {
                entity.HasKey(e => e.IdAutorizacion);
                entity.Property(e => e.IdAutorizacion).ValueGeneratedOnAdd();
                entity.Property(e => e.AlumnoId).IsRequired();
                entity.Property(e => e.TutorId).IsRequired();
            });

            modelBuilder.Entity<Retiro>(entity =>
            {
                entity.HasKey(e => e.IdRetiro);
                entity.Property(e => e.IdRetiro).ValueGeneratedOnAdd();
                entity.Property(e => e.IdAlumno).IsRequired();
                entity.Property(e => e.IdTutor).IsRequired();
                entity.Property(e => e.IdPersonal).IsRequired();
                entity.Property(e => e.FechaHora).IsRequired();
                entity.Property(e => e.Observaciones).HasMaxLength(500);
            });

            modelBuilder.Entity<HorarioEspecial>(entity =>
            {
                entity.HasKey(e => e.IdHorarioEspecial);
                entity.Property(e => e.IdHorarioEspecial).ValueGeneratedOnAdd();
                entity.Property(e => e.IdAlumno).IsRequired();
                entity.Property(e => e.DescripcionActividad).IsRequired().HasMaxLength(200);
                entity.Property(e => e.HoraSalidaEspecial).IsRequired();
            });


            modelBuilder.Entity<Usuario>().HasData(
                new { IdUsuario = 1, NombreUsuario = "admin", Contrasena = "admin123", EstaActivo = true },
                new { IdUsuario = 2, NombreUsuario = "porteria1", Contrasena = "porteria123", EstaActivo = true },
                new { IdUsuario = 3, NombreUsuario = "tutor1", Contrasena = "tutor123", EstaActivo = true }
            );

            modelBuilder.Entity<Alumno>().HasData(
                new { IdAlumno = 1, Nombre = "Juan", Apellido = "Perez", Grado = "1°", Curso = "A", Estado = "Presente" },
                new { IdAlumno = 2, Nombre = "Maria", Apellido = "Gomez", Grado = "2°", Curso = "B", Estado = "Presente" },
                new { IdAlumno = 3, Nombre = "Lautaro", Apellido = "Martinez", Grado = "1°", Curso = "A", Estado = "Presente" },
                new { IdAlumno = 4, Nombre = "Sofia", Apellido = "Rodriguez", Grado = "3°", Curso = "A", Estado = "Presente" }
            );

            modelBuilder.Entity<CursoEscolar>().HasData(
                new { IdCurso = 1, Grado = "1°", Curso = "A", HorarioSalida = new TimeSpan(12, 0, 0) },
                new { IdCurso = 2, Grado = "2°", Curso = "B", HorarioSalida = new TimeSpan(12, 15, 0) },
                new { IdCurso = 3, Grado = "3°", Curso = "A", HorarioSalida = new TimeSpan(12, 30, 0) }
            );

            modelBuilder.Entity<Secretario>().HasData(
                new { IdPersonal = 1, Nombre = "Alejandro Ciesco", NivelAccesoSistema = 5, IdUsuario = 1 }
            );

            modelBuilder.Entity<Portero>().HasData(
                new { IdPersonal = 2, Nombre = "Renzo Scollo", PuertaAsignada = "Puerta Principal", IdUsuario = 2 }
            );

            modelBuilder.Entity<Tutor>().HasData(
                new { IdTutor = 1, Nombre = "Franco", Apellido = "Testi", Dni = "12345678", Parentesco = "Padre", Telefono = "15-5555-5555", TieneRestriccion = false, IdUsuario = 3 }
            );

            modelBuilder.Entity<Autorizacion>().HasData(
                new { IdAutorizacion = 1, AlumnoId = 1, TutorId = 1 },
                new { IdAutorizacion = 2, AlumnoId = 3, TutorId = 1 }
            );
        }
    }
}   