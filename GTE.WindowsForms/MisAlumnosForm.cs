using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public partial class MisAlumnosForm : Form
    {
        private readonly TutorApiClient _apiClient = new TutorApiClient();
        private readonly System.Windows.Forms.Timer _refresco = new();
        private bool _cargando;

        public MisAlumnosForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);

            _refresco.Interval = 20000;
            _refresco.Tick += async (_, _) => await RefrescarEnSegundoPlanoAsync();
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.BotonSecundario(btnActualizar);
            Tema.Grilla(dgvAlumnos);

            lblAviso.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        }

        private async void MisAlumnosForm_Load(object sender, EventArgs e)
        {
            await CargarAsync(true);
            _refresco.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _refresco.Stop();
            _refresco.Dispose();

            base.OnFormClosed(e);
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await CargarAsync(true);
        }

        private async Task RefrescarEnSegundoPlanoAsync()
        {
            if (_cargando || !Visible)
                return;

            await CargarAsync(false);
        }

        private async Task CargarAsync(bool mostrarError)
        {
            _cargando = true;
            btnActualizar.Enabled = false;

            try
            {
                var alumnos = await _apiClient.GetMisAlumnosAsync();
                Mostrar(alumnos);
            }
            catch (Exception ex)
            {
                if (mostrarError)
                {
                    MessageBox.Show($"Error al cargar tus alumnos: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                _cargando = false;
                btnActualizar.Enabled = true;
            }
        }

        private void Mostrar(List<AlumnoACargoDTO> alumnos)
        {
            var ordenados = alumnos
                .OrderBy(a => a.HorarioSalida)
                .ThenBy(a => a.Apellido)
                .ToList();

            dgvAlumnos.DataSource = null;
            dgvAlumnos.DataSource = ordenados
                .Select(a => new
                {
                    Sale = a.HorarioSalida,
                    Alumno = $"{a.Apellido}, {a.Nombre}",
                    Curso = $"{a.Grado} {a.Curso}",
                    a.Turno,
                    a.Estado,
                    Situacion = a.EstaSaliendo ? "Está saliendo" : "-",
                    Saliendo = a.EstaSaliendo
                })
                .ToList();

            ConfigurarColumnas();
            PintarLosQueEstanSaliendo();
            MostrarResumen(ordenados);
        }

        private void ConfigurarColumnas()
        {
            if (dgvAlumnos.Columns.Count == 0)
                return;

            dgvAlumnos.Columns["Sale"].HeaderText = "Sale";
            dgvAlumnos.Columns["Sale"].Width = 80;
            dgvAlumnos.Columns["Sale"].DefaultCellStyle.Format = @"hh\:mm";
            dgvAlumnos.Columns["Alumno"].HeaderText = "Alumno";
            dgvAlumnos.Columns["Alumno"].Width = 220;
            dgvAlumnos.Columns["Curso"].HeaderText = "Curso";
            dgvAlumnos.Columns["Curso"].Width = 90;
            dgvAlumnos.Columns["Turno"].HeaderText = "Turno";
            dgvAlumnos.Columns["Turno"].Width = 100;
            dgvAlumnos.Columns["Estado"].HeaderText = "Estado";
            dgvAlumnos.Columns["Estado"].Width = 110;
            dgvAlumnos.Columns["Situacion"].HeaderText = "Situación";
            dgvAlumnos.Columns["Situacion"].Width = 130;
            dgvAlumnos.Columns["Saliendo"].Visible = false;
        }

        private void PintarLosQueEstanSaliendo()
        {
            foreach (DataGridViewRow fila in dgvAlumnos.Rows)
            {
                bool saliendo = fila.Cells["Saliendo"].Value is bool valor && valor;

                if (!saliendo)
                    continue;

                fila.DefaultCellStyle.BackColor = Color.FromArgb(231, 241, 255);
                fila.DefaultCellStyle.SelectionBackColor = Color.FromArgb(207, 226, 255);
            }
        }

        private void MostrarResumen(List<AlumnoACargoDTO> alumnos)
        {
            if (alumnos.Count == 0)
            {
                lblSub.Text = "Todavía no tenés alumnos a cargo.";
                MostrarAviso(
                    "La autorización la carga la secretaría: pedí que registren tu nombre y tu DNI para los chicos que retirás.",
                    false);
                return;
            }

            int cursos = alumnos.Select(a => $"{a.Grado} {a.Curso}").Distinct().Count();
            string alumnosTexto = alumnos.Count == 1 ? "1 alumno a cargo" : $"{alumnos.Count} alumnos a cargo";
            string cursosTexto = cursos == 1 ? "en 1 curso" : $"en {cursos} cursos";
            List<string> horarios = alumnos
                .Select(a => a.HorarioSalida.ToString(@"hh\:mm"))
                .Distinct()
                .OrderBy(h => h)
                .ToList();

            lblSub.Text = $"Tenés {alumnosTexto} {cursosTexto}. Hoy salen a las {Listar(horarios)}.";

            List<AlumnoACargoDTO> saliendo = alumnos.Where(a => a.EstaSaliendo).ToList();

            if (saliendo.Count > 0)
            {
                List<string> cursosSaliendo = saliendo.Select(a => $"{a.Grado} {a.Curso}").Distinct().ToList();
                string nombres = Listar(saliendo.Select(a => $"{a.Nombre} {a.Apellido}").ToList());

                MostrarAviso(
                    cursosSaliendo.Count == 1
                        ? $"Ahora está saliendo {cursosSaliendo[0]}: {nombres}. Decile tu DNI a la portería."
                        : $"Ahora están saliendo {Listar(cursosSaliendo)}: {nombres}. Decile tu DNI a la portería.",
                    true);
                return;
            }

            TimeSpan ahora = DateTime.Now.TimeOfDay;
            AlumnoACargoDTO? proximo = alumnos.FirstOrDefault(a => a.HorarioSalida >= ahora);

            MostrarAviso(
                proximo is null
                    ? "Ahora no está saliendo ninguno de tus cursos. Ya pasaron los horarios de hoy."
                    : $"Ahora no está saliendo ninguno de tus cursos. La próxima es a las {proximo.HorarioSalida:hh\\:mm} ({proximo.Grado} {proximo.Curso}).",
                false);
        }

        private void MostrarAviso(string texto, bool saliendo)
        {
            pnlAviso.BackColor = saliendo ? Color.FromArgb(10, 88, 202) : Color.FromArgb(241, 243, 245);
            lblAviso.ForeColor = saliendo ? Color.White : Color.FromArgb(52, 58, 64);
            lblAviso.Text = texto;
        }

        private static string Listar(List<string> elementos) =>
            elementos.Count switch
            {
                0 => string.Empty,
                1 => elementos[0],
                _ => $"{string.Join(", ", elementos.Take(elementos.Count - 1))} y {elementos[^1]}"
            };
    }
}
