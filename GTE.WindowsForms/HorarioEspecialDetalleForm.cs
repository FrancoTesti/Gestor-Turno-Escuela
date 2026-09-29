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
            
            this.lblAlumno.Location = new Point(20, 20);
            this.lblAlumno.Text = "Alumno:";
            this.cmbAlumno.Location = new Point(120, 20);
            this.cmbAlumno.Size = new Size(200, 25);
            this.cmbAlumno.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblDesc.Location = new Point(20, 60);
            this.lblDesc.Text = "Descripción:";
            this.txtDesc.Location = new Point(120, 60);
            this.txtDesc.Size = new Size(200, 25);

            this.lblHora.Location = new Point(20, 100);
            this.lblHora.Text = "Hora de salida:";
            this.dtpHora.Location = new Point(120, 100);
            this.dtpHora.Format = DateTimePickerFormat.Time;
            this.dtpHora.ShowUpDown = true;

            this.btnGuardar.Location = new Point(120, 150);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Location = new Point(220, 150);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new EventHandler(this.btnCancelar_Click);

            this.ClientSize = new Size(350, 200);
            this.Controls.Add(lblAlumno);
            this.Controls.Add(cmbAlumno);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtDesc);
            this.Controls.Add(lblHora);
            this.Controls.Add(dtpHora);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
            this.Text = "Detalle Horario Especial";
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
            cmbAlumno.DataSource = alumnos;
            cmbAlumno.DisplayMember = "Nombre"; // Should ideally be NombreCompleto but we just bind the whole object and display.
            cmbAlumno.ValueMember = "IdAlumno";

            if (_isEdit)
            {
                cmbAlumno.SelectedValue = _dto.IdAlumno;
                txtDesc.Text = _dto.DescripcionActividad;
                dtpHora.Value = DateTime.Today.Add(_dto.HoraSalidaEspecial);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            _dto.IdAlumno = (int)cmbAlumno.SelectedValue;
            _dto.DescripcionActividad = txtDesc.Text;
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
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
