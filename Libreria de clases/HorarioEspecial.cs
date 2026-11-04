using System;
using System.Collections.Generic;
using System.Linq;

namespace GTE.Dominio
{
    public class HorarioEspecial
    {
        public int IdHorarioEspecial { get; set; }
        public int IdAlumno { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public TimeSpan HoraSalidaEspecial { get; set; }

        public bool EsRecurrente { get; set; } = true;
        public string DiasSemana { get; set; } = string.Empty;
        public DateTime? FechaEspecifica { get; set; }

        private HorarioEspecial() { }

        public HorarioEspecial(int idHorario, int idAlu, string desc, TimeSpan horaSalidaEsp)
            : this(idHorario, idAlu, desc, horaSalidaEsp, true, string.Empty, null)
        {
        }

        public HorarioEspecial(int idHorario, int idAlu, string desc, TimeSpan horaSalidaEsp,
            bool esRecurrente, string diasSemana, DateTime? fechaEspecifica)
        {
            IdHorarioEspecial = idHorario;
            IdAlumno = idAlu;
            DescripcionActividad = desc;
            HoraSalidaEspecial = horaSalidaEsp;
            EsRecurrente = esRecurrente;
            DiasSemana = diasSemana ?? string.Empty;
            FechaEspecifica = fechaEspecifica;
        }

        public bool AplicaEnFecha(DateTime fecha)
        {
            if (!EsRecurrente)
            {
                return FechaEspecifica.HasValue && FechaEspecifica.Value.Date == fecha.Date;
            }

            if (string.IsNullOrWhiteSpace(DiasSemana))
                return true;

            string nombreDia = fecha.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(nombreDia))
                return false;

            return DiasSemana.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Any(d => d.Equals(nombreDia, StringComparison.OrdinalIgnoreCase) ||
                          (nombreDia == "Miércoles" && d.Equals("Miercoles", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
