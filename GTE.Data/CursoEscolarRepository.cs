using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class CursoEscolarRepository : ICursoEscolarRepository
    {
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

            existing.SetGrado(curso.Grado);
            existing.SetCurso(curso.Curso);
            existing.SetTurno(curso.Turno);
            existing.SetHorarioSalida(curso.HorarioSalida);

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
        }
    }
}
