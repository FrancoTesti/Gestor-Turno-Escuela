using Microsoft.Extensions.DependencyInjection;

namespace GTE.WebAPI;

public static class Politicas
{
    public const string RolSecretario = "Secretario";

    public const string RolPortero = "Portero";

    public const string RolTutor = "Tutor";

    public const string SoloSecretario = "SoloSecretario";

    public const string LecturaAlumnos = "LecturaAlumnos";

    public const string LecturaCursos = "LecturaCursos";

    public const string GestionRetiros = "GestionRetiros";

    public const string GestionSalidas = "GestionSalidas";

    public const string SoloTutor = "SoloTutor";

    public const string ReporteDeAlumnos = "ReporteDeAlumnos";

    public const string ReporteDeRetiros = "ReporteDeRetiros";

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
