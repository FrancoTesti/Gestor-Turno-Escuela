using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class CursoEscolar
    {
        private static readonly string[] TurnosValidos = { "Mañana", "Tarde", "Noche" };

        public int IdCurso { get; private set; }
        public string Grado { get; private set; }
        public string Curso { get; private set; }
        public string Turno { get; private set; }
        public TimeSpan HorarioSalida { get; private set; }
        public TimeSpan HorarioLunes { get; private set; }
        public TimeSpan HorarioMartes { get; private set; }
        public TimeSpan HorarioMiercoles { get; private set; }
        public TimeSpan HorarioJueves { get; private set; }
        public TimeSpan HorarioViernes { get; private set; }

        private CursoEscolar() { }

        public CursoEscolar(int id, string grado, string curso, string turno, TimeSpan horario)
            : this(id, grado, curso, turno, horario, horario, horario, horario, horario)
        {
        }

        public CursoEscolar(int id, string grado, string curso, string turno,
            TimeSpan lunes, TimeSpan martes, TimeSpan miercoles, TimeSpan jueves, TimeSpan viernes)
        {
            SetIdCurso(id);
            SetGrado(grado);
            SetCurso(curso);
            SetTurno(turno);
            SetHorariosSemana(lunes, martes, miercoles, jueves, viernes);
        }

        public void SetIdCurso(int id)
        {
            if (id < 0) throw new ArgumentException("El Id debe ser mayor o igual a 0.", nameof(id));
            IdCurso = id;
        }

        public void SetGrado(string grado)
        {
            if (string.IsNullOrWhiteSpace(grado)) throw new ArgumentException("El grado es obligatorio.", nameof(grado));
            Grado = grado.Trim();
        }

        public void SetCurso(string curso)
        {
            if (string.IsNullOrWhiteSpace(curso)) throw new ArgumentException("La división es obligatoria.", nameof(curso));
            Curso = curso.Trim();
        }

        public void SetTurno(string turno)
        {
            var turnoNormalizado = TurnosValidos.FirstOrDefault(t => t.Equals(turno?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (turnoNormalizado == null)
                throw new ArgumentException($"Turno inválido. Valores permitidos: {string.Join(", ", TurnosValidos)}.", nameof(turno));
            Turno = turnoNormalizado;
        }

        public void SetHorarioSalida(TimeSpan horario)
        {
            SetHorariosSemana(horario, horario, horario, horario, horario);
        }

        public void SetHorariosSemana(TimeSpan lunes, TimeSpan martes, TimeSpan miercoles, TimeSpan jueves, TimeSpan viernes)
        {
            ValidarHorario(lunes, nameof(lunes));
            ValidarHorario(martes, nameof(martes));
            ValidarHorario(miercoles, nameof(miercoles));
            ValidarHorario(jueves, nameof(jueves));
            ValidarHorario(viernes, nameof(viernes));

            HorarioLunes = lunes;
            HorarioMartes = martes;
            HorarioMiercoles = miercoles;
            HorarioJueves = jueves;
            HorarioViernes = viernes;
            HorarioSalida = lunes;
        }

        public void SetHorarioDia(DayOfWeek dia, TimeSpan horario)
        {
            ValidarHorario(horario, nameof(horario));

            switch (dia)
            {
                case DayOfWeek.Monday:
                    HorarioLunes = horario;
                    HorarioSalida = horario;
                    break;
                case DayOfWeek.Tuesday:
                    HorarioMartes = horario;
                    break;
                case DayOfWeek.Wednesday:
                    HorarioMiercoles = horario;
                    break;
                case DayOfWeek.Thursday:
                    HorarioJueves = horario;
                    break;
                case DayOfWeek.Friday:
                    HorarioViernes = horario;
                    break;
                default:
                    throw new ArgumentException("Solo se pueden configurar horarios para días hábiles (Lunes a Viernes).", nameof(dia));
            }
        }

        public TimeSpan ObtenerHorarioSalida(DayOfWeek diaSemana)
        {
            return diaSemana switch
            {
                DayOfWeek.Monday => HorarioLunes,
                DayOfWeek.Tuesday => HorarioMartes,
                DayOfWeek.Wednesday => HorarioMiercoles,
                DayOfWeek.Thursday => HorarioJueves,
                DayOfWeek.Friday => HorarioViernes,
                _ => HorarioLunes
            };
        }

        public TimeSpan ObtenerHorarioSalida(DateTime fecha)
        {
            return ObtenerHorarioSalida(fecha.DayOfWeek);
        }

        public string MostrarCurso()
        {
            return $"{Grado} {Curso} - {Turno}";
        }

        private static void ValidarHorario(TimeSpan horario, string paramName)
        {
            if (horario < TimeSpan.Zero || horario >= TimeSpan.FromDays(1))
                throw new ArgumentException("El horario de salida debe ser una hora válida.", paramName);
        }
    }
}
