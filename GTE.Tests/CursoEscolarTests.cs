using System;
using GTE.Dominio;
using Xunit;

namespace GTE.Tests
{
    public class CursoEscolarTests
    {
        #region Suite 1: Nine-Parameter Constructor

        [Fact]
        public void Constructor_9Parametros_ConValoresValidos_InicializaTodasLasPropiedadesCorrectamente()
        {
            // Arrange
            int id = 1;
            string grado = "3°";
            string curso = "A";
            string turno = "Mañana";
            var lunes = new TimeSpan(8, 0, 0);
            var martes = new TimeSpan(8, 15, 0);
            var miercoles = new TimeSpan(8, 30, 0);
            var jueves = new TimeSpan(8, 45, 0);
            var viernes = new TimeSpan(9, 0, 0);

            // Act
            var entity = new CursoEscolar(id, grado, curso, turno, lunes, martes, miercoles, jueves, viernes);

            // Assert
            Assert.Equal(1, entity.IdCurso);
            Assert.Equal("3°", entity.Grado);
            Assert.Equal("A", entity.Curso);
            Assert.Equal("Mañana", entity.Turno);
            Assert.Equal(lunes, entity.HorarioLunes);
            Assert.Equal(martes, entity.HorarioMartes);
            Assert.Equal(miercoles, entity.HorarioMiercoles);
            Assert.Equal(jueves, entity.HorarioJueves);
            Assert.Equal(viernes, entity.HorarioViernes);
            Assert.Equal(lunes, entity.HorarioSalida);
        }

        [Fact]
        public void Constructor_9Parametros_ConIdCero_EsValidoParaNuevaEntidad()
        {
            // Arrange & Act
            var entity = new CursoEscolar(0, "1°", "A", "Mañana",
                new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0),
                new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0));

            // Assert
            Assert.Equal(0, entity.IdCurso);
        }

        [Fact]
        public void Constructor_9Parametros_ConIdNegativo_LanzaArgumentException()
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(-1, "1°", "A", "Mañana",
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0),
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0)));

            Assert.Equal("id", ex.ParamName);
            Assert.Contains("El Id debe ser mayor o igual a 0.", ex.Message);
        }

        [Fact]
        public void Constructor_9Parametros_NormalizaStringsYTurno()
        {
            // Arrange & Act
            var entity = new CursoEscolar(2, " 2° ", " B ", " tarde ",
                new TimeSpan(13, 0, 0), new TimeSpan(13, 0, 0),
                new TimeSpan(13, 0, 0), new TimeSpan(13, 0, 0), new TimeSpan(13, 0, 0));

            // Assert
            Assert.Equal("2°", entity.Grado);
            Assert.Equal("B", entity.Curso);
            Assert.Equal("Tarde", entity.Turno);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_9Parametros_GradoInvalido_LanzaArgumentException(string? gradoInvalido)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, gradoInvalido!, "A", "Mañana",
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0),
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0)));

            Assert.Equal("grado", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_9Parametros_CursoInvalido_LanzaArgumentException(string? cursoInvalido)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, "1°", cursoInvalido!, "Mañana",
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0),
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0)));

            Assert.Equal("curso", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Vespertino")]
        [InlineData("Doble")]
        [InlineData("Madrugada")]
        public void Constructor_9Parametros_TurnoInvalido_LanzaArgumentException(string? turnoInvalido)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, "1°", "A", turnoInvalido!,
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0),
                    new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(12, 0, 0)));

            Assert.Equal("turno", ex.ParamName);
            Assert.Contains("Turno inválido", ex.Message);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -3600)]
        [InlineData(2, 86400)] // 24 hours
        [InlineData(3, 90000)] // 25 hours
        [InlineData(4, -1)]
        public void Constructor_9Parametros_HorarioInvalidoEnCualquierDia_LanzaArgumentException(int diaIndex, int segundos)
        {
            var horarios = new TimeSpan[5]
            {
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 0, 0)
            };
            horarios[diaIndex] = TimeSpan.FromSeconds(segundos);

            Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, "1°", "A", "Mañana",
                    horarios[0], horarios[1], horarios[2], horarios[3], horarios[4]));
        }

        #endregion

        #region Suite 2: Legacy Five-Parameter Constructor

        [Fact]
        public void Constructor_5Parametros_PueblaTodosLosDiasYHorarioSalidaConElMismoValor()
        {
            // Arrange & Act
            var horario = new TimeSpan(22, 15, 0);
            var entity = new CursoEscolar(5, "5°", "A", "Noche", horario);

            // Assert
            Assert.Equal(horario, entity.HorarioSalida);
            Assert.Equal(horario, entity.HorarioLunes);
            Assert.Equal(horario, entity.HorarioMartes);
            Assert.Equal(horario, entity.HorarioMiercoles);
            Assert.Equal(horario, entity.HorarioJueves);
            Assert.Equal(horario, entity.HorarioViernes);
        }

        [Fact]
        public void Constructor_5Parametros_HorarioNegativo_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, "1°", "A", "Mañana", TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void Constructor_5Parametros_Horario24HorasOMayor_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new CursoEscolar(1, "1°", "A", "Mañana", TimeSpan.FromHours(24)));
        }

        #endregion

        #region Suite 3: Day-of-Week Resolution (ObtenerHorarioSalida)

        [Theory]
        [InlineData(DayOfWeek.Monday, 8, 0)]
        [InlineData(DayOfWeek.Tuesday, 8, 15)]
        [InlineData(DayOfWeek.Wednesday, 8, 30)]
        [InlineData(DayOfWeek.Thursday, 8, 45)]
        [InlineData(DayOfWeek.Friday, 9, 0)]
        public void ObtenerHorarioSalida_DayOfWeek_DiasHabiles_RetornaHorarioExactoDelDia(DayOfWeek diaSemana, int horaEsperada, int minutoEsperado)
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana",
                new TimeSpan(8, 0, 0),
                new TimeSpan(8, 15, 0),
                new TimeSpan(8, 30, 0),
                new TimeSpan(8, 45, 0),
                new TimeSpan(9, 0, 0));

            var resultado = entity.ObtenerHorarioSalida(diaSemana);

            Assert.Equal(new TimeSpan(horaEsperada, minutoEsperado, 0), resultado);
        }

        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void ObtenerHorarioSalida_DayOfWeek_FinDeSemana_RecurreAHorarioLunes(DayOfWeek finDeSemana)
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana",
                new TimeSpan(8, 0, 0),
                new TimeSpan(8, 15, 0),
                new TimeSpan(8, 30, 0),
                new TimeSpan(8, 45, 0),
                new TimeSpan(9, 0, 0));

            var resultado = entity.ObtenerHorarioSalida(finDeSemana);

            Assert.Equal(entity.HorarioLunes, resultado);
        }

        [Fact]
        public void ObtenerHorarioSalida_DayOfWeek_ValorFueraDeEnum_RecurreAHorarioLunes()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana",
                new TimeSpan(8, 0, 0),
                new TimeSpan(8, 15, 0),
                new TimeSpan(8, 30, 0),
                new TimeSpan(8, 45, 0),
                new TimeSpan(9, 0, 0));

            Assert.Equal(entity.HorarioLunes, entity.ObtenerHorarioSalida((DayOfWeek)99));
            Assert.Equal(entity.HorarioLunes, entity.ObtenerHorarioSalida((DayOfWeek)(-1)));
        }

        [Fact]
        public void ObtenerHorarioSalida_DateTime_ResuelveCorrectamenteSegunDiaDeLaSemana()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana",
                new TimeSpan(8, 0, 0),
                new TimeSpan(8, 15, 0),
                new TimeSpan(8, 30, 0),
                new TimeSpan(8, 45, 0),
                new TimeSpan(9, 0, 0));

            // 2026-11-02 is Monday, 2026-11-08 is Sunday
            Assert.Equal(new TimeSpan(8, 0, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 2))); // Mon
            Assert.Equal(new TimeSpan(8, 15, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 3))); // Tue
            Assert.Equal(new TimeSpan(8, 30, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 4))); // Wed
            Assert.Equal(new TimeSpan(8, 45, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 5))); // Thu
            Assert.Equal(new TimeSpan(9, 0, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 6))); // Fri
            Assert.Equal(new TimeSpan(8, 0, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 7))); // Sat -> Mon
            Assert.Equal(new TimeSpan(8, 0, 0), entity.ObtenerHorarioSalida(new DateTime(2026, 11, 8))); // Sun -> Mon
        }

        #endregion

        #region Suite 4: Weekly Bulk Schedule Mutation (SetHorariosSemana)

        [Fact]
        public void SetHorariosSemana_TodosValidos_ActualizaTodosLosDiasYHorarioSalida()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));

            entity.SetHorariosSemana(
                new TimeSpan(13, 0, 0),
                new TimeSpan(13, 15, 0),
                new TimeSpan(13, 30, 0),
                new TimeSpan(13, 45, 0),
                new TimeSpan(14, 0, 0));

            Assert.Equal(new TimeSpan(13, 0, 0), entity.HorarioLunes);
            Assert.Equal(new TimeSpan(13, 15, 0), entity.HorarioMartes);
            Assert.Equal(new TimeSpan(13, 30, 0), entity.HorarioMiercoles);
            Assert.Equal(new TimeSpan(13, 45, 0), entity.HorarioJueves);
            Assert.Equal(new TimeSpan(14, 0, 0), entity.HorarioViernes);
            Assert.Equal(new TimeSpan(13, 0, 0), entity.HorarioSalida);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, 86400)]
        [InlineData(2, -3600)]
        [InlineData(3, 90000)]
        [InlineData(4, -1)]
        public void SetHorariosSemana_RechazoAtomico_SiUnHorarioEsInvalido_MantieneEstadoSinCambios(int posicionInvalida, int segundosInvalidos)
        {
            var initial = new TimeSpan(12, 0, 0);
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", initial);

            var validos = new TimeSpan[5]
            {
                new TimeSpan(13, 0, 0),
                new TimeSpan(13, 15, 0),
                new TimeSpan(13, 30, 0),
                new TimeSpan(13, 45, 0),
                new TimeSpan(14, 0, 0)
            };
            validos[posicionInvalida] = TimeSpan.FromSeconds(segundosInvalidos);

            Assert.Throws<ArgumentException>(() =>
                entity.SetHorariosSemana(validos[0], validos[1], validos[2], validos[3], validos[4]));

            // Atomic rollback verification
            Assert.Equal(initial, entity.HorarioLunes);
            Assert.Equal(initial, entity.HorarioMartes);
            Assert.Equal(initial, entity.HorarioMiercoles);
            Assert.Equal(initial, entity.HorarioJueves);
            Assert.Equal(initial, entity.HorarioViernes);
            Assert.Equal(initial, entity.HorarioSalida);
        }

        #endregion

        #region Suite 5: Single Day Schedule Mutation (SetHorarioDia)

        [Fact]
        public void SetHorarioDia_Lunes_ActualizaHorarioLunesYHorarioSalida()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            var nuevoHorario = new TimeSpan(12, 30, 0);

            entity.SetHorarioDia(DayOfWeek.Monday, nuevoHorario);

            Assert.Equal(nuevoHorario, entity.HorarioLunes);
            Assert.Equal(nuevoHorario, entity.HorarioSalida);
            Assert.Equal(new TimeSpan(8, 0, 0), entity.HorarioMartes);
            Assert.Equal(new TimeSpan(8, 0, 0), entity.HorarioMiercoles);
            Assert.Equal(new TimeSpan(8, 0, 0), entity.HorarioJueves);
            Assert.Equal(new TimeSpan(8, 0, 0), entity.HorarioViernes);
        }

        [Theory]
        [InlineData(DayOfWeek.Tuesday, 20)]
        [InlineData(DayOfWeek.Wednesday, 25)]
        [InlineData(DayOfWeek.Thursday, 35)]
        [InlineData(DayOfWeek.Friday, 40)]
        public void SetHorarioDia_MartesAViernes_ActualizaSoloEseDiaYConservaHorarioSalida(DayOfWeek dia, int nuevoMinuto)
        {
            var initial = new TimeSpan(8, 0, 0);
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", initial);
            var nuevo = new TimeSpan(12, nuevoMinuto, 0);

            entity.SetHorarioDia(dia, nuevo);

            Assert.Equal(nuevo, entity.ObtenerHorarioSalida(dia));
            Assert.Equal(initial, entity.HorarioLunes);
            Assert.Equal(initial, entity.HorarioSalida);
        }

        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        [InlineData((DayOfWeek)10)]
        public void SetHorarioDia_FinDeSemanaOInvalido_LanzaArgumentExceptionYNoModificaNada(DayOfWeek diaInvalido)
        {
            var initial = new TimeSpan(8, 0, 0);
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", initial);

            var ex = Assert.Throws<ArgumentException>(() =>
                entity.SetHorarioDia(diaInvalido, new TimeSpan(10, 0, 0)));

            Assert.Equal("dia", ex.ParamName);
            Assert.Contains("Solo se pueden configurar horarios para días hábiles", ex.Message);
            Assert.Equal(initial, entity.HorarioLunes);
            Assert.Equal(initial, entity.HorarioSalida);
        }

        [Theory]
        [InlineData(DayOfWeek.Monday, -1)]
        [InlineData(DayOfWeek.Wednesday, 86400)]
        [InlineData(DayOfWeek.Friday, 90000)]
        public void SetHorarioDia_HorarioInvalido_LanzaArgumentExceptionYNoModificaNada(DayOfWeek dia, int segundos)
        {
            var initial = new TimeSpan(8, 0, 0);
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", initial);

            Assert.Throws<ArgumentException>(() =>
                entity.SetHorarioDia(dia, TimeSpan.FromSeconds(segundos)));

            Assert.Equal(initial, entity.HorarioLunes);
            Assert.Equal(initial, entity.HorarioSalida);
        }

        #endregion

        #region Suite 6: Individual Property Setters & Edge Cases

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void SetIdCurso_ValoresValidos_ActualizaId(int id)
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            entity.SetIdCurso(id);
            Assert.Equal(id, entity.IdCurso);
        }

        [Fact]
        public void SetIdCurso_Negativo_LanzaArgumentException()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            var ex = Assert.Throws<ArgumentException>(() => entity.SetIdCurso(-5));
            Assert.Equal("id", ex.ParamName);
        }

        [Fact]
        public void SetGrado_Y_SetCurso_Validos_TrimeanEspacios()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            entity.SetGrado(" 6° ");
            entity.SetCurso(" C ");

            Assert.Equal("6°", entity.Grado);
            Assert.Equal("C", entity.Curso);
        }

        [Theory]
        [InlineData("mañana", "Mañana")]
        [InlineData("TARDE", "Tarde")]
        [InlineData(" NoChE ", "Noche")]
        public void SetTurno_Validos_NormalizaPascalCase(string entrada, string esperado)
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            entity.SetTurno(entrada);
            Assert.Equal(esperado, entity.Turno);
        }

        [Fact]
        public void SetHorarioSalida_Valido_ActualizaTodosLosDiasYHorarioSalida()
        {
            var entity = new CursoEscolar(1, "1°", "A", "Mañana", new TimeSpan(8, 0, 0));
            var nuevo = new TimeSpan(15, 0, 0);

            entity.SetHorarioSalida(nuevo);

            Assert.Equal(nuevo, entity.HorarioSalida);
            Assert.Equal(nuevo, entity.HorarioLunes);
            Assert.Equal(nuevo, entity.HorarioMartes);
            Assert.Equal(nuevo, entity.HorarioMiercoles);
            Assert.Equal(nuevo, entity.HorarioJueves);
            Assert.Equal(nuevo, entity.HorarioViernes);
        }

        [Fact]
        public void MostrarCurso_FormateaGradoCursoYTurno()
        {
            var entity = new CursoEscolar(1, "5°", "B", "Tarde", new TimeSpan(17, 0, 0));
            Assert.Equal("5° B - Tarde", entity.MostrarCurso());
        }

        #endregion

        #region Suite 7: EF Core & Backward Compatibility

        [Fact]
        public void Constructor_Privado_ParaEFCore_PermiteInstanciacion()
        {
            var instance = Activator.CreateInstance(typeof(CursoEscolar), nonPublic: true);
            Assert.NotNull(instance);
            Assert.IsType<CursoEscolar>(instance);
        }

        #endregion
    }
}
