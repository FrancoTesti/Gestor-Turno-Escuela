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
        public DbSet<DetalleRetiro> DetalleRetiros { get; set; }
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
                entity.Property(e => e.IdCurso).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
                entity.HasOne(e => e.CursoEscolar)
                      .WithMany()
                      .HasForeignKey(e => e.IdCurso)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CursoEscolar>(entity =>
            {
                entity.HasKey(e => e.IdCurso);
                entity.Property(e => e.IdCurso).ValueGeneratedOnAdd();
                entity.Property(e => e.Grado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Curso).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Turno).IsRequired().HasMaxLength(20);
                entity.Property(e => e.HorarioSalida).IsRequired();
                entity.HasIndex(e => new { e.Grado, e.Curso, e.Turno }).IsUnique();
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
                entity.Property(e => e.IdTutor).IsRequired();
                entity.Property(e => e.IdPersonal).IsRequired();
                entity.Property(e => e.FechaHora).IsRequired();
                entity.Property(e => e.Observaciones).HasMaxLength(500);

                entity.HasOne(e => e.Tutor)
                      .WithMany()
                      .HasForeignKey(e => e.IdTutor)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Personal)
                      .WithMany()
                      .HasForeignKey(e => e.IdPersonal)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Detalles)
                      .WithOne()
                      .HasForeignKey(e => e.IdRetiro)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleRetiro>(entity =>
            {
                entity.HasKey(e => e.IdDetalleRetiro);
                entity.Property(e => e.IdDetalleRetiro).ValueGeneratedOnAdd();
                entity.Property(e => e.IdRetiro).IsRequired();
                entity.Property(e => e.IdAlumno).IsRequired();
                entity.Property(e => e.HoraSalida).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Alumno)
                      .WithMany()
                      .HasForeignKey(e => e.IdAlumno)
                      .OnDelete(DeleteBehavior.Restrict);
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
                new { IdUsuario = 3, NombreUsuario = "tutor1", Contrasena = "tutor123", EstaActivo = true },
                new { IdUsuario = 4, NombreUsuario = "porteria2", Contrasena = "porteria123", EstaActivo = true },
                new { IdUsuario = 5, NombreUsuario = "tutor2", Contrasena = "tutor123", EstaActivo = true }
            );

            modelBuilder.Entity<CursoEscolar>().HasData(
                new { IdCurso = 1, Grado = "1°", Curso = "A", Turno = "Mañana", HorarioSalida = new TimeSpan(12, 0, 0) },
                new { IdCurso = 2, Grado = "2°", Curso = "B", Turno = "Mañana", HorarioSalida = new TimeSpan(12, 15, 0) },
                new { IdCurso = 3, Grado = "3°", Curso = "A", Turno = "Tarde", HorarioSalida = new TimeSpan(17, 30, 0) },
                new { IdCurso = 4, Grado = "4°", Curso = "B", Turno = "Tarde", HorarioSalida = new TimeSpan(17, 45, 0) },
                new { IdCurso = 5, Grado = "5°", Curso = "A", Turno = "Noche", HorarioSalida = new TimeSpan(21, 30, 0) },
                new { IdCurso = 6, Grado = "6°", Curso = "A", Turno = "Noche", HorarioSalida = new TimeSpan(22, 0, 0) }
            );

            modelBuilder.Entity<Alumno>().HasData(
                new { IdAlumno = 1, Nombre = "Juan", Apellido = "Perez", IdCurso = 1, Estado = "Presente" },
                new { IdAlumno = 2, Nombre = "Maria", Apellido = "Gomez", IdCurso = 2, Estado = "Presente" },
                new { IdAlumno = 3, Nombre = "Lautaro", Apellido = "Martinez", IdCurso = 1, Estado = "Presente" },
                new { IdAlumno = 4, Nombre = "Sofia", Apellido = "Rodriguez", IdCurso = 3, Estado = "Presente" },
                new { IdAlumno = 5, Nombre = "Lucas", Apellido = "Alvarez", IdCurso = 1, Estado = "Presente" },
                new { IdAlumno = 6, Nombre = "Valentina", Apellido = "Fernandez", IdCurso = 2, Estado = "Presente" },
                new { IdAlumno = 7, Nombre = "Mateo", Apellido = "Lopez", IdCurso = 2, Estado = "Presente" },
                new { IdAlumno = 8, Nombre = "Camila", Apellido = "Diaz", IdCurso = 3, Estado = "Presente" },
                new { IdAlumno = 9, Nombre = "Joaquin", Apellido = "Garcia", IdCurso = 3, Estado = "Presente" },
                new { IdAlumno = 10, Nombre = "Martina", Apellido = "Romero", IdCurso = 4, Estado = "Presente" },
                new { IdAlumno = 11, Nombre = "Thiago", Apellido = "Sosa", IdCurso = 4, Estado = "Presente" },
                new { IdAlumno = 12, Nombre = "Emma", Apellido = "Torres", IdCurso = 4, Estado = "Presente" },
                new { IdAlumno = 13, Nombre = "Benjamin", Apellido = "Benitez", IdCurso = 5, Estado = "Presente" },
                new { IdAlumno = 14, Nombre = "Olivia", Apellido = "Flores", IdCurso = 5, Estado = "Presente" },
                new { IdAlumno = 15, Nombre = "Felipe", Apellido = "Acosta", IdCurso = 5, Estado = "Presente" },
                new { IdAlumno = 16, Nombre = "Catalina", Apellido = "Medina", IdCurso = 6, Estado = "Presente" },
                new { IdAlumno = 17, Nombre = "Nicolas", Apellido = "Herrera", IdCurso = 6, Estado = "Presente" },
                new { IdAlumno = 18, Nombre = "Mia", Apellido = "Aguirre", IdCurso = 6, Estado = "Presente" },
                new { IdAlumno = 19, Nombre = "Agustin", Apellido = "Castro", IdCurso = 1, Estado = "Presente" },
                new { IdAlumno = 20, Nombre = "Julieta", Apellido = "Gimenez", IdCurso = 3, Estado = "Presente" }
            );

            modelBuilder.Entity<Secretario>().HasData(
                new { IdPersonal = 1, Nombre = "Alejandro Ciesco", NivelAccesoSistema = 5, IdUsuario = 1 }
            );

            modelBuilder.Entity<Portero>().HasData(
                new { IdPersonal = 2, Nombre = "Renzo Scollo", PuertaAsignada = "Puerta Principal", IdUsuario = 2 },
                new { IdPersonal = 3, Nombre = "Carlos Martinez", PuertaAsignada = "Puerta Secundaria", IdUsuario = 4 }
            );

            modelBuilder.Entity<Tutor>().HasData(
                new { IdTutor = 1, Nombre = "Franco", Apellido = "Testi", Dni = "34567890", Parentesco = "Padre", Telefono = "11-4455-6677", TieneRestriccion = false, IdUsuario = 3 },
                new { IdTutor = 2, Nombre = "Mariana", Apellido = "Gomez", Dni = "36123456", Parentesco = "Madre", Telefono = "11-6677-8899", TieneRestriccion = false, IdUsuario = 5 }
            );

            modelBuilder.Entity<Autorizacion>().HasData(
                new { IdAutorizacion = 1, AlumnoId = 1, TutorId = 1 },
                new { IdAutorizacion = 2, AlumnoId = 3, TutorId = 1 },
                new { IdAutorizacion = 3, AlumnoId = 5, TutorId = 1 },
                new { IdAutorizacion = 4, AlumnoId = 8, TutorId = 1 },
                new { IdAutorizacion = 5, AlumnoId = 13, TutorId = 1 },
                new { IdAutorizacion = 6, AlumnoId = 2, TutorId = 2 },
                new { IdAutorizacion = 7, AlumnoId = 4, TutorId = 2 },
                new { IdAutorizacion = 8, AlumnoId = 6, TutorId = 2 },
                new { IdAutorizacion = 9, AlumnoId = 10, TutorId = 2 },
                new { IdAutorizacion = 10, AlumnoId = 16, TutorId = 2 }
            );

            modelBuilder.Entity<Retiro>().HasData(
                new { IdRetiro = 1, IdTutor = 1, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 10, 11, 30, 0), Observaciones = "Retiro por turno médico" },
                new { IdRetiro = 2, IdTutor = 2, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 12, 16, 45, 0), Observaciones = "Actividad extraprogramática" },
                new { IdRetiro = 3, IdTutor = 1, IdPersonal = 3, FechaHora = new DateTime(2026, 9, 15, 11, 45, 0), Observaciones = "Retiro de hermanos" },
                new { IdRetiro = 4, IdTutor = 2, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 18, 17, 0, 0), Observaciones = "Autorización firmada" },
                new { IdRetiro = 5, IdTutor = 1, IdPersonal = 3, FechaHora = new DateTime(2026, 9, 20, 21, 0, 0), Observaciones = "Retiro anticipado turno noche" },
                new { IdRetiro = 6, IdTutor = 2, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 22, 17, 15, 0), Observaciones = "Retiro justificado" },
                new { IdRetiro = 7, IdTutor = 1, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 24, 11, 50, 0), Observaciones = "Cita médica" },
                new { IdRetiro = 8, IdTutor = 2, IdPersonal = 3, FechaHora = new DateTime(2026, 9, 25, 21, 15, 0), Observaciones = "Viaje familiar" },
                new { IdRetiro = 9, IdTutor = 1, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 28, 11, 40, 0), Observaciones = "Retiro por indisposición" },
                new { IdRetiro = 10, IdTutor = 2, IdPersonal = 2, FechaHora = new DateTime(2026, 9, 29, 10, 30, 0), Observaciones = "Retiro del día" }
            );

            modelBuilder.Entity<DetalleRetiro>().HasData(
                new { IdDetalleRetiro = 1, IdRetiro = 1, IdAlumno = 1, HoraSalida = new TimeSpan(11, 30, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 2, IdRetiro = 2, IdAlumno = 4, HoraSalida = new TimeSpan(16, 45, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 3, IdRetiro = 3, IdAlumno = 3, HoraSalida = new TimeSpan(11, 45, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 4, IdRetiro = 3, IdAlumno = 5, HoraSalida = new TimeSpan(11, 45, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 5, IdRetiro = 4, IdAlumno = 2, HoraSalida = new TimeSpan(17, 0, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 6, IdRetiro = 4, IdAlumno = 6, HoraSalida = new TimeSpan(17, 0, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 7, IdRetiro = 5, IdAlumno = 13, HoraSalida = new TimeSpan(21, 0, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 8, IdRetiro = 6, IdAlumno = 10, HoraSalida = new TimeSpan(17, 15, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 9, IdRetiro = 7, IdAlumno = 1, HoraSalida = new TimeSpan(11, 50, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 10, IdRetiro = 8, IdAlumno = 16, HoraSalida = new TimeSpan(21, 15, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 11, IdRetiro = 9, IdAlumno = 8, HoraSalida = new TimeSpan(11, 40, 0), Estado = "Retirado" },
                new { IdDetalleRetiro = 12, IdRetiro = 10, IdAlumno = 2, HoraSalida = new TimeSpan(10, 30, 0), Estado = "Retirado" }
            );
        }
    }
}   
