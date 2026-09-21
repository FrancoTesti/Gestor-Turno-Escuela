using GTE.Clients;
using GTE.WebAPI;

namespace GTE.Tests;

public class PermisosDeUsuarioTests
{
    private const string Secretario = "Secretario";
    private const string Portero = "Portero";
    private const string Tutor = "Tutor";

    [Theory]
    [InlineData(Secretario)]
    [InlineData(Portero)]
    [InlineData(Tutor)]
    [InlineData(null)]
    public async Task Las_reglas_de_la_interfaz_coinciden_con_las_politicas_de_la_api(string? rol)
    {
        Assert.Equal(
            await AutorizacionDePrueba.AutorizarAsync(Politicas.SoloSecretario, rol),
            PermisosDeUsuario.PuedeAdministrarAlumnos(rol));

        Assert.Equal(
            await AutorizacionDePrueba.AutorizarAsync(Politicas.SoloSecretario, rol),
            PermisosDeUsuario.PuedeAdministrarCursos(rol));

        Assert.Equal(
            await AutorizacionDePrueba.AutorizarAsync(Politicas.GestionRetiros, rol),
            PermisosDeUsuario.PuedeGestionarRetiros(rol));

        Assert.Equal(
            await AutorizacionDePrueba.AutorizarAsync(Politicas.LecturaCursos, rol),
            PermisosDeUsuario.PuedeVerCursos(rol));
    }
}
