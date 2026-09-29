using GTE.Application.Services;
using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GTE.WebAPI
{
    public static class AutorizacionEndpoints
    {
        public static void MapAutorizacionEndpoints(this WebApplication app)
        {
            app.MapGet("/autorizaciones", async (IAutorizacionService service) =>
            {
                var lista = await service.GetAllAsync();
                return Results.Ok(lista);
            })
            .WithName("GetAllAutorizaciones")
            .Produces<List<AutorizacionDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapGet("/autorizaciones/{id:int}", async (int id, IAutorizacionService service) =>
            {
                var auto = await service.GetAsync(id);
                return auto is null ? Results.NotFound() : Results.Ok(auto);
            })
            .WithName("GetAutorizacion")
            .Produces<AutorizacionDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapGet("/autorizaciones/tutor/{tutorId:int}", async (int tutorId, IAutorizacionService service) =>
            {
                var lista = await service.GetByTutorIdAsync(tutorId);
                return Results.Ok(lista);
            })
            .WithName("GetAutorizacionesByTutor")
            .Produces<List<AutorizacionDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapPost("/autorizaciones", async (AutorizacionDTO dto, IAutorizacionService service) =>
            {
                try
                {
                    var resultado = await service.AddAsync(dto);
                    if (!resultado.Exito)
                    {
                        return Results.BadRequest(new { error = resultado.Mensaje });
                    }
                    return Results.Created($"/autorizaciones/{resultado.Autorizacion!.IdAutorizacion}", resultado.Autorizacion);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddAutorizacion")
            .Produces<AutorizacionDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapDelete("/autorizaciones/{id:int}", async (int id, IAutorizacionService service) =>
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteAutorizacion")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapDelete("/autorizaciones/tutor/{tutorId:int}/alumno/{alumnoId:int}", async (int tutorId, int alumnoId, IAutorizacionService service) =>
            {
                var eliminado = await service.DeleteByTutorAndAlumnoAsync(tutorId, alumnoId);
                return eliminado ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteAutorizacionByTutorAndAlumno")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);
        }
    }
}
