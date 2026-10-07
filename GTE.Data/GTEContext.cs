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
        public DbSet<SalidaDeCurso> SalidasDeCurso { get; set; }

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
                entity.Property(e => e.Parentesco).IsRequired().HasMaxLength(50);
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

            modelBuilder.Entity<SalidaDeCurso>(entity =>
            {
                entity.HasKey(e => e.IdSalidaDeCurso);
                entity.Property(e => e.IdSalidaDeCurso).ValueGeneratedOnAdd();
                entity.Property(e => e.IdCurso).IsRequired();
                entity.Property(e => e.IdPersonal).IsRequired();
                entity.Property(e => e.FechaHoraInicio).IsRequired();
                entity.Property(e => e.FechaHoraFin);

                entity.HasOne(e => e.CursoEscolar)
                      .WithMany()
                      .HasForeignKey(e => e.IdCurso)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Personal)
                      .WithMany()
                      .HasForeignKey(e => e.IdPersonal)
                      .OnDelete(DeleteBehavior.Restrict);

                // La pantalla de la puerta consulta por los que todavía están en curso.
                entity.HasIndex(e => new { e.IdCurso, e.FechaHoraFin });
            });


            // ------------------------------------------------------------------
            // Datos de ejemplo: una escuela con doce cursos, sus alumnos, los
            // tutores que los retiran y los retiros de los últimos días.
            //
            // Se arma con listas y bucles para que la escuela tenga un tamaño
            // realista sin escribir cientos de filas a mano. Los usuarios que ya
            // usaba el grupo (admin, porteria1, tutor1, ...) se mantienen con la
            // misma contraseña, así se sigue entrando igual que antes.
            // ------------------------------------------------------------------
            const int CantidadDeCursos = 12;
            // Cada familia tiene un tutor titular; en unas pocas, además, el otro
            // progenitor también quedó registrado con su propio usuario.
            const int CantidadDeFamilias = 60;
            const int FamiliasConDosProgenitores = 8;
            const int AlumnosPorCurso = 12;
            const int TutoresQueYaRetiraron = 12;

            string[] grados = { "1°", "2°", "3°", "4°", "5°", "6°" };
            string[] divisiones = { "A", "B" };
            string[] turnosPorGrado = { "Mañana", "Mañana", "Tarde", "Tarde", "Noche", "Noche" };
            TimeSpan[] salidasPorGrado =
            {
                new TimeSpan(12, 0, 0), new TimeSpan(12, 15, 0), new TimeSpan(17, 30, 0),
                new TimeSpan(17, 45, 0), new TimeSpan(21, 30, 0), new TimeSpan(22, 0, 0)
            };

            string[] nombresDeAlumnos =
            {
                "Sofía", "Mateo", "Valentina", "Lautaro", "Camila", "Benjamín",
                "Martina", "Joaquín", "Catalina", "Thiago", "Emilia", "Bautista",
                "Delfina", "Santiago", "Julieta", "Tomás", "Mía", "Facundo",
                "Zoe", "Nicolás", "Isabella", "Agustín", "Renata", "Franco",
                "Guadalupe", "Ignacio", "Abril", "Máximo", "Clara", "Lorenzo",
                "Pilar", "Dante", "Valentino", "Malena", "Bruno", "Ámbar"
            };

            // Nombres de tutores y de tutoras por separado, así el parentesco que se
            // carga (padre o madre) es coherente con el nombre.
            string[] nombresDeTutores =
            {
                "Marcelo", "Gustavo", "Diego", "Fernando", "Alejandro", "Sergio",
                "Pablo", "Roberto", "Martín", "Jorge", "Cristian", "Hernán"
            };

            string[] nombresDeTutoras =
            {
                "Analía", "Silvana", "Verónica", "Patricia", "Lorena", "Gabriela",
                "Mónica", "Claudia", "Andrea", "Silvia", "Natalia", "Vanesa"
            };

            string[] apellidos =
            {
                "Testi", "Gomez", "Perez", "Rodriguez", "Fernandez", "Lopez",
                "Martinez", "Diaz", "Alvarez", "Romero", "Sosa", "Torres",
                "Benitez", "Flores", "Acosta", "Medina", "Herrera", "Aguirre",
                "Castro", "Gimenez", "Suarez", "Molina", "Ortiz", "Silva",
                "Rojas", "Dominguez", "Cabrera", "Luna", "Paz", "Vega",
                "Bravo", "Ferreyra", "Quiroga", "Rios", "Cardozo", "Villalba",
                "Navarro", "Arias", "Godoy", "Coronel", "Ocampo", "Ledesma",
                "Ibarra", "Salinas", "Duarte", "Escobar", "Montero", "Toledo",
                "Vera", "Ramirez", "Barrera", "Farias", "Peralta", "Maldonado",
                "Carrizo", "Alderete", "Bogado", "Zarate", "Pereyra", "Agüero",
                "Bustos", "Chavez", "Correa", "Caceres", "Zamora", "Ojeda",
                "Vargas", "Baez", "Leiva", "Arce", "Ponce", "Sanabria"
            };

            string[] motivosDeRetiro =
            {
                "Turno médico", "Actividad extraprogramática", "Retiro de hermanos",
                "Autorización firmada", "Cita médica", "Viaje familiar"
            };

            var cursosDeEjemplo = new List<object>();
            for (int i = 0; i < CantidadDeCursos; i++)
            {
                cursosDeEjemplo.Add(new
                {
                    IdCurso = i + 1,
                    Grado = grados[i / divisiones.Length],
                    Curso = divisiones[i % divisiones.Length],
                    Turno = turnosPorGrado[i / divisiones.Length],
                    HorarioSalida = salidasPorGrado[i / divisiones.Length]
                });
            }

            modelBuilder.Entity<CursoEscolar>().HasData(cursosDeEjemplo);

            var usuariosDeEjemplo = new List<object>
            {
                new { IdUsuario = 1, NombreUsuario = "admin", Contrasena = "admin123", EstaActivo = true },
                new { IdUsuario = 2, NombreUsuario = "porteria1", Contrasena = "porteria123", EstaActivo = true },
                new { IdUsuario = 3, NombreUsuario = "tutor1", Contrasena = "tutor123", EstaActivo = true },
                new { IdUsuario = 4, NombreUsuario = "porteria2", Contrasena = "porteria123", EstaActivo = true },
                new { IdUsuario = 5, NombreUsuario = "tutor2", Contrasena = "tutor123", EstaActivo = true }
            };

            // Cada familia tiene un tutor titular. Las dos primeras familias son
            // las que ya usaba el grupo, con sus mismos usuarios.
            int[] usuariosDeLasFamilias = new int[CantidadDeFamilias + 1];
            usuariosDeLasFamilias[1] = 3;
            usuariosDeLasFamilias[2] = 5;

            int proximoIdUsuario = 6;

            // Parentesco habitual del tutor con los chicos de su familia.
            bool EsMadre(int familia) => familia == 2 || (familia > 2 && familia % 2 == 0);

            var tutoresDeEjemplo = new List<object>();

            for (int familia = 1; familia <= CantidadDeFamilias; familia++)
            {
                if (familia > 2)
                {
                    usuariosDeLasFamilias[familia] = proximoIdUsuario;
                    usuariosDeEjemplo.Add(new
                    {
                        IdUsuario = proximoIdUsuario++,
                        NombreUsuario = $"tutor{familia}",
                        Contrasena = "tutor123",
                        EstaActivo = true
                    });
                }

                bool esMujer = EsMadre(familia);

                tutoresDeEjemplo.Add(new
                {
                    IdTutor = familia,
                    Nombre = familia == 1 ? "Franco"
                        : familia == 2 ? "Mariana"
                        : esMujer ? nombresDeTutoras[(familia / 2) % nombresDeTutoras.Length]
                        : nombresDeTutores[(familia / 2) % nombresDeTutores.Length],
                    Apellido = apellidos[familia - 1],
                    Dni = (30000000 + (familia * 137)).ToString(),
                    Parentesco = esMujer ? "Madre" : "Padre",
                    Telefono = $"11-{4000 + familia}-{1000 + (familia * 7)}",
                    // Un tutor con restricción, para poder probar que el sistema no
                    // lo deja retirar alumnos.
                    TieneRestriccion = familia == 7,
                    IdUsuario = usuariosDeLasFamilias[familia]
                });
            }

            // Las familias no son todas iguales: las hay con un solo hijo en la
            // escuela y otras con tres o cuatro. Entre todas suman los 144 alumnos
            // de la escuela.
            int[] tamaniosDeFamilia = new int[CantidadDeFamilias];
            var tamaniosMezclados = new List<int>();
            tamaniosMezclados.AddRange(Enumerable.Repeat(1, 12));
            tamaniosMezclados.AddRange(Enumerable.Repeat(2, 21));
            tamaniosMezclados.AddRange(Enumerable.Repeat(3, 18));
            tamaniosMezclados.AddRange(Enumerable.Repeat(4, 9));

            // Se mezclan de una forma fija (sin azar), así las familias chicas y
            // las grandes no quedan todas juntas en el listado.
            for (int familia = 0; familia < CantidadDeFamilias; familia++)
                tamaniosDeFamilia[familia] = tamaniosMezclados[(familia * 29) % CantidadDeFamilias];

            // Un lugar por cada hijo, con la familia que le toca: de ahí sale el
            // reparto de hermanos entre los cursos.
            var familiasPorHijo = new List<int>();
            for (int familia = 1; familia <= CantidadDeFamilias; familia++)
                for (int hijo = 0; hijo < tamaniosDeFamilia[familia - 1]; hijo++)
                    familiasPorHijo.Add(familia);

            // El salto hace que los hermanos no caigan en el mismo curso.
            const int SaltoEntreHermanos = 7;

            var hijosDeCadaFamilia = new Dictionary<int, List<int>>();
            var familiaDeCadaAlumno = new Dictionary<int, int>();
            var alumnosDeEjemplo = new List<object>();
            var autorizacionesDeEjemplo = new List<object>();
            int proximoAlumno = 1;

            for (int curso = 1; curso <= CantidadDeCursos; curso++)
            {
                for (int alumno = 1; alumno <= AlumnosPorCurso; alumno++)
                {
                    // Los hermanos comparten el apellido con su familia y van a
                    // cursos distintos, como en una escuela de verdad.
                    int familia = familiasPorHijo[((proximoAlumno - 1) * SaltoEntreHermanos) % familiasPorHijo.Count];

                    familiaDeCadaAlumno[proximoAlumno] = familia;

                    if (!hijosDeCadaFamilia.TryGetValue(familia, out var hijos))
                        hijosDeCadaFamilia[familia] = hijos = new List<int>();
                    hijos.Add(proximoAlumno);

                    string estado = familia <= TutoresQueYaRetiraron ? "Retirado"
                        : proximoAlumno % 30 == 0 ? "Ausente"
                        : "Presente";

                    alumnosDeEjemplo.Add(new
                    {
                        IdAlumno = proximoAlumno,
                        Nombre = nombresDeAlumnos[(proximoAlumno - 1) % nombresDeAlumnos.Length],
                        Apellido = apellidos[familia - 1],
                        IdCurso = curso,
                        Estado = estado
                    });

                    autorizacionesDeEjemplo.Add(new
                    {
                        IdAutorizacion = autorizacionesDeEjemplo.Count + 1,
                        AlumnoId = proximoAlumno,
                        TutorId = familia,
                        Parentesco = EsMadre(familia) ? "Madre" : "Padre"
                    });

                    proximoAlumno++;
                }
            }

            // En las primeras familias el otro progenitor también quedó registrado,
            // así hay alumnos con dos adultos autorizados para retirarlos.
            for (int familia = 1; familia <= FamiliasConDosProgenitores; familia++)
            {
                int idTutor = CantidadDeFamilias + familia;
                bool esMujer = !EsMadre(familia);
                int idUsuario = proximoIdUsuario++;

                usuariosDeEjemplo.Add(new
                {
                    IdUsuario = idUsuario,
                    NombreUsuario = $"tutor{idTutor}",
                    Contrasena = "tutor123",
                    EstaActivo = true
                });

                tutoresDeEjemplo.Add(new
                {
                    IdTutor = idTutor,
                    Nombre = esMujer ? nombresDeTutoras[idTutor % nombresDeTutoras.Length]
                        : nombresDeTutores[idTutor % nombresDeTutores.Length],
                    // El otro progenitor suele tener otro apellido.
                    Apellido = apellidos[CantidadDeFamilias + familia - 1],
                    Dni = (30000000 + (idTutor * 137)).ToString(),
                    Parentesco = esMujer ? "Madre" : "Padre",
                    Telefono = $"11-{4000 + idTutor}-{1000 + (idTutor * 7)}",
                    TieneRestriccion = false,
                    IdUsuario = idUsuario
                });

                foreach (int hijo in hijosDeCadaFamilia[familia])
                {
                    autorizacionesDeEjemplo.Add(new
                    {
                        IdAutorizacion = autorizacionesDeEjemplo.Count + 1,
                        AlumnoId = hijo,
                        TutorId = idTutor,
                        Parentesco = esMujer ? "Madre" : "Padre"
                    });
                }
            }

            // Unos pocos alumnos quedaron autorizados para un tío o un abuelo. El
            // adulto extra sale de las familias con un solo hijo, así ninguna
            // familia termina con demasiados chicos a cargo.
            var familiasConUnSoloHijo = new List<int>();
            for (int familia = 1; familia <= CantidadDeFamilias; familia++)
                if (tamaniosDeFamilia[familia - 1] == 1)
                    familiasConUnSoloHijo.Add(familia);

            string[] parentescosDeConfianza = { "Tío", "Tía", "Abuelo", "Abuela" };
            int[] alumnosConOtroAutorizado = { 17, 34, 51, 68, 85, 102, 119, 136 };

            for (int i = 0; i < alumnosConOtroAutorizado.Length; i++)
            {
                int alumno = alumnosConOtroAutorizado[i];
                int indice = i;
                int familiaExtra = familiasConUnSoloHijo[indice % familiasConUnSoloHijo.Count];

                while (familiaExtra == familiaDeCadaAlumno[alumno])
                {
                    indice++;
                    familiaExtra = familiasConUnSoloHijo[indice % familiasConUnSoloHijo.Count];
                }

                autorizacionesDeEjemplo.Add(new
                {
                    IdAutorizacion = autorizacionesDeEjemplo.Count + 1,
                    AlumnoId = alumno,
                    TutorId = familiaExtra,
                    // Con este otro adulto el parentesco es distinto: eso es lo que
                    // antes no se podía representar.
                    Parentesco = parentescosDeConfianza[i % parentescosDeConfianza.Length]
                });
            }

            modelBuilder.Entity<Usuario>().HasData(usuariosDeEjemplo);
            modelBuilder.Entity<Secretario>().HasData(
                new { IdPersonal = 1, Nombre = "Alejandro Ciesco", NivelAccesoSistema = 5, IdUsuario = 1 }
            );

            modelBuilder.Entity<Portero>().HasData(
                new { IdPersonal = 2, Nombre = "Renzo Scollo", PuertaAsignada = "Puerta Principal", IdUsuario = 2 },
                new { IdPersonal = 3, Nombre = "Carlos Martinez", PuertaAsignada = "Puerta Secundaria", IdUsuario = 4 }
            );

            modelBuilder.Entity<Tutor>().HasData(tutoresDeEjemplo);
            modelBuilder.Entity<Alumno>().HasData(alumnosDeEjemplo);
            modelBuilder.Entity<Autorizacion>().HasData(autorizacionesDeEjemplo);

            // Las primeras doce familias ya pasaron a retirar a sus hijos, en días
            // distintos del último mes.
            var retirosDeEjemplo = new List<object>();
            var detallesDeEjemplo = new List<object>();
            int proximoDetalle = 1;

            for (int t = 1; t <= TutoresQueYaRetiraron; t++)
            {
                TimeSpan hora = t % 2 == 0 ? new TimeSpan(17, 30, 0) : new TimeSpan(11, 30, 0);

                retirosDeEjemplo.Add(new
                {
                    IdRetiro = t,
                    IdTutor = t,
                    IdPersonal = t % 2 == 0 ? 2 : 3,
                    FechaHora = new DateTime(2026, 9, 24).AddDays(t).Add(hora),
                    Observaciones = motivosDeRetiro[t % motivosDeRetiro.Length]
                });

                // Cada familia se llevó a sus hijos: algunas uno, otras dos o más.
                foreach (int hijo in hijosDeCadaFamilia[t])
                {
                    detallesDeEjemplo.Add(new
                    {
                        IdDetalleRetiro = proximoDetalle++,
                        IdRetiro = t,
                        IdAlumno = hijo,
                        HoraSalida = hora,
                        Estado = "Retirado"
                    });
                }
            }

            modelBuilder.Entity<Retiro>().HasData(retirosDeEjemplo);
            modelBuilder.Entity<DetalleRetiro>().HasData(detallesDeEjemplo);

            modelBuilder.Entity<HorarioEspecial>().HasData(
                new { IdHorarioEspecial = 1, IdAlumno = 13, DescripcionActividad = "Turno médico", HoraSalidaEspecial = new TimeSpan(10, 30, 0) },
                new { IdHorarioEspecial = 2, IdAlumno = 62, DescripcionActividad = "Ensayo del acto", HoraSalidaEspecial = new TimeSpan(16, 0, 0) },
                new { IdHorarioEspecial = 3, IdAlumno = 110, DescripcionActividad = "Entrenamiento deportivo", HoraSalidaEspecial = new TimeSpan(18, 30, 0) }
            );
        }
    }
}   
