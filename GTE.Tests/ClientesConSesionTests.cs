using GTE.Clients;

namespace GTE.Tests;

public class ClientesConSesionTests
{
    [Fact]
    public void Todos_los_clientes_de_la_api_aceptan_una_sesion_inyectada()
    {
        var clientes = typeof(BaseApiClient).Assembly
            .GetTypes()
            .Where(tipo => tipo.IsClass && !tipo.IsAbstract && typeof(BaseApiClient).IsAssignableFrom(tipo))
            .Where(tipo => tipo != typeof(AuthApiClient))
            .ToList();

        Assert.NotEmpty(clientes);

        var sinConstructor = clientes
            .Where(tipo => tipo.GetConstructor(new[] { typeof(IAuthService) }) is null)
            .Select(tipo => tipo.Name)
            .OrderBy(nombre => nombre)
            .ToList();

        Assert.Empty(sinConstructor);
    }
}
