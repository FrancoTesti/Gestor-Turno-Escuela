using System;

namespace GTE.Dominio
{
    public class DetalleRetiro
    {
        public int IdDetalleRetiro { get; set; }
        public int IdRetiro { get; set; }
        public int IdAlumno { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string Estado { get; set; } = "Retirado";

        public Alumno? Alumno { get; set; }

        public DetalleRetiro()
        {
        }

        public DetalleRetiro(int idAlumno, TimeSpan horaSalida, string estado = "Retirado")
        {
            SetIdAlumno(idAlumno);
            HoraSalida = horaSalida;
            SetEstado(estado);
        }

        public DetalleRetiro(int idDetalleRetiro, int idRetiro, int idAlumno, TimeSpan horaSalida, string estado)
        {
            IdDetalleRetiro = idDetalleRetiro;
            IdRetiro = idRetiro;
            SetIdAlumno(idAlumno);
            HoraSalida = horaSalida;
            SetEstado(estado);
        }

        public void SetIdAlumno(int idAlumno)
        {
            if (idAlumno <= 0)
                throw new ArgumentException("El identificador del alumno debe ser mayor a 0.", nameof(idAlumno));
            IdAlumno = idAlumno;
        }

        public void SetEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado no puede ser nulo o vacío.", nameof(estado));
            Estado = estado;
        }
    }
}
