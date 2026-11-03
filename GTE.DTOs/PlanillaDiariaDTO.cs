using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    public class PlanillaDiariaDTO
    {
        public int IdCurso { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public bool Confirmada { get; set; }
        public DateTime? FechaHoraRegistro { get; set; }
        public List<AlumnoAsistenciaItemDTO> Alumnos { get; set; } = new();
        public List<TipoInasistenciaDTO> TiposDisponibles { get; set; } = new();
    }
}
