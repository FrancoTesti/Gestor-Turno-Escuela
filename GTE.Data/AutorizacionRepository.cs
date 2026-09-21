using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class AutorizacionRepository : IAutorizacionRepository
    {
        private readonly GTEContext _context;

        public AutorizacionRepository(GTEContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autorizacion>> GetAllAsync()
        {
            return await _context.Autorizaciones.ToListAsync();
        }

        public async Task<IEnumerable<Autorizacion>> GetByTutorIdAsync(int tutorId)
        {
            return await _context.Autorizaciones
                .Where(a => a.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alumno>> GetAlumnosByTutorIdAsync(int tutorId)
        {
            var alumnoIds = await _context.Autorizaciones
                .Where(a => a.TutorId == tutorId)
                .Select(a => a.AlumnoId)
                .ToListAsync();

            return await _context.Alumnos
                .Include(a => a.CursoEscolar)
                .Where(a => alumnoIds.Contains(a.IdAlumno))
                .ToListAsync();
        }

        public async Task<bool> EstaAutorizadoAsync(int tutorId, int alumnoId)
        {
            return await _context.Autorizaciones
                .AnyAsync(a => a.TutorId == tutorId && a.AlumnoId == alumnoId);
        }

        public async Task AddAsync(Autorizacion autorizacion)
        {
            await _context.Autorizaciones.AddAsync(autorizacion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var autorizacion = await _context.Autorizaciones.FindAsync(id);
            if (autorizacion == null) return false;

            _context.Autorizaciones.Remove(autorizacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
