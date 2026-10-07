using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class TutorListaForm : Form
    {
        private readonly TutorApiClient _apiClient = new TutorApiClient();
        private readonly CursoEscolarApiClient _cursosClient = new CursoEscolarApiClient();
        private readonly AlumnoApiClient _alumnosClient = new AlumnoApiClient();
        private readonly AutorizacionApiClient _autorizacionesClient = new AutorizacionApiClient();

        private List<TutorDTO> _tutores = new();
        private List<AlumnoDTO> _alumnos = new();
        private List<AutorizacionDTO> _autorizaciones = new();

        public TutorListaForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);

            // Los filtros se aplican mientras se escribe o se cambia el curso.
            txtFiltroNombre.TextChanged += (_, _) => AplicarFiltros();
            cmbFiltroCurso.SelectedIndexChanged += (_, _) => AplicarFiltros();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);

            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);

            btnNuevo.BackColor = Color.FromArgb(40, 167, 69);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 0;

            btnEditar.BackColor = Color.FromArgb(23, 162, 184);
            btnEditar.ForeColor = Color.White;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;

            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;

            Tema.Grilla(dgvTutores);
        }

        private async void TutorListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();

            try
            {
                // Para poder filtrar por curso hace falta saber qué alumnos tiene
                // autorizado cada tutor.
                _alumnos = await _alumnosClient.GetAllAsync();
                _autorizaciones = await _autorizacionesClient.GetAllAsync();

                var cursos = await _cursosClient.GetAllAsync();

                cmbFiltroCurso.Items.Clear();
                cmbFiltroCurso.Items.Add(new OpcionDeCurso(null, "Todos los cursos"));
                foreach (var curso in cursos)
                    cmbFiltroCurso.Items.Add(new OpcionDeCurso(curso.IdCurso, $"{curso.Grado} {curso.Curso} ({curso.Turno})"));

                cmbFiltroCurso.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los cursos: {ex.Message}", "Tutores",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            await RefreshGrid();
        }

        /// <summary>
        /// Oculta las acciones que el rol del usuario no puede realizar.
        /// La restricción real la aplica la API.
        /// </summary>
        private async Task AplicarPermisosSegunRol()
        {
            string? rol = await AuthServiceProvider.Instance.GetRoleAsync();
            bool puedeAdministrar = PermisosDeUsuario.PuedeAdministrarTutores(rol);

            btnNuevo.Visible = puedeAdministrar;
            btnEditar.Visible = puedeAdministrar;
            btnEliminar.Visible = puedeAdministrar;
        }

        private async Task RefreshGrid()
        {
            try
            {
                _tutores = await _apiClient.GetAllAsync();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tutores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Muestra los tutores que pasan los filtros: por nombre, apellido o DNI, y
        /// por el curso de alguno de los alumnos que tiene autorizados.
        /// </summary>
        private void AplicarFiltros()
        {
            IEnumerable<TutorDTO> visibles = _tutores;

            string texto = txtFiltroNombre.Text.Trim();
            if (texto.Length > 0)
            {
                visibles = visibles.Where(t =>
                    t.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || t.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase)
                    || t.Dni.Contains(texto, StringComparison.OrdinalIgnoreCase));
            }

            if (cmbFiltroCurso.SelectedItem is OpcionDeCurso { IdCurso: int idCurso })
            {
                var alumnosDelCurso = _alumnos
                    .Where(a => a.IdCurso == idCurso)
                    .Select(a => a.IdAlumno)
                    .ToHashSet();

                var tutoresDelCurso = _autorizaciones
                    .Where(a => alumnosDelCurso.Contains(a.AlumnoId))
                    .Select(a => a.TutorId)
                    .ToHashSet();

                visibles = visibles.Where(t => tutoresDelCurso.Contains(t.IdTutor));
            }

            dgvTutores.DataSource = null;
            dgvTutores.DataSource = visibles.ToList();
            ConfigureColumns();
        }

        /// <summary>Un curso de la lista de filtros. El nulo es "todos".</summary>
        private sealed class OpcionDeCurso
        {
            public OpcionDeCurso(int? idCurso, string texto)
            {
                IdCurso = idCurso;
                Texto = texto;
            }

            public int? IdCurso { get; }
            public string Texto { get; }

            public override string ToString() => Texto;
        }

        private void ConfigureColumns()
        {
            if (dgvTutores.Columns.Count > 0)
            {
                dgvTutores.Columns["IdTutor"].HeaderText = "ID";
                dgvTutores.Columns["IdTutor"].Width = 50;
                dgvTutores.Columns["Nombre"].HeaderText = "Nombre";
                dgvTutores.Columns["Nombre"].Width = 140;
                dgvTutores.Columns["Apellido"].HeaderText = "Apellido";
                dgvTutores.Columns["Apellido"].Width = 140;
                dgvTutores.Columns["Dni"].HeaderText = "DNI";
                dgvTutores.Columns["Dni"].Width = 100;
                dgvTutores.Columns["Parentesco"].HeaderText = "Parentesco";
                dgvTutores.Columns["Parentesco"].Width = 110;
                dgvTutores.Columns["Telefono"].HeaderText = "Teléfono";
                dgvTutores.Columns["Telefono"].Width = 120;
                dgvTutores.Columns["NombreUsuario"].HeaderText = "Usuario";
                dgvTutores.Columns["NombreUsuario"].Width = 110;
                dgvTutores.Columns["TieneRestriccion"].HeaderText = "Restricción";
                dgvTutores.Columns["TieneRestriccion"].Width = 90;

                // La contraseña se usa al crear o modificar el tutor, pero no se
                // muestra en la lista.
                if (dgvTutores.Columns.Contains("Contrasena"))
                    dgvTutores.Columns["Contrasena"].Visible = false;
            }
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var detailForm = new TutorDetalleForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTutores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un tutor de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seleccionado = dgvTutores.SelectedRows[0].DataBoundItem as TutorDTO;
            if (seleccionado == null) return;

            var detailForm = new TutorDetalleForm(seleccionado);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTutores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un tutor de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seleccionado = dgvTutores.SelectedRows[0].DataBoundItem as TutorDTO;
            if (seleccionado == null) return;

            if (MessageBox.Show(
                    $"¿Está seguro de que desea eliminar al tutor {seleccionado.Nombre} {seleccionado.Apellido}?",
                    "Eliminar Tutor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _apiClient.DeleteAsync(seleccionado.IdTutor);
                    if (ok)
                    {
                        MessageBox.Show("Tutor eliminado con éxito.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el tutor.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
