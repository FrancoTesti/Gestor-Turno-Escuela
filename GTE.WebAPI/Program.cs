using System.Text;
using GTE.Application.Services;
using GTE.Data;
using GTE.WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar DbContext con SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GTEContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configurar Autenticación JWT Bearer
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

builder.Services.AddAuthorization();

// 3. Inyección de Dependencias (Repositorios y Servicios)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IPorteroRepository, PorteroRepository>();
builder.Services.AddScoped<ISecretarioRepository, SecretarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<ICursoEscolarRepository, CursoEscolarRepository>();
builder.Services.AddScoped<ICursoEscolarService, CursoEscolarService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Crea una base nueva o adapta, sin perder alumnos, la base generada por la versión anterior.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GTEContext>();
    await DatabaseSchemaMigrator.MigrateAsync(db);
}

// 4. Configurar Middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// 5. Mapear Endpoints de Autenticación
app.MapAuthEndpoints();
app.MapAlumnoEndpoints();
app.MapCursoEscolarEndpoints();

app.Run();
