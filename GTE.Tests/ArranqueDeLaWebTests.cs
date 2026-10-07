using GTE.Blazor.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace GTE.Tests;

public class ArranqueDeLaWebTests
{
    [Fact]
    public async Task La_web_registra_un_esquema_de_autenticacion()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AgregarServiciosDeLaWeb();

        using ServiceProvider provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<IAuthenticationService>());

        IAuthenticationSchemeProvider esquemas = provider.GetRequiredService<IAuthenticationSchemeProvider>();
        AuthenticationScheme? esquemaDeDesafio = await esquemas.GetDefaultChallengeSchemeAsync();

        Assert.Equal(CookieAuthenticationDefaults.AuthenticationScheme, esquemaDeDesafio?.Name);
    }
}
