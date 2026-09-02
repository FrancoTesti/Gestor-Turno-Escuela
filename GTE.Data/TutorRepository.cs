using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class TutorRepository : ITutorRepository
    {
        private readonly GTEContext _context;

        public TutorRepository(GTEContext context)
        {
            _context = context;
        }

        public TutorRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Tutor tutor)
        {
            _context.Entry(tutor.Usuario).State = EntityState.Unchanged; // No re-crear el usuario si ya existe en BD
            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor == null) return false;
            _context.Tutores.Remove(tutor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Tutor?> GetAsync(int id) =>
            await _context.Tutores.Include(t => t.Usuario).FirstOrDefaultAsync(t => t.IdTutor == id);

        public async Task<IEnumerable<Tutor>> GetAllAsync() =>
            await _context.Tutores.Include(t => t.Usuario).ToListAsync();

        public async Task<bool> UpdateAsync(Tutor tutor)
        {
            var existing = await _context.Tutores.Include(t => t.Usuario).FirstOrDefaultAsync(t => t.IdTutor == tutor.IdTutor);
            if (existing == null) return false;

            existing.SetNombre(tutor.Nombre);
            existing.SetApellido(tutor.Apellido);
            existing.SetDni(tutor.Dni);
            existing.SetParentesco(tutor.Parentesco);
            existing.SetTelefono(tutor.Telefono);
            existing.SetTieneRestriccion(tutor.TieneRestriccion);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DniExisteAsync(string dni, int? excludeId = null)
        {
            var query = _context.Tutores.Where(t => t.Dni == dni);
            if (excludeId.HasValue) query = query.Where(t => t.IdTutor != excludeId.Value);
            return await query.AnyAsync();
        }
    }
}
