using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTE.Dominio;

namespace GTE.Data
{
    public interface ISecretarioRepository
    {
        Task AddAsync(Secretario secretario);
        Task<bool> DeleteAsync(int id);
        Task<Secretario?> GetAsync(int id);
        Task<IEnumerable<Secretario>> GetAllAsync();
        Task<bool> UpdateAsync(Secretario secretario);
    }
}
