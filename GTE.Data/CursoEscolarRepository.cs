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
    public class CursoEscolarRepository : ICursoEscolarRepository
    {
<<<<<<< HEAD
        private readonly GTEContext _context;

        public CursoEscolarRepository(GTEContext context)
        {
            _context = context;
        }

        public CursoEscolarRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(CursoEscolar curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
        }

        public async Task<CursoEscolar?> GetAsync(int id) =>
            await _context.Cursos.FindAsync(id);

        public async Task<IEnumerable<CursoEscolar>> GetAllAsync() =>
            await _context.Cursos.ToListAsync();

        public async Task<bool> UpdateAsync(CursoEscolar curso)
        {
            var existing = await _context.Cursos.FindAsync(curso.IdCurso);
            if (existing == null) return false;
=======
        private static List<CursoEscolar> _cursos = new List<CursoEscolar>();
        private static int _nextId = 1;

        public Task AddAsync(CursoEscolar curso)
        {
            curso.IdCurso = _nextId++;
            _cursos.Add(curso);
            return Task.CompletedTask;
        }

        public Task<CursoEscolar?> GetAsync(int id) =>
            Task.FromResult(_cursos.FirstOrDefault(c => c.IdCurso == id));

        public Task<IEnumerable<CursoEscolar>> GetAllAsync() =>
            Task.FromResult<IEnumerable<CursoEscolar>>(_cursos.ToList());

        public Task<bool> UpdateAsync(CursoEscolar curso)
        {
            var existing = _cursos.FirstOrDefault(c => c.IdCurso == curso.IdCurso);
            if (existing == null) return Task.FromResult(false);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

            existing.Grado = curso.Grado;
            existing.Curso = curso.Curso;
            existing.HorarioSalida = curso.HorarioSalida;
<<<<<<< HEAD

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return false;
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return true;
=======
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var curso = _cursos.FirstOrDefault(c => c.IdCurso == id);
            if (curso == null) return Task.FromResult(false);
            _cursos.Remove(curso);
            return Task.FromResult(true);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        }
    }
}