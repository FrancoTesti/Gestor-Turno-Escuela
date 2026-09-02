using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using GTE.Dominio;

namespace GTE.Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly GTEContext _context;

        public AlumnoRepository(GTEContext context)
        {
            _context = context;
        }

        public AlumnoRepository()
        {
            _context = new GTEContext();
        }

        public async Task AddAsync(Alumno alumno)
        {
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
        }

        public async Task<Alumno?> GetAsync(int id) =>
            await _context.Alumnos.Include(a => a.CursoEscolar).FirstOrDefaultAsync(a => a.IdAlumno == id);

        public async Task<IEnumerable<Alumno>> GetAllAsync() =>
            await _context.Alumnos.Include(a => a.CursoEscolar).ToListAsync();

        public async Task<bool> UpdateAsync(Alumno alumno)
        {
            var existing = await _context.Alumnos.FindAsync(alumno.IdAlumno);
            if (existing == null) return false;

            existing.SetNombre(alumno.Nombre);
            existing.SetApellido(alumno.Apellido);
            existing.SetCurso(alumno.IdCurso);
            existing.SetEstado(alumno.Estado);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return false;
            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria)
        {
            // Esta búsqueda usa ADO.NET deliberadamente. El resto del acceso a
            // datos permanece implementado con Entity Framework Core.
            const string sql = @"
                SELECT a.IdAlumno, a.Nombre, a.Apellido, a.IdCurso, a.Estado,
                       c.Grado, c.Curso, c.Turno, c.HorarioSalida
                FROM Alumnos a
                INNER JOIN Cursos c ON c.IdCurso = a.IdCurso
                WHERE (@Nombre IS NULL OR Nombre LIKE @NombreBusqueda OR Apellido LIKE @NombreBusqueda)
                  AND (@Grado IS NULL OR c.Grado = @Grado)
                  AND (@Curso IS NULL OR c.Curso = @Curso)
                  AND (@Estado IS NULL OR a.Estado = @Estado)
                ORDER BY a.Apellido, a.Nombre";

            var alumnos = new List<Alumno>();
            var connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException("No se configuró la conexión a la base de datos.");

            await using var connection = new SqlConnection(connectionString);
            await using var command = new SqlCommand(sql, connection);

            AddNullableString(command, "@Nombre", criteria.Nombre);
            AddNullableString(command, "@NombreBusqueda",
                string.IsNullOrWhiteSpace(criteria.Nombre) ? null : $"%{criteria.Nombre}%");
            AddNullableString(command, "@Grado", criteria.Grado);
            AddNullableString(command, "@Curso", criteria.Curso);
            AddNullableString(command, "@Estado", criteria.Estado);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var alumno = new Alumno(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3));
                alumno.SetEstado(reader.GetString(4));
                alumno.SetCursoEscolar(new CursoEscolar(
                    reader.GetInt32(3), reader.GetString(5), reader.GetString(6),
                    reader.GetString(7), reader.GetTimeSpan(8)));
                alumnos.Add(alumno);
            }

            return alumnos;
        }

        private static void AddNullableString(SqlCommand command, string parameterName, string? value)
        {
            command.Parameters.Add(parameterName, System.Data.SqlDbType.NVarChar, 200).Value =
                string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;
        }
    }
}
