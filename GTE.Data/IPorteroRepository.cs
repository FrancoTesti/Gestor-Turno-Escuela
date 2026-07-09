using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTE.Dominio;

namespace GTE.Data
{
    public interface IPorteroRepository
    {
        Task AddAsync(Portero portero);
        Task<bool> DeleteAsync(int id);
        Task<Portero?> GetAsync(int id);
        Task<IEnumerable<Portero>> GetAllAsync();
        Task<bool> UpdateAsync(Portero portero);
        Task<IEnumerable<Portero>> BuscarPorNombreAsync(string texto);
    }
}
