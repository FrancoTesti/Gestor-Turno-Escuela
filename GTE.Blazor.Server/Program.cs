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

// hack del TP para inyectar la instancia de Auth a los clientes estáticos
using (var scope = app.Services.CreateScope())
{
    var authSvc = scope.ServiceProvider.GetRequiredService<IAuthService>();
    AuthServiceProvider.Register(authSvc);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();