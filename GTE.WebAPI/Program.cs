using GTE.WebAPI;
using GTE.Application.Services;
using GTE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Dependency Injection
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<ICursoEscolarRepository, CursoEscolarRepository>();
builder.Services.AddScoped<ICursoEscolarService, CursoEscolarService>();

var app = builder.Build();

// Swagger habilitado siempre (es la interfaz de pruebas de la Entrega 1).
app.UseSwagger();
app.UseSwaggerUI();

// Map endpoints
app.MapAlumnoEndpoints();
app.MapCursoEscolarEndpoints();

app.Run();
