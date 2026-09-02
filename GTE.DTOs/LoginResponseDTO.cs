using System;

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

        // Propiedades para JWT
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}