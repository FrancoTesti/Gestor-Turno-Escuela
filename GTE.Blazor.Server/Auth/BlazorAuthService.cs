using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.Blazor.Server.Auth
{
    public class BlazorAuthService : AuthenticationStateProvider, IAuthService
    {
        private readonly AuthApiClient _authApiClient;
        private string? _token;
        private string? _username;
        private string? _role;
        private string? _nombreCompleto;
        private DateTime? _expiration;

        public BlazorAuthService(AuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (string.IsNullOrEmpty(_token) || _expiration < DateTime.UtcNow)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _username ?? string.Empty),
                new Claim(ClaimTypes.Role, _role ?? string.Empty)
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var result = await _authApiClient.LoginAsync(new LoginRequest { NombreUsuario = username, Contrasena = password });
            
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                _token = result.Token;
                _username = result.NombreUsuario;
                _role = result.Rol;
                _nombreCompleto = result.NombreCompleto;

                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(_token))
                {
                    _expiration = handler.ReadJwtToken(_token).ValidTo;
                }

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }
            return false;
        }

        public Task LogoutAsync()
        {
            _token = _username = _role = _nombreCompleto = null;
            _expiration = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public Task<bool> IsAuthenticatedAsync() => Task.FromResult(!string.IsNullOrEmpty(_token) && _expiration > DateTime.UtcNow);
        public Task<string?> GetTokenAsync() => Task.FromResult(_token);
        public Task<string?> GetUsernameAsync() => Task.FromResult(_username);
        public Task<string?> GetRoleAsync() => Task.FromResult(_role);
        public Task<string?> GetNombreCompletoAsync() => Task.FromResult(_nombreCompleto);
        public Task CheckTokenExpirationAsync() { if (_expiration < DateTime.UtcNow) LogoutAsync(); return Task.CompletedTask; }
    }
}