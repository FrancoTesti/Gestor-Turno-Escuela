using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class HorarioEspecialDTO
    {
        public int IdHorarioEspecial { get; set; }
        public int IdAlumno { get; set; }
        public string? AlumnoNombreCompleto { get; set; }
        public string DescripcionActividad { get; set; } = string.Empty;
        public TimeSpan HoraSalidaEspecial { get; set; }
    }
}
