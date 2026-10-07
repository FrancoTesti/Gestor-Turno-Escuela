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
    public partial class ReporteRetirosForm : Form
    {
        private readonly ReporteApiClient _apiClient = new ReporteApiClient();
        private List<RetiroDTO> _retiros = new();

        public ReporteRetirosForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTotal.ForeColor = Color.FromArgb(33, 37, 41);
            lblDetalle.ForeColor = Color.FromArgb(33, 37, 41);

            btnConsultar.BackColor = Color.FromArgb(13, 110, 253);
            btnConsultar.ForeColor = Color.White;
            btnConsultar.FlatStyle = FlatStyle.Flat;
            btnConsultar.FlatAppearance.BorderSize = 0;

            Tema.Grilla(dgvRetiros);
            Tema.Grilla(dgvDetalle);
        }

        private async void ReporteRetirosForm_Load(object sender, EventArgs e)
        {
            // Por defecto se muestra el mes en curso.
            dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpHasta.Value = DateTime.Today;

            await Consultar();
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            await Consultar();
        }

        private async Task Consultar()
        {
            try
            {
                _retiros = await _apiClient.GetRetirosAsync(dtpDesde.Value.Date, dtpHasta.Value.Date);

                dgvRetiros.DataSource = _retiros.Select(retiro => new
                {
                    Fecha = retiro.FechaHora.ToString("dd/MM/yyyy HH:mm"),
                    Tutor = retiro.TutorNombreCompleto ?? string.Empty,
                    Entrego = retiro.PersonalNombre ?? string.Empty,
                    Alumnos = retiro.Detalles.Count,
                    Observaciones = retiro.Observaciones
                }).ToList();

                lblTotal.Text = $"Retiros: {_retiros.Count}    " +
                                $"Alumnos retirados: {_retiros.Sum(r => r.Detalles.Count)}";

                dgvDetalle.DataSource = null;

                if (_retiros.Count == 0)
                {
                    MessageBox.Show("No hay retiros registrados en el período elegido.", "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRetiros_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRetiros.SelectedRows.Count == 0)
            {
                dgvDetalle.DataSource = null;
                return;
            }

            int indice = dgvRetiros.SelectedRows[0].Index;
            if (indice < 0 || indice >= _retiros.Count)
            {
                dgvDetalle.DataSource = null;
                return;
            }

            dgvDetalle.DataSource = _retiros[indice].Detalles.Select(detalle => new
            {
                Alumno = detalle.AlumnoNombreCompleto ?? $"Alumno {detalle.IdAlumno}",
                HoraSalida = detalle.HoraSalida.ToString(@"hh\:mm"),
                Estado = detalle.Estado
            }).ToList();
        }
    }
}
