using GTE.Dominio;
using GTE.Data;

class Program
{
    public static void Linea()
    {
        Console.WriteLine("______________________________________________________");
    }

    static async Task Main(string[] args)
    {
        /*
        IUsuarioRepository repo = new UsuarioRepository();

        Usuario usuario1 = new Usuario("Alejandro", "1234");
        Usuario usuario2 = new Usuario("Test", "5678");

        await repo.AddAsync(usuario1);
        await repo.AddAsync(usuario2);

        Console.WriteLine($"Id: {usuario1.IdUsuario}");
        Console.WriteLine($"Usuario: {usuario1.NombreUsuario}");
        Console.WriteLine($"Contraseña: {usuario1.Contrasena}");
        Console.WriteLine($"Activo: {usuario1.EstaActivo}");

        Console.WriteLine($"Id: {usuario2.IdUsuario}");
        Console.WriteLine($"Usuario: {usuario2.NombreUsuario}");
        Console.WriteLine($"Contraseña: {usuario2.Contrasena}");
        Console.WriteLine($"Activo: {usuario2.EstaActivo}");
        */

        IUsuarioRepository UsuarioDAO = new UsuarioRepository();
        ITutorRepository TutorDAO = new TutorRepository();
        ISecretarioRepository SecretarioDAO = new SecretarioRepository();
        IPorteroRepository PorteroDAO = new PorteroRepository();

        //var elementos = await repositorio.GetAllAsync();  // formula para alejandro magno

        /*
        int idTutor = 0;
        int idSecretario = 0;
        int idPortero = 0;
        */

        List<Tutor> arregloTutor = new List<Tutor>();
        List<Secretario> arregloSecretario = new List<Secretario>();
        List<Portero> arregloPortero = new List<Portero>();

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
                    Console.WriteLine("en construccion");
                    break;

                case "2":

                    bool menu1_2 = true;

                    while (menu1_2)
                    {
                        Linea(); ;
                        Console.WriteLine("Ingrese una opcion");
                        Console.WriteLine("1- Registrarse como Portero");
                        Console.WriteLine("2- Registrarse como Tutor");
                        Console.WriteLine("3- Registrarse como Secretario");
                        Console.WriteLine("4- Salir");
                        Linea(); ;

                        var opc1_2 = Console.ReadLine();

                        switch (opc1_2)
                        {
                            case "1":

                                bool menu1_2_1 = true;

                                while (menu1_2_1)
                                {
                                    bool menu1_2_1_1 = true;
                                    string nombrePortero = "";

                                    while (menu1_2_1_1)
                                    {
                                        Console.WriteLine("Ingrese nombre del portero");

                                        nombrePortero = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre es " + nombrePortero);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_1_1 = Console.ReadLine();

                                        switch (opc1_2_1_1)
                                        {
                                            case "1":
                                                menu1_2_1_1 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_1_2 = true;
                                    string puertaPortero = "";

                                    while (menu1_2_1_2)
                                    {
                                        Console.WriteLine("Ingrese la puerta asignada");

                                        puertaPortero = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su puerta asignada es " + puertaPortero);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar la puerta?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_1_2 = Console.ReadLine();

                                        switch (opc1_2_1_2)
                                        {
                                            case "1":
                                                menu1_2_1_2 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_1_3 = true;
                                    string nombreUsuario = "";

                                    while (menu1_2_1_3)
                                    {
                                        Console.WriteLine("Ingrese su nombre de usuario");

                                        nombreUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre de usuario es " + nombreUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre de usuario?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_1_3 = Console.ReadLine();

                                        switch (opc1_2_1_3)
                                        {
                                            case "1":
                                                menu1_2_1_3 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_1_4 = true;
                                    string contraseñaUsuario = "";

                                    while (menu1_2_1_4)
                                    {
                                        Console.WriteLine("Ingrese su contraseña");
                                        contraseñaUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su contraseña es " + contraseñaUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su contraseña?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_1_4 = Console.ReadLine();

                                        switch (opc1_2_1_4)
                                        {
                                            case "1":
                                                menu1_2_1_4 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    /*Create*/
                                    Usuario usuario = new Usuario
                                    (
                                        nombreUsuario,
                                        contraseñaUsuario
                                    );
                                    /*Save a DAO de Usuario*/
                                    await UsuarioDAO.AddAsync(usuario); /*genera id usuario*/

                                    /*Add a Portero*/
                                    Portero portero = new Portero(
                                        nombrePortero,
                                        puertaPortero,
                                        usuario
                                    );

                                    /*Save a DAO de portero*/
                                    await PorteroDAO.AddAsync(portero); /*genera id portero*/

                                    menu1_2_1 = false;
                                }

                                break;

                            case "2":

                                bool menu1_2_2 = true;

                                while (menu1_2_2)
                                {
                                    bool menu1_2_2_1 = true;
                                    string nombreTutor = "";
                                    while (menu1_2_2_1)
                                    {
                                        Console.WriteLine("Ingrese nombre de tutor");

                                        nombreTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre es " + nombreTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_1 = Console.ReadLine();

                                      switch (opc1_2_2_1)
                                        {
                                            case "1":
                                                menu1_2_2_1 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_2 = true;
                                    string apellidoTutor = "";

                                    while (menu1_2_2_2) 
                                    {
                                        Console.WriteLine("Ingrese su apellido");

                                        apellidoTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su apellido es " + apellidoTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su apellido?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_2 = Console.ReadLine();

                                        switch (opc1_2_2_2)
                                        {
                                            case "1":
                                                menu1_2_2_2 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_3 = true;
                                    string dniTutor = "";

                                    while (menu1_2_2_3)
                                    {
                                        Console.WriteLine("Ingrese su DNI");

                                        dniTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su DNI es " + dniTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su DNI?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_3 = Console.ReadLine();

                                        switch (opc1_2_2_3)
                                        {
                                            case "1":
                                                menu1_2_2_3 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }
                                    // modificado en 25 de abril de 2026
                                    bool menu1_2_2_4 = true;
                                    string parentescoTutor = "";

                                    while (menu1_2_2_4)
                                    {
                                        Console.WriteLine("Ingrese su parentesco");

                                        parentescoTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su parentesco es " + parentescoTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su parentesco?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_4 = Console.ReadLine();

                                        switch (opc1_2_2_4)
                                        {
                                            case "1":
                                                menu1_2_2_4 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_5 = true;
                                    string telefonoTutor = "";

                                    while (menu1_2_2_5)
                                    {
                                        Console.WriteLine("Ingrese su telefono");

                                        telefonoTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su telefono es " + telefonoTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su telefono?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_5 = Console.ReadLine();

                                        switch (opc1_2_2_5)
                                        {
                                            case "1":
                                                menu1_2_2_5 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_6 = true;
                                    string credencialTutor = "";

                                    while (menu1_2_2_6)
                                    {
                                        Console.WriteLine("Ingrese su credencial");

                                        credencialTutor = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su credencial es " + credencialTutor);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su credencial?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_6 = Console.ReadLine();

                                        switch (opc1_2_2_6)
                                        {
                                            case "1":
                                                menu1_2_2_6 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_7 = true;
                                    string nombreUsuario = "";

                                    while (menu1_2_2_7)
                                    {
                                        Console.WriteLine("Ingrese su nombre de usuario");

                                        nombreUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre de usuario es " + nombreUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre de usuario?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_7 = Console.ReadLine();

                                        switch (opc1_2_2_7)
                                        {
                                            case "1":
                                                menu1_2_2_7 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_2_8 = true;
                                    string contraseñaUsuario = "";

                                    while (menu1_2_2_8)
                                    {
                                        Console.WriteLine("Ingrese su contraseña");

                                        contraseñaUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su contraseña es " + contraseñaUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su contraseña?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_2_8 = Console.ReadLine();

                                        switch (opc1_2_2_8)
                                        {
                                            case "1":
                                                menu1_2_2_8 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    /*Create*/
                                    Usuario usuario = new Usuario
                                    (
                                        nombreUsuario,
                                        contraseñaUsuario
                                    );
                                    /*Save a DAO*/
                                    await UsuarioDAO.AddAsync(usuario);

                                    /*Añadir a tutor*/
                                    Tutor tutor = new Tutor
                                    (
                                        nombreTutor,
                                        apellidoTutor,
                                        dniTutor,
                                        parentescoTutor,
                                        telefonoTutor,
                                        usuario
                                    );

                                    /*Save al dao Tutor*/
                                    await TutorDAO.AddAsync(tutor); /*genera id portero*/

                                    menu1_2_2 = false;
                                }

                                break;

                            case "3":

                                bool menu1_2_3 = true;

                                while (menu1_2_3)
                                {
                                    bool menu1_2_3_1 = true;
                                    string nombreSecretario = "";

                                    while (menu1_2_3_1)
                                    {
                                        Console.WriteLine("Ingrese nombre del secretario");

                                        nombreSecretario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre es " + nombreSecretario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_3_1 = Console.ReadLine();

                                        switch (opc1_2_3_1)
                                        {
                                            case "1":
                                                menu1_2_3_1 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_3_2 = true;
                                    int nivelAcceso = 0;

                                    while (menu1_2_3_2)
                                    {
                                        Console.WriteLine("Ingrese nivel de acceso al sistema");

                                        nivelAcceso = Convert.ToInt32(Console.ReadLine());
                                        Linea();
                                        Console.WriteLine("Su nivel de acceso es " + nivelAcceso);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar el nivel de acceso?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_3_2 = Console.ReadLine();

                                        switch (opc1_2_3_2)
                                        {
                                            case "1":
                                                menu1_2_3_2 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_3_3 = true;
                                    string nombreUsuario = "";

                                    while (menu1_2_3_3)
                                    {
                                        Console.WriteLine("Ingrese su nombre de usuario");

                                        nombreUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su nombre de usuario es " + nombreUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su nombre de usuario?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_3_3 = Console.ReadLine();

                                        switch (opc1_2_3_3)
                                        {
                                            case "1":
                                                menu1_2_3_3 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    bool menu1_2_3_4 = true;
                                    string contraseñaUsuario = "";

                                    while (menu1_2_3_4)
                                    {
                                        Console.WriteLine("Ingrese su contraseña");

                                        contraseñaUsuario = Console.ReadLine();
                                        Linea();
                                        Console.WriteLine("Su contraseña es " + contraseñaUsuario);
                                        Linea();
                                        Console.WriteLine("¿Desea confirmar su contraseña?");
                                        Console.WriteLine("1- si");
                                        Console.WriteLine("2- no");
                                        Linea();
                                        var opc1_2_3_4 = Console.ReadLine();

                                        switch (opc1_2_3_4)
                                        {
                                            case "1":
                                                menu1_2_3_4 = false;
                                                break;

                                            case "2":
                                                break;

                                            default:
                                                Console.WriteLine("Opcion incorrecta");
                                                break;
                                        }
                                    }

                                    /*Create*/
                                    Usuario usuario = new Usuario
                                    (
                                        nombreUsuario,
                                        contraseñaUsuario
                                    );
                                    /*Save a DAO*/
                                    await UsuarioDAO.AddAsync(usuario);

                                    Secretario secretario = new Secretario(
                                        nombreSecretario,
                                        nivelAcceso,
                                        usuario
                                    );

                                    /*Save al dao Secretario*/
                                    await SecretarioDAO.AddAsync(secretario); /*genera id portero*/

                                    menu1_2_3 = false;
                                }

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


