using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class RetiroRepository : IRetiroRepository
    {
        private readonly GTEContext _context;

        public RetiroRepository(GTEContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Retiro>> GetAllAsync()
        {
            return await _context.Retiros
                .Include(r => r.Tutor)
                .Include(r => r.Personal)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Alumno)
                .OrderByDescending(r => r.FechaHora)
                .ToListAsync();
        }

        public async Task<Retiro?> GetAsync(int id)
        {
            return await _context.Retiros
                .Include(r => r.Tutor)
                .Include(r => r.Personal)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Alumno)
                .FirstOrDefaultAsync(r => r.IdRetiro == id);
        }

        public async Task AddAsync(Retiro retiro)
        {
            await _context.Retiros.AddAsync(retiro);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Retiro retiro)
        {
            var existing = await _context.Retiros
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.IdRetiro == retiro.IdRetiro);

            if (existing == null) return false;

            existing.IdTutor = retiro.IdTutor;
            existing.IdPersonal = retiro.IdPersonal;
            existing.FechaHora = retiro.FechaHora;
            existing.Observaciones = retiro.Observaciones;

            // Eliminar detalles ausentes en el nuevo objeto
            var detallesAEliminar = existing.Detalles
                .Where(d => !retiro.Detalles.Any(nd => nd.IdDetalleRetiro != 0 && nd.IdDetalleRetiro == d.IdDetalleRetiro))
                .ToList();

            foreach (var det in detallesAEliminar)
            {
                _context.DetalleRetiros.Remove(det);
            }

            // Actualizar o agregar detalles
            foreach (var det in retiro.Detalles)
            {
                var existenteDet = existing.Detalles.FirstOrDefault(d => d.IdDetalleRetiro != 0 && d.IdDetalleRetiro == det.IdDetalleRetiro);
                if (existenteDet != null)
                {
                    existenteDet.IdAlumno = det.IdAlumno;
                    existenteDet.HoraSalida = det.HoraSalida;
                    existenteDet.Estado = det.Estado;
                }
                else
                {
                    det.IdRetiro = existing.IdRetiro;
                    existing.Detalles.Add(det);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var retiro = await _context.Retiros
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.IdRetiro == id);

            if (retiro == null) return false;

            _context.Retiros.Remove(retiro);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
