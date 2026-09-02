using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
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

            existing.SetNombre(alumno.Nombre);
            existing.SetApellido(alumno.Apellido);
            existing.SetGrado(alumno.Grado);
            existing.SetCurso(alumno.Curso);
            existing.SetEstado(alumno.Estado);

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
