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
    public partial class HorariosMisAlumnosForm : Form
    {
        private readonly TutorApiClient _apiClient = new();
        private bool _cargando;

        private Label lblTitle = null!;
        private Label lblSub = null!;
        private Button btnActualizar = null!;
        private DataGridView dgvHorarios = null!;

        public HorariosMisAlumnosForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSub = new Label();
            btnActualizar = new Button();
            dgvHorarios = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvHorarios).BeginInit();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Horarios de Mis Alumnos";

            // lblSub
            lblSub.AutoSize = true;
            lblSub.Location = new Point(23, 62);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(580, 20);
            lblSub.TabIndex = 1;
            lblSub.Text = "Horarios de salida de lunes a viernes para cada uno de los alumnos que te corresponden.";

            // btnActualizar
            btnActualizar.Location = new Point(650, 26);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, Tema.AltoDeBoton);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += async (s, e) => await CargarAsync(true);

            // dgvHorarios
            dgvHorarios.AllowUserToAddRows = false;
            dgvHorarios.AllowUserToDeleteRows = false;
            dgvHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHorarios.Location = new Point(20, 100);
            dgvHorarios.Name = "dgvHorarios";
            dgvHorarios.ReadOnly = true;
            dgvHorarios.RowHeadersWidth = 51;
            dgvHorarios.Size = new Size(740, 410);
            dgvHorarios.TabIndex = 3;

            // Form
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 530);
            Controls.Add(dgvHorarios);
            Controls.Add(btnActualizar);
            Controls.Add(lblSub);
            Controls.Add(lblTitle);
            Name = "HorariosMisAlumnosForm";
            Text = "Horarios de Mis Alumnos";
            Load += async (s, e) =>
            {
                AcomodarBotonActualizar();
                await CargarAsync(true);
            };

            ((System.ComponentModel.ISupportInitialize)dgvHorarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void AcomodarBotonActualizar()
        {
            const int margenDerecho = 20;
            const int y = 26;
            btnActualizar.Anchor = AnchorStyles.None;
            btnActualizar.Size = new Size(110, Tema.AltoDeBoton);
            btnActualizar.Location = new Point(ClientSize.Width - margenDerecho - btnActualizar.Width, y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AcomodarBotonActualizar();
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.BotonSecundario(btnActualizar);
            Tema.Grilla(dgvHorarios);
        }

        private async Task CargarAsync(bool mostrarError)
        {
            if (_cargando) return;
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
                    MessageBox.Show($"Error al cargar los horarios de tus alumnos: {ex.Message}", "Error",
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
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToList();

            dgvHorarios.DataSource = null;
            dgvHorarios.DataSource = ordenados
                .Select(a => new
                {
                    Alumno = $"{a.Apellido}, {a.Nombre}",
                    Curso = $"{a.Grado} {a.Curso}",
                    a.Turno,
                    Hoy = a.HorarioSalida,
                    Lunes = a.HorarioLunes,
                    Martes = a.HorarioMartes,
                    Miércoles = a.HorarioMiercoles,
                    Jueves = a.HorarioJueves,
                    Viernes = a.HorarioViernes
                })
                .ToList();

            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            if (dgvHorarios.Columns.Count == 0)
                return;

            dgvHorarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvHorarios.Columns.Contains("Alumno"))
            {
                dgvHorarios.Columns["Alumno"].HeaderText = "Alumno";
                dgvHorarios.Columns["Alumno"].MinimumWidth = 140;
                dgvHorarios.Columns["Alumno"].FillWeight = 190;
            }

            if (dgvHorarios.Columns.Contains("Curso"))
            {
                dgvHorarios.Columns["Curso"].HeaderText = "Curso";
                dgvHorarios.Columns["Curso"].MinimumWidth = 70;
                dgvHorarios.Columns["Curso"].FillWeight = 85;
            }

            if (dgvHorarios.Columns.Contains("Turno"))
            {
                dgvHorarios.Columns["Turno"].HeaderText = "Turno";
                dgvHorarios.Columns["Turno"].MinimumWidth = 70;
                dgvHorarios.Columns["Turno"].FillWeight = 85;
            }

            string[] columnasHorarios = { "Hoy", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };
            const int fillWeightDia = 100;
            const int minWidthDia = 65;

            foreach (var col in columnasHorarios)
            {
                if (dgvHorarios.Columns.Contains(col))
                {
                    dgvHorarios.Columns[col].MinimumWidth = minWidthDia;
                    dgvHorarios.Columns[col].FillWeight = fillWeightDia;
                    dgvHorarios.Columns[col].DefaultCellStyle.Format = @"hh\:mm";
                    dgvHorarios.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvHorarios.Columns[col].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            if (dgvHorarios.Columns.Contains("Hoy"))
            {
                dgvHorarios.Columns["Hoy"].HeaderText = "Hoy *";
                dgvHorarios.Columns["Hoy"].ToolTipText = "Horario correspondiente al día de hoy";
                dgvHorarios.Columns["Hoy"].DefaultCellStyle.BackColor = Color.FromArgb(231, 241, 255);
                dgvHorarios.Columns["Hoy"].DefaultCellStyle.Font = new Font(dgvHorarios.Font, FontStyle.Bold);
            }
        }
    }
}
