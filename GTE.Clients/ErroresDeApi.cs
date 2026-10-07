namespace GTE.Clients;

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

            ApiNoDisponibleException =>
                (false, excepcion.Message),

            _ =>
                (false, $"Ocurrió un error inesperado: {excepcion.Message}")
        };
    }
}
