using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Clients
{
    public class AsistenciaApiClient : BaseApiClient, IAsistenciaApiClient
    {
        public AsistenciaApiClient()
        {
        }

        public AsistenciaApiClient(IAuthService authService) : base(authService)
        {
        }

        public async Task<List<TipoInasistenciaDTO>> GetTiposInasistenciaAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("asistencias/tipos");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TipoInasistenciaDTO>>()
                    ?? new List<TipoInasistenciaDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al obtener los tipos de inasistencia.");
            throw new InvalidOperationException(error);
        }

        public async Task<PlanillaDiariaDTO?> GetPlanillaDiariaAsync(int idCurso, DateTime fecha)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            string url = $"asistencias/planilla?idCurso={idCurso}&fecha={fecha:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            await HandleUnauthorizedResponseAsync(response);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PlanillaDiariaDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al obtener la planilla diaria de asistencia.");
            throw new InvalidOperationException(error);
        }

        public async Task<bool> GuardarPlanillaDiariaAsync(GuardarPlanillaAsistenciaRequestDTO request)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("asistencias/planilla", request);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al guardar la planilla diaria de asistencia.");
            throw new InvalidOperationException(error);
        }

        public async Task<List<AlumnoAsistenciaItemDTO>> SugerirInasistenciasDesdeRetirosAsync(int idCurso, DateTime fecha)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            string url = $"asistencias/sugerencias-retiros?idCurso={idCurso}&fecha={fecha:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoAsistenciaItemDTO>>()
                    ?? new List<AlumnoAsistenciaItemDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al sugerir inasistencias desde retiros.");
            throw new InvalidOperationException(error);
        }

        public async Task<List<ReporteNovedadItemDTO>> GetReporteNovedadesAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            string url = $"asistencias/reportes/novedades?idCurso={idCurso}&fechaDesde={fechaDesde:yyyy-MM-dd}&fechaHasta={fechaHasta:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ReporteNovedadItemDTO>>()
                    ?? new List<ReporteNovedadItemDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al obtener el reporte de novedades.");
            throw new InvalidOperationException(error);
        }

        public async Task<ReporteEspejoSemanaDTO?> GetReporteEspejoSemanalAsync(int idCurso, DateTime fechaSemana)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            string url = $"asistencias/reportes/espejo-semanal?idCurso={idCurso}&fechaSemana={fechaSemana:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            await HandleUnauthorizedResponseAsync(response);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReporteEspejoSemanaDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al obtener el reporte espejo semanal.");
            throw new InvalidOperationException(error);
        }

        public async Task<HistorialAsistenciaAlumnoDTO?> GetHistorialAlumnoAsync(int idAlumno)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            string url = $"asistencias/historial/{idAlumno}";
            var response = await client.GetAsync(url);
            await HandleUnauthorizedResponseAsync(response);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<HistorialAsistenciaAlumnoDTO>();
            }

            var error = await LeerMensajeDeErrorAsync(response, "Error al obtener el historial de asistencia del alumno.");
            throw new InvalidOperationException(error);
        }
    }
}
