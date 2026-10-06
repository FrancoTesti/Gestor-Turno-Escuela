using System;

namespace GTE.DTOs
{
    /// <summary>
    /// Alumno que un tutor tiene a cargo, con el curso y el horario de salida.
    /// Lo usa la pantalla "Mis alumnos" del tutor.
    /// </summary>
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

        /// <summary>Verdadero si el curso del alumno está saliendo en este momento.</summary>
        public bool EstaSaliendo { get; set; }
    }
}
