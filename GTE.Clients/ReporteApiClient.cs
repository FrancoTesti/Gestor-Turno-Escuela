using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class ReporteApiClient : BaseApiClient
    {
        public async Task<List<ReporteAlumnosPorCursoDTO>> ObtenerAlumnosPorCursoAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("reportes/alumnos-por-curso");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ReporteAlumnosPorCursoDTO>>() ?? new List<ReporteAlumnosPorCursoDTO>();
            }
            throw new Exception("Error al obtener el reporte de alumnos por curso.");
        }

        public async Task<List<RetiroDTO>> ObtenerRetirosPorRangoAsync(DateTime? desde, DateTime? hasta)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var query = "?";
            if (desde.HasValue) query += $"desde={desde.Value.ToString("yyyy-MM-dd")}&";
            if (hasta.HasValue) query += $"hasta={hasta.Value.ToString("yyyy-MM-dd")}";

            var response = await client.GetAsync($"reportes/retiros{query}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<RetiroDTO>>() ?? new List<RetiroDTO>();
            }
            throw new Exception("Error al obtener el reporte de retiros.");
        }
    }
}
