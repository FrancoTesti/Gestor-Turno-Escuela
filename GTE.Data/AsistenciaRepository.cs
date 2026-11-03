using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GTE.Dominio;

namespace GTE.Data
{
    public class AsistenciaRepository : IAsistenciaRepository
    {
        private readonly GTEContext _context;

        public AsistenciaRepository(GTEContext context)
        {
            _context = context;
        }

        public AsistenciaRepository() : this(new GTEContext())
        {
        }

        public async Task<IEnumerable<TipoInasistencia>> GetTiposInasistenciaAsync(bool soloActivos = true)
        {
            return await _context.TiposInasistencia
                .Where(t => !soloActivos || t.EsActivo)
                .OrderBy(t => t.IdTipoInasistencia)
                .ToListAsync();
        }

        public async Task<TipoInasistencia?> GetTipoInasistenciaPorIdAsync(int id)
        {
            return await _context.TiposInasistencia.FindAsync(id);
        }

        public async Task<TipoInasistencia?> GetTipoInasistenciaPorCodigoAsync(string codigo)
        {
            return await _context.TiposInasistencia
                .FirstOrDefaultAsync(t => t.CodigoOficial == codigo);
        }

        public async Task<PlanillaAsistenciaDiaria?> GetPlanillaDiariaAsync(int idCurso, DateTime fecha)
        {
            var date = fecha.Date;
            return await _context.PlanillasAsistenciasDiarias
                .Include(p => p.CursoEscolar)
                .FirstOrDefaultAsync(p => p.IdCurso == idCurso && p.Fecha == date);
        }

        public async Task GuardarOActualizarPlanillaAsync(PlanillaAsistenciaDiaria planilla)
        {
            var date = planilla.Fecha.Date;
            var existing = await _context.PlanillasAsistenciasDiarias
                .FirstOrDefaultAsync(p => p.IdCurso == planilla.IdCurso && p.Fecha == date);

            if (existing == null)
            {
                await _context.PlanillasAsistenciasDiarias.AddAsync(planilla);
            }
            else
            {
                existing.MarcarConfirmada(planilla.IdPersonalRegistrador);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorCursoYFechaAsync(int idCurso, DateTime fecha)
        {
            var date = fecha.Date;
            return await _context.InasistenciasAlumnos
                .Include(i => i.Alumno)
                .Include(i => i.TipoInasistencia)
                .Where(i => i.Fecha == date && _context.Alumnos.Any(a => a.IdAlumno == i.IdAlumno && a.IdCurso == idCurso))
                .ToListAsync();
        }

        public async Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorCursoYRangoAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta)
        {
            var desde = fechaDesde.Date;
            var hasta = fechaHasta.Date;
            return await _context.InasistenciasAlumnos
                .Include(i => i.Alumno)
                .Include(i => i.TipoInasistencia)
                .Where(i => i.Fecha >= desde && i.Fecha <= hasta && _context.Alumnos.Any(a => a.IdAlumno == i.IdAlumno && a.IdCurso == idCurso))
                .OrderBy(i => i.Fecha)
                .ThenBy(i => i.Alumno != null ? i.Alumno.Apellido : string.Empty)
                .ThenBy(i => i.Alumno != null ? i.Alumno.Nombre : string.Empty)
                .ToListAsync();
        }

        public async Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorAlumnoAsync(int idAlumno)
        {
            return await _context.InasistenciasAlumnos
                .Include(i => i.TipoInasistencia)
                .Where(i => i.IdAlumno == idAlumno)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();
        }

        public async Task<InasistenciaAlumno?> GetInasistenciaPorAlumnoYFechaAsync(int idAlumno, DateTime fecha)
        {
            var date = fecha.Date;
            return await _context.InasistenciasAlumnos
                .Include(i => i.TipoInasistencia)
                .FirstOrDefaultAsync(i => i.IdAlumno == idAlumno && i.Fecha == date);
        }

        public async Task GuardarPlanillaConInasistenciasAsync(PlanillaAsistenciaDiaria planilla, IEnumerable<InasistenciaAlumno> inasistencias)
        {
            var date = planilla.Fecha.Date;

            // 1. Planilla
            var existingPlanilla = await _context.PlanillasAsistenciasDiarias
                .FirstOrDefaultAsync(p => p.IdCurso == planilla.IdCurso && p.Fecha == date);

            if (existingPlanilla == null)
            {
                await _context.PlanillasAsistenciasDiarias.AddAsync(planilla);
            }
            else
            {
                existingPlanilla.MarcarConfirmada(planilla.IdPersonalRegistrador);
            }

            // 2. Inasistencias de alumnos del curso en la fecha
            var existingInasistencias = await _context.InasistenciasAlumnos
                .Where(i => i.Fecha == date && _context.Alumnos.Any(a => a.IdAlumno == i.IdAlumno && a.IdCurso == planilla.IdCurso))
                .ToListAsync();

            var nuevasDict = inasistencias.ToDictionary(i => i.IdAlumno);

            // Eliminar las que ya no están (alumnos que ahora figuran "Presente")
            foreach (var ex in existingInasistencias)
            {
                if (!nuevasDict.ContainsKey(ex.IdAlumno))
                {
                    _context.InasistenciasAlumnos.Remove(ex);
                }
            }

            // Agregar o actualizar las especificadas
            foreach (var kvp in nuevasDict)
            {
                var idAlumno = kvp.Key;
                var nueva = kvp.Value;
                var ex = existingInasistencias.FirstOrDefault(e => e.IdAlumno == idAlumno);

                if (ex != null)
                {
                    ex.Actualizar(nueva.IdTipoInasistencia, nueva.Justificada, nueva.Motivo, nueva.NoComputa, nueva.Observaciones, nueva.IdRetiroOrigen);
                }
                else
                {
                    await _context.InasistenciasAlumnos.AddAsync(nueva);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DetalleRetiro>> GetRetirosPorCursoYFechaAsync(int idCurso, DateTime fecha)
        {
            var date = fecha.Date;
            var retiroIdsDelDia = await _context.Retiros
                .Where(r => r.FechaHora.Date == date)
                .Select(r => r.IdRetiro)
                .ToListAsync();

            if (!retiroIdsDelDia.Any())
                return Enumerable.Empty<DetalleRetiro>();

            return await _context.DetalleRetiros
                .Include(d => d.Alumno)
                .Where(d => retiroIdsDelDia.Contains(d.IdRetiro) && _context.Alumnos.Any(a => a.IdAlumno == d.IdAlumno && a.IdCurso == idCurso))
                .ToListAsync();
        }
    }
}
