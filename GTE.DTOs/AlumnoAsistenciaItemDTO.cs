namespace GTE.DTOs
{
    public class AlumnoAsistenciaItemDTO
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string EstadoGeneral { get; set; } = "Presente";
        public int? IdInasistencia { get; set; }
        public int? IdTipoInasistencia { get; set; }
        public string CodigoOficial { get; set; } = "P";
        public bool Justificada { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public bool NoComputa { get; set; }
        public string Observaciones { get; set; } = string.Empty;

        public bool TieneRetiroRegistrado { get; set; }
        public string? DetalleRetiroInfo { get; set; }
        public int? IdRetiroOrigen { get; set; }
    }
}
