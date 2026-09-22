using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Tests;

/// <summary>
/// Dobles de los servicios, usados solo para poder construir los endpoints
/// en las pruebas sin depender de la base de datos.
/// </summary>
internal sealed class AlumnoServiceFalso : IAlumnoService
{
    public Task<AlumnoDTO> AddAsync(AlumnoDTO dto) => Task.FromResult(dto);

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<AlumnoDTO?> GetAsync(int id) => Task.FromResult<AlumnoDTO?>(null);

    public Task<IEnumerable<AlumnoDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<AlumnoDTO>>(Array.Empty<AlumnoDTO>());

    public Task<bool> UpdateAsync(AlumnoDTO dto) => Task.FromResult(true);

    public Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteriaDTO) =>
        Task.FromResult<IEnumerable<AlumnoDTO>>(Array.Empty<AlumnoDTO>());
}

internal sealed class CursoEscolarServiceFalso : ICursoEscolarService
{
    public Task<CursoEscolarDTO> AddAsync(CursoEscolarDTO dto) => Task.FromResult(dto);

    public Task<CursoEscolarDTO?> GetAsync(int id) => Task.FromResult<CursoEscolarDTO?>(null);

    public Task<IEnumerable<CursoEscolarDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<CursoEscolarDTO>>(Array.Empty<CursoEscolarDTO>());

    public Task<bool> UpdateAsync(CursoEscolarDTO dto) => Task.FromResult(true);

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);
}

internal sealed class RetiroServiceFalso : IRetiroService
{
    public Task<IEnumerable<RetiroDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<RetiroDTO>>(Array.Empty<RetiroDTO>());

    public Task<RetiroDTO?> GetAsync(int id) => Task.FromResult<RetiroDTO?>(null);

    public Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> AddAsync(RetiroDTO dto) =>
        Task.FromResult((true, string.Empty, (RetiroDTO?)dto));

    public Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> UpdateAsync(RetiroDTO dto) =>
        Task.FromResult((true, string.Empty, (RetiroDTO?)dto));

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId) =>
        Task.FromResult<IEnumerable<AlumnoDTO>>(Array.Empty<AlumnoDTO>());
}

internal sealed class TutorRepositoryFalso : ITutorRepository
{
    public Task AddAsync(Tutor tutor) => Task.CompletedTask;

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<Tutor?> GetAsync(int id) => Task.FromResult<Tutor?>(null);

    public Task<IEnumerable<Tutor>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Tutor>>(Array.Empty<Tutor>());

    public Task<bool> UpdateAsync(Tutor tutor) => Task.FromResult(true);

    public Task<bool> DniExisteAsync(string dni, int? excludeId = null) => Task.FromResult(false);
}

/// <summary>
/// Servicio de autenticación configurable, para representar la sesión de un
/// circuito de Blazor o la de un usuario del escritorio.
/// </summary>
internal sealed class AutenticacionConfigurable : GTE.Clients.IAuthService
{
    private readonly bool _sesionIniciada;

    public AutenticacionConfigurable(bool sesionIniciada, string? token = null)
    {
        _sesionIniciada = sesionIniciada;
        Token = token ?? (sesionIniciada ? "token" : null);
    }

    public string? Token { get; private set; }

    public Task<bool> IsAuthenticatedAsync() => Task.FromResult(_sesionIniciada && Token is not null);

    public Task<string?> GetTokenAsync() => Task.FromResult(Token);

    public Task<string?> GetUsernameAsync() => Task.FromResult<string?>("usuario");

    public Task<string?> GetRoleAsync() => Task.FromResult<string?>("Secretario");

    public Task<string?> GetNombreCompletoAsync() => Task.FromResult<string?>("Usuario de Prueba");

    public Task<bool> LoginAsync(string username, string password) => Task.FromResult(true);

    public Task LogoutAsync()
    {
        Token = null;
        return Task.CompletedTask;
    }

    public Task CheckTokenExpirationAsync() => Task.CompletedTask;
}
