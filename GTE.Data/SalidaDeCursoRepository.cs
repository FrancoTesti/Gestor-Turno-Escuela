using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class SalidaDeCursoRepository : ISalidaDeCursoRepository
    {
        private readonly GTEContext _context;

        public SalidaDeCursoRepository(GTEContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalidaDeCurso>> GetDelDiaAsync(DateTime dia)
        {
            return await _context.SalidasDeCurso
                .Include(s => s.CursoEscolar)
                .Include(s => s.Personal)
                .Where(s => s.FechaHoraInicio.Date == dia.Date)
                .OrderBy(s => s.FechaHoraInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalidaDeCurso>> GetEnCursoAsync()
        {
            return await _context.SalidasDeCurso
                .Include(s => s.CursoEscolar)
                .Include(s => s.Personal)
                .Where(s => s.FechaHoraFin == null)
                .OrderBy(s => s.FechaHoraInicio)
                .ToListAsync();
        }

        public async Task<SalidaDeCurso?> GetAsync(int id)
        {
            return await _context.SalidasDeCurso
                .Include(s => s.CursoEscolar)
                .Include(s => s.Personal)
                .FirstOrDefaultAsync(s => s.IdSalidaDeCurso == id);
        }

        public async Task AddAsync(SalidaDeCurso salida)
        {
            await _context.SalidasDeCurso.AddAsync(salida);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(SalidaDeCurso salida)
        {
            var existente = await _context.SalidasDeCurso
                .FirstOrDefaultAsync(s => s.IdSalidaDeCurso == salida.IdSalidaDeCurso);

            if (existente == null) return false;

            if (existente.EstaEnCurso && !salida.EstaEnCurso)
                existente.Finalizar(salida.FechaHoraFin!.Value);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
