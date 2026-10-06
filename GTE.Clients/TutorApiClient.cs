using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class TutorApiClient : BaseApiClient
    {
        public TutorApiClient()
        {
        }

        public TutorApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<TutorDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("tutores");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TutorDTO>>() ?? new List<TutorDTO>();
            }

            throw new Exception("Error al obtener tutores.");
        }

        /// <summary>Alumnos que el tutor conectado tiene a cargo.</summary>
        public async Task<List<AlumnoACargoDTO>> GetMisAlumnosAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("mis-alumnos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoACargoDTO>>()
                    ?? new List<AlumnoACargoDTO>();
            }

            throw new Exception("Error al obtener los alumnos a cargo.");
        }

        public async Task<TutorDTO?> GetAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"tutores/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TutorDTO>();
            }

            return null;
        }

        public async Task<TutorDTO> AddAsync(TutorDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("tutores", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TutorDTO>() ?? dto;
            }

            throw new Exception(await LeerMensajeDeErrorAsync(response, "Error al agregar tutor."));
        }

        public async Task<bool> UpdateAsync(TutorDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PutAsJsonAsync("tutores", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
                return true;

            if ((int)response.StatusCode == 400)
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo modificar el tutor."));

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"tutores/{id}");
            await HandleUnauthorizedResponseAsync(response);

            return response.IsSuccessStatusCode;
        }

    }
}
