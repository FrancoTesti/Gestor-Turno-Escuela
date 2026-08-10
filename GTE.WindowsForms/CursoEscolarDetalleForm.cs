using System;
using System.Drawing;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class CursoEscolarDetalleForm : Form
    {
        private readonly CursoEscolarApiClient _apiClient = new CursoEscolarApiClient();
        private readonly CursoEscolarDTO? _cursoExistente;
        private readonly bool _isEditMode;

        public CursoEscolarDetalleForm()
        {
            InitializeComponent();
            _isEditMode = false;
            ApplyStyles();
        }

        public CursoEscolarDetalleForm(CursoEscolarDTO curso) : this()
        {
            _cursoExistente = curso;
            _isEditMode = true;
            CargarDatos();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(33, 37, 41);

            lblTitle.ForeColor = Color.White;
            lblGrado.ForeColor = Color.FromArgb(206, 212, 218);
            lblCurso.ForeColor = Color.FromArgb(206, 212, 218);
            lblHorario.ForeColor = Color.FromArgb(206, 212, 218);

            txtGrado.BackColor = Color.FromArgb(49, 53, 56);
            txtGrado.ForeColor = Color.White;
            txtGrado.BorderStyle = BorderStyle.FixedSingle;

            txtCurso.BackColor = Color.FromArgb(49, 53, 56);
            txtCurso.ForeColor = Color.White;
            txtCurso.BorderStyle = BorderStyle.FixedSingle;

            txtHorario.BackColor = Color.FromArgb(49, 53, 56);
            txtHorario.ForeColor = Color.White;
            txtHorario.BorderStyle = BorderStyle.FixedSingle;

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
            if (_cursoExistente != null)
            {
                lblTitle.Text = "Editar Curso Escolar";
                txtGrado.Text = _cursoExistente.Grado;
                txtCurso.Text = _cursoExistente.Curso;
                txtHorario.Text = _cursoExistente.HorarioSalida.ToString(@"hh\:mm");
            }
        }

        private void CursoEscolarDetalleForm_Load(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                lblTitle.Text = "Nuevo Curso Escolar";
                txtHorario.Text = "12:00";
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string grado = txtGrado.Text.Trim();
            string curso = txtCurso.Text.Trim();
            string horarioText = txtHorario.Text.Trim();

            if (string.IsNullOrEmpty(grado) || string.IsNullOrEmpty(curso) || string.IsNullOrEmpty(horarioText))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Campos vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TimeSpan.TryParse(horarioText, out TimeSpan horario))
            {
                MessageBox.Show("El horario debe tener un formato válido (Ej: 12:15 o 12:15:00).", "Horario Inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (_isEditMode && _cursoExistente != null)
                {
                    var dto = new CursoEscolarDTO
                    {
                        IdCurso = _cursoExistente.IdCurso,
                        Grado = grado,
                        Curso = curso,
                        HorarioSalida = horario
                    };
                    bool ok = await _apiClient.UpdateAsync(dto);
                    if (ok)
                    {
                        MessageBox.Show("Curso actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el curso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var dto = new CursoEscolarDTO
                    {
                        IdCurso = 0,
                        Grado = grado,
                        Curso = curso,
                        HorarioSalida = horario
                    };
                    var creado = await _apiClient.AddAsync(dto);
                    if (creado != null)
                    {
                        MessageBox.Show("Curso creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
