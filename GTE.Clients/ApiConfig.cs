using System;

namespace GTE.Clients
{
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
