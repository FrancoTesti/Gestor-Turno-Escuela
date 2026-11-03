using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Clients
{
    public interface IAsistenciaApiClient
    {
        Task<List<TipoInasistenciaDTO>> GetTiposInasistenciaAsync();
        Task<PlanillaDiariaDTO?> GetPlanillaDiariaAsync(int idCurso, DateTime fecha);
        Task<bool> GuardarPlanillaDiariaAsync(GuardarPlanillaAsistenciaRequestDTO request);
        Task<List<AlumnoAsistenciaItemDTO>> SugerirInasistenciasDesdeRetirosAsync(int idCurso, DateTime fecha);
        Task<List<ReporteNovedadItemDTO>> GetReporteNovedadesAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta);
        Task<ReporteEspejoSemanaDTO?> GetReporteEspejoSemanalAsync(int idCurso, DateTime fechaSemana);
        Task<HistorialAsistenciaAlumnoDTO?> GetHistorialAlumnoAsync(int idAlumno);
    }
}
