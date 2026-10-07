using GTE.Clients;
using Microsoft.AspNetCore.Components;

namespace GTE.Blazor.Server.Components;

public abstract class PaginaConApiBase : ComponentBase
{
    [Inject]
    protected NavigationManager Nav { get; set; } = default!;

    protected string MensajeError { get; set; } = string.Empty;

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
