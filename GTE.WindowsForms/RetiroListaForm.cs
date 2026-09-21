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
    public partial class RetiroListaForm : Form
    {
        private readonly RetiroApiClient _apiClient = new RetiroApiClient();
        private List<RetiroDTO> _todosLosRetiros = new();

        public RetiroListaForm()
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

            dgvRetiros.BackgroundColor = Color.White;
            dgvRetiros.BorderStyle = BorderStyle.None;
            dgvRetiros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRetiros.MultiSelect = false;
            dgvRetiros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);
        }

        private async void RetiroListaForm_Load(object sender, EventArgs e)
        {
            await RefreshGrid();
        }

        private async Task RefreshGrid()
        {
            try
            {
                _todosLosRetiros = await _apiClient.GetAllAsync();
                MostrarRetiros(_todosLosRetiros);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar retiros: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarRetiros(List<RetiroDTO> retiros)
        {
            var vista = retiros.Select(r => new
            {
                r.IdRetiro,
                FechaHora = r.FechaHora.ToString("g"),
                Tutor = !string.IsNullOrEmpty(r.TutorNombreCompleto) ? r.TutorNombreCompleto : $"Tutor #{r.IdTutor}",
                Personal = !string.IsNullOrEmpty(r.PersonalNombre) ? r.PersonalNombre : $"Personal #{r.IdPersonal}",
                CantidadAlumnos = r.Detalles.Count,
                Alumnos = string.Join(", ", r.Detalles.Select(d => !string.IsNullOrEmpty(d.AlumnoNombreCompleto) ? d.AlumnoNombreCompleto : $"Alumno #{d.IdAlumno}")),
                r.Observaciones,
                RetiroObjeto = r
            }).ToList();

            dgvRetiros.DataSource = vista;
            ConfigureColumns();
        }

        private void ConfigureColumns()
        {
            if (dgvRetiros.Columns.Count > 0)
            {
                if (dgvRetiros.Columns.Contains("RetiroObjeto"))
                    dgvRetiros.Columns["RetiroObjeto"].Visible = false;

                if (dgvRetiros.Columns.Contains("IdRetiro"))
                {
                    dgvRetiros.Columns["IdRetiro"].HeaderText = "ID";
                    dgvRetiros.Columns["IdRetiro"].Width = 60;
                }

                if (dgvRetiros.Columns.Contains("FechaHora"))
                {
                    dgvRetiros.Columns["FechaHora"].HeaderText = "Fecha y Hora";
                    dgvRetiros.Columns["FechaHora"].Width = 140;
                }

                if (dgvRetiros.Columns.Contains("Tutor"))
                {
                    dgvRetiros.Columns["Tutor"].HeaderText = "Tutor";
                    dgvRetiros.Columns["Tutor"].Width = 150;
                }

                if (dgvRetiros.Columns.Contains("Personal"))
                {
                    dgvRetiros.Columns["Personal"].HeaderText = "Personal (Entrega)";
                    dgvRetiros.Columns["Personal"].Width = 150;
                }

                if (dgvRetiros.Columns.Contains("CantidadAlumnos"))
                {
                    dgvRetiros.Columns["CantidadAlumnos"].HeaderText = "Cant. Alumnos";
                    dgvRetiros.Columns["CantidadAlumnos"].Width = 110;
                }

                if (dgvRetiros.Columns.Contains("Alumnos"))
                {
                    dgvRetiros.Columns["Alumnos"].HeaderText = "Alumnos Retirados";
                    dgvRetiros.Columns["Alumnos"].Width = 200;
                }

                if (dgvRetiros.Columns.Contains("Observaciones"))
                {
                    dgvRetiros.Columns["Observaciones"].HeaderText = "Observaciones";
                    dgvRetiros.Columns["Observaciones"].Width = 200;
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(term))
            {
                MostrarRetiros(_todosLosRetiros);
            }
            else
            {
                var filtrados = _todosLosRetiros.Where(r =>
                    (!string.IsNullOrEmpty(r.TutorNombreCompleto) && r.TutorNombreCompleto.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(r.Observaciones) && r.Observaciones.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    r.Detalles.Any(d => !string.IsNullOrEmpty(d.AlumnoNombreCompleto) && d.AlumnoNombreCompleto.Contains(term, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                MostrarRetiros(filtrados);
            }
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var detailForm = new RetiroDetalleForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRetiros.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un retiro de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvRetiros.SelectedRows[0];
            var celdaObjeto = fila.Cells["RetiroObjeto"].Value as RetiroDTO;
            if (celdaObjeto == null) return;

            var detailForm = new RetiroDetalleForm(celdaObjeto);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRetiros.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un retiro de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvRetiros.SelectedRows[0];
            var celdaObjeto = fila.Cells["RetiroObjeto"].Value as RetiroDTO;
            if (celdaObjeto == null) return;

            if (MessageBox.Show($"¿Está seguro de que desea eliminar el retiro #{celdaObjeto.IdRetiro} y todas sus líneas de detalle?",
                "Eliminar Retiro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _apiClient.DeleteAsync(celdaObjeto.IdRetiro);
                    if (ok)
                    {
                        MessageBox.Show("Retiro eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el retiro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
