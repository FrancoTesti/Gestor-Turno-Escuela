using GTE.Blazor.Server;
using GTE.Blazor.Server.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AgregarServiciosDeLaWeb();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
