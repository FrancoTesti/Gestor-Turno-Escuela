using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface ICursoEscolarRepository
    {
        Task AddAsync(CursoEscolar curso);
        Task<CursoEscolar?> GetAsync(int id);
        Task<IEnumerable<CursoEscolar>> GetAllAsync();
        Task<bool> UpdateAsync(CursoEscolar curso);
        Task<bool> DeleteAsync(int id);
    }
}