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
using System.Threading.Tasks;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

namespace GTE.Data
{
    public class SecretarioRepository : ISecretarioRepository
    {
<<<<<<< HEAD
        private readonly GTEContext _context;

        public SecretarioRepository(GTEContext context)
        {
            _context = context;
        }

        public SecretarioRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Secretario secretario)
        {
            _context.Entry(secretario.Usuario).State = EntityState.Unchanged; // Evitar duplicar usuario si ya existe
            _context.Secretarios.Add(secretario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var secretario = await _context.Secretarios.FindAsync(id);
            if (secretario == null) return false;
            _context.Secretarios.Remove(secretario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Secretario?> GetAsync(int id) =>
            await _context.Secretarios.Include(s => s.Usuario).FirstOrDefaultAsync(s => s.IdPersonal == id);

        public async Task<IEnumerable<Secretario>> GetAllAsync() =>
            await _context.Secretarios.Include(s => s.Usuario).ToListAsync();

        public async Task<bool> UpdateAsync(Secretario secretario)
        {
            var existing = await _context.Secretarios.Include(s => s.Usuario).FirstOrDefaultAsync(s => s.IdPersonal == secretario.IdPersonal);
            if (existing == null) return false;

            existing.SetNombre(secretario.Nombre);
            existing.SetNivelAccesoSistema(secretario.NivelAccesoSistema);

            await _context.SaveChangesAsync();
            return true;
=======
        private static List<Secretario> _secretarios = new List<Secretario>();
        private static int _nextId = 1;

        public Task AddAsync(Secretario secretario)
        {
            secretario.AsignarIdGenerado(_nextId++);
            _secretarios.Add(secretario);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var secretario = _secretarios.FirstOrDefault(s => s.IdPersonal == id);
            if (secretario == null) return Task.FromResult(false);
            _secretarios.Remove(secretario);
            return Task.FromResult(true);
        }

        public Task<Secretario?> GetAsync(int id) =>
            Task.FromResult(_secretarios.FirstOrDefault(s => s.IdPersonal == id));

        public Task<IEnumerable<Secretario>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Secretario>>(_secretarios.ToList());

        public Task<bool> UpdateAsync(Secretario secretario)
        {
            var existing = _secretarios.FirstOrDefault(s => s.IdPersonal == secretario.IdPersonal);
            if (existing == null) return Task.FromResult(false);

            existing.SetNombre(secretario.Nombre);
            existing.SetNivelAccesoSistema(secretario.NivelAccesoSistema);
            return Task.FromResult(true);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        }
    }
}