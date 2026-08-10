using System;
using System.Drawing;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class AlumnoDetalleForm : Form
    {
        private readonly AlumnoApiClient _apiClient = new AlumnoApiClient();
        private readonly AlumnoDTO? _alumnoExistente;
        private readonly bool _isEditMode;

        public AlumnoDetalleForm()
        {
            InitializeComponent();
            _isEditMode = false;
            ApplyStyles();
        }

        public AlumnoDetalleForm(AlumnoDTO alumno) : this()
        {
            _alumnoExistente = alumno;
            _isEditMode = true;
            CargarDatos();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(33, 37, 41);

            lblTitle.ForeColor = Color.White;
            lblNombre.ForeColor = Color.FromArgb(206, 212, 218);
            lblApellido.ForeColor = Color.FromArgb(206, 212, 218);
            lblGrado.ForeColor = Color.FromArgb(206, 212, 218);
            lblCurso.ForeColor = Color.FromArgb(206, 212, 218);
            lblEstado.ForeColor = Color.FromArgb(206, 212, 218);

            txtNombre.BackColor = Color.FromArgb(49, 53, 56);
            txtNombre.ForeColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;

            txtApellido.BackColor = Color.FromArgb(49, 53, 56);
            txtApellido.ForeColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;

            txtGrado.BackColor = Color.FromArgb(49, 53, 56);
            txtGrado.ForeColor = Color.White;
            txtGrado.BorderStyle = BorderStyle.FixedSingle;

            txtCurso.BackColor = Color.FromArgb(49, 53, 56);
            txtCurso.ForeColor = Color.White;
            txtCurso.BorderStyle = BorderStyle.FixedSingle;

            cmbEstado.BackColor = Color.FromArgb(49, 53, 56);
            cmbEstado.ForeColor = Color.White;
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;

            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnCancelar.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
        }

        private void CargarDatos()
        {
            if (_alumnoExistente != null)
            {
                lblTitle.Text = "Editar Alumno";
                txtNombre.Text = _alumnoExistente.Nombre;
                txtApellido.Text = _alumnoExistente.Apellido;
                txtGrado.Text = _alumnoExistente.Grado;
                txtCurso.Text = _alumnoExistente.Curso;

                int index = cmbEstado.Items.IndexOf(_alumnoExistente.Estado);
                if (index >= 0) cmbEstado.SelectedIndex = index;
            }
        }

        private void AlumnoDetalleForm_Load(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                lblTitle.Text = "Nuevo Alumno";
                cmbEstado.SelectedIndex = 0;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string grado = txtGrado.Text.Trim();
            string curso = txtCurso.Text.Trim();
            string estado = cmbEstado.SelectedItem?.ToString() ?? "Presente";

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(grado) || string.IsNullOrEmpty(curso))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Campos vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_isEditMode && _alumnoExistente != null)
                {
                    var dto = new AlumnoDTO
                    {
                        IdAlumno = _alumnoExistente.IdAlumno,
                        Nombre = nombre,
                        Apellido = apellido,
                        Grado = grado,
                        Curso = curso,
                        Estado = estado
                    };
                    bool ok = await _apiClient.UpdateAsync(dto);
                    if (ok)
                    {
                        MessageBox.Show("Alumno actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el alumno.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var dto = new AlumnoDTO
                    {
                        IdAlumno = 0,
                        Nombre = nombre,
                        Apellido = apellido,
                        Grado = grado,
                        Curso = curso,
                        Estado = estado
                    };
                    var creado = await _apiClient.AddAsync(dto);
                    if (creado != null)
                    {
                        MessageBox.Show("Alumno creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
