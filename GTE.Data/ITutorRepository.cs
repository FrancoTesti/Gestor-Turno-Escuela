using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface ITutorRepository
    {
        Task AddAsync(Tutor tutor);
        Task<bool> DeleteAsync(int id);
        Task<Tutor?> GetAsync(int id);
        Task<IEnumerable<Tutor>> GetAllAsync();
        Task<bool> UpdateAsync(Tutor tutor);
        Task<bool> DniExisteAsync(string dni, int? excludeId = null);
    }
}
