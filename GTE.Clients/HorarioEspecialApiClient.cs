using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class HorarioEspecialApiClient : BaseApiClient
    {
        public HorarioEspecialApiClient()
        {
        }

        public HorarioEspecialApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<HorarioEspecialDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();
            var response = await client.GetAsync("horariosespeciales");
            await HandleUnauthorizedResponseAsync(response);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<List<HorarioEspecialDTO>>() ?? new List<HorarioEspecialDTO>();
            throw new Exception("Error al obtener horarios especiales.");
        }

        public async Task<HorarioEspecialDTO> AddAsync(HorarioEspecialDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();
            var response = await client.PostAsJsonAsync("horariosespeciales", dto);
            await HandleUnauthorizedResponseAsync(response);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<HorarioEspecialDTO>() ?? dto;
            throw new Exception($"Error al agregar: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<bool> UpdateAsync(HorarioEspecialDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();
            var response = await client.PutAsJsonAsync("horariosespeciales", dto);
            await HandleUnauthorizedResponseAsync(response);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();
            var response = await client.DeleteAsync($"horariosespeciales/{id}");
            await HandleUnauthorizedResponseAsync(response);
            return response.IsSuccessStatusCode;
        }
    }
}
