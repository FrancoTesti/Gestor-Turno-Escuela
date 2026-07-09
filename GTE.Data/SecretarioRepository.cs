using GTE.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTE.Data
{
    public class SecretarioRepository : ISecretarioRepository
    {
        private static readonly List<Secretario> _secretarios = new List<Secretario>();
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
        }
    }
}