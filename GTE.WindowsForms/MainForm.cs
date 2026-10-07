using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

            // Cuando cambia el alto de la ventana se reparte de nuevo el menú.
            pnlSidebar.Resize += (_, _) => AcomodarMenu();

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
                         btnInicio, btnSalidas, btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
                         btnReporteAlumnos, btnReporteRetiros, btnOtros, btnMisAlumnos
                     })
            {
                Tema.BotonMenu(boton);
            }

            // La pantalla de salida es un enlace al navegador, pero se ve como los
            // demás botones del menú.
            Tema.BotonMenu(btnCartelera);

            foreach (var etiqueta in new[]
                     {
                         lblSeccionDiaADia, lblSeccionAdministracion, lblSeccionReportes, lblSeccionPantalla
                     })
            {
                Tema.EtiquetaSeccion(etiqueta);
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
                btnOtros.Visible = true;
                btnOtros.Text = "Horarios Esp.";
            }
            else if (role == "Portero")
            {
                // El portero ve los mismos listados que el secretario, pero sin
                // los botones para modificar (eso lo controla la API).
                btnAlumnos.Visible = true;
                btnCursos.Visible = true;
                btnRetiros.Visible = true;
                btnTutores.Visible = true;
                btnAutorizaciones.Visible = true;
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
                btnOtros.Visible = false;
            }

            // Reportes: el de alumnos es de gestión (sólo secretaría) y el de
            // retiros lo usa también la portería, que es la que retira.
            btnReporteAlumnos.Visible = PermisosDeUsuario.PuedeVerReporteDeAlumnos(role);
            btnReporteRetiros.Visible = PermisosDeUsuario.PuedeVerReporteDeRetiros(role);

            // El tutor solo tiene, además de cursos, su pantalla de alumnos a cargo.
            btnMisAlumnos.Visible = role == "Tutor";

            // El portero es quien marca en la puerta qué curso está saliendo.
            btnSalidas.Visible = role == "Secretario" || role == "Portero";

            // La pantalla de la puerta se abre en el navegador, como en la web.
            btnCartelera.Visible = role == "Secretario" || role == "Portero";

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Se acomodan acá y no en el constructor: los controles todavía no
            // están visibles, así que en ese momento no se puede saber cuáles
            // quedaron para el rol que entró.
            AcomodarSecciones();
            AcomodarMenu();

            // El inicio se arma recién acá: las tarjetas se filtran por los botones
            // visibles, y eso no se puede saber antes de mostrar la ventana.
            MostrarInicio();
        }

        /// <summary>
        /// Reparte el alto de la barra entre las opciones visibles, así con la
        /// ventana maximizada el menú ocupa toda la altura. Si no entra, la barra
        /// se puede desplazar y ninguna opción queda fuera de alcance.
        /// </summary>
        private void AcomodarMenu()
        {
            const int AltoDelTitulo = 30;
            const int AltoMinimoDeOpcion = 44;

            // Orden en el que se ven las opciones, de arriba hacia abajo.
            var menu = new Control[]
            {
                btnInicio,
                lblSeccionDiaADia, btnSalidas, btnRetiros, btnAlumnos, btnOtros, btnMisAlumnos,
                lblSeccionAdministracion, btnCursos, btnTutores, btnAutorizaciones,
                lblSeccionReportes, btnReporteAlumnos, btnReporteRetiros,
                lblSeccionPantalla, btnCartelera
            };

            var visibles = menu.Where(control => control.Visible).ToList();
            if (visibles.Count == 0)
                return;

            var titulos = visibles.Where(control => control is Label).ToList();
            var opciones = visibles.Where(control => control is Button).ToList();

            int disponible = pnlSidebar.ClientSize.Height - (titulos.Count * AltoDelTitulo);
            int alto = opciones.Count > 0
                ? Math.Max(AltoMinimoDeOpcion, disponible / opciones.Count)
                : AltoMinimoDeOpcion;

            // Los controles se ubican uno debajo del otro a mano y no con Dock: la
            // barra de desplazamiento de un panel con controles anclados calcula de
            // más y dejaba un espacio en blanco después de la última opción.
            // El ancho deja libre el lugar de la barra de desplazamiento, así nunca
            // aparece la barra horizontal con espacio de más abajo.
            int anchoDeLaBarra = Math.Max(120, pnlSidebar.Width - 20);
            int y = 0;

            foreach (var control in visibles)
            {
                control.Dock = DockStyle.None;
                control.Height = control is Label ? AltoDelTitulo : alto;
                control.Location = new Point(0, y);
                control.Width = anchoDeLaBarra;

                y += control.Height;
            }

            pnlSidebar.PerformLayout();
        }

        /// <summary>
        /// El título de cada grupo del menú se muestra solo si el grupo tiene
        /// alguna opción disponible para el rol que entró.
        /// </summary>
        private void AcomodarSecciones()
        {
            lblSeccionDiaADia.Visible = btnSalidas.Visible || btnRetiros.Visible || btnAlumnos.Visible
                || btnOtros.Visible || btnMisAlumnos.Visible;

            lblSeccionAdministracion.Visible = btnCursos.Visible || btnTutores.Visible || btnAutorizaciones.Visible;

            lblSeccionReportes.Visible = btnReporteAlumnos.Visible || btnReporteRetiros.Visible;

            lblSeccionPantalla.Visible = btnCartelera.Visible;
        }

        /// <summary>
        /// Abre la pantalla que se deja fija en la puerta del colegio. Es la misma
        /// que el enlace del menú de la web.
        /// </summary>
        private void btnCartelera_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo($"{ApiConfig.DireccionDeLaWeb}/cartelera")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir la pantalla de salida en el navegador: {ex.Message}",
                    "Pantalla de salida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Pantalla de inicio: saluda al usuario y le muestra una tarjeta por
        /// cada sección que tiene habilitada, con el mismo aspecto que la web.
        /// </summary>
        private void MostrarInicio()
        {
            HighlightButton(btnInicio);

            var diaADia = new List<AccesoDirecto>();
            var administracion = new List<AccesoDirecto>();
            var reportes = new List<AccesoDirecto>();

            void Agregar(List<AccesoDirecto> grupo, Button boton, string titulo, string descripcion, Func<Form> pantalla)
            {
                if (boton.Visible)
                    grupo.Add(new AccesoDirecto(titulo, descripcion, () => Abrir(boton, pantalla)));
            }

            // Mismo orden y mismos grupos que el menú de la izquierda.
            Agregar(diaADia, btnSalidas, "Salidas de curso",
                "Marcá qué curso está saliendo. La pantalla de la puerta lo muestra al instante.",
                () => new SalidaDeCursoForm());
            Agregar(diaADia, btnRetiros, "Retiros",
                "Registrá la entrega de alumnos a un tutor autorizado.",
                () => new RetiroListaForm());
            Agregar(diaADia, btnAlumnos, "Alumnos",
                "Consultá el listado, buscá por curso o turno y administrá los legajos.",
                () => new AlumnoListaForm());
            Agregar(diaADia, btnOtros, "Horarios especiales",
                "Cargá las salidas a una hora distinta por una actividad puntual.",
                () => new HorarioEspecialListaForm());
            Agregar(diaADia, btnMisAlumnos, "Mis alumnos",
                "Los alumnos a tu cargo, con su curso, horario de salida y estado.",
                () => new MisAlumnosForm());

            Agregar(administracion, btnCursos, "Cursos",
                "Grados, divisiones, turnos y horario de salida de cada curso.",
                () => new CursoEscolarListaForm());
            Agregar(administracion, btnTutores, "Tutores",
                "Datos de contacto de los adultos autorizados a retirar.",
                () => new TutorListaForm());
            Agregar(administracion, btnAutorizaciones, "Autorizaciones",
                "Definí qué tutor puede retirar a cada alumno.",
                () => new AutorizacionListaForm());

            Agregar(reportes, btnReporteAlumnos, "Reporte de alumnos",
                "Cantidad de alumnos por curso y turno, con gráfico.",
                () => new ReporteAlumnosPorCursoForm());
            Agregar(reportes, btnReporteRetiros, "Reporte de retiros",
                "Retiros de un período, con el detalle de los alumnos.",
                () => new ReporteRetirosForm());

            var secciones = new List<SeccionDelInicio>
            {
                new("Día a día", diaADia),
                new("Administración", administracion),
                new("Reportes", reportes)
            };

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

        private void btnSalidas_Click(object sender, EventArgs e)
        {
            Abrir(btnSalidas, () => new SalidaDeCursoForm());
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
                         btnInicio, btnSalidas, btnAlumnos, btnCursos, btnTutores, btnRetiros, btnAutorizaciones,
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



