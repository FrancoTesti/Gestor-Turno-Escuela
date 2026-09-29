using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio;
using System;

namespace GTE.WebAPI
{
    public static class HorarioEspecialEndpoints
    {
        public static void MapHorarioEspecialEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/horariosespeciales").RequireAuthorization(Politicas.GestionRetiros);

            group.MapGet("/", async (IHorarioEspecialService service) =>
            {
                return Results.Ok(await service.ObtenerTodosAsync());
            });

            group.MapGet("/{id}", async (int id, IHorarioEspecialService service) =>
            {
                var h = await service.ObtenerPorIdAsync(id);
                return h != null ? Results.Ok(h) : Results.NotFound();
            });

            group.MapPost("/", async (HorarioEspecialDTO dto, IHorarioEspecialService service) =>
            {
                try
                {
                    var result = await service.AgregarAsync(dto);
                    return Results.Created($"/horariosespeciales/{result.IdHorarioEspecial}", result);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapPut("/", async (HorarioEspecialDTO dto, IHorarioEspecialService service) =>
            {
                try
                {
                    await service.ActualizarAsync(dto);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapDelete("/{id}", async (int id, IHorarioEspecialService service) =>
            {
                await service.EliminarAsync(id);
                return Results.NoContent();
            });
        }
    }
}
