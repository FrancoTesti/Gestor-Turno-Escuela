using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTE.DTOs
{
    public class LoginResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }

        public string? Rol { get; set; }
        public string? NombreCompleto { get; set; }
    }
}
