using GTE.DTOs;

namespace GTE.Clients;

public class FiltroDeAlumnos
{
    public string? Nombre { get; set; }

    public string? Grado { get; set; }

    public string? Curso { get; set; }

    public string? Estado { get; set; }

    public AlumnoCriteriaDTO ToCriteria() => new()
    {
        Nombre = Normalizar(Nombre),
        Grado = Normalizar(Grado),
        Curso = Normalizar(Curso),
        Estado = Normalizar(Estado)
    };

    public bool EstaVacio() =>
        Normalizar(Nombre) is null &&
        Normalizar(Grado) is null &&
        Normalizar(Curso) is null &&
        Normalizar(Estado) is null;

    public void Limpiar()
    {
        Nombre = null;
        Grado = null;
        Curso = null;
        Estado = null;
    }

    private static string? Normalizar(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
