using GTE.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GTE.Clients
{
    public class SalidaDeCursoApiClient : BaseApiClient
    {
        public SalidaDeCursoApiClient()
        {
        }

        public SalidaDeCursoApiClient(IAuthService authService) : base(authService)
        {
        }

        /// <summary>Las salidas de curso de hoy, las que terminaron y las que no.</summary>
        public async Task<List<SalidaDeCursoDTO>> GetDelDiaAsync()
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.GetAsync("salidas");
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SalidaDeCursoDTO>>()
                    ?? new List<SalidaDeCursoDTO>();
            }

            throw new Exception("Error al obtener las salidas de hoy.");
        }

        /// <summary>Marca que un curso empezó a salir.</summary>
        public async Task<SalidaDeCursoDTO> IniciarAsync(int idCurso)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsJsonAsync("salidas", new IniciarSalidaDeCursoDTO { IdCurso = idCurso });
            await HandleUnauthorizedResponseAsync(response);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SalidaDeCursoDTO>()
                    ?? new SalidaDeCursoDTO();
            }

            throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo marcar la salida del curso."));
        }

        /// <summary>Marca que un curso terminó de salir.</summary>
        public async Task FinalizarAsync(int idSalida)
        {
            await EnsureAuthenticatedAsync();
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsync($"salidas/{idSalida}/finalizar", content: null);
            await HandleUnauthorizedResponseAsync(response);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await LeerMensajeDeErrorAsync(response, "No se pudo finalizar la salida."));
            }
        }
    }
}
