using GTE.Blazor.Server.Components;
using GTE.Blazor.Server.Auth;
using GTE.Clients;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5117") });

// registro los clientes
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<CursoEscolarApiClient>();
builder.Services.AddScoped<AlumnoApiClient>();

// registro la autenticación
builder.Services.AddScoped<BlazorAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BlazorAuthService>());
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<BlazorAuthService>());

var app = builder.Build();

// Los clientes de la API reciben el servicio de autenticación por inyección de
// dependencias, así que cada circuito de Blazor usa su propia sesión.
// No hay que registrar nada en el proveedor global desde acá: hacerlo dejaría
// una instancia de otro alcance, que nunca recibe el token del usuario y hace
// que las llamadas a la API fallen como "no autenticado".

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
