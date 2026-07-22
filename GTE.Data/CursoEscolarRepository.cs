using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.Dominio;
namespace GTE.Data
{
    public class CursoEscolarRepository : ICursoEscolarRepository
    {
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

            existing.Grado = curso.Grado;
            existing.Curso = curso.Curso;
            existing.HorarioSalida = curso.HorarioSalida;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var curso = _cursos.FirstOrDefault(c => c.IdCurso == id);
            if (curso == null) return Task.FromResult(false);
            _cursos.Remove(curso);
            return Task.FromResult(true);
        }
    }
}