using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string nombreUsuario, string contrasena);
    }
}
