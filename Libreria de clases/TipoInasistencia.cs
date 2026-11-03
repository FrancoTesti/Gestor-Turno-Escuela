using System;

namespace GTE.Dominio
{
    public class TipoInasistencia
    {
        public int IdTipoInasistencia { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public decimal ValorDecimal { get; private set; }
        public string CodigoOficial { get; private set; } = string.Empty;
        public bool EsActivo { get; private set; } = true;

        private TipoInasistencia() { }

        public TipoInasistencia(int idTipoInasistencia, string nombre, decimal valorDecimal, string codigoOficial, bool esActivo = true)
        {
            IdTipoInasistencia = idTipoInasistencia;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            ValorDecimal = valorDecimal;
            CodigoOficial = codigoOficial ?? throw new ArgumentNullException(nameof(codigoOficial));
            EsActivo = esActivo;
        }

        public void Modificar(string nombre, decimal valorDecimal, string codigoOficial, bool esActivo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser vacío.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(codigoOficial))
                throw new ArgumentException("El código oficial no puede ser vacío.", nameof(codigoOficial));

            Nombre = nombre;
            ValorDecimal = valorDecimal;
            CodigoOficial = codigoOficial;
            EsActivo = esActivo;
        }
    }
}
