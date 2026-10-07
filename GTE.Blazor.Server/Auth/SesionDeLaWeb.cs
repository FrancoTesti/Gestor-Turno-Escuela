using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GTE.Clients;
using Microsoft.AspNetCore.Components.Authorization;

namespace GTE.Blazor.Server.Auth
{
    public class SesionDeLaWeb : IAuthService
    {
        public const string ClaimDelToken = "token";

        public const string ClaimDelNombreCompleto = "NombreCompleto";

        private readonly AuthenticationStateProvider _estado;

        public SesionDeLaWeb(AuthenticationStateProvider estado)
        {
            _estado = estado;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            ClaimsPrincipal usuario = await PrincipalAsync();
            string? token = usuario.FindFirst(ClaimDelToken)?.Value;

            return usuario.Identity?.IsAuthenticated == true && !Vencio(token);
        }

        public async Task<string?> GetTokenAsync() => (await PrincipalAsync()).FindFirst(ClaimDelToken)?.Value;

        public async Task<string?> GetUsernameAsync() =>
            (await PrincipalAsync()).FindFirst(ClaimTypes.Name)?.Value;

        public async Task<string?> GetRoleAsync() =>
            (await PrincipalAsync()).FindFirst(ClaimTypes.Role)?.Value;

        public async Task<string?> GetNombreCompletoAsync() =>
            (await PrincipalAsync()).FindFirst(ClaimDelNombreCompleto)?.Value;

        public Task<bool> LoginAsync(string username, string password) => Task.FromResult(false);

        public Task LogoutAsync() => Task.CompletedTask;

        public Task CheckTokenExpirationAsync() => Task.CompletedTask;

        private async Task<ClaimsPrincipal> PrincipalAsync() =>
            (await _estado.GetAuthenticationStateAsync()).User;

        private static bool Vencio(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return true;

            var manejador = new JwtSecurityTokenHandler();

            return !manejador.CanReadToken(token) || manejador.ReadJwtToken(token).ValidTo <= DateTime.UtcNow;
        }
    }
}
