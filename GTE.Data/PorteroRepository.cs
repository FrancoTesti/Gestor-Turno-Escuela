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
    public class PorteroRepository : IPorteroRepository
    {
<<<<<<< HEAD
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
=======
        private static List<Portero> _porteros = new List<Portero>();
        private static int _nextId = 1;

        public Task AddAsync(Portero portero)
        {
            portero.AsignarIdGenerado(_nextId++); 
            _porteros.Add(portero);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var portero = _porteros.FirstOrDefault(p => p.IdPersonal == id);
            if (portero == null) return Task.FromResult(false);
            
            _porteros.Remove(portero);
            return Task.FromResult(true);
        }

        public Task<Portero?> GetAsync(int id) =>
            Task.FromResult(_porteros.FirstOrDefault(p => p.IdPersonal == id));

        public Task<IEnumerable<Portero>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Portero>>(_porteros.ToList());

        public Task<bool> UpdateAsync(Portero portero)
        {
            var existing = _porteros.FirstOrDefault(p => p.IdPersonal == portero.IdPersonal);
            if (existing == null) return Task.FromResult(false);

            existing.SetNombre(portero.Nombre);
            existing.SetPuertaAsignada(portero.PuertaAsignada);
            
            return Task.FromResult(true);
        }
        public Task<IEnumerable<Portero>> BuscarPorNombreAsync(string texto)
        {
            var resultado = _porteros
                .Where(p => p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult<IEnumerable<Portero>>(resultado);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        }
    }
}