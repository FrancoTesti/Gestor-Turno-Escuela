using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class SesionExpiradaException : Exception
    {
        public SesionExpiradaException(string mensaje) : base(mensaje) { }
    }

    public class SinPermisoException : Exception
    {
        public SinPermisoException(string mensaje) : base(mensaje) { }
    }

    public class ApiNoDisponibleException : Exception
    {
        public ApiNoDisponibleException(string mensaje) : base(mensaje) { }

        public ApiNoDisponibleException(string mensaje, Exception interna) : base(mensaje, interna) { }
    }

    public abstract class BaseApiClient
    {
        private readonly IAuthService? _authService;

        protected BaseApiClient()
        {
        }

        protected BaseApiClient(IAuthService authService)
        {
            _authService = authService;
        }

        protected IAuthService Autenticacion => _authService ?? AuthServiceProvider.Instance;

        protected async Task EnsureAuthenticatedAsync()
        {
            var auth = Autenticacion;
            if (!await auth.IsAuthenticatedAsync())
                throw new Exception("Usuario no autenticado.");
            await auth.CheckTokenExpirationAsync();
        }

        protected async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient { BaseAddress = ApiConfig.Uri };
            var auth = Autenticacion;
            var token = await auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        protected static async Task<string> LeerMensajeDeErrorAsync(HttpResponseMessage respuesta, string porDefecto)
        {
            try
            {
                var cuerpo = await respuesta.Content.ReadFromJsonAsync<RespuestaDeError>();
                return string.IsNullOrWhiteSpace(cuerpo?.Error) ? porDefecto : cuerpo!.Error!;
            }
            catch (Exception)
            {
                return porDefecto;
            }
        }

        protected async Task HandleUnauthorizedResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Autenticacion.LogoutAsync();
                throw new SesionExpiradaException("La sesión expiró. Vuelva a iniciar sesión.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new SinPermisoException("No tiene permisos para realizar esta operación.");
            }
        }

        private sealed class RespuestaDeError
        {
            public string? Error { get; set; }
        }
    }
}
