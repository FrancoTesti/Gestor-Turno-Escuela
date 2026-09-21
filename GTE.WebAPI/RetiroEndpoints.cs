using GTE.Application.Services;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class RetiroEndpoints
    {
        public static void MapRetiroEndpoints(this WebApplication app)
        {
            app.MapGet("/retiros", async (IRetiroService service) =>
            {
                var retiros = await service.GetAllAsync();
                return Results.Ok(retiros);
            })
            .WithName("GetAllRetiros")
            .Produces<List<RetiroDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/retiros/{id:int}", async (int id, IRetiroService service) =>
            {
                var retiro = await service.GetAsync(id);
                return retiro is null ? Results.NotFound() : Results.Ok(retiro);
            })
            .WithName("GetRetiro")
            .Produces<RetiroDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/retiros", async (RetiroDTO dto, IRetiroService service) =>
            {
                try
                {
                    var result = await service.AddAsync(dto);
                    if (!result.Exito)
                    {
                        return Results.BadRequest(new { error = result.Mensaje });
                    }
                    return Results.Created($"/retiros/{result.Retiro!.IdRetiro}", result.Retiro);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddRetiro")
            .Produces<RetiroDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPut("/retiros", async (RetiroDTO dto, IRetiroService service) =>
            {
                try
                {
                    var result = await service.UpdateAsync(dto);
                    if (!result.Exito)
                    {
                        return Results.BadRequest(new { error = result.Mensaje });
                    }
                    return Results.Ok(result.Retiro);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateRetiro")
            .Produces<RetiroDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapDelete("/retiros/{id:int}", async (int id, IRetiroService service) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteRetiro")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/tutores/{id:int}/alumnos", async (int id, IRetiroService service) =>
            {
                var alumnos = await service.GetAlumnosAutorizadosAsync(id);
                return Results.Ok(alumnos);
            })
            .WithName("GetAlumnosAutorizadosByTutor")
            .Produces<List<AlumnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();
        }
    }
}
