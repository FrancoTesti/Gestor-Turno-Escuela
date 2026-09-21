using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface IRetiroRepository
    {
        Task<IEnumerable<Retiro>> GetAllAsync();
        Task<Retiro?> GetAsync(int id);
        Task AddAsync(Retiro retiro);
        Task<bool> UpdateAsync(Retiro retiro);
        Task<bool> DeleteAsync(int id);
    }
}
