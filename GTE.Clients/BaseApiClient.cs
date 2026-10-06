using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    /// <summary>Se lanza cuando el token expiró o no es válido (HTTP 401).</summary>
    public class SesionExpiradaException : Exception
    {
        public SesionExpiradaException(string mensaje) : base(mensaje) { }
    }

    /// <summary>Se lanza cuando el usuario está autenticado pero no tiene permisos (HTTP 403).</summary>
    public class SinPermisoException : Exception
    {
        public SinPermisoException(string mensaje) : base(mensaje) { }
    }

    /// <summary>
    /// Se lanza cuando la API no responde, por ejemplo porque el proyecto
    /// GTE.WebAPI no está en ejecución.
    /// </summary>
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

        /// <summary>
        /// Sesión que usa el cliente. Cuando la interfaz la inyecta se usa esa
        /// instancia, que es la correcta en Blazor porque cada circuito tiene
        /// su propia sesión. Si no se inyectó, se recurre al proveedor global
        /// que registran las aplicaciones que crean los clientes con new, como
        /// el escritorio.
        /// </summary>
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

        /// <summary>La API devuelve los errores de negocio en un campo "error".</summary>
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
                // El token venció o no es válido: se cierra la sesión para obligar
                // a volver a iniciar sesión.
                await Autenticacion.LogoutAsync();
                throw new SesionExpiradaException("La sesión expiró. Vuelva a iniciar sesión.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                // La sesión es válida, pero el rol del usuario no alcanza.
                throw new SinPermisoException("No tiene permisos para realizar esta operación.");
            }
        }

        private sealed class RespuestaDeError
        {
            public string? Error { get; set; }
        }
    }
}
