using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public partial class AutorizacionListaForm : Form
    {
        private readonly AutorizacionApiClient _apiClient = new AutorizacionApiClient();
        private List<TutorDTO> _tutores = new();
        private List<AlumnoDTO> _todosLosAlumnos = new();
        private List<AlumnoDTO> _alumnosAutorizados = new();

        public AutorizacionListaForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);

            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTutor.ForeColor = Color.FromArgb(73, 80, 87);
            lblAlumno.ForeColor = Color.FromArgb(73, 80, 87);

            btnAgregar.BackColor = Color.FromArgb(40, 167, 69);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.FlatAppearance.BorderSize = 0;

            btnQuitar.BackColor = Color.FromArgb(220, 53, 69);
            btnQuitar.ForeColor = Color.White;
            btnQuitar.FlatStyle = FlatStyle.Flat;
            btnQuitar.FlatAppearance.BorderSize = 0;

            Tema.Grilla(dgvAlumnosAutorizados);
        }

        private async void AutorizacionListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();
            await CargarDatosIniciales();
        }

        private async Task AplicarPermisosSegunRol()
        {
            string? rol = await AuthServiceProvider.Instance.GetRoleAsync();
            bool esSecretario = rol == "Secretario";

            pnlActions.Visible = esSecretario;
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                _tutores = await _apiClient.GetTutoresAsync();
                _todosLosAlumnos = await _apiClient.GetAllAlumnosAsync();

                cmbTutores.DataSource = null;
                var tutoresDisplay = _tutores.Select(t => new
                {
                    t.IdTutor,
                    Display = $"{t.Nombre} {t.Apellido} (DNI: {t.Dni})"
                }).ToList();

                cmbTutores.DisplayMember = "Display";
                cmbTutores.ValueMember = "IdTutor";
                cmbTutores.DataSource = tutoresDisplay;

                ActualizarComboAlumnosDisponibles();

                if (_tutores.Count > 0)
                {
                    await CargarAlumnosAutorizados(_tutores[0].IdTutor);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmbTutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTutores.SelectedValue is int tutorId)
            {
                await CargarAlumnosAutorizados(tutorId);
            }
        }

        private async Task CargarAlumnosAutorizados(int tutorId)
        {
            try
            {
                _alumnosAutorizados = await _apiClient.GetAlumnosAutorizadosByTutorAsync(tutorId);
                dgvAlumnosAutorizados.DataSource = null;
                dgvAlumnosAutorizados.DataSource = _alumnosAutorizados;
                ConfigureColumns();
                ActualizarComboAlumnosDisponibles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos autorizados: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarComboAlumnosDisponibles()
        {
            var autorizadosIds = _alumnosAutorizados.Select(a => a.IdAlumno).ToHashSet();
            var disponibles = _todosLosAlumnos
                .Where(a => !autorizadosIds.Contains(a.IdAlumno))
                .Select(a => new
                {
                    a.IdAlumno,
                    Display = $"{a.Nombre} {a.Apellido} - {a.Grado} {a.Curso} ({a.Turno})"
                }).ToList();

            cmbAlumnos.DataSource = null;
            cmbAlumnos.DisplayMember = "Display";
            cmbAlumnos.ValueMember = "IdAlumno";
            cmbAlumnos.DataSource = disponibles;
        }

        private void ConfigureColumns()
        {
            if (dgvAlumnosAutorizados.Columns.Count > 0)
            {
                dgvAlumnosAutorizados.Columns["IdAlumno"].HeaderText = "ID";
                dgvAlumnosAutorizados.Columns["IdAlumno"].Width = 60;
                if (dgvAlumnosAutorizados.Columns.Contains("IdCurso"))
                    dgvAlumnosAutorizados.Columns["IdCurso"].Visible = false;
                dgvAlumnosAutorizados.Columns["Nombre"].HeaderText = "Nombre";
                dgvAlumnosAutorizados.Columns["Nombre"].Width = 140;
                dgvAlumnosAutorizados.Columns["Apellido"].HeaderText = "Apellido";
                dgvAlumnosAutorizados.Columns["Apellido"].Width = 140;
                dgvAlumnosAutorizados.Columns["Grado"].HeaderText = "Grado";
                dgvAlumnosAutorizados.Columns["Grado"].Width = 90;
                dgvAlumnosAutorizados.Columns["Curso"].HeaderText = "Curso";
                dgvAlumnosAutorizados.Columns["Curso"].Width = 90;
                dgvAlumnosAutorizados.Columns["Turno"].HeaderText = "Turno";
                dgvAlumnosAutorizados.Columns["Turno"].Width = 90;
                dgvAlumnosAutorizados.Columns["Estado"].HeaderText = "Estado";
                dgvAlumnosAutorizados.Columns["Estado"].Width = 100;
            }
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cmbTutores.SelectedValue is not int tutorId || tutorId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un tutor válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbAlumnos.SelectedValue is not int alumnoId || alumnoId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno para autorizar.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_alumnosAutorizados.Any(a => a.IdAlumno == alumnoId))
            {
                MessageBox.Show("El alumno ya se encuentra autorizado para este tutor.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dto = new AutorizacionDTO
                {
                    TutorId = tutorId,
                    AlumnoId = alumnoId
                };

                await _apiClient.AddAsync(dto);
                MessageBox.Show("Alumno autorizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarAlumnosAutorizados(tutorId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al autorizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnQuitar_Click(object sender, EventArgs e)
        {
            if (cmbTutores.SelectedValue is not int tutorId || tutorId <= 0)
            {
                MessageBox.Show("Por favor, seleccione un tutor válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvAlumnosAutorizados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno de la grilla para quitar.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var alumnoSeleccionado = dgvAlumnosAutorizados.SelectedRows[0].DataBoundItem as AlumnoDTO;
            if (alumnoSeleccionado == null) return;

            if (MessageBox.Show($"¿Está seguro de que desea remover la autorización para el alumno {alumnoSeleccionado.Nombre} {alumnoSeleccionado.Apellido}?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool eliminado = await _apiClient.DeleteByTutorAndAlumnoAsync(tutorId, alumnoSeleccionado.IdAlumno);
                    if (eliminado)
                    {
                        MessageBox.Show("Autorización removida con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarAlumnosAutorizados(tutorId);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo remover la autorización.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al quitar autorización: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
