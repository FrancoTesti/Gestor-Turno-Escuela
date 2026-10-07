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
            client.BaseAddress = ApiConfig.Uri;

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
                throw new ApiNoDisponibleException(
                    "No se pudo conectar con la API. Verificá que el proyecto GTE.WebAPI esté en ejecución.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiNoDisponibleException(
                    "La API no respondió a tiempo. Verificá que el proyecto GTE.WebAPI esté en ejecución.", ex);
            }

            return null;
        }
    }
}
