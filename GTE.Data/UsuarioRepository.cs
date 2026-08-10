using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
=======
using System.Text;
using System.Threading.Tasks;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
using GTE.Dominio;

namespace GTE.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
<<<<<<< HEAD
        private readonly GTEContext _context;

        public UsuarioRepository(GTEContext context)
        {
            _context = context;
        }

        public UsuarioRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> GetAsync(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _context.Usuarios.ToListAsync();

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var existing = await _context.Usuarios.FindAsync(usuario.IdUsuario);
            if (existing == null) return false;

            existing.SetNombreUsuario(usuario.NombreUsuario);
            existing.SetContrasena(usuario.Contrasena);
            existing.SetEstaActivo(usuario.EstaActivo);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        public async Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null)
        {
            var query = _context.Usuarios.Where(u => u.NombreUsuario == nombreUsuario);
            if (excludeId.HasValue) query = query.Where(u => u.IdUsuario != excludeId.Value);
            return await query.AnyAsync();
=======
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
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        }
    }
}
