using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IAsistenciaService
    {
        Task<PlanillaDiariaDTO> GetPlanillaDiariaAsync(int idCurso, DateTime fecha);
        Task<(bool Exito, string Mensaje)> GuardarPlanillaDiariaAsync(GuardarPlanillaAsistenciaRequestDTO request);
        Task<List<AlumnoAsistenciaItemDTO>> SugerirInasistenciasDesdeRetirosAsync(int idCurso, DateTime fecha);
        Task<List<ReporteNovedadItemDTO>> GetReporteNovedadesAsync(int idCurso, DateTime fechaDesde, DateTime fechaHasta);
        Task<ReporteEspejoSemanaDTO> GetReporteEspejoSemanalAsync(int idCurso, DateTime fechaSemana);
        Task<HistorialAsistenciaAlumnoDTO> GetHistorialAlumnoAsync(int idAlumno);
        Task<List<TipoInasistenciaDTO>> GetTiposInasistenciaAsync();
    }
}
