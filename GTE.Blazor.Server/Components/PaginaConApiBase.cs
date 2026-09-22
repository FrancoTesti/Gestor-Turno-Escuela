using GTE.Clients;
using Microsoft.AspNetCore.Components;

namespace GTE.Blazor.Server.Components;

/// <summary>
/// Base para las páginas que consumen la API. Centraliza la interpretación de
/// los errores: si la sesión expiró vuelve al login, y si falta permiso se
/// informa en pantalla sin cerrar la sesión.
/// </summary>
public abstract class PaginaConApiBase : ComponentBase
{
    [Inject]
    protected NavigationManager Nav { get; set; } = default!;

    /// <summary>Mensaje de error que la página muestra al usuario.</summary>
    protected string MensajeError { get; set; } = string.Empty;

    /// <summary>
    /// Ejecuta una operación contra la API y traduce cualquier error a una
    /// acción concreta en la pantalla.
    /// </summary>
    protected async Task EjecutarAsync(Func<Task> operacion)
    {
        MensajeError = string.Empty;

        try
        {
            await operacion();
        }
        catch (Exception excepcion)
        {
            (bool irAlLogin, string mensaje) = ErroresDeApi.Interpretar(excepcion);
            MensajeError = mensaje;

            if (irAlLogin)
                Nav.NavigateTo("/login", forceLoad: true);
        }
    }
}
