using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public abstract class BaseApiClient
    {
        protected const string BaseUrl = "http://localhost:5117/";

        protected async Task EnsureAuthenticatedAsync()
        {
            var auth = AuthServiceProvider.Instance;
            if (!await auth.IsAuthenticatedAsync())
                throw new Exception("Usuario no autenticado.");
            await auth.CheckTokenExpirationAsync();
        }

        protected async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var auth = AuthServiceProvider.Instance;
            var token = await auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        protected Task HandleUnauthorizedResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("No autorizado (401).");
            return Task.CompletedTask;
        }
    }
}