using GTE.Blazor.Server.Auth;
using GTE.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace GTE.Blazor.Server
{
    /// <summary>
    /// Registro de servicios de la web.
    ///
    /// Está separado de Program para que se pueda probar: el esquema de
    /// autenticación que se registra acá es el que responde cuando alguien pide
    /// una pantalla protegida, y si falta la web se cae con un error 500.
    /// </summary>
    public static class ArranqueDeLaWeb
    {
        /// <summary>Dirección en la que corre la API.</summary>
        public const string DireccionDeLaApi = "http://localhost:5117";

        public static IServiceCollection AgregarServiciosDeLaWeb(this IServiceCollection services)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(DireccionDeLaApi) });

            // Los clientes de la API reciben el servicio de autenticación por
            // inyección de dependencias, así que cada circuito de Blazor usa su
            // propia sesión.
            services.AddScoped<AuthApiClient>();
            services.AddScoped<CursoEscolarApiClient>();
            services.AddScoped<AlumnoApiClient>();
            services.AddScoped<TutorApiClient>();
            services.AddScoped<AutorizacionApiClient>();
            services.AddScoped<RetiroApiClient>();
            services.AddScoped<ReporteApiClient>();
            services.AddScoped<HorarioEspecialApiClient>();

            // No hay que registrar la sesión en el proveedor global: hacerlo
            // dejaría una instancia de otro alcance, que nunca recibe el token del
            // usuario y hace que las llamadas a la API fallen como "no autenticado".
            services.AddScoped<BlazorAuthService>();
            services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BlazorAuthService>());
            services.AddScoped<IAuthService>(sp => sp.GetRequiredService<BlazorAuthService>());

            // El atributo [Authorize] de las pantallas también se revisa en el
            // pedido HTTP inicial, antes de que exista el circuito de Blazor. Sin
            // un esquema registrado ese control no sabía cómo responder y la web se
            // caía con "Unable to find the required 'IAuthenticationService'".
            // Con esta cookie, quien entra a una pantalla protegida sin sesión es
            // redirigido al login.
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/login";
                });
            services.AddAuthorization();

            return services;
        }
    }
}
