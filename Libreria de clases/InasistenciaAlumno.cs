using System;

namespace GTE.Dominio
{
    public class InasistenciaAlumno
    {
        public int IdInasistencia { get; private set; }
        public int IdAlumno { get; private set; }
        public Alumno? Alumno { get; private set; }
        public DateTime Fecha { get; private set; }
        public int IdTipoInasistencia { get; private set; }
        public TipoInasistencia? TipoInasistencia { get; private set; }
        public bool Justificada { get; private set; }
        public string Motivo { get; private set; } = string.Empty;
        public bool NoComputa { get; private set; }
        public string Observaciones { get; private set; } = string.Empty;
        public int? IdRetiroOrigen { get; private set; }

        private InasistenciaAlumno() { }

        public InasistenciaAlumno(
            int idAlumno,
            DateTime fecha,
            int idTipoInasistencia,
            bool justificada = false,
            string motivo = "",
            bool noComputa = false,
            string observaciones = "",
            int? idRetiroOrigen = null)
        {
            if (idAlumno <= 0) throw new ArgumentException("IdAlumno debe ser mayor a 0.", nameof(idAlumno));
            if (idTipoInasistencia <= 0) throw new ArgumentException("IdTipoInasistencia debe ser mayor a 0.", nameof(idTipoInasistencia));

            IdAlumno = idAlumno;
            Fecha = fecha.Date;
            IdTipoInasistencia = idTipoInasistencia;
            Justificada = justificada;
            Motivo = motivo ?? string.Empty;
            NoComputa = noComputa;
            Observaciones = observaciones ?? string.Empty;
            IdRetiroOrigen = idRetiroOrigen;
        }

        public InasistenciaAlumno(
            int idInasistencia,
            int idAlumno,
            DateTime fecha,
            int idTipoInasistencia,
            bool justificada = false,
            string motivo = "",
            bool noComputa = false,
            string observaciones = "",
            int? idRetiroOrigen = null)
            : this(idAlumno, fecha, idTipoInasistencia, justificada, motivo, noComputa, observaciones, idRetiroOrigen)
        {
            IdInasistencia = idInasistencia;
        }

        public void Actualizar(int idTipoInasistencia, bool justificada, string motivo, bool noComputa, string observaciones)
        {
            Actualizar(idTipoInasistencia, justificada, motivo, noComputa, observaciones, IdRetiroOrigen);
        }

        public void Actualizar(int idTipoInasistencia, bool justificada, string motivo, bool noComputa, string observaciones, int? idRetiroOrigen)
        {
            if (idTipoInasistencia <= 0) throw new ArgumentException("IdTipoInasistencia debe ser mayor a 0.", nameof(idTipoInasistencia));
            IdTipoInasistencia = idTipoInasistencia;
            Justificada = justificada;
            Motivo = motivo ?? string.Empty;
            NoComputa = noComputa;
            Observaciones = observaciones ?? string.Empty;
            IdRetiroOrigen = idRetiroOrigen;
        }
    }
}
