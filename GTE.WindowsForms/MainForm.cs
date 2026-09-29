using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class MainForm : Form
    {
        private string? nombreMostrado;
        private string? rolMostrado;

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
                         btnInicio, btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
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

            nombreMostrado = name ?? username;
            rolMostrado = role;

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

            MostrarInicio();
        }

        /// <summary>
        /// Pantalla de inicio: saluda al usuario y le muestra una tarjeta por
        /// cada sección que tiene habilitada, con el mismo aspecto que la web.
        /// </summary>
        private void MostrarInicio()
        {
            HighlightButton(btnInicio);

            var secciones = new List<AccesoDirecto>();

            void Agregar(Button boton, string titulo, string descripcion, Func<Form> pantalla)
            {
                if (boton.Visible)
                    secciones.Add(new AccesoDirecto(titulo, descripcion, () => Abrir(boton, pantalla)));
            }

            Agregar(btnAlumnos, "Alumnos",
                "Consultá el listado, buscá por curso o turno y administrá los legajos.",
                () => new AlumnoListaForm());
            Agregar(btnRetiros, "Retiros",
                "Registrá la entrega de alumnos a un tutor autorizado.",
                () => new RetiroListaForm());
            Agregar(btnOtros, "Horarios especiales",
                "Cargá las salidas a una hora distinta por una actividad puntual.",
                () => new HorarioEspecialListaForm());
            Agregar(btnTutores, "Tutores",
                "Datos de contacto de los adultos autorizados a retirar.",
                () => new TutorListaForm());
            Agregar(btnAutorizaciones, "Autorizaciones",
                "Definí qué tutor puede retirar a cada alumno.",
                () => new AutorizacionListaForm());
            Agregar(btnCursos, "Cursos",
                "Grados, divisiones, turnos y horario de salida de cada curso.",
                () => new CursoEscolarListaForm());
            Agregar(btnMisAlumnos, "Mis alumnos",
                "Los alumnos a tu cargo, con su curso, horario de salida y estado.",
                () => new MisAlumnosForm());
            Agregar(btnReporteAlumnos, "Reporte de alumnos",
                "Cantidad de alumnos por curso y turno, con gráfico.",
                () => new ReporteAlumnosPorCursoForm());
            Agregar(btnReporteRetiros, "Reporte de retiros",
                "Retiros de un período, con el detalle de los alumnos.",
                () => new ReporteRetirosForm());

            ShowChildForm(new HomeForm(nombreMostrado, rolMostrado, secciones));
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

        /// <summary>Abre una pantalla del menú y deja su botón resaltado.</summary>
        private void Abrir(Button boton, Func<Form> pantalla)
        {
            HighlightButton(boton);
            ShowChildForm(pantalla());
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            MostrarInicio();
        }

        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            Abrir(btnAlumnos, () => new AlumnoListaForm());
        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            Abrir(btnCursos, () => new CursoEscolarListaForm());
        }

        private void btnRetiros_Click(object sender, EventArgs e)
        {
            Abrir(btnRetiros, () => new RetiroListaForm());
        }

        private void btnTutores_Click(object sender, EventArgs e)
        {
            Abrir(btnTutores, () => new TutorListaForm());
        }

        private void btnAutorizaciones_Click(object sender, EventArgs e)
        {
            Abrir(btnAutorizaciones, () => new AutorizacionListaForm());
        }

        private void btnReporteAlumnos_Click(object sender, EventArgs e)
        {
            Abrir(btnReporteAlumnos, () => new ReporteAlumnosPorCursoForm());
        }

        private void btnReporteRetiros_Click(object sender, EventArgs e)
        {
            Abrir(btnReporteRetiros, () => new ReporteRetirosForm());
        }

        private void btnOtros_Click(object sender, EventArgs e)
        {
            Abrir(btnOtros, () => new HorarioEspecialListaForm());
        }

        private void btnMisAlumnos_Click(object sender, EventArgs e)
        {
            Abrir(btnMisAlumnos, () => new MisAlumnosForm());
        }

        private void HighlightButton(Button activeBtn)
        {
            foreach (var boton in new[]
                     {
                         btnInicio, btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
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



