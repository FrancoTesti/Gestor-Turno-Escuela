using GTE.Blazor.Server.Auth;
using GTE.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace GTE.Blazor.Server
{
    public static class ArranqueDeLaWeb
    {
        public static IServiceCollection AgregarServiciosDeLaWeb(this IServiceCollection services)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddScoped(_ => new HttpClient { BaseAddress = ApiConfig.Uri });

            services.AddScoped<AuthApiClient>();
            services.AddScoped<CursoEscolarApiClient>();
            services.AddScoped<AlumnoApiClient>();
            services.AddScoped<TutorApiClient>();
            services.AddScoped<AutorizacionApiClient>();
            services.AddScoped<RetiroApiClient>();
            services.AddScoped<ReporteApiClient>();
            services.AddScoped<HorarioEspecialApiClient>();
            services.AddScoped<SalidaDeCursoApiClient>();

            services.AddScoped<CarteleraApiClient>();

            services.AddScoped<IAuthService, SesionDeLaWeb>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/login";
                    options.Cookie.Name = "GTE.Sesion";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    options.SlidingExpiration = true;

                    options.Events.OnRedirectToLogin = contexto =>
                    {
                        contexto.Response.Redirect("/login");
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = contexto =>
                    {
                        contexto.Response.Redirect("/login");
                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorization();

            return services;
        }
    }
}
