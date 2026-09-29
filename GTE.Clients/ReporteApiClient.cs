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
        public ReporteApiClient()
        {
        }

        public ReporteApiClient(IAuthService authService) : base(authService)
        {
        }

        /// <summary>Cantidad de alumnos por curso escolar, agrupados por turno.</summary>
        public async Task<List<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("reportes/alumnos-por-curso");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnosPorCursoDTO>>()
                    ?? new List<AlumnosPorCursoDTO>();
            }

            throw new Exception("Error al obtener el reporte de alumnos por curso.");
        }

        /// <summary>Retiros de un periodo. Las dos fechas son opcionales.</summary>
        public async Task<List<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var parametros = new List<string>();
            if (desde.HasValue) parametros.Add($"desde={desde.Value:yyyy-MM-dd}");
            if (hasta.HasValue) parametros.Add($"hasta={hasta.Value:yyyy-MM-dd}");

            string consulta = parametros.Count > 0 ? "?" + string.Join("&", parametros) : string.Empty;

            var response = await client.GetAsync($"reportes/retiros{consulta}");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<RetiroDTO>>()
                    ?? new List<RetiroDTO>();
            }

            throw new Exception("Error al obtener el reporte de retiros.");
        }
    }
}
