using GTE.Clients;

namespace GTE.Tests;

public class ErroresDeApiTests
{
    [Fact]
    public void Una_sesion_expirada_obliga_a_volver_al_login()
    {
        var resultado = ErroresDeApi.Interpretar(new SesionExpiradaException("La sesión expiró."));

        Assert.True(resultado.IrAlLogin);
    }

    [Fact]
    public void Una_sesion_expirada_explica_el_motivo()
    {
        var resultado = ErroresDeApi.Interpretar(new SesionExpiradaException("La sesión expiró."));

        Assert.Contains("sesión", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Una_falta_de_permisos_no_cierra_la_sesion()
    {
        var resultado = ErroresDeApi.Interpretar(new SinPermisoException("Sin permiso."));

        Assert.False(resultado.IrAlLogin);
    }

    [Fact]
    public void Una_falta_de_permisos_avisa_que_el_rol_no_alcanza()
    {
        var resultado = ErroresDeApi.Interpretar(new SinPermisoException("Sin permiso."));

        Assert.Contains("permiso", resultado.Mensaje, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Un_error_inesperado_se_muestra_sin_cerrar_la_sesion()
    {
        var resultado = ErroresDeApi.Interpretar(new InvalidOperationException("Fallo de red."));

        Assert.False(resultado.IrAlLogin);
        Assert.Contains("Fallo de red.", resultado.Mensaje);
    }
}
