# Gestor de Turnos Escolar - Entrega Final

Bienvenido al repositorio del Trabajo Práctico Integrador (Entrega Final) del Gestor de Turnos Escolar.

## Arquitectura y Proyectos
El sistema está dividido en varias capas utilizando Clean Architecture y .NET 8:
- **Libreria de clases (Dominio)**: Entidades del negocio.
- **GTE.Data**: Acceso a datos con Entity Framework Core.
- **Servicio (GTE.Application.Services)**: Lógica de negocio y servicios de aplicación.
- **GTE.DTOs**: Objetos de transferencia de datos.
- **GTE.Clients**: Clientes HTTP para consumir la API.
- **GTE.WebAPI**: API RESTful central y endpoints.
- **GTE.WindowsForms**: Interfaz de escritorio.
- **GTE.Blazor.Server**: Interfaz web.
- **GTE.Tests**: Pruebas unitarias.

## Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB o instancia de SQL Express). La cadena de conexión por defecto apunta a `(localdb)\MSSQLLocalDB`.

## Base de Datos
**¡Importante!** No es necesario ejecutar migraciones manualmente. La base de datos se crea, migra y puebla con datos semilla de forma automática al iniciar la API (`GTE.WebAPI`).

## Cómo Compilar y Ejecutar

1. **Clonar y Compilar:**
   ```bash
   git clone https://github.com/FrancoTesti/Gestor-Turno-Escuela.git
   cd Gestor-Turno-Escuela
   dotnet build TP_IDE.sln
   ```

2. **Levantar la API (Paso Obligatorio):**
   La API debe estar corriendo para que las aplicaciones cliente funcionen.
   ```bash
   cd GTE.WebAPI
   dotnet run
   ```
   - Swagger estará disponible en: `http://localhost:5117/swagger`

3. **Levantar la Aplicación Web (Blazor):**
   En otra terminal:
   ```bash
   cd GTE.Blazor.Server
   dotnet run
   ```
   - Acceder en el navegador a: `http://localhost:5129` (o el puerto que indique la consola).

4. **Levantar la Aplicación de Escritorio (Windows Forms):**
   En otra terminal o desde Visual Studio, ejecuta el proyecto `GTE.WindowsForms`.
   ```bash
   cd GTE.WindowsForms
   dotnet run
   ```

## Credenciales de Prueba

El sistema cuenta con 3 roles. Puedes probarlos utilizando estas credenciales (la contraseña es igual al nombre de usuario seguido de '123'):

| Rol | Usuario | Contraseña | Permisos Principales |
|---|---|---|---|
| **Secretario** | `admin` | `admin123` | Control total (Ver/Editar Alumnos, Cursos, Retiros, Horarios y los dos reportes). |
| **Portero** | `porteria1` | `porteria123` | Solo lectura de alumnos, gestión de retiros y salidas, y el reporte de retiros. No puede editar alumnos ni ver el reporte de alumnos. |
| **Tutor** | `tutor1` | `tutor123` | Solo lectura de cursos. No puede ver el resto del sistema. |

---
*Desarrollado por Franco Testi, Renzo Scollo y Alejandro Ciesco.*
