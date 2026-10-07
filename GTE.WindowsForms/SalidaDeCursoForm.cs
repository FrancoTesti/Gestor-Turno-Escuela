using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    /// <summary>
    /// Pantalla del portero: marca qué curso está saliendo y cuándo terminó de
    /// salir. Eso mismo es lo que aparece en la pantalla de la puerta.
    /// </summary>
    public partial class SalidaDeCursoForm : Form
    {
        private readonly CursoEscolarApiClient _cursosClient = new CursoEscolarApiClient();
        private readonly SalidaDeCursoApiClient _salidasClient = new SalidaDeCursoApiClient();

        public SalidaDeCursoForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.Entrada(cmbCursos);
            Tema.BotonPrimario(btnIniciar);
            Tema.BotonSecundario(btnFinalizar);
            Tema.Grilla(dgvSalidas);

            lblMensaje.ForeColor = Tema.TextoSuave;
        }

        private async void SalidaDeCursoForm_Load(object sender, EventArgs e)
        {
            try
            {
                var cursos = await _cursosClient.GetAllAsync();

                cmbCursos.Items.Clear();
                foreach (var curso in cursos)
                    cmbCursos.Items.Add(new OpcionDeCurso(curso));

                if (cmbCursos.Items.Count > 0)
                    cmbCursos.SelectedIndex = 0;

                await CargarSalidasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los cursos: {ex.Message}", "Salidas de curso",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarSalidasAsync()
        {
            var salidas = await _salidasClient.GetDelDiaAsync();

            dgvSalidas.DataSource = null;
            dgvSalidas.DataSource = salidas;

            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            if (dgvSalidas.Columns.Count == 0)
                return;

            OcultarColumna("IdSalidaDeCurso");
            OcultarColumna("IdCurso");

            ConfigurarColumna("Grado", "Grado", 70);
            ConfigurarColumna("Curso", "División", 70);
            ConfigurarColumna("Turno", "Turno", 90);
            ConfigurarColumna("HorarioSalida", "Hora de salida", 110, @"hh\:mm");
            ConfigurarColumna("Puerta", "Puerta", 140);
            ConfigurarColumna("PersonalNombre", "Marcada por", 140);
            ConfigurarColumna("FechaHoraInicio", "Inicio", 80, "HH:mm");
            ConfigurarColumna("FechaHoraFin", "Fin", 80, "HH:mm");
            ConfigurarColumna("CantidadDeAlumnos", "Alumnos", 80);
            ConfigurarColumna("EstaEnCurso", "En curso", 80);
        }

        private void OcultarColumna(string nombre)
        {
            var columna = dgvSalidas.Columns[nombre];
            if (columna != null)
                columna.Visible = false;
        }

        private void ConfigurarColumna(string nombre, string titulo, int ancho, string? formato = null)
        {
            var columna = dgvSalidas.Columns[nombre];
            if (columna == null)
                return;

            columna.HeaderText = titulo;
            columna.Width = ancho;

            if (formato != null)
                columna.DefaultCellStyle.Format = formato;
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            if (cmbCursos.SelectedItem is not OpcionDeCurso opcion)
            {
                MessageBox.Show("Elegí el curso que está saliendo.", "Salidas de curso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await EjecutarAsync(async () =>
            {
                await _salidasClient.IniciarAsync(opcion.IdCurso);
                lblMensaje.Text = $"El curso {opcion.Texto} está saliendo.";
            });
        }

        private async void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (dgvSalidas.CurrentRow?.DataBoundItem is not SalidaDeCursoDTO seleccionada)
            {
                MessageBox.Show("Elegí en la lista la salida que terminó.", "Salidas de curso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await EjecutarAsync(async () =>
            {
                await _salidasClient.FinalizarAsync(seleccionada.IdSalidaDeCurso);
                lblMensaje.Text = "La salida quedó finalizada.";
            });
        }

        /// <summary>Ejecuta una acción contra la API y vuelve a mostrar la lista.</summary>
        private async Task EjecutarAsync(Func<Task> accion)
        {
            btnIniciar.Enabled = false;
            btnFinalizar.Enabled = false;

            try
            {
                await accion();
                await CargarSalidasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Salidas de curso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnIniciar.Enabled = true;
                btnFinalizar.Enabled = true;
            }
        }

        /// <summary>Un curso como se muestra en la lista desplegable.</summary>
        private sealed class OpcionDeCurso
        {
            public OpcionDeCurso(CursoEscolarDTO curso)
            {
                IdCurso = curso.IdCurso;
                Texto = $"{curso.Grado} {curso.Curso} - {curso.Turno}";
            }

            public int IdCurso { get; }
            public string Texto { get; }

            public override string ToString() => Texto;
        }
    }
}
