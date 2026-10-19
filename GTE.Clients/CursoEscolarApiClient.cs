using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class CursoEscolarApiClient : BaseApiClient
    {
        public CursoEscolarApiClient()
        {
        }

        public CursoEscolarApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<CursoEscolarDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("cursos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CursoEscolarDTO>>() ?? new List<CursoEscolarDTO>();
            }
            throw new Exception("Error al obtener cursos escolares.");
        }

        public async Task<CursoEscolarDTO?> GetAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"cursos/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CursoEscolarDTO>();
            }
            return null;
        }

        public async Task<CursoEscolarDTO> AddAsync(CursoEscolarDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("cursos", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CursoEscolarDTO>() ?? dto;
            }

            throw new Exception(await LeerMensajeDeErrorAsync(response, "Error al agregar curso escolar."));
        }

        public async Task<bool> UpdateAsync(CursoEscolarDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PutAsJsonAsync("cursos", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
                return true;

            if ((int)response.StatusCode == 400 || (int)response.StatusCode == 409)
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo actualizar el curso escolar."));

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"cursos/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
                return true;

            if ((int)response.StatusCode == 400 || (int)response.StatusCode == 409)
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo eliminar el curso escolar."));

            return false;
        }
    }
}
