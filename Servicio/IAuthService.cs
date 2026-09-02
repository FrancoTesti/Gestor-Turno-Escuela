using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}