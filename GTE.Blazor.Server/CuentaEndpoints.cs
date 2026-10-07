using System.Security.Claims;
using GTE.Clients;
using GTE.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GTE.Blazor.Server
{
    public static class CuentaEndpoints
    {
        public static void MapCuentaEndpoints(this WebApplication app)
        {
            app.MapPost("/cuenta/entrar", async (HttpContext contexto, AuthApiClient api) =>
            {
                var formulario = await contexto.Request.ReadFormAsync();
                string usuario = formulario["usuario"].ToString().Trim();
                string clave = formulario["clave"].ToString();

                LoginResponse? respuesta;

                try
                {
                    respuesta = await api.LoginAsync(new LoginRequest
                    {
                        NombreUsuario = usuario,
                        Contrasena = clave
                    });
                }
                catch (ApiNoDisponibleException)
                {
                    return Results.Redirect("/login?error=api");
                }

                if (respuesta is null || string.IsNullOrEmpty(respuesta.Token))
                    return Results.Redirect("/login?error=datos");

                var identidad = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, respuesta.NombreUsuario),
                        new Claim(ClaimTypes.Role, respuesta.Rol),
                        new Claim(
                            Auth.SesionDeLaWeb.ClaimDelNombreCompleto,
                            string.IsNullOrWhiteSpace(respuesta.NombreCompleto) ? respuesta.NombreUsuario : respuesta.NombreCompleto),
                        new Claim(Auth.SesionDeLaWeb.ClaimDelToken, respuesta.Token)
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await contexto.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identidad));

                return Results.Redirect("/");
            })
            .DisableAntiforgery();

            app.MapGet("/cuenta/salir", async (HttpContext contexto) =>
            {
                await contexto.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Results.Redirect("/login");
            });
        }
    }
}
