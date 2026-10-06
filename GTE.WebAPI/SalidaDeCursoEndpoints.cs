using System.Security.Claims;
using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GTE.WebAPI
{
    public static class SalidaDeCursoEndpoints
    {
        public static void MapSalidaDeCursoEndpoints(this WebApplication app)
        {
            // La pantalla que está en la puerta del colegio no tiene usuario que
            // inicie sesión, así que este endpoint queda abierto: solo devuelve
            // qué curso está saliendo, sin nombres de alumnos ni de personal.
            app.MapGet("/cartelera", async (ISalidaDeCursoService service) =>
            {
                var cartelera = await service.GetCarteleraAsync();
                return Results.Ok(cartelera);
            })
            .WithName("GetCartelera")
            .Produces<CarteleraDTO>(StatusCodes.Status200OK)
            .WithOpenApi()
            .AllowAnonymous();

            app.MapGet("/salidas", async (ISalidaDeCursoService service) =>
            {
                var salidas = await service.GetDelDiaAsync(DateTime.Now);
                return Results.Ok(salidas);
            })
            .WithName("GetSalidasDelDia")
            .Produces<List<SalidaDeCursoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionSalidas);

            app.MapPost("/salidas", async (
                IniciarSalidaDeCursoDTO dto,
                ClaimsPrincipal usuario,
                GTEContext db,
                ISalidaDeCursoService service) =>
            {
                var personal = await PersonalDelUsuarioAsync(usuario, db);
                if (personal is null)
                    return Results.BadRequest(new { error = "El usuario conectado no figura como personal de la escuela." });

                var resultado = await service.IniciarAsync(dto.IdCurso, personal);

                return resultado.Exito
                    ? Results.Created($"/salidas/{resultado.Salida!.IdSalidaDeCurso}", resultado.Salida)
                    : Results.BadRequest(new { error = resultado.Mensaje });
            })
            .WithName("IniciarSalidaDeCurso")
            .Produces<SalidaDeCursoDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionSalidas);

            app.MapPost("/salidas/{id:int}/finalizar", async (int id, ISalidaDeCursoService service) =>
            {
                var resultado = await service.FinalizarAsync(id);

                return resultado.Exito
                    ? Results.Ok(new { mensaje = resultado.Mensaje })
                    : Results.BadRequest(new { error = resultado.Mensaje });
            })
            .WithName("FinalizarSalidaDeCurso")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionSalidas);
        }

        /// <summary>
        /// El personal que marca la salida sale del token: así el portero no puede
        /// registrar la salida a nombre de otra persona.
        /// </summary>
        private static async Task<Personal?> PersonalDelUsuarioAsync(ClaimsPrincipal usuario, GTEContext db)
        {
            string? idTexto = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idTexto, out int idUsuario))
                return null;

            return await db.Personal.FirstOrDefaultAsync(p => p.Usuario.IdUsuario == idUsuario);
        }
    }
}
