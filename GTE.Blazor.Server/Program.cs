using GTE.Blazor.Server;
using GTE.Blazor.Server.Components;
using GTE.Clients;

var builder = WebApplication.CreateBuilder(args);

if (!string.IsNullOrWhiteSpace(builder.Configuration["Api:Url"]))
    ApiConfig.Direccion = builder.Configuration["Api:Url"]!;

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

app.MapCuentaEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
