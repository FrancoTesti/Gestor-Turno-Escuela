using System;

namespace GTE.DTOs
{
    public class DetalleRetiroDTO
    {
        public int IdDetalleRetiro { get; set; }
        public int IdRetiro { get; set; }
        public int IdAlumno { get; set; }
        public string? AlumnoNombreCompleto { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public string Estado { get; set; } = "Retirado";
    }
}
