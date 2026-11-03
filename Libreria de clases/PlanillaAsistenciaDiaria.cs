using System;

namespace GTE.Dominio
{
    public class PlanillaAsistenciaDiaria
    {
        public int IdPlanilla { get; private set; }
        public int IdCurso { get; private set; }
        public CursoEscolar? CursoEscolar { get; private set; }
        public DateTime Fecha { get; private set; }
        public bool Confirmada { get; private set; }
        public DateTime FechaHoraRegistro { get; private set; }
        public int? IdPersonalRegistrador { get; private set; }

        private PlanillaAsistenciaDiaria() { }

        public PlanillaAsistenciaDiaria(int idCurso, DateTime fecha, bool confirmada, int? idPersonalRegistrador = null)
        {
            if (idCurso <= 0) throw new ArgumentException("IdCurso debe ser mayor a 0.", nameof(idCurso));
            IdCurso = idCurso;
            Fecha = fecha.Date;
            Confirmada = confirmada;
            FechaHoraRegistro = DateTime.Now;
            IdPersonalRegistrador = idPersonalRegistrador;
        }

        public PlanillaAsistenciaDiaria(int idPlanilla, int idCurso, DateTime fecha, bool confirmada, DateTime fechaHoraRegistro, int? idPersonalRegistrador = null)
        {
            IdPlanilla = idPlanilla;
            IdCurso = idCurso;
            Fecha = fecha.Date;
            Confirmada = confirmada;
            FechaHoraRegistro = fechaHoraRegistro;
            IdPersonalRegistrador = idPersonalRegistrador;
        }

        public void MarcarConfirmada(int? idPersonalRegistrador = null)
        {
            Confirmada = true;
            FechaHoraRegistro = DateTime.Now;
            if (idPersonalRegistrador.HasValue)
                IdPersonalRegistrador = idPersonalRegistrador.Value;
        }
    }
}
