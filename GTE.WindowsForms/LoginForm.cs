using System;
using System.Drawing;
using System.Windows.Forms;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(33, 37, 41); // Sleek Dark Gray
            this.lblTitle.ForeColor = Color.FromArgb(248, 249, 250); // White
            this.lblUsername.ForeColor = Color.FromArgb(206, 212, 218); // Light gray
            this.lblPassword.ForeColor = Color.FromArgb(206, 212, 218);
            this.btnLogin.BackColor = Color.FromArgb(13, 110, 253); // Blue Accent
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.txtUsername.BackColor = Color.FromArgb(49, 53, 56);
            this.txtUsername.ForeColor = Color.White;
            this.txtUsername.BorderStyle = BorderStyle.FixedSingle;
            this.txtPassword.BackColor = Color.FromArgb(49, 53, 56);
            this.txtPassword.ForeColor = Color.White;
            this.txtPassword.BorderStyle = BorderStyle.FixedSingle;
            this.txtPassword.UseSystemPasswordChar = true;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Campos Requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Conectando...";
            lblError.Text = string.Empty;

            try
            {
                var authService = AuthServiceProvider.Instance;
                bool success = await authService.LoginAsync(username, password);

                if (success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblError.Text = "Usuario o contraseña incorrectos.";
                    btnLogin.Enabled = true;
                    btnLogin.Text = "Iniciar Sesión";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error de conexión: {ex.Message}";
                btnLogin.Enabled = true;
                btnLogin.Text = "Iniciar Sesión";
            }
        }
    }
}
