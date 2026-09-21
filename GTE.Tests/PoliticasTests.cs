using System.Security.Claims;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GTE.Tests;

public class PoliticasTests
{
    private const string Secretario = "Secretario";
    private const string Portero = "Portero";
    private const string Tutor = "Tutor";

    [Theory]
    [InlineData(Secretario, true)]
    [InlineData(Portero, false)]
    [InlineData(Tutor, false)]
    public async Task SoloSecretario_solo_lo_permite_al_secretario(string rol, bool esperado)
    {
        bool autorizado = await AutorizarAsync(Politicas.SoloSecretario, rol);

        Assert.Equal(esperado, autorizado);
    }

    [Theory]
    [InlineData(Secretario, true)]
    [InlineData(Portero, true)]
    [InlineData(Tutor, false)]
    public async Task LecturaAlumnos_lo_permite_al_secretario_y_al_portero(string rol, bool esperado)
    {
        bool autorizado = await AutorizarAsync(Politicas.LecturaAlumnos, rol);

        Assert.Equal(esperado, autorizado);
    }

    [Theory]
    [InlineData(Secretario, true)]
    [InlineData(Portero, true)]
    [InlineData(Tutor, true)]
    public async Task LecturaCursos_lo_permite_a_los_tres_roles(string rol, bool esperado)
    {
        bool autorizado = await AutorizarAsync(Politicas.LecturaCursos, rol);

        Assert.Equal(esperado, autorizado);
    }

    [Theory]
    [InlineData(Politicas.SoloSecretario)]
    [InlineData(Politicas.LecturaAlumnos)]
    [InlineData(Politicas.LecturaCursos)]
    [InlineData(Politicas.GestionRetiros)]
    public async Task Un_usuario_sin_rol_no_puede_autorizar(string politica)
    {
        bool autorizado = await AutorizarAsync(politica, rol: null);

        Assert.False(autorizado);
    }

    [Theory]
    [InlineData(Secretario, true)]
    [InlineData(Portero, true)]
    [InlineData(Tutor, false)]
    public async Task GestionRetiros_lo_permite_al_secretario_y_al_portero(string rol, bool esperado)
    {
        bool autorizado = await AutorizarAsync(Politicas.GestionRetiros, rol);

        Assert.Equal(esperado, autorizado);
    }

    private static async Task<bool> AutorizarAsync(string politica, string? rol)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddPoliticasDeAutorizacion();

        using ServiceProvider provider = services.BuildServiceProvider();
        var autorizacion = provider.GetRequiredService<IAuthorizationService>();

        var identidad = new ClaimsIdentity(
            rol is null ? Array.Empty<Claim>() : new[] { new Claim(ClaimTypes.Role, rol) },
            authenticationType: rol is null ? null : "Test");

        var usuario = new ClaimsPrincipal(identidad);

        AuthorizationResult resultado = await autorizacion.AuthorizeAsync(usuario, resource: null, politica);
        return resultado.Succeeded;
    }
}
