using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private static List<Usuario> _usuarios = new List<Usuario>();
        private static int _nextId = 1;

        public Task AddAsync(Usuario usuario)
        {
            usuario.AsignarIdGenerado(_nextId++);
            _usuarios.Add(usuario);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return Task.FromResult(false);
            _usuarios.Remove(usuario);
            return Task.FromResult(true);
        }

        public Task<Usuario?> GetAsync(int id) =>
            Task.FromResult(_usuarios.FirstOrDefault(u => u.IdUsuario == id));

        public Task<IEnumerable<Usuario>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Usuario>>(_usuarios.ToList());

        public Task<bool> UpdateAsync(Usuario usuario)
        {
            var existing = _usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
            if (existing == null) return Task.FromResult(false);
            existing.SetNombreUsuario(usuario.NombreUsuario);
            existing.SetContrasena(usuario.Contrasena);
            existing.SetEstaActivo(usuario.EstaActivo);
            return Task.FromResult(true);
        }

        public Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario) =>
            Task.FromResult(_usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario));

        public Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null)
        {
            var query = _usuarios.Where(u => u.NombreUsuario == nombreUsuario);
            if (excludeId.HasValue) query = query.Where(u => u.IdUsuario != excludeId.Value);
            return Task.FromResult(query.Any());
        }
    }
}
