using GTE.Clients;
using GTE.DTOs;
using System;
using System.Threading.Tasks;

namespace GTE.Auth.WindowsForms
{
    public class WindowsFormsAuthService : IAuthService
    {
        private static string? _currentToken;
        private static DateTime _tokenExpiration;
        private static string? _currentUsername;
        private static string? _currentRole;
        private static string? _currentNombreCompleto;

        public event Action<bool>? AuthenticationStateChanged;

        public async Task<bool> IsAuthenticatedAsync()
        {
            return !string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow < _tokenExpiration;
        }

        public async Task<string?> GetTokenAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentToken : null;
        }

        public async Task<string?> GetUsernameAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentUsername : null;
        }

        public async Task<string?> GetRoleAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentRole : null;
        }

        public async Task<string?> GetNombreCompletoAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentNombreCompleto : null;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var authClient = new AuthApiClient();
                var response = await authClient.LoginAsync(new LoginRequest
                {
                    NombreUsuario = username,
                    Contrasena = password
                });

                if (response != null && response.Exito)
                {
                    _currentToken = response.Token;
                    _tokenExpiration = response.ExpiresAt ?? DateTime.UtcNow.AddMinutes(60);
                    _currentUsername = response.NombreUsuario;
                    _currentRole = response.Rol;
                    _currentNombreCompleto = response.NombreCompleto;

                    AuthenticationStateChanged?.Invoke(true);
                    return true;
                }
            }
            catch (Exception)
            {
                // Ignorar error de red
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            _currentToken = null;
            _tokenExpiration = default;
            _currentUsername = null;
            _currentRole = null;
            _currentNombreCompleto = null;

            AuthenticationStateChanged?.Invoke(false);
        }

        public async Task CheckTokenExpirationAsync()
        {
            if (!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow >= _tokenExpiration)
            {
                await LogoutAsync();
            }
        }
    }
}
