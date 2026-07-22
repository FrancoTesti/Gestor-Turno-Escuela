using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.Dominio
{
    public class HorarioEspecial
    {
        public int IdHorarioEspecial { get; set; }
        public int IdAlumno { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public TimeSpan HoraSalidaEspecial { get; set; }

        public HorarioEspecial(int idHorario, int idAlu, string desc, TimeSpan horaSalidaEsp)
        {
            IdHorarioEspecial = idHorario;
            IdAlumno = idAlu;
            DescripcionActividad = desc;
            HoraSalidaEspecial = horaSalidaEsp;
        }
    }
}
