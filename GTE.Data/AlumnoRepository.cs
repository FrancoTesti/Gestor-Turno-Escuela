using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio;
namespace GTE.Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private static readonly List<Alumno> _alumnos = new List<Alumno>();
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

            existing.SetNombre(alumno.Nombre);
            existing.SetApellido(alumno.Apellido);
            existing.SetGrado(alumno.Grado);
            existing.SetCurso(alumno.Curso);
            existing.SetEstado(alumno.Estado);
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

