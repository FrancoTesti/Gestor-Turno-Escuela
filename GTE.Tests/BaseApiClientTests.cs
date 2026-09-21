using System.Net;
using GTE.Clients;

namespace GTE.Tests;

public class BaseApiClientTests
{
    [Fact]
    public async Task Una_respuesta_401_cierra_la_sesion_y_avisa_que_expiro()
    {
        var autenticacion = new AutenticacionFalsa();
        AuthServiceProvider.Register(autenticacion);
        var cliente = new ClienteDePrueba();

        await Assert.ThrowsAsync<SesionExpiradaException>(
            () => cliente.VerificarRespuestaAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized)));

        Assert.True(autenticacion.SesionCerrada);
    }

    [Fact]
    public async Task Una_respuesta_403_avisa_que_falta_permiso_sin_cerrar_la_sesion()
    {
        var autenticacion = new AutenticacionFalsa();
        AuthServiceProvider.Register(autenticacion);
        var cliente = new ClienteDePrueba();

        await Assert.ThrowsAsync<SinPermisoException>(
            () => cliente.VerificarRespuestaAsync(new HttpResponseMessage(HttpStatusCode.Forbidden)));

        Assert.False(autenticacion.SesionCerrada);
    }

    [Fact]
    public async Task Una_respuesta_correcta_no_lanza_error()
    {
        AuthServiceProvider.Register(new AutenticacionFalsa());
        var cliente = new ClienteDePrueba();

        await cliente.VerificarRespuestaAsync(new HttpResponseMessage(HttpStatusCode.OK));
    }

    private sealed class ClienteDePrueba : BaseApiClient
    {
        public Task VerificarRespuestaAsync(HttpResponseMessage respuesta) =>
            HandleUnauthorizedResponseAsync(respuesta);
    }

    private sealed class AutenticacionFalsa : IAuthService
    {
        public bool SesionCerrada { get; private set; }

        public Task<bool> IsAuthenticatedAsync() => Task.FromResult(!SesionCerrada);

        public Task<string?> GetTokenAsync() => Task.FromResult<string?>(SesionCerrada ? null : "token");

        public Task<string?> GetUsernameAsync() => Task.FromResult<string?>("renzo");

        public Task<string?> GetRoleAsync() => Task.FromResult<string?>("Portero");

        public Task<string?> GetNombreCompletoAsync() => Task.FromResult<string?>("Renzo Scollo");

        public Task<bool> LoginAsync(string username, string password) => Task.FromResult(true);

        public Task LogoutAsync()
        {
            SesionCerrada = true;
            return Task.CompletedTask;
        }

        public Task CheckTokenExpirationAsync() => Task.CompletedTask;
    }
}
