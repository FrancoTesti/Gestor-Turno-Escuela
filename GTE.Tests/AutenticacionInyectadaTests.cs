using System.Net.Http;
using GTE.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace GTE.Tests;

public class AutenticacionInyectadaTests
{
    [Fact]
    public async Task El_cliente_usa_la_sesion_inyectada_y_no_la_global()
    {
        AuthServiceProvider.Register(new AutenticacionConfigurable(sesionIniciada: false));

        var sesionDelCircuito = new AutenticacionConfigurable(sesionIniciada: true, token: "token-del-circuito");
        var cliente = new ClienteDePrueba(sesionDelCircuito);

        await cliente.VerificarSesionAsync();

        using HttpClient http = await cliente.CrearClienteAsync();
        Assert.Equal("token-del-circuito", http.DefaultRequestHeaders.Authorization?.Parameter);
    }

    [Fact]
    public async Task Sin_sesion_en_ninguna_parte_el_cliente_rechaza_la_llamada()
    {
        AuthServiceProvider.Register(new AutenticacionConfigurable(sesionIniciada: false));

        var cliente = new ClienteDePrueba(new AutenticacionConfigurable(sesionIniciada: false));

        await Assert.ThrowsAsync<Exception>(() => cliente.VerificarSesionAsync());
    }

    [Fact]
    public void Los_clientes_de_la_api_aceptan_el_servicio_de_autenticacion()
    {
        var sesion = new AutenticacionConfigurable(sesionIniciada: true);

        Assert.NotNull(new AlumnoApiClient(sesion));
        Assert.NotNull(new CursoEscolarApiClient(sesion));
        Assert.NotNull(new RetiroApiClient(sesion));
    }

    [Fact]
    public async Task El_cliente_resuelto_por_el_contenedor_supera_la_verificacion_de_sesion()
    {
        AuthServiceProvider.Register(new AutenticacionConfigurable(sesionIniciada: false));

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddScoped<GTE.Clients.IAuthService>(_ =>
            new AutenticacionConfigurable(sesionIniciada: true, token: "token-del-circuito"));
        services.AddScoped<AlumnoApiClient>();

        using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope alcance = provider.CreateScope();
        var cliente = alcance.ServiceProvider.GetRequiredService<AlumnoApiClient>();

        Exception? error = await Record.ExceptionAsync(() => cliente.GetAllAsync());

        if (error is not null)
            Assert.DoesNotContain("no autenticado", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class ClienteDePrueba : BaseApiClient
    {
        public ClienteDePrueba(IAuthService autenticacion) : base(autenticacion)
        {
        }

        public Task VerificarSesionAsync() => EnsureAuthenticatedAsync();

        public Task<HttpClient> CrearClienteAsync() => CreateHttpClientAsync();
    }
}
