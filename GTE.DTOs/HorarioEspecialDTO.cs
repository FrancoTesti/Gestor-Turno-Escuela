using System;

namespace GTE.DTOs
{
    public class HorarioEspecialDTO
    {
        public int IdHorarioEspecial { get; set; }
        public int IdAlumno { get; set; }
        public string? AlumnoNombreCompleto { get; set; }
        public string? Curso { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public TimeSpan HoraSalidaEspecial { get; set; }

        public bool EsRecurrente { get; set; } = true;
        public string DiasSemana { get; set; } = string.Empty;
        public DateTime? FechaEspecifica { get; set; }

        public string DiasODiasTexto =>
            !EsRecurrente
                ? (FechaEspecifica.HasValue ? $"Fecha: {FechaEspecifica.Value:dd/MM/yyyy}" : "Fecha no especificada")
                : (string.IsNullOrWhiteSpace(DiasSemana) ? "Todos los días de clase" : DiasSemana);
    }
}
