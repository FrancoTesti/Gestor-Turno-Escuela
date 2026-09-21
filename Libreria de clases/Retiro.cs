using System;
using System.Collections.Generic;
using System.Linq;

namespace GTE.Dominio
{
    public class Retiro
    {
        public int IdRetiro { get; set; }
        public int IdTutor { get; set; }
        public int IdPersonal { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string Observaciones { get; set; } = string.Empty;

        public Tutor? Tutor { get; set; }
        public Personal? Personal { get; set; }

        public List<DetalleRetiro> Detalles { get; set; } = new List<DetalleRetiro>();

        public Retiro()
        {
        }

        public Retiro(int idTutor, int idPersonal, DateTime fechaHora, string observaciones = "")
        {
            SetIdTutor(idTutor);
            SetIdPersonal(idPersonal);
            SetFechaHora(fechaHora);
            Observaciones = observaciones ?? string.Empty;
        }

        public Retiro(int idRetiro, int idTutor, int idPersonal, DateTime fechaHora, string observaciones, List<DetalleRetiro>? detalles = null)
        {
            IdRetiro = idRetiro;
            SetIdTutor(idTutor);
            SetIdPersonal(idPersonal);
            SetFechaHora(fechaHora);
            Observaciones = observaciones ?? string.Empty;
            if (detalles != null)
            {
                foreach (var det in detalles)
                {
                    AgregarDetalle(det);
                }
            }
        }

        public void SetIdTutor(int idTutor)
        {
            if (idTutor <= 0)
                throw new ArgumentException("El identificador del tutor debe ser mayor a 0.", nameof(idTutor));
            IdTutor = idTutor;
        }

        public void SetIdPersonal(int idPersonal)
        {
            if (idPersonal <= 0)
                throw new ArgumentException("El identificador del personal debe ser mayor a 0.", nameof(idPersonal));
            IdPersonal = idPersonal;
        }

        public void SetFechaHora(DateTime fechaHora)
        {
            if (fechaHora > DateTime.Now.AddMinutes(5))
                throw new ArgumentException("La fecha y hora del retiro no puede ser futura.", nameof(fechaHora));
            FechaHora = fechaHora;
        }

        public void AgregarDetalle(DetalleRetiro detalle)
        {
            ArgumentNullException.ThrowIfNull(detalle);

            if (Detalles.Any(d => d.IdAlumno == detalle.IdAlumno))
                throw new InvalidOperationException($"El alumno con ID {detalle.IdAlumno} ya está agregado a este retiro.");

            Detalles.Add(detalle);
        }

        public bool QuitarDetalle(int idAlumno)
        {
            var existente = Detalles.FirstOrDefault(d => d.IdAlumno == idAlumno);
            if (existente != null)
            {
                return Detalles.Remove(existente);
            }
            return false;
        }

        public void Validar()
        {
            if (IdTutor <= 0)
                throw new InvalidOperationException("El retiro debe tener un tutor válido asignado.");

            if (IdPersonal <= 0)
                throw new InvalidOperationException("El retiro debe tener un personal válido asignado.");

            if (FechaHora > DateTime.Now.AddMinutes(5))
                throw new InvalidOperationException("La fecha del retiro no puede ser futura.");

            if (Detalles == null || Detalles.Count == 0)
                throw new InvalidOperationException("El retiro debe contener al menos un alumno.");
        }
    }
}
