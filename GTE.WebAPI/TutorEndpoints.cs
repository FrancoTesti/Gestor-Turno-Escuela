using GTE.Application.Services;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class TutorEndpoints
    {
        public static void MapTutorEndpoints(this WebApplication app)
        {
            app.MapGet("/tutores", async (ITutorService service) =>
            {
                var dtos = await service.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllTutores")
            .Produces<List<TutorDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapGet("/tutores/{id:int}", async (int id, ITutorService service) =>
            {
                var dto = await service.GetAsync(id);
                return dto is null ? Results.NotFound() : Results.Ok(dto);
            })
            .WithName("GetTutor")
            .Produces<TutorDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapPost("/tutores", async (TutorDTO dto, ITutorService service) =>
            {
                try
                {
                    var creado = await service.AddAsync(dto);
                    return Results.Created($"/tutores/{creado.IdTutor}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddTutor")
            .Produces<TutorDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapPut("/tutores", async (TutorDTO dto, ITutorService service) =>
            {
                try
                {
                    var encontrado = await service.UpdateAsync(dto);
                    return encontrado ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateTutor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapDelete("/tutores/{id:int}", async (int id, ITutorService service) =>
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteTutor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);
        }
    }
}
