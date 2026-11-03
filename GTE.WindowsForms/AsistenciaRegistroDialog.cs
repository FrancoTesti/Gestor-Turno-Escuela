using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public class AsistenciaRegistroDialog : Form
    {
        private readonly AlumnoAsistenciaItemDTO _alumno;
        private readonly List<TipoInasistenciaDTO> _tipos;

        private Label lblTitle = null!;
        private Label lblAlumno = null!;
        private Label lblTipo = null!;
        private ComboBox cmbTipo = null!;
        private CheckBox chkJustificada = null!;
        private Label lblMotivo = null!;
        private TextBox txtMotivo = null!;
        private CheckBox chkNoComputa = null!;
        private Label lblObservaciones = null!;
        private TextBox txtObservaciones = null!;
        private Button btnAceptar = null!;
        private Button btnCancelar = null!;

        public AsistenciaRegistroDialog(AlumnoAsistenciaItemDTO alumno, List<TipoInasistenciaDTO> tipos)
        {
            _alumno = alumno ?? throw new ArgumentNullException(nameof(alumno));
            _tipos = tipos ?? new List<TipoInasistenciaDTO>();

            InitializeComponent();
            ApplyStyles();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblAlumno = new Label();
            this.lblTipo = new Label();
            this.cmbTipo = new ComboBox();
            this.chkJustificada = new CheckBox();
            this.lblMotivo = new Label();
            this.txtMotivo = new TextBox();
            this.chkNoComputa = new CheckBox();
            this.lblObservaciones = new Label();
            this.txtObservaciones = new TextBox();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();

            this.SuspendLayout();

            // Form
            this.Text = "Modificar Asistencia / Inasistencia";
            this.ClientSize = new Size(460, 430);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Title
            this.lblTitle.Text = "Registro de Asistencia";
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Size = new Size(420, 30);

            // Alumno
            this.lblAlumno.Text = $"Alumno: {_alumno.Apellido}, {_alumno.Nombre}";
            this.lblAlumno.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblAlumno.Location = new Point(20, 48);
            this.lblAlumno.Size = new Size(420, 25);

            // Tipo
            this.lblTipo.Text = "Condición / Tipo de Inasistencia:";
            this.lblTipo.Location = new Point(20, 80);
            this.lblTipo.Size = new Size(420, 20);

            this.cmbTipo.Location = new Point(20, 102);
            this.cmbTipo.Size = new Size(420, 30);
            this.cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipo.SelectedIndexChanged += CmbTipo_SelectedIndexChanged;

            // Justificada
            this.chkJustificada.Text = "Inasistencia Justificada (Código J)";
            this.chkJustificada.Location = new Point(20, 142);
            this.chkJustificada.Size = new Size(420, 25);

            // Motivo
            this.lblMotivo.Text = "Motivo de la inasistencia:";
            this.lblMotivo.Location = new Point(20, 172);
            this.lblMotivo.Size = new Size(420, 20);

            this.txtMotivo.Location = new Point(20, 194);
            this.txtMotivo.Size = new Size(420, 28);

            // No Computa
            this.chkNoComputa.Text = "No Computa (exención de cómputo ministerial Santa Fe)";
            this.chkNoComputa.Location = new Point(20, 230);
            this.chkNoComputa.Size = new Size(420, 25);

            // Observaciones
            this.lblObservaciones.Text = "Observaciones:";
            this.lblObservaciones.Location = new Point(20, 260);
            this.lblObservaciones.Size = new Size(420, 20);

            this.txtObservaciones.Location = new Point(20, 282);
            this.txtObservaciones.Size = new Size(420, 60);
            this.txtObservaciones.Multiline = true;

            // Buttons
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Location = new Point(210, 365);
            this.btnAceptar.Size = new Size(110, 36);
            this.btnAceptar.Click += BtnAceptar_Click;

            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Location = new Point(330, 365);
            this.btnCancelar.Size = new Size(110, 36);
            this.btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAlumno);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.cmbTipo);
            this.Controls.Add(this.chkJustificada);
            this.Controls.Add(this.lblMotivo);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.chkNoComputa);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.BotonPrimario(btnAceptar);
            Tema.BotonSecundario(btnCancelar);
        }

        private void CargarDatos()
        {
            var items = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Presente (Sin inasistencia)")
            };

            foreach (var t in _tipos)
            {
                items.Add(new KeyValuePair<int, string>(t.IdTipoInasistencia, $"{t.Nombre} ({t.CodigoOficial} - {t.ValorDecimal:0.##} falta)"));
            }

            cmbTipo.DisplayMember = "Value";
            cmbTipo.ValueMember = "Key";
            cmbTipo.DataSource = items;

            if (_alumno.IdTipoInasistencia.HasValue && _alumno.IdTipoInasistencia.Value > 0)
            {
                cmbTipo.SelectedValue = _alumno.IdTipoInasistencia.Value;
                chkJustificada.Checked = _alumno.Justificada;
                txtMotivo.Text = _alumno.Motivo ?? string.Empty;
                chkNoComputa.Checked = _alumno.NoComputa;
                txtObservaciones.Text = _alumno.Observaciones ?? string.Empty;
            }
            else
            {
                cmbTipo.SelectedValue = 0;
                chkJustificada.Checked = false;
                txtMotivo.Text = string.Empty;
                chkNoComputa.Checked = false;
                txtObservaciones.Text = _alumno.Observaciones ?? string.Empty;
            }

            ActualizarVisibilidadCampos();
        }

        private void CmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadCampos();
        }

        private void ActualizarVisibilidadCampos()
        {
            int id = (int)(cmbTipo.SelectedValue ?? 0);
            bool esInasistencia = id > 0;

            chkJustificada.Enabled = esInasistencia;
            txtMotivo.Enabled = esInasistencia;
            chkNoComputa.Enabled = esInasistencia;
        }

        private void BtnAceptar_Click(object? sender, EventArgs e)
        {
            int id = (int)(cmbTipo.SelectedValue ?? 0);

            if (id == 0)
            {
                _alumno.EstadoGeneral = "Presente";
                _alumno.IdTipoInasistencia = null;
                _alumno.CodigoOficial = "P";
                _alumno.Justificada = false;
                _alumno.Motivo = string.Empty;
                _alumno.NoComputa = false;
                _alumno.Observaciones = txtObservaciones.Text.Trim();
                _alumno.TieneRetiroRegistrado = false;
                _alumno.DetalleRetiroInfo = null;
                _alumno.IdRetiroOrigen = null;
            }
            else
            {
                var tipo = _tipos.FirstOrDefault(t => t.IdTipoInasistencia == id);
                _alumno.EstadoGeneral = "Inasistencia";
                _alumno.IdTipoInasistencia = id;
                _alumno.CodigoOficial = tipo?.CodigoOficial ?? "A";
                _alumno.Justificada = chkJustificada.Checked;
                _alumno.Motivo = txtMotivo.Text.Trim();
                _alumno.NoComputa = chkNoComputa.Checked;
                _alumno.Observaciones = txtObservaciones.Text.Trim();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
