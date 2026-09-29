using GTE.Clients;

namespace GTE.Tests;

public class ClientesConSesionTests
{
    /// <summary>
    /// En Blazor la sesión va por inyección de dependencias, así que todo
    /// cliente que hable con la API tiene que poder recibirla. Si a alguno le
    /// falta el constructor, cae al proveedor global, que en la web no está
    /// registrado, y la pantalla falla con "AuthService no ha sido registrado".
    /// </summary>
    [Fact]
    public void Todos_los_clientes_de_la_api_aceptan_una_sesion_inyectada()
    {
        var clientes = typeof(BaseApiClient).Assembly
            .GetTypes()
            .Where(tipo => tipo.IsClass && !tipo.IsAbstract && typeof(BaseApiClient).IsAssignableFrom(tipo))
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
