using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class TutorDTO
    {
        public int IdTutor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Parentesco { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool TieneRestriccion { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Contrasena { get; set; } = string.Empty;
    }
}
