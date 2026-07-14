using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class RetiroDTO
    {
        public int IdRetiro { get; set; }
        public int IdAlumno { get; set; }
        public string? AlumnoNombreCompleto { get; set; }
        public int IdTutor { get; set; }
        public string? TutorNombreCompleto { get; set; }
        public int IdPersonal { get; set; }
        public string? PersonalNombre { get; set; }
        public DateTime FechaHora { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}
