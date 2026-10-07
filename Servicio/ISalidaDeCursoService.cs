using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface ISalidaDeCursoService
    {
        Task<CarteleraDTO> GetCarteleraAsync();

        Task<IEnumerable<SalidaDeCursoDTO>> GetDelDiaAsync(DateTime dia);

        Task<(bool Exito, string Mensaje, SalidaDeCursoDTO? Salida)> IniciarAsync(int idCurso, Personal personal);

        Task<(bool Exito, string Mensaje)> FinalizarAsync(int idSalida);
    }
}
