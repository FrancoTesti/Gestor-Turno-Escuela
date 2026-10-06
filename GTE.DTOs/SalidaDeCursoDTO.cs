using System;

namespace GTE.DTOs
{
    /// <summary>
    /// Salida de un curso, con todo lo que necesita el portero para saber qué
    /// está saliendo y quién lo marcó. Solo la ven los usuarios del sistema.
    /// </summary>
    public class SalidaDeCursoDTO
    {
        public int IdSalidaDeCurso { get; set; }
        public int IdCurso { get; set; }
        public string Grado { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public TimeSpan HorarioSalida { get; set; }
        public string Puerta { get; set; } = string.Empty;
        public string PersonalNombre { get; set; } = string.Empty;
        public DateTime FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public int CantidadDeAlumnos { get; set; }

        public bool EstaEnCurso => FechaHoraFin is null;
    }
}
