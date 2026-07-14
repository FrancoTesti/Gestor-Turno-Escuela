using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTE.Dominio;

namespace GTE.Data
{
    public interface IUsuarioRepository
    {
        Task AddAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<Usuario?> GetAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> UpdateAsync(Usuario usuario);
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null);
    }
}
