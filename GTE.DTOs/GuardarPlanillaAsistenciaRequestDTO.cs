using System;
using System.Collections.Generic;

namespace GTE.DTOs
{
    public class GuardarPlanillaAsistenciaRequestDTO
    {
        public int IdCurso { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdPersonal { get; set; }
        public List<InasistenciaInputDTO> Inasistencias { get; set; } = new();
    }

    public class InasistenciaInputDTO
    {
        public int IdAlumno { get; set; }
        public int IdTipoInasistencia { get; set; }
        public bool Justificada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public bool NoComputa { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? IdRetiroOrigen { get; set; }
    }
}
