using GTE.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class AuthApiClient : BaseApiClient
    {
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5117/");

            try
            {
                var response = await client.PostAsJsonAsync("auth/login", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
            }
            catch (HttpRequestException ex)
            {
                // No hubo respuesta de la API. Antes esto se devolvía como null y
                // las pantallas mostraban "usuario o contraseña incorrectos", que
                // no tiene nada que ver: el problema es que la API no está corriendo.
                throw new ApiNoDisponibleException(
                    "No se pudo conectar con la API. Verificá que el proyecto GTE.WebAPI esté en ejecución.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiNoDisponibleException(
                    "La API no respondió a tiempo. Verificá que el proyecto GTE.WebAPI esté en ejecución.", ex);
            }

            // La API contestó, pero rechazó el usuario o la contraseña.
            return null;
        }
    }
}
