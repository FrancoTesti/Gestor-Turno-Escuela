using GTE.Data;
using GTE.Dominio;
using System.Security.Claims;
using System.Text;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IPorteroRepository _porteroRepository;
        private readonly ISecretarioRepository _secretarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository,
            ITutorRepository tutorRepository,
            IPorteroRepository porteroRepository,
            ISecretarioRepository secretarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tutorRepository = tutorRepository;
            _porteroRepository = porteroRepository;
            _secretarioRepository = secretarioRepository;
        }

        public async Task<LoginResponse?> LoginAsync(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);

            if (usuario == null || !usuario.ValidarCredenciales(nombreUsuario, contrasena))
            {
                // Si coinciden nombre y contraseña pero el usuario está inactivo, damos un mensaje más claro.
                if (usuario != null && !usuario.EstaActivo &&
                    usuario.NombreUsuario == nombreUsuario && usuario.Contrasena == contrasena)
                    return Fallo("El usuario está inactivo. Contacte al administrador.");

                // Mismo mensaje para "no existe" o "contraseña incorrecta": no revelamos cuál falló.
                return Fallo("Usuario o contraseña incorrectos.");
            }
            
            var (rol, nombreCompleto) = await ResolverRolAsync(usuario.IdUsuario);
            if (rol == null)
                return Fallo("El usuario no tiene un rol asignado en el sistema.");
            
            return new LoginResponse
            {
                Exito = true,
                Mensaje = "Login exitoso.",
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = rol,
                NombreCompleto = nombreCompleto
            };
        }

        private async Task<(string? rol, string? nombre)> ResolverRolAsync(int idUsuario)
        {
            var tutores = await _tutorRepository.GetAllAsync();
            var tutor = tutores.FirstOrDefault(t => t.Usuario.IdUsuario == idUsuario);
            if (tutor != null)
                return ("Tutor", $"{tutor.Nombre} {tutor.Apellido}");

            var secretarios = await _secretarioRepository.GetAllAsync();
            var secretario = secretarios.FirstOrDefault(s => s.Usuario.IdUsuario == idUsuario);
            if (secretario != null)
                return ("Secretario", secretario.Nombre);

            var porteros = await _porteroRepository.GetAllAsync();
            var portero = porteros.FirstOrDefault(p => p.Usuario.IdUsuario == idUsuario);
            if (portero != null)
                return ("Portero", portero.Nombre);

            return (null, null);
        }

        private static LoginResponse Fallo(string mensaje) =>
            new LoginResponse { Exito = false, Mensaje = mensaje };
    }

}