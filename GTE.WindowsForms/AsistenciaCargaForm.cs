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
    public class AsistenciaCargaForm : Form
    {
        private readonly IAsistenciaApiClient _asistenciaClient = new AsistenciaApiClient();
        private readonly CursoEscolarApiClient _cursoClient = new CursoEscolarApiClient();

        private List<CursoEscolarDTO> _cursos = new();
        private PlanillaDiariaDTO? _planilla;

        // UI Controls
        private Panel pnlHeader = null!;
        private Label lblTitle = null!;
        private Label lblSub = null!;
        private Panel pnlFiltros = null!;
        private Label lblCurso = null!;
        private ComboBox cmbCursos = null!;
        private Label lblFecha = null!;
        private DateTimePicker dtpFecha = null!;
        private Button btnCargar = null!;
        private Button btnSugerir = null!;
        private Button btnGuardar = null!;
        private Button btnModificar = null!;
        private Panel pnlInfo = null!;
        private Label lblPlanillaInfo = null!;
        private Label lblEstadoBadge = null!;
        private DataGridView dgvAlumnos = null!;

        public AsistenciaCargaForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSub = new Label();
            this.pnlFiltros = new Panel();
            this.lblCurso = new Label();
            this.cmbCursos = new ComboBox();
            this.lblFecha = new Label();
            this.dtpFecha = new DateTimePicker();
            this.btnCargar = new Button();
            this.btnSugerir = new Button();
            this.btnGuardar = new Button();
            this.btnModificar = new Button();
            this.pnlInfo = new Panel();
            this.lblPlanillaInfo = new Label();
            this.lblEstadoBadge = new Label();
            this.dgvAlumnos = new DataGridView();

            this.SuspendLayout();

            // Form
            this.Text = "Carga Diaria de Asistencias";
            this.ClientSize = new Size(950, 580);
            this.Load += AsistenciaCargaForm_Load;

            // pnlHeader
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 65;
            this.pnlHeader.Padding = new Padding(15, 10, 15, 0);

            this.lblTitle.Text = "Carga Diaria de Asistencias";
            this.lblTitle.Location = new Point(15, 8);
            this.lblTitle.Size = new Size(500, 28);

            this.lblSub.Text = "Seleccioná el curso y la fecha para registrar las inasistencias de la jornada.";
            this.lblSub.Location = new Point(15, 36);
            this.lblSub.Size = new Size(600, 20);

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSub);

            // pnlFiltros
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 50;
            this.pnlFiltros.Padding = new Padding(15, 8, 15, 8);

            this.lblCurso.Text = "Curso:";
            this.lblCurso.Location = new Point(15, 14);
            this.lblCurso.AutoSize = true;

            this.cmbCursos.Location = new Point(65, 10);
            this.cmbCursos.Size = new Size(200, 30);
            this.cmbCursos.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblFecha.Text = "Fecha:";
            this.lblFecha.Location = new Point(275, 14);
            this.lblFecha.AutoSize = true;

            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Location = new Point(325, 10);
            this.dtpFecha.Size = new Size(115, 30);
            this.dtpFecha.Value = DateTime.Today;

            this.btnCargar.Text = "Cargar Planilla";
            this.btnCargar.Location = new Point(450, 9);
            this.btnCargar.Size = new Size(115, 32);
            this.btnCargar.Click += async (s, e) => await CargarPlanillaAsync();

            this.btnSugerir.Text = "Sugerir Retiros";
            this.btnSugerir.Location = new Point(575, 9);
            this.btnSugerir.Size = new Size(125, 32);
            this.btnSugerir.Click += async (s, e) => await SugerirRetirosAsync();

            this.btnModificar.Text = "Modificar Alumno";
            this.btnModificar.Location = new Point(710, 9);
            this.btnModificar.Size = new Size(130, 32);
            this.btnModificar.Click += BtnModificar_Click;

            this.btnGuardar.Text = "Guardar y Confirmar";
            this.btnGuardar.Location = new Point(850, 9);
            this.btnGuardar.Size = new Size(150, 32);
            this.btnGuardar.Click += async (s, e) => await GuardarPlanillaAsync();

            this.pnlFiltros.Controls.Add(this.lblCurso);
            this.pnlFiltros.Controls.Add(this.cmbCursos);
            this.pnlFiltros.Controls.Add(this.lblFecha);
            this.pnlFiltros.Controls.Add(this.dtpFecha);
            this.pnlFiltros.Controls.Add(this.btnCargar);
            this.pnlFiltros.Controls.Add(this.btnSugerir);
            this.pnlFiltros.Controls.Add(this.btnModificar);
            this.pnlFiltros.Controls.Add(this.btnGuardar);

            // pnlInfo
            this.pnlInfo.Dock = DockStyle.Top;
            this.pnlInfo.Height = 35;
            this.pnlInfo.Padding = new Padding(15, 5, 15, 5);
            this.pnlInfo.BackColor = Color.FromArgb(238, 242, 246);

            this.lblPlanillaInfo.Text = "Planilla no cargada.";
            this.lblPlanillaInfo.Location = new Point(15, 8);
            this.lblPlanillaInfo.AutoSize = true;
            this.lblPlanillaInfo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.lblEstadoBadge.Text = "";
            this.lblEstadoBadge.Location = new Point(600, 8);
            this.lblEstadoBadge.AutoSize = true;
            this.lblEstadoBadge.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            this.pnlInfo.Controls.Add(this.lblPlanillaInfo);
            this.pnlInfo.Controls.Add(this.lblEstadoBadge);

            // dgvAlumnos
            this.dgvAlumnos.Dock = DockStyle.Fill;
            this.dgvAlumnos.CellDoubleClick += (s, e) => BtnModificar_Click(s, e);

            this.Controls.Add(this.dgvAlumnos);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.BotonPrimario(btnCargar);
            Tema.BotonSecundario(btnSugerir);
            Tema.BotonSecundario(btnModificar);
            Tema.BotonExito(btnGuardar);
            Tema.Grilla(dgvAlumnos);
        }

        private async void AsistenciaCargaForm_Load(object? sender, EventArgs e)
        {
            try
            {
                _cursos = await _cursoClient.GetAllAsync();
                cmbCursos.DisplayMember = "Nombre";
                cmbCursos.ValueMember = "IdCurso";
                cmbCursos.DataSource = _cursos.Select(c => new
                {
                    c.IdCurso,
                    Nombre = $"{c.Grado} {c.Curso} ({c.Turno})"
                }).ToList();

                if (_cursos.Count > 0)
                {
                    await CargarPlanillaAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarPlanillaAsync()
        {
            if (cmbCursos.SelectedValue is not int idCurso || idCurso <= 0)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _planilla = await _asistenciaClient.GetPlanillaDiariaAsync(idCurso, dtpFecha.Value.Date);

                if (_planilla != null)
                {
                    lblPlanillaInfo.Text = $"Curso: {_planilla.CursoNombre} ({_planilla.Turno}) | Fecha: {_planilla.Fecha:dd/MM/yyyy} | Alumnos: {_planilla.Alumnos.Count}";

                    if (_planilla.Confirmada)
                    {
                        lblEstadoBadge.Text = $"CONFIRMADA ({_planilla.FechaHoraRegistro:dd/MM/yyyy HH:mm})";
                        lblEstadoBadge.ForeColor = Color.FromArgb(25, 135, 84);
                    }
                    else
                    {
                        lblEstadoBadge.Text = "PENDIENTE DE CONFIRMACIÓN";
                        lblEstadoBadge.ForeColor = Color.FromArgb(220, 53, 69);
                    }

                    MostrarAlumnosEnGrilla();
                }
                else
                {
                    lblPlanillaInfo.Text = "No se pudo obtener la planilla para la fecha seleccionada.";
                    lblEstadoBadge.Text = "";
                    dgvAlumnos.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la planilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MostrarAlumnosEnGrilla()
        {
            if (_planilla == null) return;

            var vista = _planilla.Alumnos.Select(a => new
            {
                a.IdAlumno,
                Alumno = $"{a.Apellido}, {a.Nombre}",
                Estado = a.IdTipoInasistencia.HasValue && a.IdTipoInasistencia.Value > 0 ? "Inasistencia" : "Presente",
                Tipo = a.IdTipoInasistencia.HasValue && a.IdTipoInasistencia.Value > 0
                    ? (_planilla.TiposDisponibles.FirstOrDefault(t => t.IdTipoInasistencia == a.IdTipoInasistencia.Value)?.Nombre ?? "Falta")
                    : "-",
                Codigo = a.CodigoOficial,
                Justificada = a.IdTipoInasistencia.HasValue && a.IdTipoInasistencia.Value > 0 ? (a.Justificada ? "Sí" : "No") : "-",
                a.Motivo,
                NoComputa = a.NoComputa ? "Sí" : "-",
                Retiro = a.TieneRetiroRegistrado ? (a.DetalleRetiroInfo ?? "Retiro anticipado") : "-",
                a.Observaciones,
                AlumnoItem = a
            }).ToList();

            dgvAlumnos.DataSource = null;
            dgvAlumnos.DataSource = vista;

            ConfigurarColumnas();
            PintarFilasSegunEstado();
        }

        private void ConfigurarColumnas()
        {
            if (dgvAlumnos.Columns.Count == 0) return;

            if (dgvAlumnos.Columns.Contains("IdAlumno"))
                dgvAlumnos.Columns["IdAlumno"].Visible = false;

            if (dgvAlumnos.Columns.Contains("AlumnoItem"))
                dgvAlumnos.Columns["AlumnoItem"].Visible = false;

            if (dgvAlumnos.Columns.Contains("Alumno"))
            {
                dgvAlumnos.Columns["Alumno"].HeaderText = "Alumno";
                dgvAlumnos.Columns["Alumno"].Width = 200;
            }

            if (dgvAlumnos.Columns.Contains("Estado"))
            {
                dgvAlumnos.Columns["Estado"].HeaderText = "Estado";
                dgvAlumnos.Columns["Estado"].Width = 100;
            }

            if (dgvAlumnos.Columns.Contains("Tipo"))
            {
                dgvAlumnos.Columns["Tipo"].HeaderText = "Tipo Inasistencia";
                dgvAlumnos.Columns["Tipo"].Width = 140;
            }

            if (dgvAlumnos.Columns.Contains("Codigo"))
            {
                dgvAlumnos.Columns["Codigo"].HeaderText = "Cód.";
                dgvAlumnos.Columns["Codigo"].Width = 60;
                dgvAlumnos.Columns["Codigo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvAlumnos.Columns.Contains("Justificada"))
            {
                dgvAlumnos.Columns["Justificada"].HeaderText = "Justif.";
                dgvAlumnos.Columns["Justificada"].Width = 70;
                dgvAlumnos.Columns["Justificada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvAlumnos.Columns.Contains("Motivo"))
            {
                dgvAlumnos.Columns["Motivo"].HeaderText = "Motivo";
                dgvAlumnos.Columns["Motivo"].Width = 140;
            }

            if (dgvAlumnos.Columns.Contains("NoComputa"))
            {
                dgvAlumnos.Columns["NoComputa"].HeaderText = "No Computa";
                dgvAlumnos.Columns["NoComputa"].Width = 90;
                dgvAlumnos.Columns["NoComputa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvAlumnos.Columns.Contains("Retiro"))
            {
                dgvAlumnos.Columns["Retiro"].HeaderText = "Retiro Asociado";
                dgvAlumnos.Columns["Retiro"].Width = 150;
            }

            if (dgvAlumnos.Columns.Contains("Observaciones"))
            {
                dgvAlumnos.Columns["Observaciones"].HeaderText = "Observaciones";
                dgvAlumnos.Columns["Observaciones"].Width = 120;
            }
        }

        private void PintarFilasSegunEstado()
        {
            foreach (DataGridViewRow row in dgvAlumnos.Rows)
            {
                string estado = row.Cells["Estado"].Value?.ToString() ?? "";
                if (estado == "Inasistencia")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 205, 210);
                }
            }
        }

        private void BtnModificar_Click(object? sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count == 0 || _planilla == null)
            {
                MessageBox.Show("Por favor, seleccione un alumno de la grilla.", "Seleccionar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var celdaObjeto = dgvAlumnos.SelectedRows[0].Cells["AlumnoItem"].Value as AlumnoAsistenciaItemDTO;
            if (celdaObjeto == null) return;

            using var dialog = new AsistenciaRegistroDialog(celdaObjeto, _planilla.TiposDisponibles);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                MostrarAlumnosEnGrilla();
            }
        }

        private async Task SugerirRetirosAsync()
        {
            if (cmbCursos.SelectedValue is not int idCurso || idCurso <= 0 || _planilla == null)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                var sugerencias = await _asistenciaClient.SugerirInasistenciasDesdeRetirosAsync(idCurso, dtpFecha.Value.Date);
                int count = 0;

                foreach (var sug in sugerencias)
                {
                    var alumno = _planilla.Alumnos.FirstOrDefault(a => a.IdAlumno == sug.IdAlumno);
                    if (alumno != null)
                    {
                        alumno.EstadoGeneral = sug.EstadoGeneral;
                        alumno.IdTipoInasistencia = sug.IdTipoInasistencia;
                        alumno.CodigoOficial = sug.CodigoOficial;
                        alumno.Justificada = sug.Justificada;
                        alumno.Motivo = sug.Motivo;
                        alumno.NoComputa = sug.NoComputa;
                        alumno.Observaciones = sug.Observaciones;
                        alumno.TieneRetiroRegistrado = sug.TieneRetiroRegistrado;
                        alumno.DetalleRetiroInfo = sug.DetalleRetiroInfo;
                        alumno.IdRetiroOrigen = sug.IdRetiroOrigen;
                        count++;
                    }
                }

                MostrarAlumnosEnGrilla();

                if (count > 0)
                {
                    MessageBox.Show($"Se aplicaron {count} sugerencia(s) de inasistencias por retiros anticipados del día.", "Sugerencias Aplicadas",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se detectaron retiros anticipados que requieran sugerencia de inasistencia para esta fecha.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al sugerir retiros: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task GuardarPlanillaAsync()
        {
            if (cmbCursos.SelectedValue is not int idCurso || idCurso <= 0 || _planilla == null)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                var request = new GuardarPlanillaAsistenciaRequestDTO
                {
                    IdCurso = idCurso,
                    Fecha = dtpFecha.Value.Date,
                    Inasistencias = _planilla.Alumnos
                        .Where(a => a.IdTipoInasistencia.HasValue && a.IdTipoInasistencia.Value > 0)
                        .Select(a => new InasistenciaInputDTO
                        {
                            IdAlumno = a.IdAlumno,
                            IdTipoInasistencia = a.IdTipoInasistencia!.Value,
                            Justificada = a.Justificada,
                            Motivo = a.Motivo ?? string.Empty,
                            NoComputa = a.NoComputa,
                            Observaciones = a.Observaciones ?? string.Empty,
                            IdRetiroOrigen = a.IdRetiroOrigen
                        }).ToList()
                };

                bool ok = await _asistenciaClient.GuardarPlanillaDiariaAsync(request);
                if (ok)
                {
                    MessageBox.Show("Planilla de asistencia guardada y confirmada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarPlanillaAsync();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar la planilla de asistencia.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la planilla: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
