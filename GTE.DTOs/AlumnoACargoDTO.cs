using System;

namespace GTE.DTOs
{
    public class AlumnoACargoDTO
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Grado { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public TimeSpan HorarioSalida { get; set; }
        public string Estado { get; set; } = string.Empty;

        public bool EstaSaliendo { get; set; }
    }
}
