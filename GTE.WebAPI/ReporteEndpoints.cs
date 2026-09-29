<<<<<<< HEAD
using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio;
using System;
=======
using GTE.Application.Services;
using GTE.DTOs;
>>>>>>> origin/main

namespace GTE.WebAPI
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this WebApplication app)
        {
<<<<<<< HEAD
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
=======
            app.MapGet("/reportes/alumnos-por-curso", async (IReporteService service) =>
            {
                var filas = await service.GetAlumnosPorCursoAsync();
                return Results.Ok(filas);
            })
            .WithName("GetAlumnosPorCurso")
            .Produces<List<AlumnosPorCursoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapGet("/reportes/retiros", async (DateTime? desde, DateTime? hasta, IReporteService service) =>
            {
                var retiros = await service.GetRetirosAsync(desde, hasta);
                return Results.Ok(retiros);
            })
            .WithName("GetRetirosPorFecha")
            .Produces<List<RetiroDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);
>>>>>>> origin/main
        }
    }
}
