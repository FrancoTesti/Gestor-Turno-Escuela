using System.Security.Claims;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GTE.Tests;

/// <summary>
/// Permite evaluar una política de la API con un rol concreto, usando los
/// mismos servicios de autorización que la aplicación real.
/// </summary>
internal static class AutorizacionDePrueba
{
    public static async Task<bool> AutorizarAsync(string politica, string? rol)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddPoliticasDeAutorizacion();

        using ServiceProvider provider = services.BuildServiceProvider();
        var autorizacion = provider.GetRequiredService<IAuthorizationService>();

        var identidad = new ClaimsIdentity(
            rol is null ? Array.Empty<Claim>() : new[] { new Claim(ClaimTypes.Role, rol) },
            authenticationType: rol is null ? null : "Test");

        AuthorizationResult resultado =
            await autorizacion.AuthorizeAsync(new ClaimsPrincipal(identidad), resource: null, politica);

        return resultado.Succeeded;
    }
}
