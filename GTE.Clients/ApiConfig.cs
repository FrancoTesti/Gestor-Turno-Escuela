using System;

namespace GTE.Clients
{
    /// <summary>
    /// Dirección en la que corre la API.
    ///
    /// Por defecto es la máquina local, que es como se trabaja mientras se
    /// programa. Para usar el sistema desde otro equipo (el celular del tutor o
    /// la pantalla de la puerta) hay que apuntar a la máquina donde se publica:
    /// alcanza con definir la variable de entorno GTE_API_URL, o con cargarla
    /// desde la configuración de la aplicación.
    /// </summary>
    public static class ApiConfig
    {
        public const string VariableDeEntorno = "GTE_API_URL";

        public const string VariableDeEntornoDeLaWeb = "GTE_WEB_URL";

        private const string DireccionPorDefecto = "http://localhost:5117/";

        private const string WebPorDefecto = "http://localhost:5001";

        private static string direccion = LeerDelEntorno();

        public static string Direccion
        {
            get => direccion;
            set => direccion = Normalizar(value);
        }

        public static Uri Uri => new Uri(Direccion);

        /// <summary>
        /// Dirección de la web. El escritorio la usa para abrir la pantalla de la
        /// puerta en el navegador.
        /// </summary>
        public static string DireccionDeLaWeb { get; set; } =
            (Environment.GetEnvironmentVariable(VariableDeEntornoDeLaWeb) ?? WebPorDefecto).TrimEnd('/');

        private static string LeerDelEntorno() =>
            Normalizar(Environment.GetEnvironmentVariable(VariableDeEntorno) ?? DireccionPorDefecto);

        private static string Normalizar(string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                return DireccionPorDefecto;

            string recortada = direccion.Trim();
            return recortada.EndsWith("/", StringComparison.Ordinal) ? recortada : recortada + "/";
        }
    }
}
