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

        private CursoEscolar() { }

        public CursoEscolar(int id, string grado, string curso, string turno, TimeSpan horario)
        {
            SetIdCurso(id);
            SetGrado(grado);
            SetCurso(curso);
            SetTurno(turno);
            SetHorarioSalida(horario);
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
            if (horario < TimeSpan.Zero || horario >= TimeSpan.FromDays(1))
                throw new ArgumentException("El horario de salida debe ser una hora válida.", nameof(horario));
            HorarioSalida = horario;
        }
        public string MostrarCurso()
        {
            return $"{Grado} {Curso} - {Turno}";
        }
    }
}
