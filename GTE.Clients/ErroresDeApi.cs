namespace GTE.Clients;

/// <summary>
/// Interpretación de los errores que devuelve la API para que las interfaces
/// sepan si deben volver al login o solamente mostrar un mensaje.
/// </summary>
public static class ErroresDeApi
{
    public static (bool IrAlLogin, string Mensaje) Interpretar(Exception excepcion)
    {
        return excepcion switch
        {
            SesionExpiradaException =>
                (true, "La sesión expiró. Vuelva a iniciar sesión."),

            SinPermisoException =>
                (false, "No tiene permisos para realizar esta operación con su usuario."),

            _ =>
                (false, $"Ocurrió un error inesperado: {excepcion.Message}")
        };
    }
}
