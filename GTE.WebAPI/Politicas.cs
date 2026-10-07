using Microsoft.Extensions.DependencyInjection;

namespace GTE.WebAPI;

/// <summary>
/// Nombres de las políticas de autorización que usa la API para separar
/// las funcionalidades de cada tipo de usuario.
/// </summary>
public static class Politicas
{
    /// <summary>Rol del secretario, que administra el sistema.</summary>
    public const string RolSecretario = "Secretario";

    /// <summary>Rol del portero, que atiende la puerta.</summary>
    public const string RolPortero = "Portero";

    /// <summary>Rol del tutor, que consulta información de sus alumnos.</summary>
    public const string RolTutor = "Tutor";

    /// <summary>Solo el Secretario puede administrar alumnos, cursos y retiros.</summary>
    public const string SoloSecretario = "SoloSecretario";

    /// <summary>Pueden consultar alumnos el Secretario y el Portero.</summary>
    public const string LecturaAlumnos = "LecturaAlumnos";

    /// <summary>Pueden consultar cursos los tres tipos de usuario.</summary>
    public const string LecturaCursos = "LecturaCursos";

    /// <summary>Pueden operar los retiros el Secretario y el Portero.</summary>
    public const string GestionRetiros = "GestionRetiros";

    /// <summary>Pueden marcar la salida de un curso el Secretario y el Portero.</summary>
    public const string GestionSalidas = "GestionSalidas";

    /// <summary>Solo el Tutor, para sus propias pantallas.</summary>
    public const string SoloTutor = "SoloTutor";

    /// <summary>
    /// El reporte de alumnos es de gestión, así que lo ve sólo el Secretario.
    /// </summary>
    public const string ReporteDeAlumnos = "ReporteDeAlumnos";

    /// <summary>
    /// El reporte de retiros lo usan también los porteros, que son quienes
    /// entregan a los alumnos en la puerta.
    /// </summary>
    public const string ReporteDeRetiros = "ReporteDeRetiros";

    /// <summary>
    /// Registra las políticas de autorización de la aplicación.
    /// </summary>
    public static IServiceCollection AddPoliticasDeAutorizacion(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(SoloSecretario, politica =>
                politica.RequireRole(RolSecretario));

            options.AddPolicy(LecturaAlumnos, politica =>
                politica.RequireRole(RolSecretario, RolPortero));

            options.AddPolicy(LecturaCursos, politica =>
                politica.RequireRole(RolSecretario, RolPortero, RolTutor));

            options.AddPolicy(GestionRetiros, politica =>
                politica.RequireRole(RolSecretario, RolPortero));

            options.AddPolicy(GestionSalidas, politica =>
                politica.RequireRole(RolSecretario, RolPortero));

            options.AddPolicy(SoloTutor, politica =>
                politica.RequireRole(RolTutor));

            options.AddPolicy(ReporteDeAlumnos, politica =>
                politica.RequireRole(RolSecretario));

            options.AddPolicy(ReporteDeRetiros, politica =>
                politica.RequireRole(RolSecretario, RolPortero));
        });

        return services;
    }
}
