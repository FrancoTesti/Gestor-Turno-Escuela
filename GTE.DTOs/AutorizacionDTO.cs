using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class AutorizacionDTO
    {
        public int IdAutorizacion { get; set; }
        public int AlumnoId { get; set; }
        public string? AlumnoNombreCompleto { get; set; }
        public int TutorId { get; set; }
        public string? TutorNombreCompleto { get; set; }
    }
}
