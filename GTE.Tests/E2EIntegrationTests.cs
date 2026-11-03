using System;
using System.Linq;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Dominio;
using GTE.DTOs;
using Xunit;

namespace GTE.Tests;

/// <summary>
/// End-to-End integration tests validating full multi-tier workflows across Domain,
/// Application Services, Cartelera calculation, and Departure lifecycle (Tiers 3 & 4).
/// </summary>
public class E2EIntegrationTests
{
    [Fact]
    [Trait("Category", "Tier4")]
    [Trait("Scenario", "S3_DailySchoolOperationsProgression")]
    public async Task Escenario_S3_Ciclo_completo_de_operacion_escolar_y_cartelera()
    {
        // Arrange: Configurar cursos con horarios y alumnos correspondientes
        var cursoPrimero = new CursoEscolar(1, "1°", "A", "Noche", new TimeSpan(23, 40, 0));
        var cursoSegundo = new CursoEscolar(2, "2°", "A", "Noche", new TimeSpan(23, 50, 0));
        var cursoTercero = new CursoEscolar(3, "3°", "A", "Noche", new TimeSpan(23, 59, 0));

        var repoCursos = new CursoEscolarRepositoryFalso(cursoPrimero, cursoSegundo, cursoTercero);
        var repoAlumnos = new AlumnoRepositoryFalso(
            new Alumno(1, "Sofia", "Perez", 1),
            new Alumno(2, "Lucas", "Gomez", 1),
            new Alumno(3, "Mateo", "Lopez", 2),
            new Alumno(4, "Valentina", "Diaz", 3));

        var repoSalidas = new SalidaDeCursoRepositoryFalso();
        var servicio = new SalidaDeCursoService(repoSalidas, repoCursos, repoAlumnos);
        var portero = new Portero("Carlos Guardia", "Puerta Principal", new Usuario("porteria", "pass123"));

        // Step 1: Cartelera inicial antes de cualquier salida
        var carteleraInicial = await servicio.GetCarteleraAsync();
        Assert.Empty(carteleraInicial.EnSalida);
        Assert.NotNull(carteleraInicial.Proximo);

        // Step 2: Se inicia la salida del primer curso (1° A)
        var inicio1 = await servicio.IniciarAsync(1, portero);
        Assert.True(inicio1.Exito);
        Assert.NotNull(inicio1.Salida);

        // Step 3: La cartelera refleja 1° A en salida activa con 2 alumnos
        var carteleraPaso1 = await servicio.GetCarteleraAsync();
        var enSalida = Assert.Single(carteleraPaso1.EnSalida);
        Assert.Equal("1°", enSalida.Grado);
        Assert.Equal("A", enSalida.Curso);
        Assert.Equal("Puerta Principal", enSalida.Puerta);
        Assert.Equal(2, enSalida.CantidadDeAlumnos);

        // Step 4: Finalizar la salida de 1° A
        var fin1 = await servicio.FinalizarAsync(inicio1.Salida!.IdSalidaDeCurso);
        Assert.True(fin1.Exito);

        // Step 5: Cartelera ya no tiene cursos en salida
        var carteleraPaso2 = await servicio.GetCarteleraAsync();
        Assert.Empty(carteleraPaso2.EnSalida);
    }

    [Fact]
    [Trait("Category", "Tier4")]
    [Trait("Scenario", "S4_MultiDoorConcurrentExitRush")]
    public async Task Escenario_S4_Salidas_concurrentes_en_multiples_puertas()
    {
        // Arrange: 2 cursos saliendo simultáneamente por puertas distintas
        var curso6A = new CursoEscolar(6, "6°", "A", "Tarde", new TimeSpan(17, 30, 0));
        var curso7A = new CursoEscolar(7, "7°", "A", "Tarde", new TimeSpan(17, 30, 0));

        var repoCursos = new CursoEscolarRepositoryFalso(curso6A, curso7A);
        var repoAlumnos = new AlumnoRepositoryFalso(
            new Alumno(10, "Juan", "Romero", 6),
            new Alumno(11, "Ana", "Benitez", 7),
            new Alumno(12, "Pedro", "Alvarez", 7));

        var repoSalidas = new SalidaDeCursoRepositoryFalso();
        var servicio = new SalidaDeCursoService(repoSalidas, repoCursos, repoAlumnos);

        var porteroPuertaPrincipal = new Portero("Renzo Scollo", "Puerta Principal", new Usuario("p1", "pass"));
        var porteroPuertaSecundaria = new Portero("Maria Ruiz", "Puerta Secundaria", new Usuario("p2", "pass"));

        // Act: Iniciar ambas salidas concurrentemente
        var inicio6A = await servicio.IniciarAsync(6, porteroPuertaPrincipal);
        var inicio7A = await servicio.IniciarAsync(7, porteroPuertaSecundaria);

        Assert.True(inicio6A.Exito);
        Assert.True(inicio7A.Exito);

        // Assert: Cartelera muestra ambos cursos en salida simultánea
        var cartelera = await servicio.GetCarteleraAsync();
        Assert.Equal(2, cartelera.EnSalida.Count);

        var salidaPpal = cartelera.EnSalida.FirstOrDefault(s => s.Puerta == "Puerta Principal");
        var salidaSec = cartelera.EnSalida.FirstOrDefault(s => s.Puerta == "Puerta Secundaria");

        Assert.NotNull(salidaPpal);
        Assert.Equal("6°", salidaPpal!.Grado);
        Assert.Equal(1, salidaPpal.CantidadDeAlumnos);

        Assert.NotNull(salidaSec);
        Assert.Equal("7°", salidaSec!.Grado);
        Assert.Equal(2, salidaSec.CantidadDeAlumnos);

        // Act & Assert: Finalizar una no afecta a la otra
        var fin6A = await servicio.FinalizarAsync(inicio6A.Salida!.IdSalidaDeCurso);
        Assert.True(fin6A.Exito);

        var carteleraIntermedia = await servicio.GetCarteleraAsync();
        var salidaRestante = Assert.Single(carteleraIntermedia.EnSalida);
        Assert.Equal("7°", salidaRestante.Grado);

        // Finalizar la restante
        var fin7A = await servicio.FinalizarAsync(inicio7A.Salida!.IdSalidaDeCurso);
        Assert.True(fin7A.Exito);

        var carteleraFinal = await servicio.GetCarteleraAsync();
        Assert.Empty(carteleraFinal.EnSalida);
    }

    [Fact]
    [Trait("Category", "Tier2")]
    [Trait("Feature", "F4")]
    public async Task Cartelera_con_repositorio_vacio_retorna_objeto_valido_sin_excepcion()
    {
        // Boundary case: Repositorio sin cursos registrados
        var servicio = new SalidaDeCursoService(
            new SalidaDeCursoRepositoryFalso(),
            new CursoEscolarRepositoryFalso(),
            new AlumnoRepositoryFalso());

        var cartelera = await servicio.GetCarteleraAsync();

        Assert.NotNull(cartelera);
        Assert.Empty(cartelera.EnSalida);
        Assert.Null(cartelera.Proximo);
    }

    [Fact]
    [Trait("Category", "Tier2")]
    [Trait("Feature", "F4")]
    public async Task Iniciar_salida_de_curso_inexistente_retorna_error_controlado()
    {
        // Corner case: Curso inexistente (-1 o 9999)
        var servicio = new SalidaDeCursoService(
            new SalidaDeCursoRepositoryFalso(),
            new CursoEscolarRepositoryFalso(),
            new AlumnoRepositoryFalso());

        var portero = new Portero("Portero", "Puerta 1", new Usuario("port", "pass"));
        var resultado = await servicio.IniciarAsync(9999, portero);

        Assert.False(resultado.Exito);
        Assert.Contains("no existe", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    [Trait("Category", "Tier3")]
    [Trait("Interface", "F4xF2")]
    public async Task Multiples_salidas_finalizadas_se_registran_en_el_historico()
    {
        // Pairwise integration: SalidaDeCursoService <-> SalidaDeCursoRepository
        var curso = new CursoEscolar(1, "5°", "A", "Tarde", new TimeSpan(17, 30, 0));
        var repoCursos = new CursoEscolarRepositoryFalso(curso);
        var repoAlumnos = new AlumnoRepositoryFalso(new Alumno(1, "Ana", "Gomez", 1));
        var repoSalidas = new SalidaDeCursoRepositoryFalso();
        var servicio = new SalidaDeCursoService(repoSalidas, repoCursos, repoAlumnos);
        var portero = new Portero("Portero", "Puerta 1", new Usuario("port", "pass"));

        // Ciclo 1
        var salida1 = await servicio.IniciarAsync(1, portero);
        await servicio.FinalizarAsync(salida1.Salida!.IdSalidaDeCurso);

        // Ciclo 2
        var salida2 = await servicio.IniciarAsync(1, portero);
        await servicio.FinalizarAsync(salida2.Salida!.IdSalidaDeCurso);

        var todas = (await servicio.GetDelDiaAsync(DateTime.Today)).ToList();
        Assert.Equal(2, todas.Count);
        Assert.All(todas, s => Assert.NotNull(s.FechaHoraFin));
    }

    [Fact]
    [Trait("Category", "Tier4")]
    [Trait("Scenario", "S1_AcademicYearInitializationAndCourseSetup")]
    public async Task Escenario_S1_Inicializacion_y_creacion_de_curso_escolar_en_repositorio()
    {
        // Arrange: Servicio de gestión de cursos
        var repoCursos = new CursoEscolarRepositoryFalso();
        var servicioCursos = new CursoEscolarService(repoCursos);

        var nuevoCursoDto = new CursoEscolarDTO
        {
            IdCurso = 0,
            Grado = "1°",
            Curso = "A",
            Turno = "Mañana",
            HorarioSalida = new TimeSpan(12, 0, 0)
        };

        // Act: El secretario guarda el nuevo curso
        var creado = await servicioCursos.AddAsync(nuevoCursoDto);

        // Assert: El curso se persiste correctamente y se recupera
        Assert.NotNull(creado);
        var todosLosCursos = (await servicioCursos.GetAllAsync()).ToList();
        var recuperado = Assert.Single(todosLosCursos);

        Assert.Equal("1°", recuperado.Grado);
        Assert.Equal("A", recuperado.Curso);
        Assert.Equal("Mañana", recuperado.Turno);
        Assert.Equal(new TimeSpan(12, 0, 0), recuperado.HorarioSalida);
    }

    [Fact]
    [Trait("Category", "Tier4")]
    [Trait("Scenario", "S2_MidWeekCurriculumAdjustment")]
    public async Task Escenario_S2_Ajuste_de_horario_y_actualizacion_en_cartelera()
    {
        // Arrange: Curso configurado inicialmente para salir a las 23:59
        var curso = new CursoEscolar(1, "5°", "B", "Noche", new TimeSpan(23, 59, 0));
        var repoCursos = new CursoEscolarRepositoryFalso(curso);
        var servicioCursos = new CursoEscolarService(repoCursos);
        var servicioSalidas = new SalidaDeCursoService(
            new SalidaDeCursoRepositoryFalso(),
            repoCursos,
            new AlumnoRepositoryFalso());

        // Verificación inicial: Cartelera muestra horario original
        var carteleraInicial = await servicioSalidas.GetCarteleraAsync();
        Assert.NotNull(carteleraInicial.Proximo);
        Assert.Equal(new TimeSpan(23, 59, 0), carteleraInicial.Proximo!.HorarioSalida);

        // Act: Ajuste de horario por nuevo taller (adelanto a las 23:50)
        var dtoModificado = new CursoEscolarDTO
        {
            IdCurso = 1,
            Grado = "5°",
            Curso = "B",
            Turno = "Noche",
            HorarioSalida = new TimeSpan(23, 50, 0)
        };
        var modificadoExitosamente = await servicioCursos.UpdateAsync(dtoModificado);
        Assert.True(modificadoExitosamente);

        // Assert: Cartelera refleja inmediatamente el nuevo horario de salida
        var carteleraActualizada = await servicioSalidas.GetCarteleraAsync();
        Assert.NotNull(carteleraActualizada.Proximo);
        Assert.Equal(new TimeSpan(23, 50, 0), carteleraActualizada.Proximo!.HorarioSalida);
    }

    [Fact]
    [Trait("Category", "Tier4")]
    [Trait("Scenario", "S6_WeekendOffHoursStability")]
    public async Task Escenario_S6_Cartelera_estable_y_segura_fuera_de_horario_de_clases()
    {
        // Arrange: Cursos con horarios ya pasados en el día
        var cursoManana = new CursoEscolar(1, "1°", "A", "Mañana", TimeSpan.Zero);
        var repoCursos = new CursoEscolarRepositoryFalso(cursoManana);
        var servicio = new SalidaDeCursoService(
            new SalidaDeCursoRepositoryFalso(),
            repoCursos,
            new AlumnoRepositoryFalso());

        // Act: Consulta de cartelera en horario posterior al cierre escolar
        var cartelera = await servicio.GetCarteleraAsync();

        // Assert: Estado estable sin excepción; Proximo es null al haber concluido las salidas
        Assert.NotNull(cartelera);
        Assert.Empty(cartelera.EnSalida);
        Assert.Null(cartelera.Proximo);
    }
}
