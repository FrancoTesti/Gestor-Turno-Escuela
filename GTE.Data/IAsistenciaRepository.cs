using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.Dominio;

namespace GTE.Data
{
    public interface IAsistenciaRepository
    {
        Task<IEnumerable<TipoInasistencia>> GetTiposInasistenciaAsync(bool soloActivos = true);
        Task<TipoInasistencia?> GetTipoInasistenciaPorIdAsync(int id);
        Task<TipoInasistencia?> GetTipoInasistenciaPorCodigoAsync(string codigo);
        Task<PlanillaAsistenciaDiaria?> GetPlanillaDiariaAsync(int idCurso, DateTime fecha);
        Task GuardarOActualizarPlanillaAsync(PlanillaAsistenciaDiaria planilla);
        Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorCursoYFechaAsync(int idCurso, DateTime fecha);
        Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorCursoYRangoAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta);
        Task<IEnumerable<InasistenciaAlumno>> GetInasistenciasPorAlumnoAsync(int idAlumno);
        Task<InasistenciaAlumno?> GetInasistenciaPorAlumnoYFechaAsync(int idAlumno, DateTime fecha);
        Task GuardarPlanillaConInasistenciasAsync(PlanillaAsistenciaDiaria planilla, IEnumerable<InasistenciaAlumno> inasistencias);
        Task<IEnumerable<DetalleRetiro>> GetRetirosPorCursoYFechaAsync(int idCurso, DateTime fecha);
    }
}
