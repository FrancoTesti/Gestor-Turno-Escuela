using Microsoft.EntityFrameworkCore;

namespace GTE.Data
{
    public static class DatabaseSchemaMigrator
    {
        public static async Task MigrateAsync(GTEContext context)
        {
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

            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('SalidasDeCurso', 'U') IS NULL
BEGIN
    CREATE TABLE SalidasDeCurso (
        IdSalidaDeCurso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdCurso int NOT NULL,
        IdPersonal int NOT NULL,
        FechaHoraInicio datetime2 NOT NULL,
        FechaHoraFin datetime2 NULL,
        CONSTRAINT FK_SalidasDeCurso_Cursos_IdCurso FOREIGN KEY (IdCurso) REFERENCES Cursos(IdCurso),
        CONSTRAINT FK_SalidasDeCurso_Personal_IdPersonal FOREIGN KEY (IdPersonal) REFERENCES Personal(IdPersonal)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SalidasDeCurso_IdCurso_FechaHoraFin' AND object_id = OBJECT_ID('SalidasDeCurso'))
    CREATE INDEX IX_SalidasDeCurso_IdCurso_FechaHoraFin ON SalidasDeCurso(IdCurso, FechaHoraFin);");

            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('Cursos', 'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('Cursos', 'HorarioLunes') IS NULL
        EXEC(N'ALTER TABLE Cursos ADD HorarioLunes time NULL');
    IF COL_LENGTH('Cursos', 'HorarioMartes') IS NULL
        EXEC(N'ALTER TABLE Cursos ADD HorarioMartes time NULL');
    IF COL_LENGTH('Cursos', 'HorarioMiercoles') IS NULL
        EXEC(N'ALTER TABLE Cursos ADD HorarioMiercoles time NULL');
    IF COL_LENGTH('Cursos', 'HorarioJueves') IS NULL
        EXEC(N'ALTER TABLE Cursos ADD HorarioJueves time NULL');
    IF COL_LENGTH('Cursos', 'HorarioViernes') IS NULL
        EXEC(N'ALTER TABLE Cursos ADD HorarioViernes time NULL');
END;");

            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('Cursos', 'U') IS NOT NULL AND COL_LENGTH('Cursos', 'HorarioLunes') IS NOT NULL
BEGIN
    UPDATE Cursos
    SET HorarioLunes = ISNULL(HorarioLunes, ISNULL(HorarioSalida, CAST('12:00:00' AS time))),
        HorarioMartes = ISNULL(HorarioMartes, ISNULL(HorarioSalida, CAST('12:00:00' AS time))),
        HorarioMiercoles = ISNULL(HorarioMiercoles, ISNULL(HorarioSalida, CAST('12:00:00' AS time))),
        HorarioJueves = ISNULL(HorarioJueves, ISNULL(HorarioSalida, CAST('12:00:00' AS time))),
        HorarioViernes = ISNULL(HorarioViernes, ISNULL(HorarioSalida, CAST('12:00:00' AS time)))
    WHERE HorarioLunes IS NULL
       OR HorarioMartes IS NULL
       OR HorarioMiercoles IS NULL
       OR HorarioJueves IS NULL
       OR HorarioViernes IS NULL;

    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cursos') AND name = 'HorarioLunes' AND is_nullable = 1)
        ALTER TABLE Cursos ALTER COLUMN HorarioLunes time NOT NULL;
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cursos') AND name = 'HorarioMartes' AND is_nullable = 1)
        ALTER TABLE Cursos ALTER COLUMN HorarioMartes time NOT NULL;
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cursos') AND name = 'HorarioMiercoles' AND is_nullable = 1)
        ALTER TABLE Cursos ALTER COLUMN HorarioMiercoles time NOT NULL;
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cursos') AND name = 'HorarioJueves' AND is_nullable = 1)
        ALTER TABLE Cursos ALTER COLUMN HorarioJueves time NOT NULL;
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cursos') AND name = 'HorarioViernes' AND is_nullable = 1)
        ALTER TABLE Cursos ALTER COLUMN HorarioViernes time NOT NULL;
END;");

            await context.Database.ExecuteSqlRawAsync(@"
IF OBJECT_ID('TiposInasistencia', 'U') IS NULL
BEGIN
    CREATE TABLE TiposInasistencia (
        IdTipoInasistencia int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre nvarchar(100) NOT NULL,
        ValorDecimal decimal(4,2) NOT NULL,
        CodigoOficial nvarchar(10) NOT NULL,
        EsActivo bit NOT NULL CONSTRAINT DF_TiposInasistencia_EsActivo DEFAULT (1)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_TiposInasistencia_CodigoOficial' AND object_id = OBJECT_ID('TiposInasistencia'))
    CREATE UNIQUE INDEX IX_TiposInasistencia_CodigoOficial ON TiposInasistencia(CodigoOficial);

IF NOT EXISTS (SELECT 1 FROM TiposInasistencia WHERE CodigoOficial = 'A')
BEGIN
    SET IDENTITY_INSERT TiposInasistencia ON;
    INSERT INTO TiposInasistencia (IdTipoInasistencia, Nombre, ValorDecimal, CodigoOficial, EsActivo)
    VALUES (1, N'Inasistencia Completa', 1.00, N'A', 1);
    SET IDENTITY_INSERT TiposInasistencia OFF;
END;

IF NOT EXISTS (SELECT 1 FROM TiposInasistencia WHERE CodigoOficial = 'M')
BEGIN
    SET IDENTITY_INSERT TiposInasistencia ON;
    INSERT INTO TiposInasistencia (IdTipoInasistencia, Nombre, ValorDecimal, CodigoOficial, EsActivo)
    VALUES (2, N'Media Falta', 0.50, N'M', 1);
    SET IDENTITY_INSERT TiposInasistencia OFF;
END;

IF NOT EXISTS (SELECT 1 FROM TiposInasistencia WHERE CodigoOficial = 'C')
BEGIN
    SET IDENTITY_INSERT TiposInasistencia ON;
    INSERT INTO TiposInasistencia (IdTipoInasistencia, Nombre, ValorDecimal, CodigoOficial, EsActivo)
    VALUES (3, N'Cuarto de Falta', 0.25, N'C', 1);
    SET IDENTITY_INSERT TiposInasistencia OFF;
END;

IF NOT EXISTS (SELECT 1 FROM TiposInasistencia WHERE CodigoOficial = 'TC')
BEGIN
    SET IDENTITY_INSERT TiposInasistencia ON;
    INSERT INTO TiposInasistencia (IdTipoInasistencia, Nombre, ValorDecimal, CodigoOficial, EsActivo)
    VALUES (4, N'Tres Cuartos de Falta', 0.75, N'TC', 1);
    SET IDENTITY_INSERT TiposInasistencia OFF;
END;

IF OBJECT_ID('PlanillasAsistenciasDiarias', 'U') IS NULL
BEGIN
    CREATE TABLE PlanillasAsistenciasDiarias (
        IdPlanilla int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdCurso int NOT NULL,
        Fecha datetime2 NOT NULL,
        Confirmada bit NOT NULL,
        FechaHoraRegistro datetime2 NOT NULL,
        IdPersonalRegistrador int NULL,
        CONSTRAINT FK_PlanillasAsistenciasDiarias_Cursos_IdCurso FOREIGN KEY (IdCurso) REFERENCES Cursos(IdCurso),
        CONSTRAINT FK_PlanillasAsistenciasDiarias_Personal_IdPersonalRegistrador FOREIGN KEY (IdPersonalRegistrador) REFERENCES Personal(IdPersonal) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PlanillasAsistenciasDiarias_IdCurso_Fecha' AND object_id = OBJECT_ID('PlanillasAsistenciasDiarias'))
    CREATE UNIQUE INDEX IX_PlanillasAsistenciasDiarias_IdCurso_Fecha ON PlanillasAsistenciasDiarias(IdCurso, Fecha);

IF OBJECT_ID('InasistenciasAlumnos', 'U') IS NULL
BEGIN
    CREATE TABLE InasistenciasAlumnos (
        IdInasistencia int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdAlumno int NOT NULL,
        Fecha datetime2 NOT NULL,
        IdTipoInasistencia int NOT NULL,
        Justificada bit NOT NULL,
        Motivo nvarchar(250) NOT NULL,
        NoComputa bit NOT NULL,
        Observaciones nvarchar(500) NOT NULL,
        IdRetiroOrigen int NULL,
        CONSTRAINT FK_InasistenciasAlumnos_Alumnos_IdAlumno FOREIGN KEY (IdAlumno) REFERENCES Alumnos(IdAlumno),
        CONSTRAINT FK_InasistenciasAlumnos_TiposInasistencia_IdTipoInasistencia FOREIGN KEY (IdTipoInasistencia) REFERENCES TiposInasistencia(IdTipoInasistencia),
        CONSTRAINT FK_InasistenciasAlumnos_Retiros_IdRetiroOrigen FOREIGN KEY (IdRetiroOrigen) REFERENCES Retiros(IdRetiro) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_InasistenciasAlumnos_IdAlumno_Fecha' AND object_id = OBJECT_ID('InasistenciasAlumnos'))
    CREATE UNIQUE INDEX IX_InasistenciasAlumnos_IdAlumno_Fecha ON InasistenciasAlumnos(IdAlumno, Fecha);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_InasistenciasAlumnos_Fecha' AND object_id = OBJECT_ID('InasistenciasAlumnos'))
    CREATE INDEX IX_InasistenciasAlumnos_Fecha ON InasistenciasAlumnos(Fecha);");
        }
    }
}
