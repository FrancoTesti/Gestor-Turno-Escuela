<<<<<<< HEAD
﻿using System.Threading.Tasks;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
using GTE.DTOs;

namespace GTE.Application.Services
{
    public interface IAuthService
    {
<<<<<<< HEAD
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
=======
        Task<LoginResponse> LoginAsync(string nombreUsuario, string contrasena);
    }
}
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
