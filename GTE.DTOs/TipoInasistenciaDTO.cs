namespace GTE.DTOs
{
    public class TipoInasistenciaDTO
    {
        public int IdTipoInasistencia { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal ValorDecimal { get; set; }
        public string CodigoOficial { get; set; } = string.Empty;
        public bool EsActivo { get; set; }
    }
}
