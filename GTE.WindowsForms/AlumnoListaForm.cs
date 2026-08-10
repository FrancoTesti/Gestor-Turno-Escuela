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

        public AlumnoListaForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);

            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblSearch.ForeColor = Color.FromArgb(73, 80, 87);

            btnBuscar.BackColor = Color.FromArgb(108, 117, 125);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.FlatAppearance.BorderSize = 0;

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

            dgvAlumnos.BackgroundColor = Color.White;
            dgvAlumnos.BorderStyle = BorderStyle.None;
            dgvAlumnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlumnos.MultiSelect = false;
            dgvAlumnos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);
        }

        private async void AlumnoListaForm_Load(object sender, EventArgs e)
        {
            await RefreshGrid();
        }

        private async Task RefreshGrid()
        {
            try
            {
                var alumnos = await _apiClient.GetAllAsync();
                dgvAlumnos.DataSource = alumnos;
                ConfigureColumns();
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
                dgvAlumnos.Columns["Nombre"].HeaderText = "Nombre";
                dgvAlumnos.Columns["Nombre"].Width = 150;
                dgvAlumnos.Columns["Apellido"].HeaderText = "Apellido";
                dgvAlumnos.Columns["Apellido"].Width = 150;
                dgvAlumnos.Columns["Grado"].HeaderText = "Grado";
                dgvAlumnos.Columns["Grado"].Width = 100;
                dgvAlumnos.Columns["Curso"].HeaderText = "Curso";
                dgvAlumnos.Columns["Curso"].Width = 100;
                dgvAlumnos.Columns["Estado"].HeaderText = "Estado";
                dgvAlumnos.Columns["Estado"].Width = 120;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            try
            {
                var criteria = new AlumnoCriteriaDTO { Nombre = term };
                var filtrados = await _apiClient.GetByCriteriaAsync(criteria);
                dgvAlumnos.DataSource = filtrados;
                ConfigureColumns();
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
