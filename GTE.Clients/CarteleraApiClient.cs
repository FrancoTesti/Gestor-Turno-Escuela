using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Clients
{
    public class CarteleraApiClient
    {
        private readonly HttpClient? _httpClient;

        public CarteleraApiClient()
        {
        }

        public CarteleraApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CarteleraDTO> GetAsync(DateTime? fecha = null)
        {
            using var localClient = _httpClient is null ? new HttpClient { BaseAddress = ApiConfig.Uri } : null;
            var client = _httpClient ?? localClient!;

            string ruta = fecha.HasValue
                ? $"cartelera?fecha={Uri.EscapeDataString(fecha.Value.ToString("o"))}"
                : "cartelera";

            var respuesta = await client.GetAsync(ruta);
            respuesta.EnsureSuccessStatusCode();

            return await respuesta.Content.ReadFromJsonAsync<CarteleraDTO>() ?? new CarteleraDTO();
        }
    }
}
