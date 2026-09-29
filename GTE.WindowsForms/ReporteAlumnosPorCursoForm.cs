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
    public partial class ReporteAlumnosPorCursoForm : Form
    {
        private readonly ReporteApiClient _apiClient = new ReporteApiClient();
        private List<AlumnosPorCursoDTO> _filas = new();

        public ReporteAlumnosPorCursoForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTotal.ForeColor = Color.FromArgb(33, 37, 41);

            dgvDatos.BackgroundColor = Color.White;
            dgvDatos.BorderStyle = BorderStyle.None;
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatos.MultiSelect = false;
            dgvDatos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);
        }

        private async void ReporteAlumnosPorCursoForm_Load(object sender, EventArgs e)
        {
            try
            {
                _filas = await _apiClient.GetAlumnosPorCursoAsync();

                dgvDatos.DataSource = _filas;
                if (dgvDatos.Columns.Count > 0)
                {
                    dgvDatos.Columns["IdCurso"].HeaderText = "ID Curso";
                    dgvDatos.Columns["IdCurso"].Width = 90;
                    dgvDatos.Columns["Grado"].HeaderText = "Grado";
                    dgvDatos.Columns["Grado"].Width = 120;
                    dgvDatos.Columns["Curso"].HeaderText = "División";
                    dgvDatos.Columns["Curso"].Width = 120;
                    dgvDatos.Columns["Turno"].HeaderText = "Turno";
                    dgvDatos.Columns["Turno"].Width = 140;
                    dgvDatos.Columns["Cantidad"].HeaderText = "Cantidad de alumnos";
                    dgvDatos.Columns["Cantidad"].Width = 180;
                }

                int total = _filas.Sum(fila => fila.Cantidad);
                lblTotal.Text = $"Total de alumnos: {total}    Cursos: {_filas.Count}";

                pnlGrafico.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dibuja el gráfico de barras a mano, sin librerías externas, para que
        /// el reporte funcione en cualquier máquina sin dependencias extra.
        /// </summary>
        private void pnlGrafico_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            const int margenIzquierdo = 55;
            const int margenDerecho = 25;
            const int margenSuperior = 30;
            const int margenInferior = 55;

            var area = new Rectangle(
                margenIzquierdo,
                margenSuperior,
                pnlGrafico.Width - margenIzquierdo - margenDerecho,
                pnlGrafico.Height - margenSuperior - margenInferior);

            using var lapizEje = new Pen(Color.FromArgb(160, 168, 176), 1);
            g.DrawLine(lapizEje, area.Left, area.Bottom, area.Right, area.Bottom);
            g.DrawLine(lapizEje, area.Left, area.Top, area.Left, area.Bottom);

            if (_filas.Count == 0 || area.Width <= 0 || area.Height <= 0)
            {
                using var aviso = new Font("Segoe UI", 11F);
                g.DrawString("Sin datos para mostrar.", aviso, Brushes.Gray,
                    new PointF(area.Left + 10, area.Top + 10));
                return;
            }

            int maximo = Math.Max(1, _filas.Max(fila => fila.Cantidad));

            using var fuenteValor = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            using var fuenteEtiqueta = new Font("Segoe UI", 8.5F);
            using var fuenteTurno = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            int anchoBarra = Math.Max(18, area.Width / (_filas.Count * 2));
            int separacion = (area.Width - (_filas.Count * anchoBarra)) / (_filas.Count + 1);

            // Lineas guia horizontales con la referencia de cantidad.
            using var lapizGuia = new Pen(Color.FromArgb(228, 232, 236), 1);
            for (int valor = 0; valor <= maximo; valor++)
            {
                int y = area.Bottom - (int)((double)valor / maximo * area.Height);
                g.DrawLine(lapizGuia, area.Left, y, area.Right, y);

                using var brochaTexto = new SolidBrush(Color.FromArgb(110, 118, 126));
                var medida = g.MeasureString(valor.ToString(), fuenteEtiqueta);
                g.DrawString(valor.ToString(), fuenteEtiqueta, brochaTexto,
                    area.Left - medida.Width - 6, y - medida.Height / 2);
            }

            for (int i = 0; i < _filas.Count; i++)
            {
                AlumnosPorCursoDTO fila = _filas[i];

                int alto = (int)((double)fila.Cantidad / maximo * area.Height);
                int x = area.Left + separacion + i * (anchoBarra + separacion);
                int y = area.Bottom - alto;

                using var brocha = new SolidBrush(ColorDeTurno(fila.Turno));
                g.FillRectangle(brocha, x, y, anchoBarra, alto);
                g.DrawRectangle(Pens.White, x, y, anchoBarra, alto);

                using var brochaValor = new SolidBrush(Color.FromArgb(33, 37, 41));
                var medidaValor = g.MeasureString(fila.Cantidad.ToString(), fuenteValor);
                g.DrawString(fila.Cantidad.ToString(), fuenteValor, brochaValor,
                    x + (anchoBarra - medidaValor.Width) / 2, y - medidaValor.Height - 2);

                string etiqueta = $"{fila.Grado} {fila.Curso}";
                var medidaEtiqueta = g.MeasureString(etiqueta, fuenteEtiqueta);
                g.DrawString(etiqueta, fuenteEtiqueta, Brushes.DimGray,
                    x + (anchoBarra - medidaEtiqueta.Width) / 2, area.Bottom + 6);

                var medidaTurno = g.MeasureString(fila.Turno, fuenteTurno);
                g.DrawString(fila.Turno, fuenteTurno, new SolidBrush(ColorDeTurno(fila.Turno)),
                    x + (anchoBarra - medidaTurno.Width) / 2, area.Bottom + 24);
            }
        }

        /// <summary>Un color por turno, para distinguirlos de un vistazo.</summary>
        private static Color ColorDeTurno(string? turno) => turno switch
        {
            "Mañana" => Color.FromArgb(13, 110, 253),
            "Tarde" => Color.FromArgb(255, 153, 0),
            "Noche" => Color.FromArgb(111, 66, 193),
            _ => Color.FromArgb(108, 117, 125)
        };
    }
}
