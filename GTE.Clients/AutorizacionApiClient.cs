using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class AutorizacionApiClient : BaseApiClient
    {
        public AutorizacionApiClient()
        {
        }

        public AutorizacionApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<AutorizacionDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("autorizaciones");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AutorizacionDTO>>() ?? new List<AutorizacionDTO>();
            }
            throw new Exception("Error al obtener el listado de autorizaciones.");
        }

        public async Task<AutorizacionDTO?> GetAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"autorizaciones/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AutorizacionDTO>();
            }
            return null;
        }

        public async Task<List<AutorizacionDTO>> GetByTutorIdAsync(int tutorId)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"autorizaciones/tutor/{tutorId}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AutorizacionDTO>>() ?? new List<AutorizacionDTO>();
            }
            throw new Exception("Error al obtener las autorizaciones del tutor.");
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

        public async Task<AutorizacionDTO> AddAsync(AutorizacionDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("autorizaciones", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AutorizacionDTO>() ?? dto;
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

            throw new InvalidOperationException(string.IsNullOrWhiteSpace(errorBody) ? "Error al registrar la autorización." : errorBody);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"autorizaciones/{id}");
            await HandleUnauthorizedResponseAsync(response);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"autorizaciones/tutor/{tutorId}/alumno/{alumnoId}");
            await HandleUnauthorizedResponseAsync(response);

            return response.IsSuccessStatusCode;
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

        public async Task<List<AlumnoDTO>> GetAllAlumnosAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("alumnos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDTO>>() ?? new List<AlumnoDTO>();
            }
            return new List<AlumnoDTO>();
        }
    }
}
