using System.Text;
using GTE.Application.Services;
using GTE.Data;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GTEContext>(options =>
    options.UseSqlServer(connectionString));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "SuperClaveSecretaGestorTurnoEscuela2026SecureKey12345!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddPoliticasDeAutorizacion();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IPorteroRepository, PorteroRepository>();
builder.Services.AddScoped<ISecretarioRepository, SecretarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<ICursoEscolarRepository, CursoEscolarRepository>();
builder.Services.AddScoped<ICursoEscolarService, CursoEscolarService>();
builder.Services.AddScoped<IAutorizacionRepository, AutorizacionRepository>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<IRetiroRepository, RetiroRepository>();
builder.Services.AddScoped<IRetiroService, RetiroService>();
builder.Services.AddScoped<ISalidaDeCursoRepository, SalidaDeCursoRepository>();
builder.Services.AddScoped<ISalidaDeCursoService, SalidaDeCursoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<GTE.Data.IHorarioEspecialRepository, GTE.Data.HorarioEspecialRepository>();
builder.Services.AddScoped<Servicio.IHorarioEspecialService, Servicio.HorarioEspecialService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GTEContext>();
    await DatabaseSchemaMigrator.MigrateAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapAlumnoEndpoints();
app.MapCursoEscolarEndpoints();
app.MapAutorizacionEndpoints();
app.MapRetiroEndpoints();
app.MapSalidaDeCursoEndpoints();
app.MapReporteEndpoints();
app.MapHorarioEspecialEndpoints();
app.MapTutorEndpoints();

app.Run();
