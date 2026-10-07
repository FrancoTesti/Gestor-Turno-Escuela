namespace GTE.Clients;

public static class PermisosDeUsuario
{
    private const string Secretario = "Secretario";
    private const string Portero = "Portero";
    private const string Tutor = "Tutor";

    public static bool PuedeAdministrarAlumnos(string? rol) => rol == Secretario;

    public static bool PuedeAdministrarCursos(string? rol) => rol == Secretario;

    public static bool PuedeAdministrarTutores(string? rol) => rol == Secretario;

    public static bool PuedeGestionarRetiros(string? rol) =>
        rol is Secretario or Portero;

    public static bool PuedeVerCursos(string? rol) =>
        rol is Secretario or Portero or Tutor;

    public static bool PuedeVerReporteDeAlumnos(string? rol) => rol == Secretario;

    public static bool PuedeVerReporteDeRetiros(string? rol) =>
        rol is Secretario or Portero;
}
