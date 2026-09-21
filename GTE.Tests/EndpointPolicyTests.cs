using GTE.Application.Services;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
    public void El_endpoint_exige_la_politica_correspondiente(string metodo, string ruta, string politicaEsperada)
    {
        Endpoint? endpoint = BuscarEndpoint(metodo, ruta);

        Assert.NotNull(endpoint);

        IAuthorizeData? autorizacion = endpoint!.Metadata.GetOrderedMetadata<IAuthorizeData>().FirstOrDefault();

        Assert.NotNull(autorizacion);
        Assert.Equal(politicaEsperada, autorizacion!.Policy);
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

        WebApplication app = builder.Build();
        app.MapAlumnoEndpoints();
        app.MapCursoEscolarEndpoints();

        return ((IEndpointRouteBuilder)app).DataSources
            .SelectMany(origen => origen.Endpoints)
            .ToList();
    }
}
