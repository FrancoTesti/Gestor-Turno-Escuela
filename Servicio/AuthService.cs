<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GTE.Data;
using GTE.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
=======
﻿using GTE.Data;
using GTE.Dominio;
using System.Security.Claims;
using System.Text;
using GTE.DTOs;
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0

namespace GTE.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IPorteroRepository _porteroRepository;
        private readonly ISecretarioRepository _secretarioRepository;
<<<<<<< HEAD
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository,
                           ITutorRepository tutorRepository,
                           IPorteroRepository porteroRepository,
                           ISecretarioRepository secretarioRepository,
                           IConfiguration configuration)
=======

        public AuthService(IUsuarioRepository usuarioRepository,
            ITutorRepository tutorRepository,
            IPorteroRepository porteroRepository,
            ISecretarioRepository secretarioRepository)
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
        {
            _usuarioRepository = usuarioRepository;
            _tutorRepository = tutorRepository;
            _porteroRepository = porteroRepository;
            _secretarioRepository = secretarioRepository;
<<<<<<< HEAD
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NombreUsuario) || string.IsNullOrWhiteSpace(request.Contrasena))
                return Fallo("Nombre de usuario y contraseña son requeridos.");

            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(request.NombreUsuario);

            if (usuario == null || !usuario.ValidarCredenciales(request.NombreUsuario, request.Contrasena))
            {
                if (usuario != null && !usuario.EstaActivo &&
                    usuario.NombreUsuario == request.NombreUsuario && usuario.Contrasena == request.Contrasena)
=======
        }

        public async Task<LoginResponse?> LoginAsync(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);

            if (usuario == null || !usuario.ValidarCredenciales(nombreUsuario, contrasena))
            {
                if (usuario != null && !usuario.EstaActivo &&
                    usuario.NombreUsuario == nombreUsuario && usuario.Contrasena == contrasena)
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
                    return Fallo("El usuario está inactivo. Contacte al administrador.");

                return Fallo("Usuario o contraseña incorrectos.");
            }
<<<<<<< HEAD

            var (rol, nombreCompleto) = await ResolverRolAsync(usuario.IdUsuario);
            if (rol == null)
                return Fallo("El usuario no tiene un rol asignado en el sistema.");

            // Generación de Token JWT combinando claims del usuario y el rol resuelto
            var (token, expiresAt) = GenerateJwtToken(usuario.IdUsuario, usuario.NombreUsuario, rol, nombreCompleto);

=======
            
            var (rol, nombreCompleto) = await ResolverRolAsync(usuario.IdUsuario);
            if (rol == null)
                return Fallo("El usuario no tiene un rol asignado en el sistema.");
            
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
            return new LoginResponse
            {
                Exito = true,
                Mensaje = "Login exitoso.",
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = rol,
<<<<<<< HEAD
                NombreCompleto = nombreCompleto,
                Token = token,
                ExpiresAt = expiresAt
=======
                NombreCompleto = nombreCompleto
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
            };
        }

        private async Task<(string? rol, string? nombre)> ResolverRolAsync(int idUsuario)
        {
            var tutores = await _tutorRepository.GetAllAsync();
<<<<<<< HEAD
            var tutor = tutores.FirstOrDefault(t => t.Usuario != null && t.Usuario.IdUsuario == idUsuario);
=======
            var tutor = tutores.FirstOrDefault(t => t.Usuario.IdUsuario == idUsuario);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
            if (tutor != null)
                return ("Tutor", $"{tutor.Nombre} {tutor.Apellido}");

            var secretarios = await _secretarioRepository.GetAllAsync();
<<<<<<< HEAD
            var secretario = secretarios.FirstOrDefault(s => s.Usuario != null && s.Usuario.IdUsuario == idUsuario);
=======
            var secretario = secretarios.FirstOrDefault(s => s.Usuario.IdUsuario == idUsuario);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
            if (secretario != null)
                return ("Secretario", secretario.Nombre);

            var porteros = await _porteroRepository.GetAllAsync();
<<<<<<< HEAD
            var portero = porteros.FirstOrDefault(p => p.Usuario != null && p.Usuario.IdUsuario == idUsuario);
=======
            var portero = porteros.FirstOrDefault(p => p.Usuario.IdUsuario == idUsuario);
>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
            if (portero != null)
                return ("Portero", portero.Nombre);

            return (null, null);
        }

<<<<<<< HEAD
        private (string token, DateTime expiresAt) GenerateJwtToken(int idUsuario, string nombreUsuario, string rol, string? nombreCompleto)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? jwtSettings["Secret"] ?? "ClaveSecretaPorDefectoSuperSegura123456789!";
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.TryParse(jwtSettings["ExpirationMinutes"], out var min) ? min : 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim(ClaimTypes.Role, rol),
                new Claim("NombreCompleto", nombreCompleto ?? string.Empty),
                new Claim("jti", Guid.NewGuid().ToString())
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        private static LoginResponse Fallo(string mensaje) =>
            new LoginResponse { Exito = false, Mensaje = mensaje };
    }
=======
        private static LoginResponse Fallo(string mensaje) =>
            new LoginResponse { Exito = false, Mensaje = mensaje };
    }

>>>>>>> 1194efef233d2fe95e39f88eb2d8ef8f1afabda0
}