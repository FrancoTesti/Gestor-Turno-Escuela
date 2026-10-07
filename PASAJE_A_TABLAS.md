# Pasaje a tablas

Cómo pasa cada clase de `Libreria de clases` a las tablas de SQL Server.
Lo arma Entity Framework a partir del modelo que está en `GTE.Data/GTEContext.cs`,
que es donde quedan las reglas de cada tabla (claves, largos, relaciones).

Los tipos y los largos que figuran acá son los que tiene hoy la base
`GestorTurnoEscuelaDB`, leídos de la propia base.

## Resumen

| Clase del dominio | Tabla | Qué guarda |
| --- | --- | --- |
| `Usuario` | `Usuarios` | Las cuentas con las que se entra al sistema. |
| `CursoEscolar` | `Cursos` | Grado, división, turno y horario de salida de cada curso. |
| `Alumno` | `Alumnos` | Los alumnos, su curso y su estado del día. |
| `Tutor` | `Tutores` | Los adultos autorizados a retirar, con su usuario. |
| `Personal` | `Personal` | El personal de la escuela. |
| `Secretario` | `Personal` | Misma tabla que `Personal`, con el discriminador `Secretario`. |
| `Portero` | `Personal` | Misma tabla que `Personal`, con el discriminador `Portero`. |
| `Autorizacion` | `Autorizaciones` | Qué tutor puede retirar a qué alumno. |
| `Retiro` | `Retiros` | La cabecera de cada retiro. |
| `DetalleRetiro` | `DetalleRetiros` | Los alumnos que se llevaron en cada retiro. |
| `HorarioEspecial` | `HorariosEspeciales` | Salidas a otra hora por una actividad puntual. |
| `SalidaDeCurso` | `SalidasDeCurso` | Cuándo empezó y terminó de salir cada curso. |

## Usuarios

Clave primaria: `IdUsuario`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdUsuario` | `int` identidad | No | Clave primaria. |
| `NombreUsuario` | `nvarchar(100)` | No | Único: no se puede repetir. |
| `Contrasena` | `nvarchar(100)` | No | |
| `EstaActivo` | `bit` | No | Si está en falso, no puede iniciar sesión. |

## Cursos

Clase `CursoEscolar`. Clave primaria: `IdCurso`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdCurso` | `int` identidad | No | Clave primaria. |
| `Grado` | `nvarchar(50)` | No | Por ejemplo `4°`. |
| `Curso` | `nvarchar(50)` | No | La división: `A`, `B`, ... |
| `HorarioSalida` | `time` | No | Hora de salida del curso. |
| `Turno` | `nvarchar(20)` | No | Mañana, Tarde o Noche. |

## Alumnos

Clave primaria: `IdAlumno`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdAlumno` | `int` identidad | No | Clave primaria. |
| `Nombre` | `nvarchar(100)` | No | |
| `Apellido` | `nvarchar(100)` | No | |
| `Estado` | `nvarchar(50)` | No | Presente, Retirado o Ausente. |
| `IdCurso` | `int` | No | Clave foránea a `Cursos.IdCurso`, con índice. |

## Tutores

Clave primaria: `IdTutor`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdTutor` | `int` identidad | No | Clave primaria. |
| `Nombre` | `nvarchar(100)` | No | |
| `Apellido` | `nvarchar(100)` | No | |
| `Dni` | `nvarchar(20)` | No | |
| `Parentesco` | `nvarchar(100)` | No | Padre, madre, tutor, etc. |
| `Telefono` | `nvarchar(100)` | No | |
| `TieneRestriccion` | `bit` | No | Si está en verdadero, el sistema no lo deja retirar. |
| `IdUsuario` | `int` | No | Clave foránea a `Usuarios.IdUsuario`, única: cada tutor tiene un usuario. |

## Personal (Secretario y Portero)

`Personal` es abstracta y `Secretario` y `Portero` heredan de ella. Entity
Framework guarda las tres en **una sola tabla** y agrega la columna
`TipoPersonal`, que dice a qué clase corresponde cada fila. Por eso las columnas
propias de cada una quedan anulables: un portero no tiene nivel de acceso y un
secretario no tiene puerta asignada.

Clave primaria: `IdPersonal`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdPersonal` | `int` identidad | No | Clave primaria. |
| `Nombre` | `nvarchar(100)` | No | |
| `IdUsuario` | `int` | No | Clave foránea a `Usuarios.IdUsuario`, única. |
| `TipoPersonal` | `nvarchar(13)` | No | Discriminador: `Secretario` o `Portero`. |
| `PuertaAsignada` | `nvarchar(100)` | Sí | Solo la usan los porteros. |
| `NivelAccesoSistema` | `int` | Sí | Solo lo usan los secretarios. |

## Autorizaciones

Clave primaria: `IdAutorizacion`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdAutorizacion` | `int` identidad | No | Clave primaria. |
| `AlumnoId` | `int` | No | Alumno autorizado. |
| `TutorId` | `int` | No | Tutor que puede retirarlo. |

## Retiros

Cabecera del retiro. Clave primaria: `IdRetiro`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdRetiro` | `int` identidad | No | Clave primaria. |
| `IdTutor` | `int` | No | Clave foránea a `Tutores.IdTutor`: quién retira. |
| `IdPersonal` | `int` | No | Clave foránea a `Personal.IdPersonal`: quién entrega al alumno. |
| `FechaHora` | `datetime2` | No | Cuándo se registró el retiro. |
| `Observaciones` | `nvarchar(500)` | No | |

## DetalleRetiros

Los alumnos de cada retiro. Es el detalle del maestro `Retiros`, uno a muchos.
Clave primaria: `IdDetalleRetiro`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdDetalleRetiro` | `int` identidad | No | Clave primaria. |
| `IdRetiro` | `int` | No | Clave foránea a `Retiros.IdRetiro`, con borrado en cascada. |
| `IdAlumno` | `int` | No | Clave foránea a `Alumnos.IdAlumno`. |
| `HoraSalida` | `time` | No | Hora en que salió ese alumno. |
| `Estado` | `nvarchar(50)` | No | |

## HorariosEspeciales

Clave primaria: `IdHorarioEspecial`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdHorarioEspecial` | `int` identidad | No | Clave primaria. |
| `IdAlumno` | `int` | No | Alumno que sale a otra hora. |
| `DescripcionActividad` | `nvarchar(200)` | No | Por qué sale antes o después. |
| `HoraSalidaEspecial` | `time` | No | La hora de salida de ese día. |

## SalidasDeCurso

Lo que marca el portero cuando un curso empieza a salir y lo que muestra la
pantalla de la puerta. Clave primaria: `IdSalidaDeCurso`.

| Columna | Tipo | ¿Nulo? | Observaciones |
| --- | --- | --- | --- |
| `IdSalidaDeCurso` | `int` identidad | No | Clave primaria. |
| `IdCurso` | `int` | No | Clave foránea a `Cursos.IdCurso`. |
| `IdPersonal` | `int` | No | Clave foránea a `Personal.IdPersonal`: quién la marcó. |
| `FechaHoraInicio` | `datetime2` | No | Cuándo empezó a salir. |
| `FechaHoraFin` | `datetime2` | Sí | Cuándo terminó. Mientras está vacío, el curso está saliendo. |

## Índices y claves

| Tabla | Índice | Tipo | Columnas |
| --- | --- | --- | --- |
| `Usuarios` | `IX_Usuarios_NombreUsuario` | Único | `NombreUsuario` |
| `Tutores` | `IX_Tutores_IdUsuario` | Único | `IdUsuario` |
| `Personal` | `IX_Personal_IdUsuario` | Único | `IdUsuario` |
| `Alumnos` | `IX_Alumnos_IdCurso` | Normal | `IdCurso` |
| `DetalleRetiros` | `IX_DetalleRetiros_IdRetiro` | Normal | `IdRetiro` |
| `DetalleRetiros` | `IX_DetalleRetiros_IdAlumno` | Normal | `IdAlumno` |
| `SalidasDeCurso` | `IX_SalidasDeCurso_IdCurso_FechaHoraFin` | Normal | `IdCurso`, `FechaHoraFin` |

## Cosas para revisar

Comparando la base que está en uso con lo que declara el modelo, quedaron estos
puntos flojos:

1. `Retiros` se creó antes de que el modelo declarara las relaciones con
   `Tutores` y `Personal`, así que esas dos claves foráneas no existen en la
   base. `EnsureCreated` no modifica tablas que ya existen, y el migrador no
   cubre ese caso.
2. Lo mismo pasa con el índice único de `Cursos` por `Grado`, `Curso` y `Turno`:
   está declarado en el modelo pero la base no lo tiene, así que hoy se podría
   cargar dos veces el mismo curso.
3. `Autorizaciones` guarda `AlumnoId` y `TutorId` como enteros sueltos: no tiene
   claves foráneas ni índice único por par. Si se borra un alumno o un tutor, la
   autorización queda apuntando a algo que ya no existe.
4. `HorariosEspeciales.IdAlumno` tampoco tiene clave foránea.

Las dos primeras son diferencias entre el modelo y la base actual; las otras dos
son relaciones que directamente no están declaradas en el modelo.
