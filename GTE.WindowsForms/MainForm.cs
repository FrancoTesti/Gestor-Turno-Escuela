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
            pnlHeader.BackColor = Tema.Barra;
            pnlSidebar.BackColor = Tema.Barra;
            pnlContent.BackColor = Tema.Fondo;

            lblUserTitle.ForeColor = Color.White;
            lblUserSub.ForeColor = Color.FromArgb(173, 181, 189);

            Tema.BotonPeligro(btnLogOut);

            foreach (var boton in new[]
                     {
                         btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
                         btnReporteAlumnos, btnReporteRetiros, btnOtros, btnMisAlumnos
                     })
            {
                Tema.BotonMenu(boton);
            }
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
                btnRetiros.Visible = true;
                btnTutores.Visible = true;
                btnAutorizaciones.Visible = true;
                btnReporteAlumnos.Visible = true;
                btnReporteRetiros.Visible = true;
                btnOtros.Visible = true;
                btnOtros.Text = "Horarios Esp.";
            }
            else if (role == "Portero")
            {
                btnAlumnos.Visible = false;
                btnCursos.Visible = false;
                btnRetiros.Visible = true;
                btnTutores.Visible = false;
                btnAutorizaciones.Visible = true;
                btnReporteAlumnos.Visible = true;
                btnReporteRetiros.Visible = true;
                btnOtros.Visible = true;
                btnOtros.Text = "Horarios Esp.";
            }
            else
            {
                btnAlumnos.Visible = false;
                btnCursos.Visible = true;
                btnRetiros.Visible = false;
                btnTutores.Visible = false;
                btnAutorizaciones.Visible = false;
                btnReporteAlumnos.Visible = false;
                btnReporteRetiros.Visible = false;
                btnOtros.Visible = false;
            }

            // El tutor solo tiene, además de cursos, su pantalla de alumnos a cargo.
            btnMisAlumnos.Visible = role == "Tutor";
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

        private void btnRetiros_Click(object sender, EventArgs e)
        {
            HighlightButton(btnRetiros);
            ShowChildForm(new RetiroListaForm());
        }

        private void btnTutores_Click(object sender, EventArgs e)
        {
            HighlightButton(btnTutores);
            ShowChildForm(new TutorListaForm());
        }

        private void btnAutorizaciones_Click(object sender, EventArgs e)
        {
            HighlightButton(btnAutorizaciones);
            ShowChildForm(new AutorizacionListaForm());
        }

        private void btnReporteAlumnos_Click(object sender, EventArgs e)
        {
            HighlightButton(btnReporteAlumnos);
            ShowChildForm(new ReporteAlumnosPorCursoForm());
        }

        private void btnReporteRetiros_Click(object sender, EventArgs e)
        {
            HighlightButton(btnReporteRetiros);
            ShowChildForm(new ReporteRetirosForm());
        }

        private void btnOtros_Click(object sender, EventArgs e)
        {
            HighlightButton(btnOtros);
            ShowChildForm(new HorarioEspecialListaForm());
        }

        private void btnMisAlumnos_Click(object sender, EventArgs e)
        {
            HighlightButton(btnMisAlumnos);
            ShowChildForm(new MisAlumnosForm());
        }

        private void HighlightButton(Button activeBtn)
        {
            foreach (var boton in new[]
                     {
                         btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
                         btnReporteAlumnos, btnReporteRetiros, btnOtros, btnMisAlumnos
                     })
            {
                boton.BackColor = Tema.Barra;
            }

            activeBtn.BackColor = Tema.Primario;
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



