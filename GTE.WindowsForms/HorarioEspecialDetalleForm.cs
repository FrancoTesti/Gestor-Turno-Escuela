using System;
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
        private Label lblAlumno, lblDesc, lblHora;
        private ComboBox cmbAlumno;
        private TextBox txtDesc;
        private DateTimePicker dtpHora;
        private Button btnGuardar, btnCancelar;

        private void InitializeComponent()
        {
            this.lblAlumno = new Label();
            this.lblDesc = new Label();
            this.lblHora = new Label();
            this.cmbAlumno = new ComboBox();
            this.txtDesc = new TextBox();
            this.dtpHora = new DateTimePicker();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();
            this.SuspendLayout();

            this.lblAlumno.AutoSize = true;
            this.lblAlumno.Location = new Point(20, 23);
            this.lblAlumno.Text = "Alumno:";
            this.cmbAlumno.Location = new Point(140, 20);
            this.cmbAlumno.Size = new Size(250, 28);
            this.cmbAlumno.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new Point(20, 63);
            this.lblDesc.Text = "Descripción:";
            this.txtDesc.Location = new Point(140, 60);
            this.txtDesc.Size = new Size(250, 28);

            this.lblHora.AutoSize = true;
            this.lblHora.Location = new Point(20, 103);
            this.lblHora.Text = "Hora de salida:";
            this.dtpHora.Location = new Point(140, 100);
            this.dtpHora.Size = new Size(140, 28);
            this.dtpHora.Format = DateTimePickerFormat.Time;
            this.dtpHora.ShowUpDown = true;

            this.btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnGuardar.Location = new Point(98, 155);
            this.btnGuardar.Size = new Size(140, 36);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancelar.Location = new Point(250, 155);
            this.btnCancelar.Size = new Size(140, 36);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(420, 215);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Controls.Add(lblAlumno);
            this.Controls.Add(cmbAlumno);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtDesc);
            this.Controls.Add(lblHora);
            this.Controls.Add(dtpHora);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
            this.Text = "Detalle de Horario Especial";
            this.ResumeLayout(false);
            this.PerformLayout();
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
            Tema.BotonPrimario(btnGuardar);
            Tema.BotonSecundario(btnCancelar);
        }

        public HorarioEspecialDetalleForm(HorarioEspecialDTO dto) : this()
        {
            _dto = dto;
            _isEdit = true;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                var alumnos = await _alumnoClient.GetAllAsync();
                var items = alumnos
                    .OrderBy(a => a.Apellido)
                    .ThenBy(a => a.Nombre)
                    .Select(a => new
                    {
                        a.IdAlumno,
                        NombreCompleto = $"{a.Apellido}, {a.Nombre} ({a.Grado} {a.Curso})"
                    })
                    .ToList();

                cmbAlumno.DataSource = items;
                cmbAlumno.DisplayMember = "NombreCompleto";
                cmbAlumno.ValueMember = "IdAlumno";

                if (_isEdit)
                {
                    cmbAlumno.SelectedValue = _dto.IdAlumno;
                    txtDesc.Text = _dto.DescripcionActividad;
                    dtpHora.Value = DateTime.Today.Add(_dto.HoraSalidaEspecial);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los alumnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbAlumno.SelectedValue is not int idAlumno || idAlumno <= 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                MessageBox.Show("La descripción de la actividad es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDesc.Focus();
                return;
            }

            _dto.IdAlumno = idAlumno;
            _dto.DescripcionActividad = txtDesc.Text.Trim();
            _dto.HoraSalidaEspecial = dtpHora.Value.TimeOfDay;

            try
            {
                if (_isEdit) await _apiClient.UpdateAsync(_dto);
                else await _apiClient.AddAsync(_dto);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el horario especial: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
