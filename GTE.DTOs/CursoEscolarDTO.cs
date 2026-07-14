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
        public TimeSpan HorarioSalida { get; set; }
    }
}
