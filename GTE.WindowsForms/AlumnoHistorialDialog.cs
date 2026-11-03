using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public class AlumnoHistorialDialog : Form
    {
        private readonly int _idAlumno;
        private readonly string _alumnoNombre;
        private readonly IAsistenciaApiClient _asistenciaClient = new AsistenciaApiClient();

        private Label lblTitle = null!;
        private Label lblAlumno = null!;
        private Panel pnlStats = null!;
        private Label lblComputables = null!;
        private Label lblJustificadas = null!;
        private Label lblInjustificadas = null!;
        private DataGridView dgvHistorial = null!;
        private Button btnCerrar = null!;

        public AlumnoHistorialDialog(int idAlumno, string alumnoNombre)
        {
            _idAlumno = idAlumno;
            _alumnoNombre = alumnoNombre;

            InitializeComponent();
            ApplyStyles();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblAlumno = new Label();
            this.pnlStats = new Panel();
            this.lblComputables = new Label();
            this.lblJustificadas = new Label();
            this.lblInjustificadas = new Label();
            this.dgvHistorial = new DataGridView();
            this.btnCerrar = new Button();

            this.SuspendLayout();

            // Form
            this.Text = "Historial de Asistencias del Alumno";
            this.ClientSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += AlumnoHistorialDialog_Load;

            // Title
            this.lblTitle.Text = "Historial y Presentismo del Alumno";
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Size = new Size(600, 28);

            // Alumno
            this.lblAlumno.Text = $"Alumno: {_alumnoNombre}";
            this.lblAlumno.Location = new Point(20, 45);
            this.lblAlumno.Size = new Size(600, 22);
            this.lblAlumno.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);

            // pnlStats
            this.pnlStats.Location = new Point(20, 75);
            this.pnlStats.Size = new Size(760, 60);
            this.pnlStats.BackColor = Color.FromArgb(238, 242, 246);
            this.pnlStats.BorderStyle = BorderStyle.FixedSingle;

            this.lblComputables.Location = new Point(15, 12);
            this.lblComputables.Size = new Size(230, 35);
            this.lblComputables.Font = new Font("Segoe UI", 9.5F);
            this.lblComputables.Text = "Faltas Computables: -";

            this.lblJustificadas.Location = new Point(260, 12);
            this.lblJustificadas.Size = new Size(230, 35);
            this.lblJustificadas.Font = new Font("Segoe UI", 9.5F);
            this.lblJustificadas.Text = "Faltas Justificadas: -";

            this.lblInjustificadas.Location = new Point(505, 12);
            this.lblInjustificadas.Size = new Size(230, 35);
            this.lblInjustificadas.Font = new Font("Segoe UI", 9.5F);
            this.lblInjustificadas.Text = "Faltas Injustificadas: -";

            this.pnlStats.Controls.Add(this.lblComputables);
            this.pnlStats.Controls.Add(this.lblJustificadas);
            this.pnlStats.Controls.Add(this.lblInjustificadas);

            // dgvHistorial
            this.dgvHistorial.Location = new Point(20, 150);
            this.dgvHistorial.Size = new Size(760, 285);

            // btnCerrar
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Location = new Point(660, 450);
            this.btnCerrar.Size = new Size(120, 35);
            this.btnCerrar.Click += (s, e) => this.Close();

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAlumno);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.btnCerrar);

            this.ResumeLayout(false);
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Grilla(dgvHistorial);
            Tema.BotonSecundario(btnCerrar);
        }

        private async void AlumnoHistorialDialog_Load(object? sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var historial = await _asistenciaClient.GetHistorialAlumnoAsync(_idAlumno);

                if (historial != null)
                {
                    lblComputables.Text = $"Faltas Computables:\n{historial.TotalFaltasComputables:0.##}";
                    lblComputables.ForeColor = Color.FromArgb(220, 53, 69);
                    lblComputables.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                    lblJustificadas.Text = $"Faltas Justificadas:\n{historial.TotalFaltasJustificadas:0.##}";
                    lblJustificadas.ForeColor = Color.FromArgb(13, 110, 253);
                    lblJustificadas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                    lblInjustificadas.Text = $"Faltas Injustificadas:\n{historial.TotalFaltasInjustificadas:0.##}";
                    lblInjustificadas.ForeColor = Color.FromArgb(108, 117, 125);
                    lblInjustificadas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                    dgvHistorial.DataSource = historial.Inasistencias.Select(i => new
                    {
                        Fecha = i.Fecha.ToString("dd/MM/yyyy"),
                        Tipo = i.NombreTipoInasistencia,
                        Codigo = i.CodigoOficial,
                        Valor = i.ValorDecimal.ToString("0.##"),
                        Justificada = i.Justificada ? "Sí" : "No",
                        i.Motivo,
                        NoComputa = i.NoComputa ? "Sí" : "-",
                        i.Observaciones
                    }).ToList();

                    ConfigurarColumnas();
                }
                else
                {
                    MessageBox.Show("No se encontró información de asistencia para este alumno.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar historial: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvHistorial.Columns.Count == 0) return;

            if (dgvHistorial.Columns.Contains("Fecha")) dgvHistorial.Columns["Fecha"].Width = 95;
            if (dgvHistorial.Columns.Contains("Tipo")) dgvHistorial.Columns["Tipo"].Width = 140;
            if (dgvHistorial.Columns.Contains("Codigo"))
            {
                dgvHistorial.Columns["Codigo"].HeaderText = "Cód.";
                dgvHistorial.Columns["Codigo"].Width = 60;
                dgvHistorial.Columns["Codigo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorial.Columns.Contains("Valor"))
            {
                dgvHistorial.Columns["Valor"].Width = 65;
                dgvHistorial.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorial.Columns.Contains("Justificada"))
            {
                dgvHistorial.Columns["Justificada"].Width = 75;
                dgvHistorial.Columns["Justificada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorial.Columns.Contains("NoComputa"))
            {
                dgvHistorial.Columns["NoComputa"].HeaderText = "No Computa";
                dgvHistorial.Columns["NoComputa"].Width = 90;
                dgvHistorial.Columns["NoComputa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvHistorial.Columns.Contains("Motivo")) dgvHistorial.Columns["Motivo"].Width = 130;
            if (dgvHistorial.Columns.Contains("Observaciones")) dgvHistorial.Columns["Observaciones"].Width = 130;
        }
    }
}
