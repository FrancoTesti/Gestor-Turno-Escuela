using GTE.Dominio;
using GTE.Data;
using GTE.DTOs;
using GTE.Application.Services;
class Program
{
    public static void Linea()
    {
        Console.WriteLine("______________________________________________________");
    }

    static string PedirYConfirmar(string prompt, string etiqueta)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string valor = Console.ReadLine() ?? "";
            Linea();
            Console.WriteLine($"Su {etiqueta} es {valor}");
            Linea();
            Console.WriteLine("¿Desea confirmar?");
            Console.WriteLine("1- si");
            Console.WriteLine("2- no");
            Linea();
            var opc = Console.ReadLine();

            if (opc == "1") return valor;
            if (opc != "2") Console.WriteLine("Opcion incorrecta");
        }
    }

    static async Task<string> PedirYConfirmarAsync(string prompt, string etiqueta, Func<string, Task<string?>>? validar = null)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string valor = Console.ReadLine() ?? "";
            Linea();
            Console.WriteLine($"Su {etiqueta} es {valor}");

            if (validar != null)
            {
                string? error = await validar(valor);
                if (error != null)
                {
                    Console.WriteLine(error);
                    continue;
                }
            }

            Linea();
            Console.WriteLine("¿Desea confirmar?");
            Console.WriteLine("1- si");
            Console.WriteLine("2- no");
            Linea();
            var opc = Console.ReadLine();

            if (opc == "1") return valor;
            if (opc != "2") Console.WriteLine("Opcion incorrecta");
        }
    }

    static int PedirYConfirmarEntero(string prompt, string etiqueta)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            if (!int.TryParse(Console.ReadLine(), out int valor))
            {
                Console.WriteLine("Debe ingresar un número válido.");
                continue;
            }
            Linea();
            Console.WriteLine($"Su {etiqueta} es {valor}");
            Linea();
            Console.WriteLine("¿Desea confirmar?");
            Console.WriteLine("1- si");
            Console.WriteLine("2- no");
            Linea();
            var opc = Console.ReadLine();

            if (opc == "1") return valor;
            if (opc != "2") Console.WriteLine("Opcion incorrecta");
        }
    }

    static async Task RegistrarPorteroAsync(IUsuarioRepository usuarioDAO, IPorteroRepository porteroDAO)
    {
        while (true)
        {
            string nombrePortero = PedirYConfirmar("Ingrese nombre del portero", "nombre");
            string puertaPortero = PedirYConfirmar("Ingrese la puerta asignada", "puerta asignada");
            string nombreUsuario = await PedirYConfirmarAsync("Ingrese su nombre de usuario", "nombre de usuario", async nombre =>
                await usuarioDAO.NombreUsuarioExisteAsync(nombre) ? "Ese nombre de usuario ya está en uso. Ingrese otro." : null);
            string contraseñaUsuario = PedirYConfirmar("Ingrese su contraseña", "contraseña");

            try
            {
                Usuario usuario = new Usuario(nombreUsuario, contraseñaUsuario);
                Portero portero = new Portero(nombrePortero, puertaPortero, usuario);

                /*Save recién si las dos construcciones sobrevivieron*/
                await usuarioDAO.AddAsync(usuario); /*genera id usuario*/
                await porteroDAO.AddAsync(portero); /*genera id portero*/
                return;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"No se pudo registrar: {ex.Message}");
            }
        }
    }

    static async Task RegistrarTutorAsync(IUsuarioRepository usuarioDAO, ITutorRepository tutorDAO)
    {
        while (true)
        {
            string nombreTutor = PedirYConfirmar("Ingrese nombre de tutor", "nombre");
            string apellidoTutor = PedirYConfirmar("Ingrese su apellido", "apellido");
            string dniTutor = await PedirYConfirmarAsync("Ingrese su DNI", "DNI", async dni =>
                await tutorDAO.DniExisteAsync(dni) ? "Ya existe un tutor registrado con ese DNI. Ingrese otro." : null);
            string parentescoTutor = PedirYConfirmar("Ingrese su parentesco", "parentesco");
            string telefonoTutor = PedirYConfirmar("Ingrese su telefono", "telefono");
            string nombreUsuario = await PedirYConfirmarAsync("Ingrese su nombre de usuario", "nombre de usuario", async nombre =>
                await usuarioDAO.NombreUsuarioExisteAsync(nombre) ? "Ese nombre de usuario ya está en uso. Ingrese otro." : null);
            string contraseñaUsuario = PedirYConfirmar("Ingrese su contraseña", "contraseña");

            try
            {
                Usuario usuario = new Usuario(nombreUsuario, contraseñaUsuario);
                Tutor tutor = new Tutor(nombreTutor, apellidoTutor, dniTutor, parentescoTutor, telefonoTutor, usuario);

                /*Save recién si las dos construcciones sobrevivieron*/
                await usuarioDAO.AddAsync(usuario);
                await tutorDAO.AddAsync(tutor); /*genera id tutor*/
                return;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"No se pudo registrar: {ex.Message}");
            }
        }
    }

    static async Task RegistrarSecretarioAsync(IUsuarioRepository usuarioDAO, ISecretarioRepository secretarioDAO)
    {
        while (true)
        {
            string nombreSecretario = PedirYConfirmar("Ingrese nombre del secretario", "nombre");
            int nivelAcceso = PedirYConfirmarEntero("Ingrese nivel de acceso al sistema", "nivel de acceso");
            string nombreUsuario = await PedirYConfirmarAsync("Ingrese su nombre de usuario", "nombre de usuario", async nombre =>
                await usuarioDAO.NombreUsuarioExisteAsync(nombre) ? "Ese nombre de usuario ya está en uso. Ingrese otro." : null);
            string contraseñaUsuario = PedirYConfirmar("Ingrese su contraseña", "contraseña");

            try
            {
                Usuario usuario = new Usuario(nombreUsuario, contraseñaUsuario);
                Secretario secretario = new Secretario(nombreSecretario, nivelAcceso, usuario);

                /*Save recién si las dos construcciones sobrevivieron*/
                await usuarioDAO.AddAsync(usuario);
                await secretarioDAO.AddAsync(secretario); /*genera id secretario*/
                return;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"No se pudo registrar: {ex.Message}");
            }
        }
    }
    static async Task LoginAsync(IUsuarioRepository usuarioDAO, ITutorRepository tutorDAO, ISecretarioRepository secretarioDAO, IPorteroRepository porteroDAO)
    {
        IAuthService authService =
            new AuthService(usuarioDAO, tutorDAO, porteroDAO, secretarioDAO);
        Linea();
        Console.WriteLine("Ingrese su nombre de usuario");
        string nombreUsuario = Console.ReadLine() ?? "";
        Console.WriteLine("Ingrese su contraseña");
        string contrasena = Console.ReadLine() ?? "";

        LoginResponse resultado = await authService.LoginAsync(nombreUsuario, contrasena);
        Linea();
        if (!resultado.Exito)
        {
            Console.WriteLine($"[X] {resultado.Mensaje}");
            Linea();
            return;
        }
        Console.WriteLine($"[OK] Bienvenido/a {resultado.NombreCompleto} ({resultado.Rol})");
        Linea();

        string[] opciones = resultado.Rol switch
        {
            "Secretario" => new[] { "Gestionar autorizaciones", "Gestionar alumnos", "Ver reportes" },
            "Tutor" => new[] { "Ver mis alumnos autorizados", "Ver historial de retiros" },
            "Portero" => new[] { "Registrar retiro de alumno", "Ver retiros del día" },
            _ => Array.Empty<string>()
        };

        bool sesionActiva = true;
        while (sesionActiva)
        {
            Linea();
            Console.WriteLine($"Sesión: {resultado.NombreUsuario} ({resultado.Rol})");
            for (int i = 0; i < opciones.Length; i++)
                Console.WriteLine($"{i + 1}- {opciones[i]}");
            Console.WriteLine($"{opciones.Length + 1}- Cerrar sesión");
            Linea();

            var opcion = Console.ReadLine();
            if (int.TryParse(opcion, out int n) && n >= 1 && n <= opciones.Length)
                Console.WriteLine($"[{opciones[n - 1]}] - En construcción.");
            else if (int.TryParse(opcion, out int m) && m == opciones.Length + 1)
                sesionActiva = false;
            else
                Console.WriteLine("Opción incorrecta.");
        }
    }

    static async Task Main(string[] args)
    {
        IUsuarioRepository UsuarioDAO = new UsuarioRepository();
        ITutorRepository TutorDAO = new TutorRepository();
        ISecretarioRepository SecretarioDAO = new SecretarioRepository();
        IPorteroRepository PorteroDAO = new PorteroRepository();

        bool menu1 = true;

        while (menu1)
        {
            Linea();
            Console.WriteLine("Ingrese una opcion");
            Console.WriteLine("1- Loguearse");
            Console.WriteLine("2- Registrarse");
            Console.WriteLine("3- Salir");
            Linea();

            var opc1 = Console.ReadLine();

            switch (opc1)
            {
                case "1":
                    await LoginAsync(UsuarioDAO, TutorDAO, SecretarioDAO, PorteroDAO);
                    break;

                case "2":

                    bool menu1_2 = true;

                    while (menu1_2)
                    {
                        Linea();
                        Console.WriteLine("Ingrese una opcion");
                        Console.WriteLine("1- Registrarse como Portero");
                        Console.WriteLine("2- Registrarse como Tutor");
                        Console.WriteLine("3- Registrarse como Secretario");
                        Console.WriteLine("4- Salir");
                        Linea();

                        var opc1_2 = Console.ReadLine();

                        switch (opc1_2)
                        {
                            case "1":
                                await RegistrarPorteroAsync(UsuarioDAO, PorteroDAO);
                                menu1_2 = false;
                                break;

                            case "2":
                                await RegistrarTutorAsync(UsuarioDAO, TutorDAO);
                                menu1_2 = false;
                                break;

                            case "3":
                                await RegistrarSecretarioAsync(UsuarioDAO, SecretarioDAO);
                                menu1_2 = false;
                                break;

                            case "4":
                                menu1_2 = false;
                                break;

                            default:
                                Console.WriteLine("opcion incorrecta");
                                break;
                        }
                    }

                    break;

                case "3":
                    Console.WriteLine("Saliendo...");
                    menu1 = false;
                    break;

                default:
                    Console.WriteLine("opcion incorrecta");
                    break;
            }
        }

        IEnumerable<Usuario> usuarios = await UsuarioDAO.GetAllAsync();
        foreach (var usuario in usuarios)
        {
            Console.WriteLine($"Id: {usuario.IdUsuario} - Nombre usuario: {usuario.NombreUsuario} - Estado Usuario: {usuario.EstaActivo}");
        }

        IEnumerable<Tutor> tutores = await TutorDAO.GetAllAsync();
        foreach (var tutor in tutores)
        {
            Console.WriteLine($"IdTutor: {tutor.Usuario.IdUsuario} - Nombre usuario tutor: {tutor.Usuario.NombreUsuario} - Estado usuario Tutor: {tutor.Usuario.EstaActivo}");
        }

        IEnumerable<Secretario> secretarios = await SecretarioDAO.GetAllAsync();
        foreach (var secretario in secretarios)
        {
            Console.WriteLine($"Id: {secretario.Usuario.IdUsuario} - Nombre usuario secretario: {secretario.Usuario.NombreUsuario} - Estado usuario secretario: {secretario.Usuario.EstaActivo}");
        }

        IEnumerable<Portero> porteros = await PorteroDAO.GetAllAsync();
        foreach (var portero in porteros)
        {
            Console.WriteLine($"Id: {portero.Usuario.IdUsuario} - Nombre usuario portero: {portero.Usuario.NombreUsuario} - Activo: {portero.Usuario.EstaActivo}");
        }
    }
}
