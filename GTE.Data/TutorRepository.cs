<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;
=======
﻿using GTE.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

namespace GTE.Data
{
    public class TutorRepository : ITutorRepository
    {
<<<<<<< HEAD
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
=======
        private static List<Tutor> _tutores = new List<Tutor>();
        private static int _nextId = 1;

        public Task AddAsync(Tutor tutor)
        {
            tutor.AsignarIdGenerado(_nextId++); 
            _tutores.Add(tutor);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var tutor = _tutores.FirstOrDefault(u => u.IdTutor == id);
            if (tutor == null) return Task.FromResult(false);
            _tutores.Remove(tutor);
            return Task.FromResult(true);
        }

        public Task<Tutor?> GetAsync(int id) =>
            Task.FromResult(_tutores.FirstOrDefault(u => u.IdTutor == id));

        public Task<IEnumerable<Tutor>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Tutor>>(_tutores.ToList());

        public Task<bool> UpdateAsync(Tutor tutor)
        {
            var existing = _tutores.FirstOrDefault(t => t.IdTutor == tutor.IdTutor);
            if (existing == null) return Task.FromResult(false);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

            existing.SetNombre(tutor.Nombre);
            existing.SetApellido(tutor.Apellido);
            existing.SetDni(tutor.Dni);
            existing.SetParentesco(tutor.Parentesco);
            existing.SetTelefono(tutor.Telefono);
            existing.SetTieneRestriccion(tutor.TieneRestriccion);
<<<<<<< HEAD

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DniExisteAsync(string dni, int? excludeId = null)
        {
            var query = _context.Tutores.Where(t => t.Dni == dni);
            if (excludeId.HasValue) query = query.Where(t => t.IdTutor != excludeId.Value);
            return await query.AnyAsync();
=======
            return Task.FromResult(true);
        }

        public Task<bool> DniExisteAsync(string dni, int? excludeId = null)
        {
            var query = _tutores.Where(t => t.Dni == dni);
            if (excludeId.HasValue) query = query.Where(t => t.IdTutor != excludeId.Value);
            return Task.FromResult(query.Any());
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        }
    }
}
