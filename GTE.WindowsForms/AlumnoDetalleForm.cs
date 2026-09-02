using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class AlumnoDetalleForm : Form
    {
        private readonly AlumnoApiClient _apiClient = new AlumnoApiClient();
        private readonly CursoEscolarApiClient _cursoApiClient = new CursoEscolarApiClient();
        private List<CursoEscolarDTO> _cursos = new();
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
            lblTurno.ForeColor = Color.FromArgb(206, 212, 218);
            lblCursoEscolar.ForeColor = Color.FromArgb(206, 212, 218);
            lblEstado.ForeColor = Color.FromArgb(206, 212, 218);

            txtNombre.BackColor = Color.FromArgb(49, 53, 56);
            txtNombre.ForeColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;

            txtApellido.BackColor = Color.FromArgb(49, 53, 56);
            txtApellido.ForeColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;

            cmbTurno.BackColor = Color.FromArgb(49, 53, 56);
            cmbTurno.ForeColor = Color.White;
            cmbTurno.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbCursoEscolar.BackColor = Color.FromArgb(49, 53, 56);
            cmbCursoEscolar.ForeColor = Color.White;
            cmbCursoEscolar.DropDownStyle = ComboBoxStyle.DropDownList;

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
                int index = cmbEstado.Items.IndexOf(_alumnoExistente.Estado);
                if (index >= 0) cmbEstado.SelectedIndex = index;
            }
        }

        private async void AlumnoDetalleForm_Load(object sender, EventArgs e)
        {
            try
            {
                _cursos = await _cursoApiClient.GetAllAsync();
                if (_cursos.Count == 0)
                {
                    MessageBox.Show("Primero debe registrar al menos un curso escolar.", "Sin cursos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnGuardar.Enabled = false;
                    return;
                }

                cmbTurno.Items.Clear();
                cmbTurno.Items.AddRange(_cursos.Select(c => c.Turno).Distinct().OrderBy(t => t).Cast<object>().ToArray());

                if (_isEditMode && _alumnoExistente != null)
                {
                    cmbTurno.SelectedItem = _alumnoExistente.Turno;
                    CargarCursosDelTurno(_alumnoExistente.IdCurso);
                }
                else
                {
                    lblTitle.Text = "Nuevo Alumno";
                    cmbEstado.SelectedIndex = 0;
                    cmbTurno.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = false;
            }
        }

        private void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCursosDelTurno();
        }

        private void CargarCursosDelTurno(int? idCursoSeleccionado = null)
        {
            string? turno = cmbTurno.SelectedItem?.ToString();
            var opciones = _cursos
                .Where(c => c.Turno == turno)
                .OrderBy(c => c.Grado).ThenBy(c => c.Curso)
                .Select(c => new CursoOpcion(c.IdCurso, $"{c.Grado} {c.Curso}"))
                .ToList();

            cmbCursoEscolar.DataSource = opciones;
            cmbCursoEscolar.DisplayMember = nameof(CursoOpcion.Descripcion);
            cmbCursoEscolar.ValueMember = nameof(CursoOpcion.IdCurso);
            if (idCursoSeleccionado.HasValue)
                cmbCursoEscolar.SelectedValue = idCursoSeleccionado.Value;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string estado = cmbEstado.SelectedItem?.ToString() ?? "Presente";
            int idCurso = cmbCursoEscolar.SelectedValue is int valor ? valor : 0;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
                idCurso == 0)
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
                        IdCurso = idCurso,
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
                        IdCurso = idCurso,
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

        private sealed record CursoOpcion(int IdCurso, string Descripcion);

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
