using GTE.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Data
{
    public class TutorRepository : ITutorRepository
    {
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

            existing.SetNombre(tutor.Nombre);
            existing.SetApellido(tutor.Apellido);
            existing.SetDni(tutor.Dni);
            existing.SetParentesco(tutor.Parentesco);
            existing.SetTelefono(tutor.Telefono);
            existing.SetTieneRestriccion(tutor.TieneRestriccion);
            return Task.FromResult(true);
        }

        public Task<bool> DniExisteAsync(string dni, int? excludeId = null)
        {
            var query = _tutores.Where(t => t.Dni == dni);
            if (excludeId.HasValue) query = query.Where(t => t.IdTutor != excludeId.Value);
            return Task.FromResult(query.Any());
        }
    }
}
