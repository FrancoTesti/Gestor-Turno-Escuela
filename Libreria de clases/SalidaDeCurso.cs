using System;

namespace GTE.Dominio
{
    /// <summary>
    /// Salida de un curso por la puerta. Mientras no tenga hora de fin, el curso
    /// está saliendo: eso es lo que se muestra en la pantalla de la puerta.
    /// </summary>
    public class SalidaDeCurso
    {
        public int IdSalidaDeCurso { get; private set; }
        public int IdCurso { get; private set; }
        public CursoEscolar CursoEscolar { get; private set; } = null!;
        public int IdPersonal { get; private set; }
        public Personal Personal { get; private set; } = null!;
        public DateTime FechaHoraInicio { get; private set; }
        public DateTime? FechaHoraFin { get; private set; }

        /// <summary>Verdadero mientras el curso no terminó de salir.</summary>
        public bool EstaEnCurso => FechaHoraFin is null;

        // Constructor privado sin parámetros requerido por Entity Framework
        // para la materialización de entidades.
        private SalidaDeCurso() { }

        public SalidaDeCurso(CursoEscolar curso, Personal personal, DateTime fechaHoraInicio)
        {
            SetCursoEscolar(curso);
            SetPersonal(personal);
            SetFechaHoraInicio(fechaHoraInicio);
        }

        public void SetIdSalidaDeCurso(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual a 0.", nameof(id));
            IdSalidaDeCurso = id;
        }

        public void SetCursoEscolar(CursoEscolar cursoEscolar)
        {
            ArgumentNullException.ThrowIfNull(cursoEscolar);
            CursoEscolar = cursoEscolar;
            IdCurso = cursoEscolar.IdCurso;
        }

        public void SetPersonal(Personal personal)
        {
            ArgumentNullException.ThrowIfNull(personal);
            Personal = personal;
            IdPersonal = personal.IdPersonal;
        }

        public void SetFechaHoraInicio(DateTime fechaHoraInicio)
        {
            if (fechaHoraInicio > DateTime.Now.AddMinutes(5))
                throw new ArgumentException("La salida del curso no puede empezar en el futuro.", nameof(fechaHoraInicio));
            FechaHoraInicio = fechaHoraInicio;
        }

        /// <summary>Marca que el curso terminó de salir.</summary>
        public void Finalizar(DateTime fechaHoraFin)
        {
            if (!EstaEnCurso)
                throw new InvalidOperationException("La salida de este curso ya estaba finalizada.");

            if (fechaHoraFin < FechaHoraInicio)
                throw new ArgumentException("La salida no puede terminar antes de haber empezado.", nameof(fechaHoraFin));

            FechaHoraFin = fechaHoraFin;
        }
    }
}
