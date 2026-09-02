using System;
using System.Threading;
using System.Windows.Forms;
using GTE.Clients;
using GTE.Auth.WindowsForms;

namespace GTE.WindowsForms
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Handler para excepciones de UI no manejadas
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Registrar AuthService en el singleton
            var authService = new WindowsFormsAuthService();
            AuthServiceProvider.Register(authService);

            // Bucle principal de autenticación
            while (true)
            {
                if (!authService.IsAuthenticatedAsync().GetAwaiter().GetResult())
                {
                    var loginForm = new LoginForm();
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        // Usuario canceló el login: cerrar la aplicación.
                        return;
                    }
                }

                try
                {
                    Application.Run(new MainForm());

                    // Si MainForm se cerró por logout, la sesión ya no está activa
                    // y el bucle vuelve a mostrar LoginForm.
                    if (!authService.IsAuthenticatedAsync().GetAwaiter().GetResult())
                        continue;

                    break; // El usuario cerró la aplicación sin cerrar sesión.
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, "Sesión expirada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception is UnauthorizedAccessException)
            {
                MessageBox.Show("Su sesión ha expirado. Debe volver a autenticarse.", "Sesión expirada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Application.Restart();
            }
            else
            {
                MessageBox.Show($"Error inesperado: {e.Exception.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
