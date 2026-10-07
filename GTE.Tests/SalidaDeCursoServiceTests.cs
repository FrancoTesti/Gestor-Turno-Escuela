using System;
using System.Linq;
using System.Threading.Tasks;
using GTE.Application.Services;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Tests;

public class SalidaDeCursoServiceTests
{
    [Fact]
    public async Task Marcar_la_salida_deja_el_curso_en_la_cartelera()
    {
        var servicio = CrearServicio(
            new CursoEscolarRepositoryFalso(CursoDe(1, "4°", "B", "Tarde")),
            new AlumnoRepositoryFalso(AlumnoDe(1, 1), AlumnoDe(2, 1), AlumnoDe(3, 2)));

        var resultado = await servicio.IniciarAsync(1, PorteroDePrueba());

        Assert.True(resultado.Exito);

        var cartelera = await servicio.GetCarteleraAsync();
        var curso = Assert.Single(cartelera.EnSalida);

        Assert.Equal("4°", curso.Grado);
        Assert.Equal("B", curso.Curso);
        Assert.Equal("Tarde", curso.Turno);
        Assert.Equal("Puerta Principal", curso.Puerta);
        Assert.Equal(2, curso.CantidadDeAlumnos);
    }

    [Fact]
    public async Task No_se_puede_marcar_dos_veces_el_mismo_curso()
    {
        var servicio = CrearServicio(new CursoEscolarRepositoryFalso(CursoDe(1, "4°", "B", "Tarde")));

        var primera = await servicio.IniciarAsync(1, PorteroDePrueba());
        var segunda = await servicio.IniciarAsync(1, PorteroDePrueba());

        Assert.True(primera.Exito);
        Assert.False(segunda.Exito);
        Assert.Contains("ya está saliendo", segunda.Mensaje);
    }

    [Fact]
    public async Task Al_finalizar_el_curso_deja_de_estar_en_la_cartelera()
    {
        var servicio = CrearServicio(new CursoEscolarRepositoryFalso(CursoDe(1, "4°", "B", "Tarde")));

        var iniciada = await servicio.IniciarAsync(1, PorteroDePrueba());
        var finalizada = await servicio.FinalizarAsync(iniciada.Salida!.IdSalidaDeCurso);

        Assert.True(finalizada.Exito);

        var cartelera = await servicio.GetCarteleraAsync();
        Assert.Empty(cartelera.EnSalida);
    }

    [Fact]
    public async Task No_se_puede_finalizar_dos_veces_la_misma_salida()
    {
        var servicio = CrearServicio(new CursoEscolarRepositoryFalso(CursoDe(1, "4°", "B", "Tarde")));

        var iniciada = await servicio.IniciarAsync(1, PorteroDePrueba());
        await servicio.FinalizarAsync(iniciada.Salida!.IdSalidaDeCurso);
        var segunda = await servicio.FinalizarAsync(iniciada.Salida!.IdSalidaDeCurso);

        Assert.False(segunda.Exito);
        Assert.Contains("ya estaba finalizada", segunda.Mensaje);
    }

    [Fact]
    public async Task No_se_puede_marcar_la_salida_de_un_curso_que_no_existe()
    {
        var servicio = CrearServicio(new CursoEscolarRepositoryFalso());

        var resultado = await servicio.IniciarAsync(99, PorteroDePrueba());

        Assert.False(resultado.Exito);
        Assert.Contains("no existe", resultado.Mensaje);
    }

    [Fact]
    public void La_cartelera_no_expone_datos_personales()
    {
        var propiedades = typeof(CursoEnSalidaDTO)
            .GetProperties()
            .Select(p => p.Name.ToLowerInvariant())
            .ToList();

        Assert.DoesNotContain(propiedades, p => p.Contains("nombre"));
        Assert.DoesNotContain(propiedades, p => p.Contains("apellido"));
        Assert.DoesNotContain(propiedades, p => p.Contains("dni"));
    }

    [Fact]
    public async Task La_cartelera_avisa_cual_es_el_proximo_curso()
    {
        var cursos = new CursoEscolarRepositoryFalso(
            CursoDe(1, "1°", "A", "Mañana", TimeSpan.Zero),
            CursoDe(2, "6°", "A", "Noche", new TimeSpan(23, 59, 59)));

        var servicio = CrearServicio(cursos);

        var cartelera = await servicio.GetCarteleraAsync();

        Assert.NotNull(cartelera.Proximo);
        Assert.Equal(2, cartelera.Proximo!.IdCurso);
    }

    private static SalidaDeCursoService CrearServicio(
        CursoEscolarRepositoryFalso cursos,
        AlumnoRepositoryFalso? alumnos = null) =>
        new SalidaDeCursoService(
            new SalidaDeCursoRepositoryFalso(),
            cursos,
            alumnos ?? new AlumnoRepositoryFalso());

    private static CursoEscolar CursoDe(
        int id, string grado, string division, string turno, TimeSpan? horarioSalida = null) =>
        new CursoEscolar(id, grado, division, turno, horarioSalida ?? new TimeSpan(12, 0, 0));

    private static Alumno AlumnoDe(int id, int idCurso) =>
        new Alumno(id, "Nombre", $"Apellido{id}", idCurso);

    private static Portero PorteroDePrueba() =>
        new Portero("Renzo Scollo", "Puerta Principal", new Usuario("porteria1", "porteria123"));
}
