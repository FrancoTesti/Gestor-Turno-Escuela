using GTE.Application.Services;
using GTE.Data;
using GTE.Dominio;
using GTE.DTOs;

namespace GTE.Tests;

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

internal sealed class AutorizacionServiceFalso : IAutorizacionService
{
    public Task<IEnumerable<AutorizacionDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<AutorizacionDTO>>(Array.Empty<AutorizacionDTO>());

    public Task<AutorizacionDTO?> GetAsync(int id) => Task.FromResult<AutorizacionDTO?>(null);

    public Task<IEnumerable<AutorizacionDTO>> GetByTutorIdAsync(int tutorId) =>
        Task.FromResult<IEnumerable<AutorizacionDTO>>(Array.Empty<AutorizacionDTO>());

    public Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId) =>
        Task.FromResult<IEnumerable<AlumnoDTO>>(Array.Empty<AlumnoDTO>());

    public Task<(bool Exito, string Mensaje, AutorizacionDTO? Autorizacion)> AddAsync(AutorizacionDTO dto) =>
        Task.FromResult((true, string.Empty, (AutorizacionDTO?)dto));

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId) => Task.FromResult(true);
}

internal sealed class TutorRepositoryFalso : ITutorRepository
{
    private readonly List<Tutor> _tutores;

    public TutorRepositoryFalso()
    {
        _tutores = new List<Tutor>();
    }

    public TutorRepositoryFalso(IEnumerable<Tutor> tutores)
    {
        _tutores = tutores.ToList();
    }

    public Tutor? UltimoAgregado { get; private set; }

    public Task AddAsync(Tutor tutor)
    {
        _tutores.Add(tutor);
        UltimoAgregado = tutor;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<Tutor?> GetAsync(int id) =>
        Task.FromResult(_tutores.FirstOrDefault(t => t.IdTutor == id));

    public Task<IEnumerable<Tutor>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Tutor>>(_tutores.ToList());

    public Task<bool> UpdateAsync(Tutor tutor) => Task.FromResult(true);

    public Task<bool> DniExisteAsync(string dni, int? excludeId = null) =>
        Task.FromResult(_tutores.Any(t =>
            t.Dni == dni && (!excludeId.HasValue || t.IdTutor != excludeId.Value)));
}

internal sealed class UsuarioRepositoryFalso : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios;

    public UsuarioRepositoryFalso(IEnumerable<Usuario>? usuarios = null)
    {
        _usuarios = usuarios?.ToList() ?? new List<Usuario>();
    }

    public Usuario? UltimoAgregado { get; private set; }

    public List<int> Eliminados { get; } = new();

    public Task AddAsync(Usuario usuario)
    {
        _usuarios.Add(usuario);
        UltimoAgregado = usuario;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(int id)
    {
        Eliminados.Add(id);
        return Task.FromResult(true);
    }

    public Task<Usuario?> GetAsync(int id) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.IdUsuario == id));

    public Task<IEnumerable<Usuario>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Usuario>>(_usuarios.ToList());

    public Task<bool> UpdateAsync(Usuario usuario) => Task.FromResult(true);

    public Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario));

    public Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null) =>
        Task.FromResult(_usuarios.Any(u =>
            u.NombreUsuario == nombreUsuario && (!excludeId.HasValue || u.IdUsuario != excludeId.Value)));
}

internal sealed class AutorizacionRepositoryFalso : IAutorizacionRepository
{
    public Task<IEnumerable<Autorizacion>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Autorizacion>>(Array.Empty<Autorizacion>());

    public Task<Autorizacion?> GetByIdAsync(int id) => Task.FromResult<Autorizacion?>(null);

    public Task<IEnumerable<Autorizacion>> GetByTutorIdAsync(int tutorId) =>
        Task.FromResult<IEnumerable<Autorizacion>>(Array.Empty<Autorizacion>());

    public Task<IEnumerable<Alumno>> GetAlumnosByTutorIdAsync(int tutorId) =>
        Task.FromResult<IEnumerable<Alumno>>(Array.Empty<Alumno>());

    public Task<Autorizacion?> GetByTutorAndAlumnoAsync(int tutorId, int alumnoId) =>
        Task.FromResult<Autorizacion?>(null);

    public Task<bool> EstaAutorizadoAsync(int tutorId, int alumnoId) => Task.FromResult(false);

    public Task AddAsync(Autorizacion autorizacion) => Task.CompletedTask;

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<bool> DeleteByTutorAndAlumnoAsync(int tutorId, int alumnoId) => Task.FromResult(true);
}

internal sealed class ReporteServiceFalso : IReporteService
{
    public Task<IEnumerable<AlumnosPorCursoDTO>> GetAlumnosPorCursoAsync() =>
        Task.FromResult<IEnumerable<AlumnosPorCursoDTO>>(Array.Empty<AlumnosPorCursoDTO>());

    public Task<IEnumerable<RetiroDTO>> GetRetirosAsync(DateTime? desde, DateTime? hasta) =>
        Task.FromResult<IEnumerable<RetiroDTO>>(Array.Empty<RetiroDTO>());
}

internal sealed class TutorServiceFalso : ITutorService
{
    public Task<IEnumerable<TutorDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<TutorDTO>>(Array.Empty<TutorDTO>());

    public Task<TutorDTO?> GetAsync(int id) => Task.FromResult<TutorDTO?>(null);

    public Task<TutorDTO> AddAsync(TutorDTO dto) => Task.FromResult(dto);

    public Task<bool> UpdateAsync(TutorDTO dto) => Task.FromResult(true);

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);
}

internal sealed class AlumnoRepositoryFalso : IAlumnoRepository
{
    private readonly List<Alumno> _alumnos;

    public AlumnoRepositoryFalso(params Alumno[] alumnos)
    {
        _alumnos = alumnos.ToList();
    }

    public Task AddAsync(Alumno alumno)
    {
        _alumnos.Add(alumno);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<Alumno?> GetAsync(int id) =>
        Task.FromResult(_alumnos.FirstOrDefault(a => a.IdAlumno == id));

    public Task<IEnumerable<Alumno>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Alumno>>(_alumnos.ToList());

    public Task<bool> UpdateAsync(Alumno alumno)
    {
        var existente = _alumnos.FirstOrDefault(a => a.IdAlumno == alumno.IdAlumno);
        if (existente is null) return Task.FromResult(false);

        existente.SetNombre(alumno.Nombre);
        existente.SetApellido(alumno.Apellido);
        existente.SetCurso(alumno.IdCurso);
        existente.SetEstado(alumno.Estado);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria) =>
        Task.FromResult<IEnumerable<Alumno>>(_alumnos.ToList());
}

internal sealed class CursoEscolarRepositoryFalso : ICursoEscolarRepository
{
    private readonly List<CursoEscolar> _cursos;

    public CursoEscolarRepositoryFalso(params CursoEscolar[] cursos)
    {
        _cursos = cursos.ToList();
    }

    public Task AddAsync(CursoEscolar curso)
    {
        _cursos.Add(curso);
        return Task.CompletedTask;
    }

    public Task<CursoEscolar?> GetAsync(int id) =>
        Task.FromResult(_cursos.FirstOrDefault(c => c.IdCurso == id));

    public Task<IEnumerable<CursoEscolar>> GetAllAsync() =>
        Task.FromResult<IEnumerable<CursoEscolar>>(_cursos.ToList());

    public Task<bool> UpdateAsync(CursoEscolar curso) => Task.FromResult(true);

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);
}

internal sealed class RetiroServiceConDatos : IRetiroService
{
    private readonly List<RetiroDTO> _retiros;

    public RetiroServiceConDatos(params RetiroDTO[] retiros)
    {
        _retiros = retiros.ToList();
    }

    public Task<IEnumerable<RetiroDTO>> GetAllAsync() =>
        Task.FromResult<IEnumerable<RetiroDTO>>(_retiros.ToList());

    public Task<RetiroDTO?> GetAsync(int id) =>
        Task.FromResult(_retiros.FirstOrDefault(r => r.IdRetiro == id));

    public Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> AddAsync(RetiroDTO dto) =>
        Task.FromResult((true, string.Empty, (RetiroDTO?)dto));

    public Task<(bool Exito, string Mensaje, RetiroDTO? Retiro)> UpdateAsync(RetiroDTO dto) =>
        Task.FromResult((true, string.Empty, (RetiroDTO?)dto));

    public Task<bool> DeleteAsync(int id) => Task.FromResult(true);

    public Task<IEnumerable<AlumnoDTO>> GetAlumnosAutorizadosAsync(int tutorId) =>
        Task.FromResult<IEnumerable<AlumnoDTO>>(Array.Empty<AlumnoDTO>());
}

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

internal sealed class SalidaDeCursoRepositoryFalso : ISalidaDeCursoRepository
{
    private readonly List<SalidaDeCurso> _salidas;
    private int _proximoId;

    public SalidaDeCursoRepositoryFalso(params SalidaDeCurso[] salidas)
    {
        _salidas = salidas.ToList();
        _proximoId = salidas.Length + 1;
    }

    public Task AddAsync(SalidaDeCurso salida)
    {
        salida.SetIdSalidaDeCurso(_proximoId++);
        _salidas.Add(salida);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<SalidaDeCurso>> GetDelDiaAsync(DateTime dia) =>
        Task.FromResult<IEnumerable<SalidaDeCurso>>(
            _salidas.Where(s => s.FechaHoraInicio.Date == dia.Date).ToList());

    public Task<IEnumerable<SalidaDeCurso>> GetEnCursoAsync() =>
        Task.FromResult<IEnumerable<SalidaDeCurso>>(_salidas.Where(s => s.EstaEnCurso).ToList());

    public Task<SalidaDeCurso?> GetAsync(int id) =>
        Task.FromResult(_salidas.FirstOrDefault(s => s.IdSalidaDeCurso == id));

    public Task<bool> UpdateAsync(SalidaDeCurso salida)
    {
        var existente = _salidas.FirstOrDefault(s => s.IdSalidaDeCurso == salida.IdSalidaDeCurso);
        if (existente is null) return Task.FromResult(false);

        if (existente.EstaEnCurso && !salida.EstaEnCurso)
            existente.Finalizar(salida.FechaHoraFin!.Value);

        return Task.FromResult(true);
    }
}

internal sealed class SalidaDeCursoServiceFalso : ISalidaDeCursoService
{
    public Task<CarteleraDTO> GetCarteleraAsync() => Task.FromResult(new CarteleraDTO());

    public Task<IEnumerable<SalidaDeCursoDTO>> GetDelDiaAsync(DateTime dia) =>
        Task.FromResult<IEnumerable<SalidaDeCursoDTO>>(Array.Empty<SalidaDeCursoDTO>());

    public Task<(bool Exito, string Mensaje, SalidaDeCursoDTO? Salida)> IniciarAsync(int idCurso, Personal personal) =>
        Task.FromResult((true, string.Empty, (SalidaDeCursoDTO?)null));

    public Task<(bool Exito, string Mensaje)> FinalizarAsync(int idSalida) =>
        Task.FromResult((true, string.Empty));
}
