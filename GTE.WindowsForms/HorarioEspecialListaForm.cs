using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class HorarioEspecialListaForm : Form
    {
        private Label lblTitle;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private DataGridView dgvHorarios;

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.btnNuevo = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.dgvHorarios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorarios)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(300, 30);
            this.lblTitle.Text = "Gestión de Horarios Especiales";

            this.btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnNuevo.Location = new Point(430, 50);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new Size(110, 36);
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Click += new EventHandler(this.btnNuevo_Click);

            this.btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnEditar.Location = new Point(550, 50);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new Size(110, 36);
            this.btnEditar.Text = "Editar";
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);

            this.btnEliminar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnEliminar.Location = new Point(670, 50);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new Size(110, 36);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            this.dgvHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHorarios.Location = new Point(17, 95);
            this.dgvHorarios.Name = "dgvHorarios";
            this.dgvHorarios.Size = new Size(766, 340);

            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.dgvHorarios);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.lblTitle);
            this.Name = "HorarioEspecialListaForm";
            this.Text = "Horarios Especiales";
            this.Load += new EventHandler(this.HorarioEspecialListaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHorarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private readonly HorarioEspecialApiClient _apiClient = new HorarioEspecialApiClient();

        public HorarioEspecialListaForm()
        {
            InitializeComponent();
            ApplyStyles();
            Tema.AcomodarControles(this);
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            btnNuevo.BackColor = Color.FromArgb(40, 167, 69);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnEditar.BackColor = Color.FromArgb(23, 162, 184);
            btnEditar.ForeColor = Color.White;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            Tema.Grilla(dgvHorarios);
        }

        private async void HorarioEspecialListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();
            await RefreshGrid();
        }

        private async Task AplicarPermisosSegunRol()
        {
            string? rol = await AuthServiceProvider.Instance.GetRoleAsync();
            bool esSecretario = rol == "Secretario";
            bool esPortero = rol == "Portero";
            btnNuevo.Visible = esSecretario || esPortero;
            btnEditar.Visible = esSecretario || esPortero;
            btnEliminar.Visible = esSecretario || esPortero;
        }

        private async Task RefreshGrid()
        {
            try
            {
                var lista = await _apiClient.GetAllAsync();
                dgvHorarios.DataSource = lista;
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar horarios: {ex.Message}");
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvHorarios.Columns.Count == 0)
                return;

            OcultarColumna("IdHorarioEspecial");
            OcultarColumna("IdAlumno");

            ConfigurarColumna("AlumnoNombreCompleto", "Alumno", 250);
            ConfigurarColumna("DescripcionActividad", "Descripción de la actividad", 300);
            ConfigurarColumna("HoraSalidaEspecial", "Hora de salida", 140, @"hh\:mm");
        }

        private void OcultarColumna(string nombre)
        {
            if (dgvHorarios.Columns.Contains(nombre))
                dgvHorarios.Columns[nombre].Visible = false;
        }

        private void ConfigurarColumna(string nombre, string titulo, int ancho, string? formato = null)
        {
            if (!dgvHorarios.Columns.Contains(nombre))
                return;

            var columna = dgvHorarios.Columns[nombre];
            columna.HeaderText = titulo;
            columna.Width = ancho;

            if (formato != null)
                columna.DefaultCellStyle.Format = formato;
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var frm = new HorarioEspecialDetalleForm();
            if (frm.ShowDialog() == DialogResult.OK) await RefreshGrid();
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvHorarios.SelectedRows.Count == 0) return;
            var obj = dgvHorarios.SelectedRows[0].DataBoundItem as HorarioEspecialDTO;
            if (obj == null) return;
            var frm = new HorarioEspecialDetalleForm(obj);
            if (frm.ShowDialog() == DialogResult.OK) await RefreshGrid();
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvHorarios.SelectedRows.Count == 0) return;
            var obj = dgvHorarios.SelectedRows[0].DataBoundItem as HorarioEspecialDTO;
            if (obj == null) return;
            if (MessageBox.Show("¿Eliminar horario?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await _apiClient.DeleteAsync(obj.IdHorarioEspecial);
                await RefreshGrid();
            }
        }
    }
}
