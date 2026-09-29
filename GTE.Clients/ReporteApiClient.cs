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
<<<<<<< HEAD
        public async Task<List<ReporteAlumnosPorCursoDTO>> ObtenerAlumnosPorCursoAsync()
=======
        public ReporteApiClient()
        {
        }

        public ReporteApiClient(IAuthService authService) : base(authService)
        {
        }

        /// <summary>Cantidad de alumnos por curso escolar, agrupados por turno.</summary>
        public async Task<List<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync()
>>>>>>> origin/main
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("reportes/alumnos-por-curso");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
<<<<<<< HEAD
                return await response.Content.ReadFromJsonAsync<List<ReporteAlumnosPorCursoDTO>>() ?? new List<ReporteAlumnosPorCursoDTO>();
            }
            throw new Exception("Error al obtener el reporte de alumnos por curso.");
        }

        public async Task<List<RetiroDTO>> ObtenerRetirosPorRangoAsync(DateTime? desde, DateTime? hasta)
=======
                return await response.Content.ReadFromJsonAsync<List<AlumnosPorCursoDTO>>()
                    ?? new List<AlumnosPorCursoDTO>();
            }

            throw new Exception("Error al obtener el reporte de alumnos por curso.");
        }

        /// <summary>Retiros de un periodo. Las dos fechas son opcionales.</summary>
        public async Task<List<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta)
>>>>>>> origin/main
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

<<<<<<< HEAD
            var query = "?";
            if (desde.HasValue) query += $"desde={desde.Value.ToString("yyyy-MM-dd")}&";
            if (hasta.HasValue) query += $"hasta={hasta.Value.ToString("yyyy-MM-dd")}";

            var response = await client.GetAsync($"reportes/retiros{query}");
=======
            var parametros = new List<string>();
            if (desde.HasValue) parametros.Add($"desde={desde.Value:yyyy-MM-dd}");
            if (hasta.HasValue) parametros.Add($"hasta={hasta.Value:yyyy-MM-dd}");

            string consulta = parametros.Count > 0 ? "?" + string.Join("&", parametros) : string.Empty;

            var response = await client.GetAsync($"reportes/retiros{consulta}");
>>>>>>> origin/main
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
<<<<<<< HEAD
                return await response.Content.ReadFromJsonAsync<List<RetiroDTO>>() ?? new List<RetiroDTO>();
            }
=======
                return await response.Content.ReadFromJsonAsync<List<RetiroDTO>>()
                    ?? new List<RetiroDTO>();
            }

>>>>>>> origin/main
            throw new Exception("Error al obtener el reporte de retiros.");
        }
    }
}
