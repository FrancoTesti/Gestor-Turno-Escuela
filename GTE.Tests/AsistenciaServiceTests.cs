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

namespace GTE.Tests;

public class AsistenciaServiceTests
{
    // Lunes 02/11/2026; el curso sale a las 12:00 todos los días.
    private static readonly DateTime Lunes = new(2026, 11, 2);
    private const int IdCurso = 1;

    private readonly Mock<IAsistenciaRepository> repoAsistencia = new();
    private readonly Mock<IAlumnoRepository> repoAlumnos = new();
    private readonly Mock<ICursoEscolarRepository> repoCursos = new();
    private readonly Mock<IRetiroRepository> repoRetiros = new();

    public AsistenciaServiceTests()
    {
        repoCursos.Setup(r => r.GetAsync(IdCurso))
            .ReturnsAsync(new CursoEscolar(IdCurso, "1°", "A", "Mañana", new TimeSpan(12, 0, 0)));

        repoAlumnos.Setup(r => r.GetAllAsync()).ReturnsAsync(new[]
        {
            new Alumno(1, "Sofía", "Perez", IdCurso),
            new Alumno(2, "Mateo", "Gomez", IdCurso),
            new Alumno(3, "Lucas", "Diaz", IdCurso)
        });

        repoAsistencia.Setup(r => r.GetTiposInasistenciaAsync(It.IsAny<bool>())).ReturnsAsync(new[]
        {
            new TipoInasistencia(1, "Completa", 1.0m, "A"),
            new TipoInasistencia(2, "Media", 0.5m, "M"),
            new TipoInasistencia(3, "Cuarta", 0.25m, "C"),
            new TipoInasistencia(4, "Tres Cuartas", 0.75m, "TC")
        });

        repoAsistencia.Setup(r => r.GetInasistenciasPorCursoYFechaAsync(IdCurso, It.IsAny<DateTime>()))
            .ReturnsAsync(Array.Empty<InasistenciaAlumno>());
        repoAsistencia.Setup(r => r.GetRetirosPorCursoYFechaAsync(IdCurso, It.IsAny<DateTime>()))
            .ReturnsAsync(Array.Empty<DetalleRetiro>());
        repoRetiros.Setup(r => r.GetAllAsync()).ReturnsAsync(Array.Empty<Retiro>());
    }

    private AsistenciaService Servicio() =>
        new(repoAsistencia.Object, repoAlumnos.Object, repoCursos.Object, repoRetiros.Object);

    private void ConRetiro(int idAlumno, TimeSpan hora) =>
        repoAsistencia.Setup(r => r.GetRetirosPorCursoYFechaAsync(IdCurso, Lunes))
            .ReturnsAsync(new[] { new DetalleRetiro(1, 10, idAlumno, hora, "Retirado") });

    [Fact]
    public async Task Planilla_marca_presentes_por_defecto_y_respeta_las_inasistencias_cargadas()
    {
        repoAsistencia.Setup(r => r.GetInasistenciasPorCursoYFechaAsync(IdCurso, Lunes))
            .ReturnsAsync(new[] { new InasistenciaAlumno(2, Lunes, 1, justificada: true, motivo: "Enfermedad") });

        var planilla = await Servicio().GetPlanillaDiariaAsync(IdCurso, Lunes);

        Assert.Equal(3, planilla.Alumnos.Count);
        var mateo = planilla.Alumnos.Single(a => a.IdAlumno == 2);
        Assert.Equal("A", mateo.CodigoOficial);
        Assert.True(mateo.Justificada);
        Assert.All(planilla.Alumnos.Where(a => a.IdAlumno != 2), a => Assert.Equal("P", a.CodigoOficial));
    }

    [Theory]
    [InlineData(9, 0, "M")]   // quedan 3 h de clase -> media falta
    [InlineData(11, 0, "C")]  // queda 1 h -> cuarta falta
    [InlineData(12, 0, "P")]  // se fue en horario -> presente
    public async Task Sugerencia_por_retiro_depende_de_cuanto_falta_para_la_salida(int hora, int minuto, string codigoEsperado)
    {
        ConRetiro(1, new TimeSpan(hora, minuto, 0));

        var sugerencias = await Servicio().SugerirInasistenciasDesdeRetirosAsync(IdCurso, Lunes);

        var sofia = sugerencias.Single(s => s.IdAlumno == 1);
        Assert.Equal(codigoEsperado, sofia.CodigoOficial);
        Assert.True(sofia.TieneRetiroRegistrado);
        if (codigoEsperado != "P")
            Assert.True(sofia.Justificada);
    }

    [Fact]
    public async Task Sugerencia_deja_presentes_a_los_alumnos_sin_retiro()
    {
        ConRetiro(1, new TimeSpan(9, 0, 0));

        var sugerencias = await Servicio().SugerirInasistenciasDesdeRetirosAsync(IdCurso, Lunes);

        Assert.All(sugerencias.Where(s => s.IdAlumno != 1), s =>
        {
            Assert.Equal("P", s.CodigoOficial);
            Assert.False(s.TieneRetiroRegistrado);
        });
    }

    [Fact]
    public async Task Guardar_confirma_la_planilla_y_persiste_solo_inasistencias_validas()
    {
        PlanillaAsistenciaDiaria? planillaGuardada = null;
        List<InasistenciaAlumno>? guardadas = null;
        repoAsistencia
            .Setup(r => r.GuardarPlanillaConInasistenciasAsync(It.IsAny<PlanillaAsistenciaDiaria>(), It.IsAny<IEnumerable<InasistenciaAlumno>>()))
            .Callback<PlanillaAsistenciaDiaria, IEnumerable<InasistenciaAlumno>>((p, i) => { planillaGuardada = p; guardadas = i.ToList(); })
            .Returns(Task.CompletedTask);

        var request = new GuardarPlanillaAsistenciaRequestDTO
        {
            IdCurso = IdCurso,
            Fecha = Lunes.AddHours(10),
            Inasistencias =
            {
                new InasistenciaInputDTO { IdAlumno = 1, IdTipoInasistencia = 1 },
                new InasistenciaInputDTO { IdAlumno = 1, IdTipoInasistencia = 2 }, // la última gana
                new InasistenciaInputDTO { IdAlumno = 0, IdTipoInasistencia = 1 }  // inválida
            }
        };

        var (exito, _) = await Servicio().GuardarPlanillaDiariaAsync(request);

        Assert.True(exito);
        Assert.NotNull(planillaGuardada);
        Assert.True(planillaGuardada!.Confirmada);
        Assert.Equal(Lunes, planillaGuardada.Fecha);
        var unica = Assert.Single(guardadas!);
        Assert.Equal(2, unica.IdTipoInasistencia);
    }

    [Fact]
    public async Task Espejo_semanal_usa_codigos_oficiales_y_no_suma_las_que_no_computan()
    {
        repoAsistencia.Setup(r => r.GetInasistenciasPorCursoYRangoAsync(IdCurso, Lunes, Lunes.AddDays(4)))
            .ReturnsAsync(new[]
            {
                new InasistenciaAlumno(1, Lunes, 2),                            // M
                new InasistenciaAlumno(1, Lunes.AddDays(1), 1, justificada: true), // J
                new InasistenciaAlumno(1, Lunes.AddDays(2), 1),                 // I
                new InasistenciaAlumno(1, Lunes.AddDays(3), 1, noComputa: true) // P (no computa)
            });

        // Se pide con un miércoles: debe normalizar al lunes de esa semana.
        var espejo = await Servicio().GetReporteEspejoSemanalAsync(IdCurso, Lunes.AddDays(2));

        Assert.Equal(Lunes, espejo.FechaLunes);
        var sofia = espejo.Filas.Single(f => f.IdAlumno == 1);
        Assert.Equal(new[] { "M", "J", "I", "P", "P" },
            new[] { sofia.Lunes, sofia.Martes, sofia.Miercoles, sofia.Jueves, sofia.Viernes });
        Assert.Equal(2.5m, sofia.TotalFaltasPeriodo);
    }

    [Fact]
    public async Task Novedades_solo_incluye_excepciones()
    {
        repoAsistencia.Setup(r => r.GetInasistenciasPorCursoYRangoAsync(IdCurso, Lunes, Lunes.AddDays(4)))
            .ReturnsAsync(new[] { new InasistenciaAlumno(3, Lunes, 1, motivo: "Sin aviso") });

        var novedades = await Servicio().GetReporteNovedadesAsync(IdCurso, Lunes, Lunes.AddDays(4));

        var unica = Assert.Single(novedades);
        Assert.Equal(3, unica.IdAlumno);
        Assert.Equal("Diaz, Lucas", unica.AlumnoNombreCompleto);
    }

    [Fact]
    public async Task Historial_separa_justificadas_injustificadas_y_excluye_las_que_no_computan()
    {
        repoAlumnos.Setup(r => r.GetAsync(1)).ReturnsAsync(new Alumno(1, "Sofía", "Perez", IdCurso));
        repoAsistencia.Setup(r => r.GetInasistenciasPorAlumnoAsync(1)).ReturnsAsync(new[]
        {
            new InasistenciaAlumno(1, Lunes, 1, justificada: true),
            new InasistenciaAlumno(1, Lunes.AddDays(1), 2),
            new InasistenciaAlumno(1, Lunes.AddDays(2), 1, noComputa: true)
        });

        var historial = await Servicio().GetHistorialAlumnoAsync(1);

        Assert.Equal(1.5m, historial.TotalFaltasComputables);
        Assert.Equal(1.0m, historial.TotalFaltasJustificadas);
        Assert.Equal(0.5m, historial.TotalFaltasInjustificadas);
        Assert.Equal(3, historial.Inasistencias.Count);
    }
}
