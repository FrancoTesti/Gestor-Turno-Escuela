using GTE.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    /// <summary>
    /// Cliente de la cartelera de la puerta, que es pública: la pantalla que está
    /// en la calle no tiene ningún usuario que inicie sesión.
    ///
    /// A propósito no hereda de BaseApiClient, porque ese exige una sesión y, sin
    /// ella, cortaría la llamada antes de llegar a la API.
    /// </summary>
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
