namespace GTE.Clients;

/// <summary>
/// Reglas de permisos que usan las interfaces para mostrar u ocultar acciones.
/// Deben coincidir con las políticas definidas en la API, que son las que
/// finalmente autorizan o rechazan cada operación.
/// </summary>
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

    /// <summary>El reporte de alumnos es de gestión: sólo lo ve el Secretario.</summary>
    public static bool PuedeVerReporteDeAlumnos(string? rol) => rol == Secretario;

    /// <summary>El reporte de retiros lo ve también la portería.</summary>
    public static bool PuedeVerReporteDeRetiros(string? rol) =>
        rol is Secretario or Portero;
}
