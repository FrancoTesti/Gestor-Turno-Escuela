using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace GTE.Clients
{
    public class AlumnoApiClient : BaseApiClient
    {
        public AlumnoApiClient()
        {
        }

        public AlumnoApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<AlumnoDTO>> GetAllAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("alumnos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDTO>>() ?? new List<AlumnoDTO>();
            }
            throw new Exception("Error al obtener alumnos.");
        }

        public async Task<List<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteria)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var query = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(criteria.Nombre)) query["nombre"] = criteria.Nombre;
            if (!string.IsNullOrEmpty(criteria.Grado)) query["grado"] = criteria.Grado;
            if (!string.IsNullOrEmpty(criteria.Curso)) query["curso"] = criteria.Curso;
            if (!string.IsNullOrEmpty(criteria.Estado)) query["estado"] = criteria.Estado;

            var response = await client.GetAsync($"alumnos/criteria?{query}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDTO>>() ?? new List<AlumnoDTO>();
            }
            throw new Exception("Error al realizar búsqueda filtrada de alumnos.");
        }

        public async Task<AlumnoDTO?> GetAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync($"alumnos/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlumnoDTO>();
            }
            return null;
        }

        public async Task<AlumnoDTO> AddAsync(AlumnoDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("alumnos", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AlumnoDTO>() ?? dto;
            }

            throw new Exception(await LeerMensajeDeErrorAsync(response, "Error al agregar alumno."));
        }

        public async Task<bool> UpdateAsync(AlumnoDTO dto)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PutAsJsonAsync("alumnos", dto);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
                return true;

            if ((int)response.StatusCode == 400 || (int)response.StatusCode == 409)
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo actualizar el alumno."));

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.DeleteAsync($"alumnos/{id}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
                return true;

            if ((int)response.StatusCode == 400 || (int)response.StatusCode == 409)
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo eliminar el alumno."));

            return false;
        }
    }
}
