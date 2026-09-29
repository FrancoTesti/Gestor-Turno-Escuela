using GTE.Application.Services;
using GTE.Data;
using GTE.DTOs;

namespace GTE.WebAPI
{
    public static class TutorEndpoints
    {
        public static void MapTutorEndpoints(this WebApplication app)
        {
            // Alumnos que el tutor que está conectado tiene autorizados.
            app.MapGet("/mis-alumnos", async (
                System.Security.Claims.ClaimsPrincipal usuario,
                ITutorRepository tutorRepository,
                IAutorizacionRepository autorizacionRepository) =>
            {
                string? idTexto = usuario.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idTexto, out int idUsuario))
                    return Results.Ok(new List<AlumnoACargoDTO>());

                var tutores = await tutorRepository.GetAllAsync();
                var tutor = tutores.FirstOrDefault(t => t.Usuario != null && t.Usuario.IdUsuario == idUsuario);

                if (tutor == null)
                    return Results.Ok(new List<AlumnoACargoDTO>());

                var alumnos = await autorizacionRepository.GetAlumnosByTutorIdAsync(tutor.IdTutor);

                var dtos = alumnos.Select(alumno => new AlumnoACargoDTO
                {
                    IdAlumno = alumno.IdAlumno,
                    Nombre = alumno.Nombre,
                    Apellido = alumno.Apellido,
                    Grado = alumno.CursoEscolar?.Grado ?? string.Empty,
                    Curso = alumno.CursoEscolar?.Curso ?? string.Empty,
                    Turno = alumno.CursoEscolar?.Turno ?? string.Empty,
                    HorarioSalida = alumno.CursoEscolar?.HorarioSalida ?? TimeSpan.Zero,
                    Estado = alumno.Estado
                }).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetMisAlumnos")
            .Produces<List<AlumnoACargoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloTutor);

            app.MapGet("/tutores", async (ITutorService service) =>
            {
                var dtos = await service.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllTutores")
            .Produces<List<TutorDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapGet("/tutores/{id:int}", async (int id, ITutorService service) =>
            {
                var dto = await service.GetAsync(id);
                return dto is null ? Results.NotFound() : Results.Ok(dto);
            })
            .WithName("GetTutor")
            .Produces<TutorDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.LecturaAlumnos);

            app.MapPost("/tutores", async (TutorDTO dto, ITutorService service) =>
            {
                try
                {
                    var creado = await service.AddAsync(dto);
                    return Results.Created($"/tutores/{creado.IdTutor}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddTutor")
            .Produces<TutorDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapPut("/tutores", async (TutorDTO dto, ITutorService service) =>
            {
                try
                {
                    var encontrado = await service.UpdateAsync(dto);
                    return encontrado ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateTutor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);

            app.MapDelete("/tutores/{id:int}", async (int id, ITutorService service) =>
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteTutor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Politicas.SoloSecretario);
        }
    }
}
