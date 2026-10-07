using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface ISalidaDeCursoRepository
    {
        Task<IEnumerable<SalidaDeCurso>> GetDelDiaAsync(DateTime dia);
        Task<IEnumerable<SalidaDeCurso>> GetEnCursoAsync();
        Task<SalidaDeCurso?> GetAsync(int id);
        Task AddAsync(SalidaDeCurso salida);
        Task<bool> UpdateAsync(SalidaDeCurso salida);
    }
}
