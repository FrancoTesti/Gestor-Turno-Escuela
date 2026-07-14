using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class CursoEscolar
    {
        public int IdCurso { get; set; }
        public string Grado { get; set; }
        public string Curso { get; set; }
        public TimeSpan HorarioSalida { get; set; }

        public CursoEscolar(int id, string grado, string curso, TimeSpan horario)
        {
            IdCurso = id;
            Grado = grado;
            Curso = curso;
            HorarioSalida = horario;
        }
        public string MostrarCurso()
        {
            return Grado + " " + Curso;
        }
    }
}
