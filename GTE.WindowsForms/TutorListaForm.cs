using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class TutorListaForm : Form
    {
        private readonly TutorApiClient _apiClient = new TutorApiClient();

        public TutorListaForm()
        {
            InitializeComponent();
            ApplyStyles();
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

            dgvTutores.BackgroundColor = Color.White;
            dgvTutores.BorderStyle = BorderStyle.None;
            dgvTutores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTutores.MultiSelect = false;
            dgvTutores.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);
        }

        private async void TutorListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();
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
                var tutores = await _apiClient.GetAllAsync();
                dgvTutores.DataSource = tutores;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tutores: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
