using Microsoft.EntityFrameworkCore;

namespace GTE.Data
{
    public static class DatabaseSchemaMigrator
    {
        public static async Task MigrateAsync(GTEContext context)
        {
            // Compatibiliza las bases creadas por la versión anterior, que usaba
            // Grado y Curso directamente en Alumnos y no tenía Turno en Cursos.
            await context.Database.ExecuteSqlRawAsync(@"
IF COL_LENGTH('Cursos', 'Turno') IS NULL
    EXEC(N'ALTER TABLE Cursos ADD Turno nvarchar(20) NULL');");

            await context.Database.ExecuteSqlRawAsync(@"
UPDATE Cursos SET Turno = N'Mañana' WHERE Turno IS NULL OR LTRIM(RTRIM(Turno)) = N'';
ALTER TABLE Cursos ALTER COLUMN Turno nvarchar(20) NOT NULL;");

            await context.Database.ExecuteSqlRawAsync(@"
IF COL_LENGTH('Alumnos', 'IdCurso') IS NULL
    EXEC(N'ALTER TABLE Alumnos ADD IdCurso int NULL');");

            await context.Database.ExecuteSqlRawAsync(@"
IF COL_LENGTH('Alumnos', 'Grado') IS NOT NULL
    EXEC(N'
        INSERT INTO Cursos (Grado, Curso, Turno, HorarioSalida)
        SELECT DISTINCT a.Grado, a.Curso, N''Mañana'', CAST(''12:00:00'' AS time)
        FROM Alumnos a
        WHERE NOT EXISTS (
            SELECT 1 FROM Cursos c
            WHERE c.Grado = a.Grado AND c.Curso = a.Curso
        );

        UPDATE a
           SET IdCurso = c.IdCurso
        FROM Alumnos a
        INNER JOIN Cursos c ON c.Grado = a.Grado AND c.Curso = a.Curso;
    ');");

            await context.Database.ExecuteSqlRawAsync(@"
IF EXISTS (SELECT 1 FROM Alumnos WHERE IdCurso IS NULL)
    THROW 51000, 'No se pudo asociar uno o más alumnos con un curso.', 1;

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('Alumnos') AND name = 'IdCurso' AND is_nullable = 1
)
    ALTER TABLE Alumnos ALTER COLUMN IdCurso int NOT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Alumnos_Cursos_IdCurso')
    ALTER TABLE Alumnos ADD CONSTRAINT FK_Alumnos_Cursos_IdCurso
        FOREIGN KEY (IdCurso) REFERENCES Cursos(IdCurso);

IF COL_LENGTH('Alumnos', 'Grado') IS NOT NULL
    ALTER TABLE Alumnos DROP COLUMN Grado;
IF COL_LENGTH('Alumnos', 'Curso') IS NOT NULL
    ALTER TABLE Alumnos DROP COLUMN Curso;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Alumnos_IdCurso' AND object_id = OBJECT_ID('Alumnos'))
    CREATE INDEX IX_Alumnos_IdCurso ON Alumnos(IdCurso);");

            // Migración para maestro-detalle de Retiros:
            // 1. Si la tabla Retiros tiene la columna IdAlumno:
            //    - Si la tabla Retiros contiene datos, se detiene y avisa.
            //    - Si está vacía, se elimina la columna IdAlumno.
            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('Retiros', 'U') IS NOT NULL AND COL_LENGTH('Retiros', 'IdAlumno') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM Retiros)
        THROW 51001, 'La tabla Retiros contiene registros previos del modelo anterior. Deteniendo la migración para evitar pérdida de datos.', 1;

    DECLARE @FkName nvarchar(200);
    SELECT @FkName = name FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID('Retiros') AND referenced_object_id = OBJECT_ID('Alumnos');
    IF @FkName IS NOT NULL
        EXEC('ALTER TABLE Retiros DROP CONSTRAINT ' + @FkName);

    ALTER TABLE Retiros DROP COLUMN IdAlumno;
END;");

            // 2. Crear la tabla DetalleRetiros si no existe
            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('DetalleRetiros', 'U') IS NULL
BEGIN
    CREATE TABLE DetalleRetiros (
        IdDetalleRetiro int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdRetiro int NOT NULL,
        IdAlumno int NOT NULL,
        HoraSalida time NOT NULL,
        Estado nvarchar(50) NOT NULL,
        CONSTRAINT FK_DetalleRetiros_Retiros_IdRetiro FOREIGN KEY (IdRetiro) REFERENCES Retiros(IdRetiro) ON DELETE CASCADE,
        CONSTRAINT FK_DetalleRetiros_Alumnos_IdAlumno FOREIGN KEY (IdAlumno) REFERENCES Alumnos(IdAlumno)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleRetiros_IdRetiro' AND object_id = OBJECT_ID('DetalleRetiros'))
    CREATE INDEX IX_DetalleRetiros_IdRetiro ON DetalleRetiros(IdRetiro);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleRetiros_IdAlumno' AND object_id = OBJECT_ID('DetalleRetiros'))
    CREATE INDEX IX_DetalleRetiros_IdAlumno ON DetalleRetiros(IdAlumno);");
        }
    }
}
