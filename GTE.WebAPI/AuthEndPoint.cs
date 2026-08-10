using GTE.Application.Services;
using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GTE.WebAPI
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (LoginRequest request, IAuthService authService) =>
            {
                var response = await authService.LoginAsync(request);

                if (!response.Exito)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(response);
            })
            .WithName("Login")
            .WithOpenApi();
        }
    }
}