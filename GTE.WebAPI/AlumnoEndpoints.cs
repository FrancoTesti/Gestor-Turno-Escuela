using GTE.Application.Services;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class AlumnoEndpoints
    {
        public static void MapAlumnoEndpoints(this WebApplication app)
        {
            app.MapGet("/alumnos", async (IAlumnoService service) =>
            {
                var dtos = await service.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllAlumnos")
            .Produces<List<AlumnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            // Búsqueda con filtros (requisito de la consigna).
            app.MapGet("/alumnos/criteria", async (string? nombre, string? grado, string? curso, string? estado, IAlumnoService service) =>
            {
                var criteria = new AlumnoCriteriaDTO { Nombre = nombre, Grado = grado, Curso = curso, Estado = estado };
                var dtos = await service.GetByCriteriaAsync(criteria);
                return Results.Ok(dtos);
            })
            .WithName("GetAlumnosByCriteria")
            .Produces<List<AlumnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapGet("/alumnos/{id:int}", async (int id, IAlumnoService service) =>
            {
                var dto = await service.GetAsync(id);
                return dto is null ? Results.NotFound() : Results.Ok(dto);
            })
            .WithName("GetAlumno")
            .Produces<AlumnoDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapPost("/alumnos", async (AlumnoDTO dto, IAlumnoService service) =>
            {
                try
                {
                    var creado = await service.AddAsync(dto);
                    return Results.Created($"/alumnos/{creado.IdAlumno}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddAlumno")
            .Produces<AlumnoDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/alumnos", async (AlumnoDTO dto, IAlumnoService service) =>
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
            .WithName("UpdateAlumno")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/alumnos/{id:int}", async (int id, IAlumnoService service) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteAlumno")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}
