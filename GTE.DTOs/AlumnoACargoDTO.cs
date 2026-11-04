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
        public TimeSpan HorarioLunes { get; set; }
        public TimeSpan HorarioMartes { get; set; }
        public TimeSpan HorarioMiercoles { get; set; }
        public TimeSpan HorarioJueves { get; set; }
        public TimeSpan HorarioViernes { get; set; }
        public string Estado { get; set; } = string.Empty;

        public bool EstaSaliendo { get; set; }
    }
}
