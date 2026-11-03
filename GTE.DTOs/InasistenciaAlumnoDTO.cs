using System;

namespace GTE.DTOs
{
    public class InasistenciaAlumnoDTO
    {
        public int IdInasistencia { get; set; }
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; } = string.Empty;
        public string ApellidoAlumno { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int IdTipoInasistencia { get; set; }
        public string NombreTipoInasistencia { get; set; } = string.Empty;
        public string CodigoOficial { get; set; } = string.Empty;
        public decimal ValorDecimal { get; set; }
        public bool Justificada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public bool NoComputa { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? IdRetiroOrigen { get; set; }
    }
}
