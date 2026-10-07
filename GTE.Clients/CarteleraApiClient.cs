using GTE.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class CarteleraApiClient
    {
        public async Task<CarteleraDTO> GetAsync()
        {
            using var client = new HttpClient { BaseAddress = ApiConfig.Uri };

            var respuesta = await client.GetAsync("cartelera");
            respuesta.EnsureSuccessStatusCode();

            return await respuesta.Content.ReadFromJsonAsync<CarteleraDTO>() ?? new CarteleraDTO();
        }
    }
}
