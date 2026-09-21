using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;
using Microsoft.EntityFrameworkCore;

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
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/retiros/{id:int}", async (int id, IRetiroService service) =>
            {
                var retiro = await service.GetAsync(id);
                return retiro is null ? Results.NotFound() : Results.Ok(retiro);
            })
            .WithName("GetRetiro")
            .Produces<RetiroDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

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
            .RequireAuthorization(Politicas.GestionRetiros);

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
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapDelete("/retiros/{id:int}", async (int id, IRetiroService service) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteRetiro")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/tutores/{id:int}/alumnos", async (int id, IRetiroService service) =>
            {
                var alumnos = await service.GetAlumnosAutorizadosAsync(id);
                return Results.Ok(alumnos);
            })
            .WithName("GetAlumnosAutorizadosByTutor")
            .Produces<List<AlumnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/tutores", async (ITutorRepository tutorRepo) =>
            {
                var tutores = await tutorRepo.GetAllAsync();
                return Results.Ok(tutores.Select(t => new TutorDTO
                {
                    IdTutor = t.IdTutor,
                    Nombre = t.Nombre,
                    Apellido = t.Apellido,
                    Dni = t.Dni,
                    Parentesco = t.Parentesco,
                    Telefono = t.Telefono,
                    TieneRestriccion = t.TieneRestriccion
                }).ToList());
            })
            .WithName("GetAllTutores")
            .Produces<List<TutorDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/personal", async (GTEContext db) =>
            {
                var lista = await db.Personal.ToListAsync();
                var dtos = lista.Select(p => new PorteroDTO
                {
                    IdPersonal = p.IdPersonal,
                    Nombre = p.Nombre,
                    PuertaAsignada = p is Portero portero ? portero.PuertaAsignada : "Secretaría"
                }).ToList();
                return Results.Ok(dtos);
            })
            .WithName("GetAllPersonal")
            .Produces<List<PorteroDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);
        }
    }
}
