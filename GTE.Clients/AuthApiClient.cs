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
            catch (Exception)
            {
                // Ignorar error de red y retornar null
            }
            return null;
        }
    }
}
