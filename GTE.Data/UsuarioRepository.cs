using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
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
        }
    }
}
