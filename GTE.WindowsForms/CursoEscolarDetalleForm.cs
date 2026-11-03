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
            Tema.AcomodarControles(this);
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
            lblTurno.ForeColor = Color.FromArgb(206, 212, 218);
            lblHorariosSemana.ForeColor = Color.FromArgb(206, 212, 218);
            lblLunes.ForeColor = Color.FromArgb(206, 212, 218);
            lblMartes.ForeColor = Color.FromArgb(206, 212, 218);
            lblMiercoles.ForeColor = Color.FromArgb(206, 212, 218);
            lblJueves.ForeColor = Color.FromArgb(206, 212, 218);
            lblViernes.ForeColor = Color.FromArgb(206, 212, 218);

            txtGrado.BackColor = Color.FromArgb(49, 53, 56);
            txtGrado.ForeColor = Color.White;
            txtGrado.BorderStyle = BorderStyle.FixedSingle;

            txtCurso.BackColor = Color.FromArgb(49, 53, 56);
            txtCurso.ForeColor = Color.White;
            txtCurso.BorderStyle = BorderStyle.FixedSingle;

            cmbTurno.BackColor = Color.FromArgb(49, 53, 56);
            cmbTurno.ForeColor = Color.White;
            cmbTurno.DropDownStyle = ComboBoxStyle.DropDownList;

            Tema.Entrada(dtpLunes);
            Tema.Entrada(dtpMartes);
            Tema.Entrada(dtpMiercoles);
            Tema.Entrada(dtpJueves);
            Tema.Entrada(dtpViernes);

            btnCopiarSemana.BackColor = Color.FromArgb(23, 162, 184);
            btnCopiarSemana.ForeColor = Color.White;
            btnCopiarSemana.FlatStyle = FlatStyle.Flat;
            btnCopiarSemana.FlatAppearance.BorderSize = 0;

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
                cmbTurno.SelectedItem = _cursoExistente.Turno;

                TimeSpan fallback = _cursoExistente.HorarioSalida != TimeSpan.Zero
                    ? _cursoExistente.HorarioSalida
                    : new TimeSpan(12, 0, 0);

                TimeSpan lun = _cursoExistente.HorarioSalidaLunes != TimeSpan.Zero ? _cursoExistente.HorarioSalidaLunes : fallback;
                TimeSpan mar = _cursoExistente.HorarioSalidaMartes != TimeSpan.Zero ? _cursoExistente.HorarioSalidaMartes : fallback;
                TimeSpan mie = _cursoExistente.HorarioSalidaMiercoles != TimeSpan.Zero ? _cursoExistente.HorarioSalidaMiercoles : fallback;
                TimeSpan jue = _cursoExistente.HorarioSalidaJueves != TimeSpan.Zero ? _cursoExistente.HorarioSalidaJueves : fallback;
                TimeSpan vie = _cursoExistente.HorarioSalidaViernes != TimeSpan.Zero ? _cursoExistente.HorarioSalidaViernes : fallback;

                DateTime baseDate = DateTime.Today;
                dtpLunes.Value = baseDate.Add(lun);
                dtpMartes.Value = baseDate.Add(mar);
                dtpMiercoles.Value = baseDate.Add(mie);
                dtpJueves.Value = baseDate.Add(jue);
                dtpViernes.Value = baseDate.Add(vie);
            }
        }

        private void CursoEscolarDetalleForm_Load(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                lblTitle.Text = "Nuevo Curso Escolar";
                var defaultTime = DateTime.Today.AddHours(12);
                dtpLunes.Value = defaultTime;
                dtpMartes.Value = defaultTime;
                dtpMiercoles.Value = defaultTime;
                dtpJueves.Value = defaultTime;
                dtpViernes.Value = defaultTime;
                if (cmbTurno.Items.Count > 0)
                    cmbTurno.SelectedIndex = 0;
            }
        }

        private void btnCopiarSemana_Click(object sender, EventArgs e)
        {
            var horarioLunes = dtpLunes.Value;
            dtpMartes.Value = horarioLunes;
            dtpMiercoles.Value = horarioLunes;
            dtpJueves.Value = horarioLunes;
            dtpViernes.Value = horarioLunes;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string grado = txtGrado.Text.Trim();
            string curso = txtCurso.Text.Trim();
            string turno = cmbTurno.SelectedItem?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(grado) || string.IsNullOrEmpty(curso) || string.IsNullOrEmpty(turno))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Campos vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan horarioLunes = new TimeSpan(dtpLunes.Value.Hour, dtpLunes.Value.Minute, 0);
            TimeSpan horarioMartes = new TimeSpan(dtpMartes.Value.Hour, dtpMartes.Value.Minute, 0);
            TimeSpan horarioMiercoles = new TimeSpan(dtpMiercoles.Value.Hour, dtpMiercoles.Value.Minute, 0);
            TimeSpan horarioJueves = new TimeSpan(dtpJueves.Value.Hour, dtpJueves.Value.Minute, 0);
            TimeSpan horarioViernes = new TimeSpan(dtpViernes.Value.Hour, dtpViernes.Value.Minute, 0);

            try
            {
                if (_isEditMode && _cursoExistente != null)
                {
                    var dto = new CursoEscolarDTO
                    {
                        IdCurso = _cursoExistente.IdCurso,
                        Grado = grado,
                        Curso = curso,
                        Turno = turno,
                        HorarioSalida = horarioLunes,
                        HorarioSalidaLunes = horarioLunes,
                        HorarioSalidaMartes = horarioMartes,
                        HorarioSalidaMiercoles = horarioMiercoles,
                        HorarioSalidaJueves = horarioJueves,
                        HorarioSalidaViernes = horarioViernes
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
                        Turno = turno,
                        HorarioSalida = horarioLunes,
                        HorarioSalidaLunes = horarioLunes,
                        HorarioSalidaMartes = horarioMartes,
                        HorarioSalidaMiercoles = horarioMiercoles,
                        HorarioSalidaJueves = horarioJueves,
                        HorarioSalidaViernes = horarioViernes
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
