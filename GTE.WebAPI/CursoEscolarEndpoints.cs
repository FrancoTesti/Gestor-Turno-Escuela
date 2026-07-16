using GTE.Application.Services;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class CursoEscolarEndpoints
    {
        public static void MapCursoEscolarEndpoints(this WebApplication app)
        {
            app.MapGet("/cursos", async (ICursoEscolarService service) =>
            {
                var dtos = await service.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllCursos")
            .Produces<List<CursoEscolarDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapGet("/cursos/{id:int}", async (int id, ICursoEscolarService service) =>
            {
                var dto = await service.GetAsync(id);
                return dto is null ? Results.NotFound() : Results.Ok(dto);
            })
            .WithName("GetCurso")
            .Produces<CursoEscolarDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapPost("/cursos", async (CursoEscolarDTO dto, ICursoEscolarService service) =>
            {
                try
                {
                    var creado = await service.AddAsync(dto);
                    return Results.Created($"/cursos/{creado.IdCurso}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddCurso")
            .Produces<CursoEscolarDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/cursos", async (CursoEscolarDTO dto, ICursoEscolarService service) =>
            {
                try
                {
                    var found = await service.UpdateAsync(dto);
                    return found ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateCurso")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/cursos/{id:int}", async (int id, ICursoEscolarService service) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteCurso")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}
