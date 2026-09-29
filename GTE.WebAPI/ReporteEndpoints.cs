using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio;
using System;

namespace GTE.WebAPI
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/reportes").RequireAuthorization(Politicas.LecturaAlumnos);

            group.MapGet("/alumnos-por-curso", async (IReporteService service) =>
            {
                var result = await service.ObtenerAlumnosPorCursoAsync();
                return Results.Ok(result);
            });

            group.MapGet("/retiros", async (IReporteService service, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta) =>
            {
                var result = await service.ObtenerRetirosPorRangoAsync(desde, hasta);
                return Results.Ok(result);
            });
        }
    }
}
