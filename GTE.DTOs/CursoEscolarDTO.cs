using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class CursoEscolarDTO
    {
        public int IdCurso { get; set; }
        public string Grado { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Turno { get; set; } = "Mañana";
        /// <summary>
        /// Horario de salida efectivo para el día actual o la fecha consultada.
        /// Mantiene compatibilidad con componentes y vistas vinculadas a HorarioSalida.
        /// </summary>
        public TimeSpan HorarioSalida { get; set; }

        public TimeSpan HorarioSalidaLunes { get; set; }
        public TimeSpan HorarioSalidaMartes { get; set; }
        public TimeSpan HorarioSalidaMiercoles { get; set; }
        public TimeSpan HorarioSalidaJueves { get; set; }
        public TimeSpan HorarioSalidaViernes { get; set; }

        /// <summary>
        /// Obtiene el horario de salida para un día específico de la semana.
        /// Si se consulta fin de semana (sábado/domingo), recurre al horario del lunes.
        /// Si el horario específico del día no está asignado (TimeSpan.Zero), recurre a HorarioSalida.
        /// </summary>
        /// <param name="diaSemana">Día de la semana a consultar.</param>
        /// <returns>Horario de salida correspondiente.</returns>
        public TimeSpan ObtenerHorarioPorDia(DayOfWeek diaSemana)
        {
            var horario = diaSemana switch
            {
                DayOfWeek.Monday => HorarioSalidaLunes,
                DayOfWeek.Tuesday => HorarioSalidaMartes,
                DayOfWeek.Wednesday => HorarioSalidaMiercoles,
                DayOfWeek.Thursday => HorarioSalidaJueves,
                DayOfWeek.Friday => HorarioSalidaViernes,
                _ => HorarioSalidaLunes
            };

            return horario != TimeSpan.Zero ? horario : HorarioSalida;
        }

        /// <summary>
        /// Sobrecarga auxiliar para consultar horario por fecha (DateTime).
        /// </summary>
        public TimeSpan ObtenerHorarioPorDia(DateTime fecha) => ObtenerHorarioPorDia(fecha.DayOfWeek);
    }
}
