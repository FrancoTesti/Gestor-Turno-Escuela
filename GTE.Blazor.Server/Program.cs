using GTE.Blazor.Server;
using GTE.Blazor.Server.Components;
using GTE.Clients;

var builder = WebApplication.CreateBuilder(args);

// La dirección de la API se puede cambiar desde appsettings.json ("Api:Url") o
// con la variable de entorno GTE_API_URL. Es lo que hace falta para usar el
// sistema desde otro equipo, como el celular del tutor o la pantalla de la puerta.
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
