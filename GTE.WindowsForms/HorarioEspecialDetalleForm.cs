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
    public partial class HorarioEspecialDetalleForm : Form
    {
        private Label lblAlumno, lblDesc, lblHora, lblTipo;
        private ComboBox cmbAlumno;
        private TextBox txtDesc;
        private DateTimePicker dtpHora;
        private RadioButton rbSemanal, rbPuntual;
        private Panel pnlSemanal, pnlPuntual;
        private CheckBox chkLun, chkMar, chkMie, chkJue, chkVie;
        private DateTimePicker dtpFecha;
        private Button btnGuardar, btnCancelar;

        private void InitializeComponent()
        {
            this.lblAlumno = new Label();
            this.lblDesc = new Label();
            this.lblHora = new Label();
            this.lblTipo = new Label();
            this.cmbAlumno = new ComboBox();
            this.txtDesc = new TextBox();
            this.dtpHora = new DateTimePicker();
            this.rbSemanal = new RadioButton();
            this.rbPuntual = new RadioButton();
            this.pnlSemanal = new Panel();
            this.pnlPuntual = new Panel();
            this.chkLun = new CheckBox();
            this.chkMar = new CheckBox();
            this.chkMie = new CheckBox();
            this.chkJue = new CheckBox();
            this.chkVie = new CheckBox();
            this.dtpFecha = new DateTimePicker();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.SuspendLayout();

            this.lblAlumno.Location = new Point(20, 20);
            this.lblAlumno.Size = new Size(110, 25);
            this.lblAlumno.Text = "Alumno:";
            this.cmbAlumno.Location = new Point(135, 20);
            this.cmbAlumno.Size = new Size(250, 25);
            this.cmbAlumno.DropDownStyle = ComboBoxStyle.DropDown;
            this.cmbAlumno.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.cmbAlumno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            this.lblDesc.Location = new Point(20, 60);
            this.lblDesc.Size = new Size(110, 25);
            this.lblDesc.Text = "Descripción:";
            this.txtDesc.Location = new Point(135, 60);
            this.txtDesc.Size = new Size(250, 25);

            this.lblHora.Location = new Point(20, 100);
            this.lblHora.Size = new Size(110, 25);
            this.lblHora.Text = "Hora de salida:";
            this.dtpHora.Location = new Point(135, 100);
            this.dtpHora.Size = new Size(120, 25);
            this.dtpHora.Format = DateTimePickerFormat.Time;
            this.dtpHora.ShowUpDown = true;

            // Tipo / Recurrencia
            this.lblTipo.Location = new Point(20, 140);
            this.lblTipo.Size = new Size(110, 25);
            this.lblTipo.Text = "Aplica:";

            this.rbSemanal.Location = new Point(135, 140);
            this.rbSemanal.Size = new Size(120, 25);
            this.rbSemanal.Text = "Días de semana";
            this.rbSemanal.Checked = true;
            this.rbSemanal.CheckedChanged += (s, e) => ActualizarModo();

            this.rbPuntual.Location = new Point(265, 140);
            this.rbPuntual.Size = new Size(120, 25);
            this.rbPuntual.Text = "Fecha puntual";
            this.rbPuntual.CheckedChanged += (s, e) => ActualizarModo();

            // Panel Semanal
            this.pnlSemanal.Location = new Point(135, 175);
            this.pnlSemanal.Size = new Size(255, 65);

            this.chkLun.Text = "Lun";
            this.chkLun.Location = new Point(0, 0);
            this.chkLun.Size = new Size(50, 25);
            this.chkMar.Text = "Mar";
            this.chkMar.Location = new Point(52, 0);
            this.chkMar.Size = new Size(50, 25);
            this.chkMie.Text = "Mié";
            this.chkMie.Location = new Point(104, 0);
            this.chkMie.Size = new Size(50, 25);
            this.chkJue.Text = "Jue";
            this.chkJue.Location = new Point(156, 0);
            this.chkJue.Size = new Size(50, 25);
            this.chkVie.Text = "Vie";
            this.chkVie.Location = new Point(208, 0);
            this.chkVie.Size = new Size(50, 25);

            this.pnlSemanal.Controls.Add(chkLun);
            this.pnlSemanal.Controls.Add(chkMar);
            this.pnlSemanal.Controls.Add(chkMie);
            this.pnlSemanal.Controls.Add(chkJue);
            this.pnlSemanal.Controls.Add(chkVie);

            // Panel Puntual
            this.pnlPuntual.Location = new Point(135, 175);
            this.pnlPuntual.Size = new Size(255, 40);
            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Location = new Point(0, 0);
            this.dtpFecha.Size = new Size(160, 25);
            this.pnlPuntual.Controls.Add(dtpFecha);
            this.pnlPuntual.Visible = false;

            // Botones
            this.btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnGuardar.Location = new Point(135, 255);
            this.btnGuardar.Size = new Size(120, 36);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancelar.Location = new Point(265, 255);
            this.btnCancelar.Size = new Size(120, 36);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(415, 310);
            this.Controls.Add(lblAlumno);
            this.Controls.Add(cmbAlumno);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtDesc);
            this.Controls.Add(lblHora);
            this.Controls.Add(dtpHora);
            this.Controls.Add(lblTipo);
            this.Controls.Add(rbSemanal);
            this.Controls.Add(rbPuntual);
            this.Controls.Add(pnlSemanal);
            this.Controls.Add(pnlPuntual);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Detalle Horario Especial";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ActualizarModo()
        {
            pnlSemanal.Visible = rbSemanal.Checked;
            pnlPuntual.Visible = rbPuntual.Checked;
        }

        private readonly HorarioEspecialApiClient _apiClient = new HorarioEspecialApiClient();
        private readonly AlumnoApiClient _alumnoClient = new AlumnoApiClient();
        private HorarioEspecialDTO _dto;
        private bool _isEdit = false;

        public HorarioEspecialDetalleForm()
        {
            InitializeComponent();
            _dto = new HorarioEspecialDTO();
            Tema.AcomodarControles(this);
        }

        public HorarioEspecialDetalleForm(HorarioEspecialDTO dto) : this()
        {
            _dto = dto;
            _isEdit = true;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var alumnos = await _alumnoClient.GetAllAsync();
            var alumnosDisplay = (alumnos ?? new List<AlumnoDTO>())
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .Select(a => new
                {
                    a.IdAlumno,
                    Display = $"{a.Apellido}, {a.Nombre} ({a.Grado} {a.Curso} - {a.Turno})"
                })
                .ToList();

            cmbAlumno.DisplayMember = "Display";
            cmbAlumno.ValueMember = "IdAlumno";
            cmbAlumno.DataSource = alumnosDisplay;
            cmbAlumno.DropDownStyle = ComboBoxStyle.DropDown;
            cmbAlumno.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbAlumno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            if (_isEdit)
            {
                cmbAlumno.SelectedValue = _dto.IdAlumno;
                txtDesc.Text = _dto.DescripcionActividad;
                dtpHora.Value = DateTime.Today.Add(_dto.HoraSalidaEspecial);

                if (!_dto.EsRecurrente && _dto.FechaEspecifica.HasValue)
                {
                    rbPuntual.Checked = true;
                    dtpFecha.Value = _dto.FechaEspecifica.Value;
                }
                else
                {
                    rbSemanal.Checked = true;
                    string dias = _dto.DiasSemana ?? string.Empty;
                    chkLun.Checked = dias.Contains("Lunes", StringComparison.OrdinalIgnoreCase);
                    chkMar.Checked = dias.Contains("Martes", StringComparison.OrdinalIgnoreCase);
                    chkMie.Checked = dias.Contains("Miércoles", StringComparison.OrdinalIgnoreCase) || dias.Contains("Miercoles", StringComparison.OrdinalIgnoreCase);
                    chkJue.Checked = dias.Contains("Jueves", StringComparison.OrdinalIgnoreCase);
                    chkVie.Checked = dias.Contains("Viernes", StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                dtpHora.Value = DateTime.Today.AddHours(12);
                dtpFecha.Value = DateTime.Today;
            }

            ActualizarModo();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            int idAlumnoSeleccionado = 0;
            if (cmbAlumno.SelectedValue is int val && val > 0)
            {
                idAlumnoSeleccionado = val;
            }
            else if (!string.IsNullOrWhiteSpace(cmbAlumno.Text))
            {
                string escrito = cmbAlumno.Text.Trim();
                foreach (var item in cmbAlumno.Items)
                {
                    dynamic d = item;
                    string display = d.Display;
                    if (display.IndexOf(escrito, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        idAlumnoSeleccionado = d.IdAlumno;
                        break;
                    }
                }
            }

            if (idAlumnoSeleccionado <= 0)
            {
                MessageBox.Show("Debe seleccionar un alumno válido de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dto.IdAlumno = idAlumnoSeleccionado;
            _dto.DescripcionActividad = txtDesc.Text.Trim();
            _dto.HoraSalidaEspecial = dtpHora.Value.TimeOfDay;

            if (rbSemanal.Checked)
            {
                var dias = new List<string>();
                if (chkLun.Checked) dias.Add("Lunes");
                if (chkMar.Checked) dias.Add("Martes");
                if (chkMie.Checked) dias.Add("Miércoles");
                if (chkJue.Checked) dias.Add("Jueves");
                if (chkVie.Checked) dias.Add("Viernes");

                _dto.EsRecurrente = true;
                _dto.DiasSemana = string.Join(",", dias);
                _dto.FechaEspecifica = null;
            }
            else
            {
                _dto.EsRecurrente = false;
                _dto.DiasSemana = string.Empty;
                _dto.FechaEspecifica = dtpFecha.Value.Date;
            }

            try
            {
                if (_isEdit) await _apiClient.UpdateAsync(_dto);
                else await _apiClient.AddAsync(_dto);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
