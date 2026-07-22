using GTE.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTE.Data
{
    public class PorteroRepository : IPorteroRepository
    {
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
        }
    }
}