using GTE.Application.Services;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Tests;

public class ReporteServiceTests
{
    private static Alumno UnAlumno(int id, int idCurso) =>
        new(id, $"Nombre{id}", $"Apellido{id}", idCurso);

    private static CursoEscolar UnCurso(int id, string grado, string division, string turno) =>
        new(id, grado, division, turno, new TimeSpan(12, 0, 0));

    private static ReporteService Servicio(
        IEnumerable<Alumno>? alumnos = null,
        IEnumerable<CursoEscolar>? cursos = null,
        IEnumerable<RetiroDTO>? retiros = null)
    {
        return new ReporteService(
            new AlumnoRepositoryFalso(alumnos?.ToArray() ?? Array.Empty<Alumno>()),
            new CursoEscolarRepositoryFalso(cursos?.ToArray() ?? Array.Empty<CursoEscolar>()),
            new RetiroServiceConDatos(retiros?.ToArray() ?? Array.Empty<RetiroDTO>()));
    }

    private static RetiroDTO UnRetiro(int id, DateTime fecha) =>
        new() { IdRetiro = id, FechaHora = fecha, Observaciones = $"Retiro {id}" };

    [Fact]
    public async Task Cuenta_los_alumnos_de_cada_curso()
    {
        var servicio = Servicio(
            alumnos: new[] { UnAlumno(1, 1), UnAlumno(2, 1), UnAlumno(3, 2) },
            cursos: new[] { UnCurso(1, "1°", "A", "Mañana"), UnCurso(2, "2°", "B", "Tarde") });

        var resultado = (await servicio.GetAlumnosPorCursoAsync()).ToList();

        Assert.Equal(2, resultado.Single(r => r.IdCurso == 1).Cantidad);
        Assert.Equal(1, resultado.Single(r => r.IdCurso == 2).Cantidad);
    }

    [Fact]
    public async Task Incluye_los_cursos_sin_alumnos_con_cantidad_cero()
    {
        var servicio = Servicio(
            alumnos: new[] { UnAlumno(1, 1) },
            cursos: new[] { UnCurso(1, "1°", "A", "Mañana"), UnCurso(2, "2°", "B", "Tarde") });

        var resultado = (await servicio.GetAlumnosPorCursoAsync()).ToList();

        Assert.Equal(0, resultado.Single(r => r.IdCurso == 2).Cantidad);
    }

    [Fact]
    public async Task El_reporte_de_alumnos_trae_los_datos_del_curso()
    {
        var servicio = Servicio(
            cursos: new[] { UnCurso(7, "3°", "C", "Noche") });

        var fila = (await servicio.GetAlumnosPorCursoAsync()).Single();

        Assert.Equal("3°", fila.Grado);
        Assert.Equal("C", fila.Curso);
        Assert.Equal("Noche", fila.Turno);
    }

    [Fact]
    public async Task Ordena_por_turno_grado_y_division()
    {
        var servicio = Servicio(
            cursos: new[]
            {
                UnCurso(1, "3°", "A", "Noche"),
                UnCurso(2, "1°", "B", "Mañana"),
                UnCurso(3, "2°", "A", "Tarde"),
                UnCurso(4, "1°", "A", "Mañana"),
            });

        var orden = (await servicio.GetAlumnosPorCursoAsync())
            .Select(r => $"{r.Turno} {r.Grado} {r.Curso}")
            .ToList();

        Assert.Equal(
            new[] { "Mañana 1° A", "Mañana 1° B", "Tarde 2° A", "Noche 3° A" },
            orden);
    }

    [Fact]
    public async Task Sin_fechas_devuelve_todos_los_retiros()
    {
        var servicio = Servicio(retiros: new[]
        {
            UnRetiro(1, new DateTime(2026, 3, 1)),
            UnRetiro(2, new DateTime(2026, 6, 15)),
        });

        var resultado = await servicio.GetRetirosAsync(desde: null, hasta: null);

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task Excluye_los_retiros_anteriores_a_la_fecha_desde()
    {
        var servicio = Servicio(retiros: new[]
        {
            UnRetiro(1, new DateTime(2026, 3, 1)),
            UnRetiro(2, new DateTime(2026, 6, 15)),
        });

        var resultado = await servicio.GetRetirosAsync(new DateTime(2026, 6, 1), hasta: null);

        Assert.Equal(new[] { 2 }, resultado.Select(r => r.IdRetiro));
    }

    [Fact]
    public async Task Excluye_los_retiros_posteriores_a_la_fecha_hasta()
    {
        var servicio = Servicio(retiros: new[]
        {
            UnRetiro(1, new DateTime(2026, 3, 1)),
            UnRetiro(2, new DateTime(2026, 6, 15)),
        });

        var resultado = await servicio.GetRetirosAsync(desde: null, new DateTime(2026, 3, 31));

        Assert.Equal(new[] { 1 }, resultado.Select(r => r.IdRetiro));
    }

    [Fact]
    public async Task El_rango_incluye_los_dias_de_los_extremos()
    {
        var servicio = Servicio(retiros: new[]
        {
            UnRetiro(1, new DateTime(2026, 3, 1, 8, 30, 0)),
            UnRetiro(2, new DateTime(2026, 3, 31, 17, 45, 0)),
            UnRetiro(3, new DateTime(2026, 4, 1)),
        });

        var resultado = await servicio.GetRetirosAsync(
            new DateTime(2026, 3, 1), new DateTime(2026, 3, 31));

        Assert.Equal(new[] { 1, 2 }, resultado.Select(r => r.IdRetiro));
    }
}
