using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class CursoEscolarListaForm : Form
    {
        private readonly CursoEscolarApiClient _apiClient = new CursoEscolarApiClient();

        public CursoEscolarListaForm()
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

            dgvCursos.BackgroundColor = Color.White;
            dgvCursos.BorderStyle = BorderStyle.None;
            dgvCursos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCursos.MultiSelect = false;
            dgvCursos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);
        }

        private async void CursoEscolarListaForm_Load(object sender, EventArgs e)
        {
            await RefreshGrid();
        }

        private async Task RefreshGrid()
        {
            try
            {
                var cursos = await _apiClient.GetAllAsync();
                dgvCursos.DataSource = cursos;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureColumns()
        {
            if (dgvCursos.Columns.Count > 0)
            {
                dgvCursos.Columns["IdCurso"].HeaderText = "ID Curso";
                dgvCursos.Columns["IdCurso"].Width = 100;
                dgvCursos.Columns["Grado"].HeaderText = "Grado";
                dgvCursos.Columns["Grado"].Width = 150;
                dgvCursos.Columns["Curso"].HeaderText = "Curso / División";
                dgvCursos.Columns["Curso"].Width = 150;
                dgvCursos.Columns["Turno"].HeaderText = "Turno";
                dgvCursos.Columns["Turno"].Width = 120;
                dgvCursos.Columns["HorarioSalida"].HeaderText = "Horario Salida";
                dgvCursos.Columns["HorarioSalida"].Width = 200;
            }
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var detailForm = new CursoEscolarDetalleForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un curso de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvCursos.SelectedRows[0].DataBoundItem as CursoEscolarDTO;
            if (selected == null) return;

            var detailForm = new CursoEscolarDetalleForm(selected);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un curso de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvCursos.SelectedRows[0].DataBoundItem as CursoEscolarDTO;
            if (selected == null) return;

            if (MessageBox.Show($"¿Está seguro de que desea eliminar el curso {selected.Grado} {selected.Curso}?",
                "Eliminar Curso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _apiClient.DeleteAsync(selected.IdCurso);
                    if (ok)
                    {
                        MessageBox.Show("Curso eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el curso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
