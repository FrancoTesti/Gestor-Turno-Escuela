using GTE.Application.Services;
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
