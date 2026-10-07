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

            // Los filtros se aplican mientras se escribe o se cambia alguno.
            txtFiltroNombre.TextChanged += (_, _) => AplicarFiltros();
            cmbFiltroGrado.SelectedIndexChanged += (_, _) => AplicarFiltros();
            cmbFiltroDivision.SelectedIndexChanged += (_, _) => AplicarFiltros();
            cmbFiltroTurno.SelectedIndexChanged += (_, _) => AplicarFiltros();
            cmbFiltroEstado.SelectedIndexChanged += (_, _) => AplicarFiltros();
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

                void Cargar(ComboBox combo, IEnumerable<string> opciones, string todos)
                {
                    combo.Items.Clear();
                    combo.Items.Add(todos);
                    foreach (var opcion in opciones.Distinct().OrderBy(o => o))
                        combo.Items.Add(opcion);
                    combo.SelectedIndex = 0;
                }

                Cargar(cmbFiltroGrado, cursos.Select(c => c.Grado), "Todos");
                Cargar(cmbFiltroDivision, cursos.Select(c => c.Curso), "Todas");
                Cargar(cmbFiltroTurno, cursos.Select(c => c.Turno), "Todos");
                Cargar(cmbFiltroEstado, new[] { "Presente", "Retirado", "Ausente" }, "Todos");
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

            // Los filtros de grado, división, turno y estado son del alumno: se
            // buscan los tutores que tienen autorizado algún alumno que los cumpla.
            string? grado = Elegido(cmbFiltroGrado);
            string? division = Elegido(cmbFiltroDivision);
            string? turno = Elegido(cmbFiltroTurno);
            string? estado = Elegido(cmbFiltroEstado);

            if (grado != null || division != null || turno != null || estado != null)
            {
                var alumnosQueCumplen = _alumnos
                    .Where(a => (grado == null || a.Grado == grado)
                        && (division == null || a.Curso == division)
                        && (turno == null || a.Turno == turno)
                        && (estado == null || a.Estado == estado))
                    .Select(a => a.IdAlumno)
                    .ToHashSet();

                var tutoresQueCumplen = _autorizaciones
                    .Where(a => alumnosQueCumplen.Contains(a.AlumnoId))
                    .Select(a => a.TutorId)
                    .ToHashSet();

                visibles = visibles.Where(t => tutoresQueCumplen.Contains(t.IdTutor));
            }

            dgvTutores.DataSource = null;
            dgvTutores.DataSource = visibles.ToList();
            ConfigureColumns();
        }

        /// <summary>El texto elegido en un filtro, o nulo si está en "todos".</summary>
        private static string? Elegido(ComboBox combo) =>
            combo.SelectedIndex <= 0 ? null : combo.SelectedItem?.ToString();

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
