<<<<<<< HEAD
using System;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

namespace GTE.DTOs
{
    public class LoginResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
<<<<<<< HEAD
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Rol { get; set; }
        public string? NombreCompleto { get; set; }

        // Propiedades para JWT
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
=======

        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }

        public string? Rol { get; set; }
        public string? NombreCompleto { get; set; }
    }
}
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
