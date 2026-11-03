using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using Moq;
using Xunit;

namespace GTE.Tests
{
    public class SalidaDeCursoServiceSemanalesTests
    {
        #region Test Fixtures & Mock Factories

        private readonly Mock<ISalidaDeCursoRepository> _mockSalidaRepo;
        private readonly Mock<ICursoEscolarRepository> _mockCursoRepo;
        private readonly Mock<IAlumnoRepository> _mockAlumnoRepo;
        private readonly SalidaDeCursoService _servicio;

        public SalidaDeCursoServiceSemanalesTests()
        {
            _mockSalidaRepo = new Mock<ISalidaDeCursoRepository>(MockBehavior.Strict);
            _mockCursoRepo = new Mock<ICursoEscolarRepository>(MockBehavior.Strict);
            _mockAlumnoRepo = new Mock<IAlumnoRepository>(MockBehavior.Strict);

            _servicio = new SalidaDeCursoService(
                _mockSalidaRepo.Object,
                _mockCursoRepo.Object,
                _mockAlumnoRepo.Object);
        }

        private static Portero CrearPortero(string puerta = "Puerta Principal") =>
            new Portero("Carlos Portero", puerta, new Usuario("carlos_port", "Password123!"));

        /// <summary>
        /// Course 1: Mon 12:00, Tue 12:00, Wed 12:15, Thu 12:00, Fri 11:30.
        /// </summary>
        private static CursoEscolar CrearCurso1Diferenciado() =>
            new CursoEscolar(
                id: 1,
                grado: "1°",
                curso: "A",
                turno: "Mañana",
                lunes: new TimeSpan(12, 0, 0),
                martes: new TimeSpan(12, 0, 0),
                miercoles: new TimeSpan(12, 15, 0),
                jueves: new TimeSpan(12, 0, 0),
                viernes: new TimeSpan(11, 30, 0)
            );

        /// <summary>
        /// Course 2: Uniform departure at 12:30 Mon-Fri.
        /// </summary>
        private static CursoEscolar CrearCurso2Uniforme() =>
            new CursoEscolar(
                id: 2,
                grado: "2°",
                curso: "A",
                turno: "Mañana",
                horario: new TimeSpan(12, 30, 0)
            );

        private void SetupCarteleraBase(List<CursoEscolar> cursos, List<SalidaDeCurso>? enCurso = null, List<Alumno>? alumnos = null)
        {
            _mockSalidaRepo.Setup(r => r.GetEnCursoAsync())
                .ReturnsAsync(enCurso ?? new List<SalidaDeCurso>());

            _mockAlumnoRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(alumnos ?? new List<Alumno>());

            _mockCursoRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(cursos);
        }

        #endregion

        #region 1. Differentiated Weekday Schedule Resolution

        [Fact]
        public async Task GetCarteleraAsync_Miercoles_12_05_Curso1_Es_Proximo_Antes_De_Curso2()
        {
            // Wednesday, 2026-11-04 at 12:05:00
            // Course 1 departs 12:15 (upcoming in 10 min), Course 2 departs 12:30 (upcoming in 25 min)
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 4, 12, 5, 0);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 15, 0), cartelera.Proximo.HorarioSalida);
            Assert.Equal("1°", cartelera.Proximo.Grado);
            _mockCursoRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCarteleraAsync_Lunes_12_05_Curso1_Ya_Salio_Curso2_Es_Proximo()
        {
            // Monday, 2026-11-02 at 12:05:00
            // Course 1 departed at 12:00 (past). Course 2 departs at 12:30 (upcoming).
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 2, 12, 5, 0);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(2, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 30, 0), cartelera.Proximo.HorarioSalida);
            Assert.Equal("2°", cartelera.Proximo.Grado);
        }

        [Fact]
        public async Task GetCarteleraAsync_Viernes_11_20_Curso1_Es_Proximo_Antes_De_Curso2()
        {
            // Friday, 2026-11-06 at 11:20:00
            // Course 1 departs at 11:30 (upcoming in 10 min), Course 2 departs at 12:30 (upcoming in 70 min)
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 6, 11, 20, 0);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(11, 30, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Viernes_11_35_Curso1_Ya_Salio_Curso2_Es_Proximo()
        {
            // Friday, 2026-11-06 at 11:35:00
            // Course 1 departed at 11:30 (past). Course 2 departs at 12:30 (upcoming).
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 6, 11, 35, 0);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(2, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 30, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Miercoles_Exactamente_En_Horario_12_15_Incluye_Curso1()
        {
            // Boundary condition: exactly at 12:15:00 on Wednesday
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 4, 12, 15, 0);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 15, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Miercoles_Un_Segundo_Despues_12_15_01_Pasa_A_Curso2()
        {
            // Boundary condition: 1 second past Course 1 departure
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var momento = new DateTime(2026, 11, 4, 12, 15, 1);

            var cartelera = await _servicio.GetCarteleraAsync(momento);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(2, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 30, 0), cartelera.Proximo.HorarioSalida);
        }

        #endregion

        #region 2. Weekend Queries & Resilience

        [Fact]
        public async Task GetCarteleraAsync_Sabado_Manana_Evalua_Horario_Lunes_Y_No_Falla()
        {
            // Saturday, 2026-11-07 at 10:00:00 (Evaluates Monday schedules: Course 1 = 12:00, Course 2 = 12:30)
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var sabadoManana = new DateTime(2026, 11, 7, 10, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(sabadoManana);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 0, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Sabado_Noche_Despues_De_Horarios_Recurre_A_Primer_Curso_Lunes()
        {
            // Saturday, 2026-11-07 at 21:00:00 (Past all Monday schedules 12:00 and 12:30)
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var sabadoNoche = new DateTime(2026, 11, 7, 21, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(sabadoNoche);

            // Fallback activates: must return the earliest Monday course without throwing or returning null
            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 0, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Domingo_Tarde_Evalua_Horario_Lunes_Sin_Excepcion()
        {
            // Sunday, 2026-11-08 at 16:00:00
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var domingoTarde = new DateTime(2026, 11, 8, 16, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(domingoTarde);

            Assert.NotNull(cartelera.Proximo);
            Assert.Equal(1, cartelera.Proximo.IdCurso);
            Assert.Equal(new TimeSpan(12, 0, 0), cartelera.Proximo.HorarioSalida);
        }

        [Fact]
        public async Task GetCarteleraAsync_Fin_De_Semana_Sin_Cursos_Retorna_Proximo_Null_Sin_Excepcion()
        {
            // Saturday with empty repository
            SetupCarteleraBase(new List<CursoEscolar>());

            var sabado = new DateTime(2026, 11, 7, 12, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(sabado);

            Assert.NotNull(cartelera);
            Assert.Null(cartelera.Proximo);
            Assert.Empty(cartelera.EnSalida);
        }

        #endregion

        #region 3. After-Hours Weekday Queries & Departure Tracking

        [Fact]
        public async Task GetCarteleraAsync_DiaHabil_22_00_Todos_Cursos_Salieron_Retorna_Proximo_Null()
        {
            // Monday at 22:00:00 (All departures occurred at 12:00 and 12:30)
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            SetupCarteleraBase(new List<CursoEscolar> { curso1, curso2 });

            var lunes2200 = new DateTime(2026, 11, 2, 22, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(lunes2200);

            Assert.NotNull(cartelera);
            Assert.Null(cartelera.Proximo);
        }

        [Fact]
        public async Task GetCarteleraAsync_DiaHabil_22_00_Preserva_Cursos_En_Salida()
        {
            // Monday at 22:00:00, course 1 has an active departure at the door
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero("Puerta Norte");
            var salidaActiva = new SalidaDeCurso(curso1, portero, new DateTime(2026, 11, 2, 21, 55, 0));
            var alumnos = new List<Alumno>
            {
                new Alumno(1, "Ana", "Gomez", 1),
                new Alumno(2, "Luis", "Perez", 1),
                new Alumno(3, "Sofia", "Diaz", 2)
            };

            SetupCarteleraBase(new List<CursoEscolar> { curso1 }, new List<SalidaDeCurso> { salidaActiva }, alumnos);

            var lunes2200 = new DateTime(2026, 11, 2, 22, 0, 0);

            var cartelera = await _servicio.GetCarteleraAsync(lunes2200);

            Assert.Null(cartelera.Proximo);
            var enSalida = Assert.Single(cartelera.EnSalida);
            Assert.Equal("1°", enSalida.Grado);
            Assert.Equal("A", enSalida.Curso);
            Assert.Equal("Puerta Norte", enSalida.Puerta);
            Assert.Equal(2, enSalida.CantidadDeAlumnos);
        }

        [Fact]
        public async Task GetDelDiaAsync_Consulta_Nocturna_Retorna_Todas_Las_Salidas_Del_Dia()
        {
            // Query departures recorded for Monday
            var curso1 = CrearCurso1Diferenciado();
            var curso2 = CrearCurso2Uniforme();
            var portero = CrearPortero();

            var fechaLunes = new DateTime(2026, 11, 2);
            var salida1 = new SalidaDeCurso(curso1, portero, new DateTime(2026, 11, 2, 12, 2, 0));
            var salida2 = new SalidaDeCurso(curso2, portero, new DateTime(2026, 11, 2, 12, 31, 0));

            _mockSalidaRepo.Setup(r => r.GetDelDiaAsync(fechaLunes))
                .ReturnsAsync(new List<SalidaDeCurso> { salida1, salida2 });

            _mockAlumnoRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Alumno>());

            var salidas = (await _servicio.GetDelDiaAsync(fechaLunes)).ToList();

            Assert.Equal(2, salidas.Count);
            Assert.Contains(salidas, s => s.IdCurso == 1 && s.HorarioSalida == new TimeSpan(12, 0, 0));
            Assert.Contains(salidas, s => s.IdCurso == 2 && s.HorarioSalida == new TimeSpan(12, 30, 0));
            _mockSalidaRepo.Verify(r => r.GetDelDiaAsync(fechaLunes), Times.Once);
        }

        #endregion

        #region 4. Departure Registration & Historical Schedule Mapping

        [Fact]
        public async Task GetDelDiaAsync_Mapea_HorarioSalida_Correspondiente_Al_Dia_De_La_Semana_Miercoles()
        {
            // Historical departure on Wednesday (2024-10-02 is Wednesday)
            // Course 1 has Wednesday departure at 12:15 (vs Mon 12:00, Fri 11:30)
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();
            var fechaMiercoles = new DateTime(2024, 10, 2);
            var salidaMiercoles = new SalidaDeCurso(curso1, portero, new DateTime(2024, 10, 2, 12, 14, 0));

            _mockSalidaRepo.Setup(r => r.GetDelDiaAsync(fechaMiercoles))
                .ReturnsAsync(new List<SalidaDeCurso> { salidaMiercoles });

            _mockAlumnoRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Alumno> { new Alumno(10, "Martin", "Lopez", 1) });

            var resultado = (await _servicio.GetDelDiaAsync(fechaMiercoles)).ToList();

            var dto = Assert.Single(resultado);
            Assert.Equal(new TimeSpan(12, 15, 0), dto.HorarioSalida);
            Assert.Equal(1, dto.CantidadDeAlumnos);
            Assert.Equal("Carlos Portero", dto.PersonalNombre);
        }

        [Fact]
        public async Task GetDelDiaAsync_Mapea_HorarioSalida_Correspondiente_Al_Dia_De_La_Semana_Viernes()
        {
            // Historical departure on Friday (2024-10-04 is Friday)
            // Course 1 has Friday departure at 11:30
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();
            var fechaViernes = new DateTime(2024, 10, 4);
            var salidaViernes = new SalidaDeCurso(curso1, portero, new DateTime(2024, 10, 4, 11, 28, 0));

            _mockSalidaRepo.Setup(r => r.GetDelDiaAsync(fechaViernes))
                .ReturnsAsync(new List<SalidaDeCurso> { salidaViernes });

            _mockAlumnoRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Alumno>());

            var resultado = (await _servicio.GetDelDiaAsync(fechaViernes)).ToList();

            var dto = Assert.Single(resultado);
            Assert.Equal(new TimeSpan(11, 30, 0), dto.HorarioSalida);
        }

        [Fact]
        public async Task IniciarAsync_Invoca_AddAsync_Y_Mapea_HorarioSalida_Del_Dia_Actual()
        {
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();

            _mockCursoRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(curso1);
            _mockSalidaRepo.Setup(r => r.GetEnCursoAsync()).ReturnsAsync(new List<SalidaDeCurso>());
            _mockSalidaRepo.Setup(r => r.AddAsync(It.IsAny<SalidaDeCurso>())).Returns(Task.CompletedTask);
            _mockAlumnoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Alumno>());

            var resultado = await _servicio.IniciarAsync(1, portero);

            Assert.True(resultado.Exito);
            Assert.NotNull(resultado.Salida);
            var horarioEsperado = curso1.ObtenerHorarioSalida(DateTime.Now.DayOfWeek);
            Assert.Equal(horarioEsperado, resultado.Salida.HorarioSalida);

            _mockSalidaRepo.Verify(r => r.AddAsync(It.Is<SalidaDeCurso>(s => s.IdCurso == 1 && s.Personal == portero)), Times.Once);
        }

        #endregion

        #region 5. Moq Interaction Verification & Business Guard Clauses

        [Fact]
        public async Task IniciarAsync_CursoNoExiste_NoLlamaAddAsync_Y_RetornaFalso()
        {
            _mockCursoRepo.Setup(r => r.GetAsync(999)).ReturnsAsync((CursoEscolar?)null);

            var resultado = await _servicio.IniciarAsync(999, CrearPortero());

            Assert.False(resultado.Exito);
            Assert.Contains("no existe", resultado.Mensaje);
            Assert.Null(resultado.Salida);
            _mockSalidaRepo.Verify(r => r.AddAsync(It.IsAny<SalidaDeCurso>()), Times.Never);
        }

        [Fact]
        public async Task IniciarAsync_CursoYaEnSalida_NoLlamaAddAsync_Y_RetornaFalso()
        {
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();
            var salidaEnCurso = new SalidaDeCurso(curso1, portero, DateTime.Now.AddMinutes(-5));

            _mockCursoRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(curso1);
            _mockSalidaRepo.Setup(r => r.GetEnCursoAsync()).ReturnsAsync(new List<SalidaDeCurso> { salidaEnCurso });

            var resultado = await _servicio.IniciarAsync(1, portero);

            Assert.False(resultado.Exito);
            Assert.Contains("ya está saliendo", resultado.Mensaje);
            Assert.Null(resultado.Salida);
            _mockSalidaRepo.Verify(r => r.AddAsync(It.IsAny<SalidaDeCurso>()), Times.Never);
        }

        [Fact]
        public async Task FinalizarAsync_SalidaExistente_InvocaUpdateAsync_Y_RetornaExito()
        {
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();
            var salida = new SalidaDeCurso(curso1, portero, DateTime.Now.AddMinutes(-10));
            salida.SetIdSalidaDeCurso(42);

            _mockSalidaRepo.Setup(r => r.GetAsync(42)).ReturnsAsync(salida);
            _mockSalidaRepo.Setup(r => r.UpdateAsync(It.IsAny<SalidaDeCurso>())).ReturnsAsync(true);

            var resultado = await _servicio.FinalizarAsync(42);

            Assert.True(resultado.Exito);
            Assert.False(salida.EstaEnCurso);
            Assert.NotNull(salida.FechaHoraFin);
            _mockSalidaRepo.Verify(r => r.UpdateAsync(It.Is<SalidaDeCurso>(s => s.IdSalidaDeCurso == 42 && !s.EstaEnCurso)), Times.Once);
        }

        [Fact]
        public async Task FinalizarAsync_SalidaNoExiste_NoLlamaUpdateAsync_Y_RetornaFalso()
        {
            _mockSalidaRepo.Setup(r => r.GetAsync(999)).ReturnsAsync((SalidaDeCurso?)null);

            var resultado = await _servicio.FinalizarAsync(999);

            Assert.False(resultado.Exito);
            Assert.Contains("no existe", resultado.Mensaje);
            _mockSalidaRepo.Verify(r => r.UpdateAsync(It.IsAny<SalidaDeCurso>()), Times.Never);
        }

        [Fact]
        public async Task FinalizarAsync_SalidaYaFinalizada_NoLlamaUpdateAsync_Y_RetornaFalso()
        {
            var curso1 = CrearCurso1Diferenciado();
            var portero = CrearPortero();
            var salida = new SalidaDeCurso(curso1, portero, DateTime.Now.AddMinutes(-20));
            salida.SetIdSalidaDeCurso(15);
            salida.Finalizar(DateTime.Now.AddMinutes(-10)); // Already finalized

            _mockSalidaRepo.Setup(r => r.GetAsync(15)).ReturnsAsync(salida);

            var resultado = await _servicio.FinalizarAsync(15);

            Assert.False(resultado.Exito);
            Assert.Contains("ya estaba finalizada", resultado.Mensaje);
            _mockSalidaRepo.Verify(r => r.UpdateAsync(It.IsAny<SalidaDeCurso>()), Times.Never);
        }

        #endregion
    }
}
