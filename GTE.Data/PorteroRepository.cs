using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class PorteroRepository : IPorteroRepository
    {
        private readonly GTEContext _context;

        public PorteroRepository(GTEContext context)
        {
            _context = context;
        }

        public PorteroRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Portero portero)
        {
            _context.Entry(portero.Usuario).State = EntityState.Unchanged; // Evitar duplicar usuario si ya existe
            _context.Porteros.Add(portero);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var portero = await _context.Porteros.FindAsync(id);
            if (portero == null) return false;
            _context.Porteros.Remove(portero);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Portero?> GetAsync(int id) =>
            await _context.Porteros.Include(p => p.Usuario).FirstOrDefaultAsync(p => p.IdPersonal == id);

        public async Task<IEnumerable<Portero>> GetAllAsync() =>
            await _context.Porteros.Include(p => p.Usuario).ToListAsync();

        public async Task<bool> UpdateAsync(Portero portero)
        {
            var existing = await _context.Porteros.Include(p => p.Usuario).FirstOrDefaultAsync(p => p.IdPersonal == portero.IdPersonal);
            if (existing == null) return false;

            existing.SetNombre(portero.Nombre);
            existing.SetPuertaAsignada(portero.PuertaAsignada);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Portero>> BuscarPorNombreAsync(string texto)
        {
            return await _context.Porteros
                .Include(p => p.Usuario)
                .Where(p => p.Nombre.Contains(texto))
                .ToListAsync();
        }
    }
}