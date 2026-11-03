using System;
using System.Collections.Generic;
using GTE.Application.Services;
using GTE.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace GTE.WebAPI
{
    public static class AsistenciaEndpoints
    {
        public static void MapAsistenciaEndpoints(this WebApplication app)
        {
            app.MapGet("/asistencias/tipos", async (IAsistenciaService service) =>
            {
                var tipos = await service.GetTiposInasistenciaAsync();
                return Results.Ok(tipos);
            })
            .WithName("GetTiposInasistencia")
            .Produces<List<TipoInasistenciaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaCursos);

            app.MapGet("/asistencias/planilla", async (int idCurso, DateTime fecha, IAsistenciaService service) =>
            {
                try
                {
                    if (idCurso <= 0)
                    {
                        return Results.BadRequest(new { error = "El Id de curso debe ser mayor a 0." });
                    }

                    var planilla = await service.GetPlanillaDiariaAsync(idCurso, fecha);
                    return Results.Ok(planilla);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetPlanillaDiaria")
            .Produces<PlanillaDiariaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapPost("/asistencias/planilla", async (GuardarPlanillaAsistenciaRequestDTO request, IAsistenciaService service) =>
            {
                try
                {
                    if (request == null || request.IdCurso <= 0)
                    {
                        return Results.BadRequest(new { error = "Los datos de la planilla son inválidos." });
                    }

                    var result = await service.GuardarPlanillaDiariaAsync(request);
                    if (!result.Exito)
                    {
                        return Results.BadRequest(new { error = result.Mensaje });
                    }

                    return Results.Ok(new { exito = true, mensaje = result.Mensaje });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GuardarPlanillaDiaria")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/asistencias/sugerencias-retiros", async (int idCurso, DateTime fecha, IAsistenciaService service) =>
            {
                try
                {
                    if (idCurso <= 0)
                    {
                        return Results.BadRequest(new { error = "El Id de curso debe ser mayor a 0." });
                    }

                    var sugerencias = await service.SugerirInasistenciasDesdeRetirosAsync(idCurso, fecha);
                    return Results.Ok(sugerencias);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("SugerirInasistenciasDesdeRetiros")
            .Produces<List<AlumnoAsistenciaItemDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/asistencias/reportes/novedades", async (int idCurso, DateTime fechaDesde, DateTime fechaHasta, IAsistenciaService service) =>
            {
                try
                {
                    if (idCurso <= 0)
                    {
                        return Results.BadRequest(new { error = "El Id de curso debe ser mayor a 0." });
                    }

                    var novedades = await service.GetReporteNovedadesAsync(idCurso, fechaDesde, fechaHasta);
                    return Results.Ok(novedades);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetReporteNovedades")
            .Produces<List<ReporteNovedadItemDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/asistencias/reportes/espejo-semanal", async (int idCurso, DateTime fechaSemana, IAsistenciaService service) =>
            {
                try
                {
                    if (idCurso <= 0)
                    {
                        return Results.BadRequest(new { error = "El Id de curso debe ser mayor a 0." });
                    }

                    var espejo = await service.GetReporteEspejoSemanalAsync(idCurso, fechaSemana);
                    return Results.Ok(espejo);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetReporteEspejoSemanal")
            .Produces<ReporteEspejoSemanaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.GestionRetiros);

            app.MapGet("/asistencias/historial/{idAlumno:int}", async (int idAlumno, IAsistenciaService service) =>
            {
                try
                {
                    if (idAlumno <= 0)
                    {
                        return Results.NotFound();
                    }

                    var historial = await service.GetHistorialAlumnoAsync(idAlumno);
                    if (historial == null || string.IsNullOrEmpty(historial.NombreCompleto))
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(historial);
                }
                catch (ArgumentException)
                {
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetHistorialAlumno")
            .Produces<HistorialAsistenciaAlumnoDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaCursos);
        }
    }
}
