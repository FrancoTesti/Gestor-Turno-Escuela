using GTE.Clients;

namespace GTE.Tests;

public class ClientesConSesionTests
{
    /// <summary>
    /// En Blazor la sesión va por inyección de dependencias, así que todo
    /// cliente que hable con la API tiene que poder recibirla. Si a alguno le
    /// falta el constructor, cae al proveedor global, que en la web no está
    /// registrado, y la pantalla falla con "AuthService no ha sido registrado".
    ///
    /// Se exceptúa AuthApiClient: es el cliente que usa el propio servicio de
    /// autenticación para iniciar sesión. Si además pidiera una sesión, el
    /// servicio lo necesitaría a él y él a la sesión, que es el servicio, y
    /// quedaría un círculo que deja la aplicación colgada.
    /// </summary>
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
