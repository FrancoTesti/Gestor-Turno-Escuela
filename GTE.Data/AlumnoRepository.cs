using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

=======
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
namespace GTE.Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
<<<<<<< HEAD
        private readonly GTEContext _context;

        public AlumnoRepository(GTEContext context)
        {
            _context = context;
        }

        public AlumnoRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Alumno alumno)
        {
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
        }

        public async Task<Alumno?> GetAsync(int id) =>
            await _context.Alumnos.FindAsync(id);

        public async Task<IEnumerable<Alumno>> GetAllAsync() =>
            await _context.Alumnos.ToListAsync();

        public async Task<bool> UpdateAsync(Alumno alumno)
        {
            var existing = await _context.Alumnos.FindAsync(alumno.IdAlumno);
            if (existing == null) return false;
=======
        private static List<Alumno> _alumnos = new List<Alumno>();
        private static int _nextId = 1;

        public Task AddAsync(Alumno alumno)
        {
            alumno.SetIdAlumno(_nextId++);
            _alumnos.Add(alumno);
            return Task.CompletedTask;
        }

        public Task<Alumno?> GetAsync(int id) =>
            Task.FromResult(_alumnos.FirstOrDefault(a => a.IdAlumno == id));

        public Task<IEnumerable<Alumno>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Alumno>>(_alumnos.ToList());

        public Task<bool> UpdateAsync(Alumno alumno)
        {
            var existing = _alumnos.FirstOrDefault(a => a.IdAlumno == alumno.IdAlumno);
            if (existing == null) return Task.FromResult(false);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

            existing.SetNombre(alumno.Nombre);
            existing.SetApellido(alumno.Apellido);
            existing.SetGrado(alumno.Grado);
            existing.SetCurso(alumno.Curso);
            existing.SetEstado(alumno.Estado);
<<<<<<< HEAD

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return false;
            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria)
        {
            IQueryable<Alumno> query = _context.Alumnos;

            if (!string.IsNullOrWhiteSpace(criteria.Nombre))
            {
                var term = criteria.Nombre.ToLower();
                query = query.Where(a => a.Nombre.ToLower().Contains(term) || a.Apellido.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Grado))
            {
                query = query.Where(a => a.Grado == criteria.Grado);
            }

            if (!string.IsNullOrWhiteSpace(criteria.Curso))
            {
                query = query.Where(a => a.Curso == criteria.Curso);
            }

            if (!string.IsNullOrWhiteSpace(criteria.Estado))
            {
                query = query.Where(a => a.Estado == criteria.Estado);
            }

            return await query.OrderBy(a => a.Apellido)
                              .ThenBy(a => a.Nombre)
                              .ToListAsync();
        }
    }
}
=======
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var alumno = _alumnos.FirstOrDefault(a => a.IdAlumno == id);
            if (alumno == null) return Task.FromResult(false);
            _alumnos.Remove(alumno);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria)
        {
            IEnumerable<Alumno> query = _alumnos;

            if (!string.IsNullOrWhiteSpace(criteria.Nombre))
                query = query.Where(a =>
                    a.Nombre.Contains(criteria.Nombre, StringComparison.OrdinalIgnoreCase) ||
                    a.Apellido.Contains(criteria.Nombre, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(criteria.Grado))
                query = query.Where(a => a.Grado.Equals(criteria.Grado, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(criteria.Curso))
                query = query.Where(a => a.Curso.Equals(criteria.Curso, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(criteria.Estado))
                query = query.Where(a => a.Estado.Equals(criteria.Estado, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult<IEnumerable<Alumno>>(
                query.OrderBy(a => a.Apellido).ThenBy(a => a.Nombre).ToList());
        }
    }
}

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
