using GTE.Application.Services;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this WebApplication app)
        {
            app.MapGet("/reportes/alumnos-por-curso", async (IReporteService service) =>
            {
                var filas = await service.GetAlumnosPorCursoAsync();
                return Results.Ok(filas);
            })
            .WithName("GetAlumnosPorCurso")
            .Produces<List<AlumnosPorCursoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.ReporteDeAlumnos);

            app.MapGet("/reportes/retiros", async (DateTime? desde, DateTime? hasta, IReporteService service) =>
            {
                var retiros = await service.GetRetirosAsync(desde, hasta);
                return Results.Ok(retiros);
            })
            .WithName("GetRetirosPorFecha")
            .Produces<List<RetiroDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.ReporteDeRetiros);
        }
    }
}
