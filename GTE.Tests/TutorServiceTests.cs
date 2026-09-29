using GTE.Application.Services;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Tests;

public class TutorServiceTests
{
    private static Usuario UnUsuario(string nombre) => new(nombre, "clave123");

    private static Tutor UnTutor(int id, string dni, string nombreUsuario)
    {
        var usuario = UnUsuario(nombreUsuario);
        usuario.AsignarIdGenerado(id);

        var tutor = new Tutor("Nombre", "Apellido", dni, "Padre", "11-5555-5555", usuario);
        tutor.AsignarIdGenerado(id);
        return tutor;
    }

    private static TutorDTO UnDto(string dni = "12345678", string nombreUsuario = "tutor1") => new()
    {
        Nombre = "Nombre",
        Apellido = "Apellido",
        Dni = dni,
        Parentesco = "Padre",
        Telefono = "11-5555-5555",
        NombreUsuario = nombreUsuario,
        Contrasena = "clave123"
    };

    [Fact]
    public async Task No_se_puede_registrar_un_tutor_con_un_dni_ya_existente()
    {
        var tutores = new TutorRepositoryFalso(new[] { UnTutor(1, "12345678", "tutor1") });
        var servicio = new TutorService(tutores, new UsuarioRepositoryFalso());

        var excepcion = await Assert.ThrowsAsync<ArgumentException>(
            () => servicio.AddAsync(UnDto(dni: "12345678", nombreUsuario: "otro")));

        Assert.Contains("DNI", excepcion.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task No_se_puede_registrar_un_tutor_con_un_usuario_ya_en_uso()
    {
        var usuarios = new UsuarioRepositoryFalso(new[] { UnUsuario("tutor1") });
        var servicio = new TutorService(new TutorRepositoryFalso(), usuarios);

        var excepcion = await Assert.ThrowsAsync<ArgumentException>(
            () => servicio.AddAsync(UnDto(dni: "99999999", nombreUsuario: "tutor1")));

        Assert.Contains("usuario", excepcion.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task No_se_guarda_nada_si_el_dni_tiene_formato_invalido()
    {
        var tutores = new TutorRepositoryFalso();
        var usuarios = new UsuarioRepositoryFalso();
        var servicio = new TutorService(tutores, usuarios);

        await Assert.ThrowsAsync<ArgumentException>(
            () => servicio.AddAsync(UnDto(dni: "abc")));

        Assert.Null(tutores.UltimoAgregado);
        Assert.Null(usuarios.UltimoAgregado);
    }

    [Fact]
    public async Task Al_registrar_un_tutor_se_crea_su_usuario()
    {
        var tutores = new TutorRepositoryFalso();
        var usuarios = new UsuarioRepositoryFalso();
        var servicio = new TutorService(tutores, usuarios);

        await servicio.AddAsync(UnDto());

        Assert.NotNull(usuarios.UltimoAgregado);
        Assert.Equal("tutor1", usuarios.UltimoAgregado!.NombreUsuario);
        Assert.NotNull(tutores.UltimoAgregado);
    }

    [Fact]
    public async Task Las_consultas_no_devuelven_la_contrasena()
    {
        var tutores = new TutorRepositoryFalso(new[] { UnTutor(1, "12345678", "tutor1") });
        var servicio = new TutorService(tutores, new UsuarioRepositoryFalso());

        var listado = (await servicio.GetAllAsync()).ToList();

        Assert.Single(listado);
        Assert.Equal("tutor1", listado[0].NombreUsuario);
        Assert.Empty(listado[0].Contrasena);
    }

    [Fact]
    public async Task No_se_puede_cambiar_el_dni_al_de_otro_tutor()
    {
        var tutores = new TutorRepositoryFalso(new[]
        {
            UnTutor(1, "12345678", "tutor1"),
            UnTutor(2, "87654321", "tutor2"),
        });
        var servicio = new TutorService(tutores, new UsuarioRepositoryFalso());

        var dto = UnDto(dni: "87654321", nombreUsuario: "tutor1");
        dto.IdTutor = 1;

        await Assert.ThrowsAsync<ArgumentException>(() => servicio.UpdateAsync(dto));
    }

    [Fact]
    public async Task Se_puede_modificar_un_tutor_conservando_su_propio_dni()
    {
        var tutores = new TutorRepositoryFalso(new[] { UnTutor(1, "12345678", "tutor1") });
        var servicio = new TutorService(tutores, new UsuarioRepositoryFalso());

        var dto = UnDto(dni: "12345678", nombreUsuario: "tutor1");
        dto.IdTutor = 1;

        Assert.True(await servicio.UpdateAsync(dto));
    }

    [Fact]
    public async Task Al_eliminar_un_tutor_se_elimina_su_usuario()
    {
        var tutores = new TutorRepositoryFalso(new[] { UnTutor(1, "12345678", "tutor1") });
        var usuarios = new UsuarioRepositoryFalso();
        var servicio = new TutorService(tutores, usuarios);

        Assert.True(await servicio.DeleteAsync(1));

        Assert.Contains(1, usuarios.Eliminados);
    }
}
