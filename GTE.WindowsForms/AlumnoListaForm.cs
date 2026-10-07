using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class AlumnoListaForm : Form
    {
        private readonly AlumnoApiClient _apiClient = new AlumnoApiClient();
        private readonly CursoEscolarApiClient _cursosClient = new CursoEscolarApiClient();

        private List<AlumnoDTO> _alumnos = new();

        public AlumnoListaForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSearch);

            Tema.BotonSecundario(btnBuscar);
            Tema.BotonExito(btnNuevo);
            Tema.BotonPrimario(btnEditar);
            Tema.BotonPeligro(btnEliminar);

            Tema.Grilla(dgvAlumnos);
        }

        private async Task CargarOpcionesDeFiltro()
        {
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

            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Presente");
            cmbFiltroEstado.Items.Add("Retirado");
            cmbFiltroEstado.Items.Add("Ausente");
            cmbFiltroEstado.SelectedIndex = 0;
        }

        private static string? Elegido(ComboBox combo) =>
            combo.SelectedIndex <= 0 ? null : combo.SelectedItem?.ToString();

        private async void AlumnoListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();
            await CargarOpcionesDeFiltro();
            await RefreshGrid();
        }

        private void MostrarAlumnos()
        {
            string? turno = Elegido(cmbFiltroTurno);

            var visibles = string.IsNullOrEmpty(turno)
                ? _alumnos
                : _alumnos.Where(a => string.Equals(a.Turno, turno, StringComparison.OrdinalIgnoreCase)).ToList();

            dgvAlumnos.DataSource = null;
            dgvAlumnos.DataSource = visibles;
            ConfigureColumns();
        }

        private async Task AplicarPermisosSegunRol()
        {
            string? rol = await AuthServiceProvider.Instance.GetRoleAsync();
            bool puedeAdministrar = PermisosDeUsuario.PuedeAdministrarAlumnos(rol);

            btnNuevo.Visible = puedeAdministrar;
            btnEditar.Visible = puedeAdministrar;
            btnEliminar.Visible = puedeAdministrar;
        }

        private async Task RefreshGrid()
        {
            try
            {
                _alumnos = await _apiClient.GetAllAsync();
                MostrarAlumnos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureColumns()
        {
            if (dgvAlumnos.Columns.Count > 0)
            {
                dgvAlumnos.Columns["IdAlumno"].HeaderText = "ID";
                dgvAlumnos.Columns["IdAlumno"].Width = 60;
                dgvAlumnos.Columns["IdCurso"].Visible = false;
                dgvAlumnos.Columns["Nombre"].HeaderText = "Nombre";
                dgvAlumnos.Columns["Nombre"].Width = 150;
                dgvAlumnos.Columns["Apellido"].HeaderText = "Apellido";
                dgvAlumnos.Columns["Apellido"].Width = 150;
                dgvAlumnos.Columns["Grado"].HeaderText = "Grado";
                dgvAlumnos.Columns["Grado"].Width = 100;
                dgvAlumnos.Columns["Curso"].HeaderText = "Curso";
                dgvAlumnos.Columns["Curso"].Width = 100;
                dgvAlumnos.Columns["Turno"].HeaderText = "Turno";
                dgvAlumnos.Columns["Turno"].Width = 100;
                dgvAlumnos.Columns["Estado"].HeaderText = "Estado";
                dgvAlumnos.Columns["Estado"].Width = 120;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            try
            {
                var criteria = new AlumnoCriteriaDTO
                {
                    Nombre = string.IsNullOrWhiteSpace(term) ? null : term,
                    Grado = Elegido(cmbFiltroGrado),
                    Curso = Elegido(cmbFiltroDivision),
                    Estado = Elegido(cmbFiltroEstado)
                };

                _alumnos = await _apiClient.GetByCriteriaAsync(criteria);
                MostrarAlumnos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var detailForm = new AlumnoDetalleForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvAlumnos.SelectedRows[0].DataBoundItem as AlumnoDTO;
            if (selected == null) return;

            var detailForm = new AlumnoDetalleForm(selected);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvAlumnos.SelectedRows[0].DataBoundItem as AlumnoDTO;
            if (selected == null) return;

            if (MessageBox.Show($"¿Está seguro de que desea eliminar al alumno {selected.Nombre} {selected.Apellido}?",
                "Eliminar Alumno", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _apiClient.DeleteAsync(selected.IdAlumno);
                    if (ok)
                    {
                        MessageBox.Show("Alumno eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el alumno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
