using System.Threading.Tasks;

namespace GTE.Clients
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<string?> GetUsernameAsync();
        Task<string?> GetRoleAsync();
        Task<string?> GetNombreCompletoAsync();
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task CheckTokenExpirationAsync();
    }
}
