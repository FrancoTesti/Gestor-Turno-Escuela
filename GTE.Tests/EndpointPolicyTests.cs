using GTE.Application.Services;
using GTE.Data;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GTE.Tests;

public class EndpointPolicyTests
{
    [Theory]
    [InlineData("GET", "/alumnos", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/alumnos/criteria", Politicas.LecturaAlumnos)]
    [InlineData("POST", "/alumnos", Politicas.SoloSecretario)]
    [InlineData("PUT", "/alumnos", Politicas.SoloSecretario)]
    [InlineData("DELETE", "/alumnos/{id:int}", Politicas.SoloSecretario)]
    [InlineData("GET", "/cursos", Politicas.LecturaCursos)]
    [InlineData("POST", "/cursos", Politicas.SoloSecretario)]
    [InlineData("PUT", "/cursos", Politicas.SoloSecretario)]
    [InlineData("DELETE", "/cursos/{id:int}", Politicas.SoloSecretario)]
    [InlineData("GET", "/retiros", Politicas.GestionRetiros)]
    [InlineData("GET", "/retiros/{id:int}", Politicas.GestionRetiros)]
    [InlineData("POST", "/retiros", Politicas.GestionRetiros)]
    [InlineData("PUT", "/retiros", Politicas.GestionRetiros)]
    [InlineData("DELETE", "/retiros/{id:int}", Politicas.GestionRetiros)]
    [InlineData("GET", "/tutores/{id:int}/alumnos", Politicas.GestionRetiros)]
    [InlineData("GET", "/personal", Politicas.GestionRetiros)]
    [InlineData("GET", "/salidas", Politicas.GestionSalidas)]
    [InlineData("POST", "/salidas", Politicas.GestionSalidas)]
    [InlineData("POST", "/salidas/{id:int}/finalizar", Politicas.GestionSalidas)]
    [InlineData("GET", "/autorizaciones", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/autorizaciones/{id:int}", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/autorizaciones/tutor/{tutorId:int}", Politicas.LecturaAlumnos)]
    [InlineData("POST", "/autorizaciones", Politicas.SoloSecretario)]
    [InlineData("DELETE", "/autorizaciones/{id:int}", Politicas.SoloSecretario)]
    [InlineData("DELETE", "/autorizaciones/tutor/{tutorId:int}/alumno/{alumnoId:int}", Politicas.SoloSecretario)]
    [InlineData("GET", "/reportes/alumnos-por-curso", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/reportes/retiros", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/tutores", Politicas.LecturaAlumnos)]
    [InlineData("GET", "/mis-alumnos", Politicas.SoloTutor)]
    [InlineData("GET", "/tutores/{id:int}", Politicas.LecturaAlumnos)]
    [InlineData("POST", "/tutores", Politicas.SoloSecretario)]
    [InlineData("PUT", "/tutores", Politicas.SoloSecretario)]
    [InlineData("DELETE", "/tutores/{id:int}", Politicas.SoloSecretario)]
    public void El_endpoint_exige_la_politica_correspondiente(string metodo, string ruta, string politicaEsperada)
    {
        Endpoint? endpoint = BuscarEndpoint(metodo, ruta);

        Assert.NotNull(endpoint);

        IAuthorizeData? autorizacion = endpoint!.Metadata.GetOrderedMetadata<IAuthorizeData>().FirstOrDefault();

        Assert.NotNull(autorizacion);
        Assert.Equal(politicaEsperada, autorizacion!.Policy);
    }

    [Fact]
    public void No_hay_dos_endpoints_con_la_misma_ruta_y_metodo()
    {
        // Dos endpoints con la misma combinacion de ruta y metodo hacen que la
        // aplicacion falle al atender el pedido, porque no puede decidir cual
        // corresponde. Paso al mover el listado de tutores a su propio archivo.
        var repetidos = ConstruirEndpoints()
            .SelectMany(endpoint => MetodosDe(endpoint).Select(metodo => new
            {
                Metodo = metodo,
                Ruta = (endpoint as RouteEndpoint)?.RoutePattern.RawText
            }))
            .Where(par => par.Ruta is not null)
            .GroupBy(par => $"{par.Metodo} {par.Ruta}")
            .Where(grupo => grupo.Count() > 1)
            .Select(grupo => grupo.Key)
            .ToList();

        Assert.Empty(repetidos);
    }

    [Fact]
    public void La_cartelera_de_la_puerta_es_publica()
    {
        // La pantalla que está en la calle no tiene usuario que inicie sesión, así
        // que este endpoint queda abierto. A cambio, no devuelve nombres de nadie.
        Endpoint? endpoint = BuscarEndpoint("GET", "/cartelera");

        Assert.NotNull(endpoint);
        Assert.NotNull(endpoint!.Metadata.GetMetadata<IAllowAnonymous>());
        Assert.Null(endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>().FirstOrDefault());
    }

    private static Endpoint? BuscarEndpoint(string metodo, string ruta)
    {
        return ConstruirEndpoints().FirstOrDefault(endpoint =>
            MetodosDe(endpoint).Contains(metodo, StringComparer.OrdinalIgnoreCase) &&
            (endpoint as RouteEndpoint)?.RoutePattern.RawText == ruta);
    }

    private static IEnumerable<string> MetodosDe(Endpoint endpoint)
    {
        return endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods ?? Array.Empty<string>();
    }

    private static List<Endpoint> ConstruirEndpoints()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        builder.Services.AddPoliticasDeAutorizacion();
        builder.Services.AddSingleton<IAlumnoService, AlumnoServiceFalso>();
        builder.Services.AddSingleton<ICursoEscolarService, CursoEscolarServiceFalso>();
        builder.Services.AddSingleton<IRetiroService, RetiroServiceFalso>();
        builder.Services.AddSingleton<IAutorizacionService, AutorizacionServiceFalso>();
        builder.Services.AddSingleton<IReporteService, ReporteServiceFalso>();
        builder.Services.AddSingleton<ITutorService, TutorServiceFalso>();
        builder.Services.AddSingleton<ISalidaDeCursoService, SalidaDeCursoServiceFalso>();
        builder.Services.AddSingleton<ITutorRepository, TutorRepositoryFalso>();
        builder.Services.AddSingleton<ISalidaDeCursoRepository, SalidaDeCursoRepositoryFalso>();
        builder.Services.AddSingleton<IAutorizacionRepository, AutorizacionRepositoryFalso>();
        builder.Services.AddDbContext<GTEContext>(opciones =>
            opciones.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SoloParaMapear;Trusted_Connection=True"));

        WebApplication app = builder.Build();
        app.MapAlumnoEndpoints();
        app.MapCursoEscolarEndpoints();
        app.MapAutorizacionEndpoints();
        app.MapRetiroEndpoints();
        app.MapSalidaDeCursoEndpoints();
        app.MapReporteEndpoints();
        app.MapTutorEndpoints();

        return ((IEndpointRouteBuilder)app).DataSources
            .SelectMany(origen => origen.Endpoints)
            .ToList();
    }
}
