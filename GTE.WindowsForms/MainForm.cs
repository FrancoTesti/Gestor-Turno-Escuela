using System;
using System.Drawing;
using System.Windows.Forms;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ApplyStyles();
            LoadUserData();
        }

        private void ApplyStyles()
        {
            pnlHeader.BackColor = Color.FromArgb(24, 28, 36);
            pnlSidebar.BackColor = Color.FromArgb(33, 37, 41);
            pnlContent.BackColor = Color.FromArgb(248, 249, 250);

            lblUserTitle.ForeColor = Color.White;
            lblUserSub.ForeColor = Color.FromArgb(173, 181, 189);

            btnLogOut.BackColor = Color.FromArgb(220, 53, 69);
            btnLogOut.ForeColor = Color.White;
            btnLogOut.FlatStyle = FlatStyle.Flat;
            btnLogOut.FlatAppearance.BorderSize = 0;

            StyleMenuButton(btnAlumnos);
            StyleMenuButton(btnCursos);
            StyleMenuButton(btnOtros);
        }

        private void StyleMenuButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(33, 37, 41);
            btn.ForeColor = Color.FromArgb(222, 226, 230);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
        }

        private async void LoadUserData()
        {
            var authService = AuthServiceProvider.Instance;
            string? username = await authService.GetUsernameAsync();
            string? role = await authService.GetRoleAsync();
            string? name = await authService.GetNombreCompletoAsync();

            lblUserTitle.Text = name ?? username;
            lblUserSub.Text = $"Rol: {role}";

            if (role == "Secretario")
            {
                btnAlumnos.Visible = true;
                btnCursos.Visible = true;
                btnOtros.Visible = false;
            }
            else if (role == "Portero")
            {
                btnAlumnos.Visible = false;
                btnCursos.Visible = false;
                btnOtros.Text = "Opciones Portería";
                btnOtros.Visible = true;
            }
            else
            {
                btnAlumnos.Visible = false;
                btnCursos.Visible = false;
                btnOtros.Text = "Opciones Tutor";
                btnOtros.Visible = true;
            }
        }

        private void ShowChildForm(Form childForm)
        {
            pnlContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.Show();
        }

        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            HighlightButton(btnAlumnos);
            ShowChildForm(new AlumnoListaForm());
        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            HighlightButton(btnCursos);
            ShowChildForm(new CursoEscolarListaForm());
        }

        private void btnOtros_Click(object sender, EventArgs e)
        {
            HighlightButton(btnOtros);
            MessageBox.Show("Funcionalidad en construcción para este rol.", "En construcción", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HighlightButton(Button activeBtn)
        {
            btnAlumnos.BackColor = Color.FromArgb(33, 37, 41);
            btnCursos.BackColor = Color.FromArgb(33, 37, 41);
            btnOtros.BackColor = Color.FromArgb(33, 37, 41);

            activeBtn.BackColor = Color.FromArgb(13, 110, 253);
        }

        private async void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cerrar sesión?", "Cerrar Sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
