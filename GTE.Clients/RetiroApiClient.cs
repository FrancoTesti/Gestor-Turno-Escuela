using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class RetiroApiClient : BaseApiClient
    {
        public async Task<List<RetiroDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("retiros");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<RetiroDTO>>() ?? new List<RetiroDTO>();
            }
            throw new Exception("Error al obtener el listado de retiros.");
        }

        public async Task<RetiroDTO?> GetAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"retiros/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RetiroDTO>();
            }
            return null;
        }

        public async Task<RetiroDTO> AddAsync(RetiroDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("retiros", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RetiroDTO>() ?? dto;
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(errorBody);
                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    throw new InvalidOperationException(errorProp.GetString());
                }
            }
            catch (JsonException)
            {
            }

            throw new InvalidOperationException(string.IsNullOrWhiteSpace(errorBody) ? "Error al registrar el retiro." : errorBody);
        }

        public async Task<RetiroDTO> UpdateAsync(RetiroDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PutAsJsonAsync("retiros", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RetiroDTO>() ?? dto;
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(errorBody);
                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    throw new InvalidOperationException(errorProp.GetString());
                }
            }
            catch (JsonException)
            {
            }

            throw new InvalidOperationException(string.IsNullOrWhiteSpace(errorBody) ? "Error al modificar el retiro." : errorBody);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"retiros/{id}");
            await HandleUnauthorizedResponseAsync(response);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<AlumnoDTO>> GetAlumnosAutorizadosByTutorAsync(int tutorId)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"tutores/{tutorId}/alumnos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDTO>>() ?? new List<AlumnoDTO>();
            }
            throw new Exception("Error al obtener los alumnos autorizados del tutor.");
        }

        public async Task<List<TutorDTO>> GetTutoresAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("tutores");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TutorDTO>>() ?? new List<TutorDTO>();
            }
            return new List<TutorDTO>();
        }

        public async Task<List<PorteroDTO>> GetPersonalAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("personal");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PorteroDTO>>() ?? new List<PorteroDTO>();
            }
            return new List<PorteroDTO>();
        }
    }
}
