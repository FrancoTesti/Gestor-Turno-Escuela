using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public class ReportesMiEscuelaForm : Form
    {
        private readonly IAsistenciaApiClient _asistenciaClient = new AsistenciaApiClient();
        private readonly CursoEscolarApiClient _cursoClient = new CursoEscolarApiClient();

        private List<CursoEscolarDTO> _cursos = new();
        private List<ReporteNovedadItemDTO> _novedades = new();
        private ReporteEspejoSemanaDTO? _espejo;

        // UI Controls
        private Panel pnlHeader = null!;
        private Label lblTitle = null!;
        private Label lblSub = null!;
        private Button btnExportar = null!;
        private Button btnImprimir = null!;
        private TabControl tabControl = null!;
        private TabPage tabNovedades = null!;
        private TabPage tabEspejo = null!;

        // Tab Novedades
        private Panel pnlFiltrosNovedades = null!;
        private Label lblCursoNov = null!;
        private ComboBox cmbCursosNov = null!;
        private Label lblDesde = null!;
        private DateTimePicker dtpDesde = null!;
        private Label lblHasta = null!;
        private DateTimePicker dtpHasta = null!;
        private Button btnConsultarNov = null!;
        private Label lblResumenNov = null!;
        private DataGridView dgvNovedades = null!;

        // Tab Espejo
        private Panel pnlFiltrosEspejo = null!;
        private Label lblCursoEsp = null!;
        private ComboBox cmbCursosEsp = null!;
        private Label lblSemana = null!;
        private DateTimePicker dtpSemana = null!;
        private Button btnConsultarEsp = null!;
        private Label lblResumenEsp = null!;
        private DataGridView dgvEspejo = null!;

        // Printing support
        private PrintDocument printDoc = null!;

        public ReportesMiEscuelaForm()
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
            this.btnExportar = new Button();
            this.btnImprimir = new Button();
            this.tabControl = new TabControl();
            this.tabNovedades = new TabPage();
            this.tabEspejo = new TabPage();

            // Tab Novedades
            this.pnlFiltrosNovedades = new Panel();
            this.lblCursoNov = new Label();
            this.cmbCursosNov = new ComboBox();
            this.lblDesde = new Label();
            this.dtpDesde = new DateTimePicker();
            this.lblHasta = new Label();
            this.dtpHasta = new DateTimePicker();
            this.btnConsultarNov = new Button();
            this.lblResumenNov = new Label();
            this.dgvNovedades = new DataGridView();

            // Tab Espejo
            this.pnlFiltrosEspejo = new Panel();
            this.lblCursoEsp = new Label();
            this.cmbCursosEsp = new ComboBox();
            this.lblSemana = new Label();
            this.dtpSemana = new DateTimePicker();
            this.btnConsultarEsp = new Button();
            this.lblResumenEsp = new Label();
            this.dgvEspejo = new DataGridView();

            this.printDoc = new PrintDocument();
            this.printDoc.PrintPage += PrintDoc_PrintPage;

            this.SuspendLayout();

            // Form
            this.Text = "Reportes Oficiales Mi ESCUELA (Santa Fe)";
            this.ClientSize = new Size(1000, 600);
            this.Load += ReportesMiEscuelaForm_Load;

            // pnlHeader
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 65;
            this.pnlHeader.Padding = new Padding(15, 10, 15, 0);

            this.lblTitle.Text = "Reportes Oficiales Mi ESCUELA";
            this.lblTitle.Location = new Point(15, 8);
            this.lblTitle.Size = new Size(500, 28);

            this.lblSub.Text = "Generación de planillas de excepciones y espejo semanal para carga ministerial.";
            this.lblSub.Location = new Point(15, 36);
            this.lblSub.Size = new Size(600, 20);

            this.btnExportar.Text = "Exportar a CSV";
            this.btnExportar.Location = new Point(690, 15);
            this.btnExportar.Size = new Size(135, 35);
            this.btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnExportar.Click += BtnExportar_Click;

            this.btnImprimir.Text = "Imprimir Reporte";
            this.btnImprimir.Location = new Point(835, 15);
            this.btnImprimir.Size = new Size(140, 35);
            this.btnImprimir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnImprimir.Click += BtnImprimir_Click;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSub);
            this.pnlHeader.Controls.Add(this.btnExportar);
            this.pnlHeader.Controls.Add(this.btnImprimir);

            // TabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // TAB NOVEDADES
            this.tabNovedades.Text = "Novedades (Excepciones)";
            this.tabNovedades.Padding = new Padding(10);

            this.pnlFiltrosNovedades.Dock = DockStyle.Top;
            this.pnlFiltrosNovedades.Height = 70;

            this.lblCursoNov.Text = "Curso:";
            this.lblCursoNov.Location = new Point(10, 10);
            this.lblCursoNov.AutoSize = true;

            this.cmbCursosNov.Location = new Point(55, 6);
            this.cmbCursosNov.Size = new Size(200, 28);
            this.cmbCursosNov.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblDesde.Text = "Desde:";
            this.lblDesde.Location = new Point(265, 10);
            this.lblDesde.AutoSize = true;

            this.dtpDesde.Format = DateTimePickerFormat.Short;
            this.dtpDesde.Location = new Point(315, 6);
            this.dtpDesde.Size = new Size(115, 28);
            this.dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            this.lblHasta.Text = "Hasta:";
            this.lblHasta.Location = new Point(440, 10);
            this.lblHasta.AutoSize = true;

            this.dtpHasta.Format = DateTimePickerFormat.Short;
            this.dtpHasta.Location = new Point(490, 6);
            this.dtpHasta.Size = new Size(115, 28);
            this.dtpHasta.Value = DateTime.Today;

            this.btnConsultarNov.Text = "Consultar";
            this.btnConsultarNov.Location = new Point(620, 5);
            this.btnConsultarNov.Size = new Size(110, 32);
            this.btnConsultarNov.Click += async (s, e) => await ConsultarNovedadesAsync();

            this.lblResumenNov.Text = "Seleccioná un curso y período para consultar.";
            this.lblResumenNov.Location = new Point(10, 42);
            this.lblResumenNov.AutoSize = true;
            this.lblResumenNov.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.pnlFiltrosNovedades.Controls.Add(this.lblCursoNov);
            this.pnlFiltrosNovedades.Controls.Add(this.cmbCursosNov);
            this.pnlFiltrosNovedades.Controls.Add(this.lblDesde);
            this.pnlFiltrosNovedades.Controls.Add(this.dtpDesde);
            this.pnlFiltrosNovedades.Controls.Add(this.lblHasta);
            this.pnlFiltrosNovedades.Controls.Add(this.dtpHasta);
            this.pnlFiltrosNovedades.Controls.Add(this.btnConsultarNov);
            this.pnlFiltrosNovedades.Controls.Add(this.lblResumenNov);

            this.dgvNovedades.Dock = DockStyle.Fill;
            this.tabNovedades.Controls.Add(this.dgvNovedades);
            this.tabNovedades.Controls.Add(this.pnlFiltrosNovedades);

            // TAB ESPEJO
            this.tabEspejo.Text = "Espejo Semanal (Mi ESCUELA)";
            this.tabEspejo.Padding = new Padding(10);

            this.pnlFiltrosEspejo.Dock = DockStyle.Top;
            this.pnlFiltrosEspejo.Height = 70;

            this.lblCursoEsp.Text = "Curso:";
            this.lblCursoEsp.Location = new Point(10, 10);
            this.lblCursoEsp.AutoSize = true;

            this.cmbCursosEsp.Location = new Point(55, 6);
            this.cmbCursosEsp.Size = new Size(200, 28);
            this.cmbCursosEsp.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblSemana.Text = "Semana de referencia:";
            this.lblSemana.Location = new Point(270, 10);
            this.lblSemana.AutoSize = true;

            this.dtpSemana.Format = DateTimePickerFormat.Short;
            this.dtpSemana.Location = new Point(415, 6);
            this.dtpSemana.Size = new Size(120, 28);
            this.dtpSemana.Value = DateTime.Today;

            this.btnConsultarEsp.Text = "Consultar";
            this.btnConsultarEsp.Location = new Point(550, 5);
            this.btnConsultarEsp.Size = new Size(110, 32);
            this.btnConsultarEsp.Click += async (s, e) => await ConsultarEspejoAsync();

            this.lblResumenEsp.Text = "Seleccioná un curso y fecha para armar la grilla ministerial semanal.";
            this.lblResumenEsp.Location = new Point(10, 42);
            this.lblResumenEsp.AutoSize = true;
            this.lblResumenEsp.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.pnlFiltrosEspejo.Controls.Add(this.lblCursoEsp);
            this.pnlFiltrosEspejo.Controls.Add(this.cmbCursosEsp);
            this.pnlFiltrosEspejo.Controls.Add(this.lblSemana);
            this.pnlFiltrosEspejo.Controls.Add(this.dtpSemana);
            this.pnlFiltrosEspejo.Controls.Add(this.btnConsultarEsp);
            this.pnlFiltrosEspejo.Controls.Add(this.lblResumenEsp);

            this.dgvEspejo.Dock = DockStyle.Fill;
            this.tabEspejo.Controls.Add(this.dgvEspejo);
            this.tabEspejo.Controls.Add(this.pnlFiltrosEspejo);

            this.tabControl.TabPages.Add(this.tabNovedades);
            this.tabControl.TabPages.Add(this.tabEspejo);

            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.BotonPrimario(btnExportar);
            Tema.BotonSecundario(btnImprimir);
            Tema.BotonPrimario(btnConsultarNov);
            Tema.BotonPrimario(btnConsultarEsp);
            Tema.Grilla(dgvNovedades);
            Tema.Grilla(dgvEspejo);
        }

        private async void ReportesMiEscuelaForm_Load(object? sender, EventArgs e)
        {
            try
            {
                _cursos = await _cursoClient.GetAllAsync();

                var listaCursos = _cursos.Select(c => new
                {
                    c.IdCurso,
                    Nombre = $"{c.Grado} {c.Curso} ({c.Turno})"
                }).ToList();

                cmbCursosNov.DisplayMember = "Nombre";
                cmbCursosNov.ValueMember = "IdCurso";
                cmbCursosNov.DataSource = listaCursos;

                cmbCursosEsp.DisplayMember = "Nombre";
                cmbCursosEsp.ValueMember = "IdCurso";
                cmbCursosEsp.DataSource = listaCursos.ToList();

                if (_cursos.Count > 0)
                {
                    await ConsultarNovedadesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConsultarNovedadesAsync()
        {
            if (cmbCursosNov.SelectedValue is not int idCurso || idCurso <= 0)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _novedades = await _asistenciaClient.GetReporteNovedadesAsync(idCurso, dtpDesde.Value.Date, dtpHasta.Value.Date);

                dgvNovedades.DataSource = _novedades.Select(n => new
                {
                    Fecha = n.Fecha.ToString("dd/MM/yyyy"),
                    Dia = n.DiaSemana,
                    Alumno = n.AlumnoNombreCompleto,
                    n.Curso,
                    n.Turno,
                    Tipo = n.TipoInasistencia,
                    Valor = n.ValorFalta.ToString("0.##"),
                    Justificada = n.Justificada ? "Sí" : "No",
                    n.Motivo,
                    NoComputa = n.NoComputa ? "Sí" : "-",
                    n.Observaciones
                }).ToList();

                lblResumenNov.Text = _novedades.Count == 0
                    ? "No se registraron excepciones en el período seleccionado."
                    : $"Total excepciones registradas: {_novedades.Count} en el período consultado.";

                ConfigurarColumnasNovedades();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar novedades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ConfigurarColumnasNovedades()
        {
            if (dgvNovedades.Columns.Count == 0) return;

            if (dgvNovedades.Columns.Contains("Fecha")) dgvNovedades.Columns["Fecha"].Width = 95;
            if (dgvNovedades.Columns.Contains("Dia")) dgvNovedades.Columns["Dia"].Width = 85;
            if (dgvNovedades.Columns.Contains("Alumno")) dgvNovedades.Columns["Alumno"].Width = 200;
            if (dgvNovedades.Columns.Contains("Curso")) dgvNovedades.Columns["Curso"].Width = 90;
            if (dgvNovedades.Columns.Contains("Turno")) dgvNovedades.Columns["Turno"].Width = 90;
            if (dgvNovedades.Columns.Contains("Tipo")) dgvNovedades.Columns["Tipo"].Width = 140;
            if (dgvNovedades.Columns.Contains("Valor"))
            {
                dgvNovedades.Columns["Valor"].Width = 65;
                dgvNovedades.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvNovedades.Columns.Contains("Justificada"))
            {
                dgvNovedades.Columns["Justificada"].Width = 75;
                dgvNovedades.Columns["Justificada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvNovedades.Columns.Contains("NoComputa"))
            {
                dgvNovedades.Columns["NoComputa"].Width = 90;
                dgvNovedades.Columns["NoComputa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private async Task ConsultarEspejoAsync()
        {
            if (cmbCursosEsp.SelectedValue is not int idCurso || idCurso <= 0)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _espejo = await _asistenciaClient.GetReporteEspejoSemanalAsync(idCurso, dtpSemana.Value.Date);

                if (_espejo != null)
                {
                    dgvEspejo.DataSource = _espejo.Filas.Select(f => new
                    {
                        f.Alumno,
                        f.Lunes,
                        f.Martes,
                        Miercoles = f.Miercoles,
                        f.Jueves,
                        f.Viernes,
                        TotalFaltas = f.TotalFaltasPeriodo.ToString("0.##")
                    }).ToList();

                    lblResumenEsp.Text = $"Curso: {_espejo.Curso} ({_espejo.Turno}) | Semana: {_espejo.FechaLunes:dd/MM/yyyy} al {_espejo.FechaViernes:dd/MM/yyyy} | Alumnos: {_espejo.Filas.Count}";

                    ConfigurarColumnasEspejo();
                }
                else
                {
                    dgvEspejo.DataSource = null;
                    lblResumenEsp.Text = "No se encontraron datos para armar la grilla semanal.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar espejo semanal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ConfigurarColumnasEspejo()
        {
            if (dgvEspejo.Columns.Count == 0) return;

            if (dgvEspejo.Columns.Contains("Alumno")) dgvEspejo.Columns["Alumno"].Width = 240;

            foreach (var col in new[] { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes" })
            {
                if (dgvEspejo.Columns.Contains(col))
                {
                    dgvEspejo.Columns[col].Width = 80;
                    dgvEspejo.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            if (dgvEspejo.Columns.Contains("TotalFaltas"))
            {
                dgvEspejo.Columns["TotalFaltas"].HeaderText = "Total Faltas";
                dgvEspejo.Columns["TotalFaltas"].Width = 110;
                dgvEspejo.Columns["TotalFaltas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvEspejo.Columns["TotalFaltas"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            }
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = "Archivos CSV (*.csv)|*.csv",
                    Title = "Exportar Reporte Mi ESCUELA",
                    FileName = tabControl.SelectedTab == tabNovedades
                        ? $"Reporte_Novedades_{DateTime.Today:yyyyMMdd}.csv"
                        : $"Reporte_Espejo_Semanal_{DateTime.Today:yyyyMMdd}.csv"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                var sb = new StringBuilder();

                if (tabControl.SelectedTab == tabNovedades)
                {
                    sb.AppendLine("Fecha;Dia;Alumno;Curso;Turno;Tipo Inasistencia;Valor Falta;Justificada;Motivo;No Computa;Observaciones");
                    foreach (var n in _novedades)
                    {
                        string justificada = n.Justificada ? "Sí" : "No";
                        string noComputa = n.NoComputa ? "Sí" : "No";
                        sb.AppendLine($"{n.Fecha:dd/MM/yyyy};{Sanitizar(n.DiaSemana)};{Sanitizar(n.AlumnoNombreCompleto)};{Sanitizar(n.Curso)};{Sanitizar(n.Turno)};{Sanitizar(n.TipoInasistencia)};{n.ValorFalta:0.##};{justificada};{Sanitizar(n.Motivo)};{noComputa};{Sanitizar(n.Observaciones)}");
                    }
                }
                else
                {
                    if (_espejo != null)
                    {
                        sb.AppendLine($"Curso: {_espejo.Curso};Turno: {_espejo.Turno};Lunes: {_espejo.FechaLunes:dd/MM/yyyy};Viernes: {_espejo.FechaViernes:dd/MM/yyyy}");
                        sb.AppendLine("Alumno;Lunes;Martes;Miercoles;Jueves;Viernes;Total Faltas Periodo");
                        foreach (var f in _espejo.Filas)
                        {
                            sb.AppendLine($"{Sanitizar(f.Alumno)};{f.Lunes};{f.Martes};{f.Miercoles};{f.Jueves};{f.Viernes};{f.TotalFaltasPeriodo:0.##}");
                        }
                    }
                }

                // Escribir con UTF-8 BOM
                var encodingConBom = new UTF8Encoding(true);
                File.WriteAllText(sfd.FileName, sb.ToString(), encodingConBom);

                MessageBox.Show("Reporte exportado exitosamente con compatibilidad Microsoft Excel (UTF-8 con BOM).",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a CSV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Sanitizar(string? s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            string limpio = s.Replace("\"", "\"\"");
            if (limpio.Contains(';') || limpio.Contains('\n') || limpio.Contains('\r') || limpio.Contains('"'))
            {
                return $"\"{limpio}\"";
            }
            return limpio;
        }

        private void BtnImprimir_Click(object? sender, EventArgs e)
        {
            try
            {
                using var preview = new PrintPreviewDialog
                {
                    Document = printDoc,
                    Width = 800,
                    Height = 600,
                    StartPosition = FormStartPosition.CenterParent
                };

                preview.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al preparar impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            if (g == null) return;

            using var fontTitulo = new Font("Segoe UI", 14F, FontStyle.Bold);
            using var fontSub = new Font("Segoe UI", 10F, FontStyle.Regular);
            using var fontCabecera = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            using var fontDatos = new Font("Segoe UI", 8.5F, FontStyle.Regular);

            int y = 40;
            g.DrawString("Reporte Oficial Mi ESCUELA — Santa Fe", fontTitulo, Brushes.Black, 40, y);
            y += 26;

            if (tabControl.SelectedTab == tabNovedades)
            {
                g.DrawString($"Novedades y Excepciones | Rango: {dtpDesde.Value:dd/MM/yyyy} al {dtpHasta.Value:dd/MM/yyyy}", fontSub, Brushes.DarkSlateGray, 40, y);
                y += 30;

                g.DrawLine(Pens.Black, 40, y, 760, y);
                y += 5;

                g.DrawString("Fecha", fontCabecera, Brushes.Black, 40, y);
                g.DrawString("Alumno", fontCabecera, Brushes.Black, 130, y);
                g.DrawString("Tipo Falta", fontCabecera, Brushes.Black, 380, y);
                g.DrawString("Valor", fontCabecera, Brushes.Black, 520, y);
                g.DrawString("Justif.", fontCabecera, Brushes.Black, 570, y);
                g.DrawString("Motivo", fontCabecera, Brushes.Black, 630, y);
                y += 20;

                g.DrawLine(Pens.Gray, 40, y, 760, y);
                y += 6;

                foreach (var n in _novedades.Take(40))
                {
                    g.DrawString(n.Fecha.ToString("dd/MM/yyyy"), fontDatos, Brushes.Black, 40, y);
                    g.DrawString(n.AlumnoNombreCompleto, fontDatos, Brushes.Black, 130, y);
                    g.DrawString(n.TipoInasistencia, fontDatos, Brushes.Black, 380, y);
                    g.DrawString(n.ValorFalta.ToString("0.##"), fontDatos, Brushes.Black, 520, y);
                    g.DrawString(n.Justificada ? "Sí" : "No", fontDatos, Brushes.Black, 570, y);
                    g.DrawString(n.Motivo ?? "-", fontDatos, Brushes.Black, 630, y);
                    y += 18;
                    if (y > 1050) break;
                }
            }
            else
            {
                string infoCurso = _espejo != null ? $"{_espejo.Curso} ({_espejo.Turno})" : "";
                g.DrawString($"Espejo Semanal — {infoCurso} | Semana: {(_espejo?.FechaLunes.ToString("dd/MM/yyyy") ?? "")} al {(_espejo?.FechaViernes.ToString("dd/MM/yyyy") ?? "")}", fontSub, Brushes.DarkSlateGray, 40, y);
                y += 30;

                g.DrawLine(Pens.Black, 40, y, 760, y);
                y += 5;

                g.DrawString("Alumno", fontCabecera, Brushes.Black, 40, y);
                g.DrawString("Lun", fontCabecera, Brushes.Black, 340, y);
                g.DrawString("Mar", fontCabecera, Brushes.Black, 400, y);
                g.DrawString("Mié", fontCabecera, Brushes.Black, 460, y);
                g.DrawString("Jue", fontCabecera, Brushes.Black, 520, y);
                g.DrawString("Vie", fontCabecera, Brushes.Black, 580, y);
                g.DrawString("Total", fontCabecera, Brushes.Black, 650, y);
                y += 20;

                g.DrawLine(Pens.Gray, 40, y, 760, y);
                y += 6;

                if (_espejo != null)
                {
                    foreach (var f in _espejo.Filas.Take(40))
                    {
                        g.DrawString(f.Alumno, fontDatos, Brushes.Black, 40, y);
                        g.DrawString(f.Lunes, fontDatos, Brushes.Black, 340, y);
                        g.DrawString(f.Martes, fontDatos, Brushes.Black, 400, y);
                        g.DrawString(f.Miercoles, fontDatos, Brushes.Black, 460, y);
                        g.DrawString(f.Jueves, fontDatos, Brushes.Black, 520, y);
                        g.DrawString(f.Viernes, fontDatos, Brushes.Black, 580, y);
                        g.DrawString(f.TotalFaltasPeriodo.ToString("0.##"), fontCabecera, Brushes.Black, 650, y);
                        y += 18;
                        if (y > 1050) break;
                    }
                }
            }

            e.HasMorePages = false;
        }
    }
}
