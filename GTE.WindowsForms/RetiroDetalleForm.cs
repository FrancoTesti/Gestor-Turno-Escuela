using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;
using GTE.DTOs;

namespace GTE.WindowsForms
{
    public partial class RetiroDetalleForm : Form
    {
        private readonly RetiroApiClient _apiClient = new RetiroApiClient();
        private readonly RetiroDTO? _retiroExistente;
        private readonly bool _isEditMode;

        private List<TutorDTO> _tutores = new();
        private List<PorteroDTO> _personal = new();
        private List<AlumnoDTO> _alumnosAutorizados = new();
        private readonly BindingList<DetalleRetiroDTO> _detalles = new();

        public RetiroDetalleForm()
        {
            InitializeComponent();
            _isEditMode = false;
            ApplyStyles();
        }

        public RetiroDetalleForm(RetiroDTO retiro) : this()
        {
            _retiroExistente = retiro;
            _isEditMode = true;
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(33, 37, 41);

            lblTitle.ForeColor = Color.White;
            pnlCabecera.BackColor = Color.FromArgb(40, 44, 52);
            grpDetalle.ForeColor = Color.White;

            cmbTutor.BackColor = Color.FromArgb(49, 53, 56);
            cmbTutor.ForeColor = Color.White;
            cmbPersonal.BackColor = Color.FromArgb(49, 53, 56);
            cmbPersonal.ForeColor = Color.White;
            txtObservaciones.BackColor = Color.FromArgb(49, 53, 56);
            txtObservaciones.ForeColor = Color.White;
            txtObservaciones.BorderStyle = BorderStyle.FixedSingle;

            cmbAlumno.BackColor = Color.FromArgb(49, 53, 56);
            cmbAlumno.ForeColor = Color.White;
            cmbEstado.BackColor = Color.FromArgb(49, 53, 56);
            cmbEstado.ForeColor = Color.White;

            btnAgregarAlumno.BackColor = Color.FromArgb(40, 167, 69);
            btnAgregarAlumno.ForeColor = Color.White;
            btnAgregarAlumno.FlatStyle = FlatStyle.Flat;
            btnAgregarAlumno.FlatAppearance.BorderSize = 0;

            btnQuitarAlumno.BackColor = Color.FromArgb(220, 53, 69);
            btnQuitarAlumno.ForeColor = Color.White;
            btnQuitarAlumno.FlatStyle = FlatStyle.Flat;
            btnQuitarAlumno.FlatAppearance.BorderSize = 0;

            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.FlatAppearance.BorderSize = 0;

            btnCancelar.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;

            dgvDetalles.BackgroundColor = Color.FromArgb(49, 53, 56);
            dgvDetalles.ForeColor = Color.Black;
            dgvDetalles.BorderStyle = BorderStyle.None;
            dgvDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalles.MultiSelect = false;
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 243, 245);

            ConfigureDetallesGrid();
        }

        private void ConfigureDetallesGrid()
        {
            dgvDetalles.Columns.Clear();

            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DetalleRetiroDTO.IdAlumno),
                HeaderText = "ID Alumno",
                Width = 90,
                ReadOnly = true
            };

            var colNombre = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DetalleRetiroDTO.AlumnoNombreCompleto),
                HeaderText = "Alumno",
                Width = 220,
                ReadOnly = true
            };

            var colHora = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DetalleRetiroDTO.HoraSalida),
                HeaderText = "Hora de Salida",
                Width = 140,
                ReadOnly = true
            };

            var colEstado = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DetalleRetiroDTO.Estado),
                HeaderText = "Estado",
                Width = 130,
                ReadOnly = true
            };

            dgvDetalles.Columns.AddRange(colId, colNombre, colHora, colEstado);
            dgvDetalles.DataSource = _detalles;
        }

        private async void RetiroDetalleForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar tutores y personal
                _tutores = await _apiClient.GetTutoresAsync();
                _personal = await _apiClient.GetPersonalAsync();

                if (_tutores.Count == 0)
                {
                    MessageBox.Show("No se encontraron tutores registrados en el sistema.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                cmbTutor.DisplayMember = "NombreCompleto";
                cmbTutor.ValueMember = "IdTutor";
                cmbTutor.DataSource = _tutores.Select(t => new
                {
                    t.IdTutor,
                    NombreCompleto = $"{t.Nombre} {t.Apellido} (DNI: {t.Dni})"
                }).ToList();

                cmbPersonal.DisplayMember = "NombreCompleto";
                cmbPersonal.ValueMember = "IdPersonal";
                cmbPersonal.DataSource = _personal.Select(p => new
                {
                    p.IdPersonal,
                    NombreCompleto = $"{p.Nombre} ({p.PuertaAsignada})"
                }).ToList();

                cmbEstado.SelectedIndex = 0;
                dtpHoraSalida.Value = DateTime.Now;

                if (_isEditMode && _retiroExistente != null)
                {
                    lblTitle.Text = $"Editar Retiro #{_retiroExistente.IdRetiro}";
                    cmbTutor.SelectedValue = _retiroExistente.IdTutor;
                    cmbPersonal.SelectedValue = _retiroExistente.IdPersonal;
                    dtpFechaHora.Value = _retiroExistente.FechaHora;
                    txtObservaciones.Text = _retiroExistente.Observaciones;

                    // Cargar alumnos autorizados
                    await CargarAlumnosAutorizados(_retiroExistente.IdTutor);

                    // Cargar líneas existentes
                    _detalles.Clear();
                    foreach (var det in _retiroExistente.Detalles)
                    {
                        _detalles.Add(new DetalleRetiroDTO
                        {
                            IdDetalleRetiro = det.IdDetalleRetiro,
                            IdRetiro = det.IdRetiro,
                            IdAlumno = det.IdAlumno,
                            AlumnoNombreCompleto = det.AlumnoNombreCompleto,
                            HoraSalida = det.HoraSalida,
                            Estado = det.Estado
                        });
                    }
                }
                else
                {
                    lblTitle.Text = "Nuevo Retiro de Alumnos";
                    dtpFechaHora.Value = DateTime.Now;

                    if (cmbTutor.SelectedValue is int tutorId)
                    {
                        await CargarAlumnosAutorizados(tutorId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar formulario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmbTutor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTutor.SelectedValue is int tutorId)
            {
                await CargarAlumnosAutorizados(tutorId);
            }
        }

        private async Task CargarAlumnosAutorizados(int tutorId)
        {
            try
            {
                _alumnosAutorizados = await _apiClient.GetAlumnosAutorizadosByTutorAsync(tutorId);

                cmbAlumno.DisplayMember = "NombreCompleto";
                cmbAlumno.ValueMember = "IdAlumno";
                cmbAlumno.DataSource = _alumnosAutorizados.Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = $"{a.Nombre} {a.Apellido} ({a.Grado} {a.Curso} - {a.Estado})"
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos autorizados: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarAlumno_Click(object sender, EventArgs e)
        {
            if (cmbAlumno.SelectedValue is not int idAlumno || idAlumno <= 0)
            {
                MessageBox.Show("Por favor, seleccione un alumno autorizado de la lista.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar si ya está agregado en la grilla
            if (_detalles.Any(d => d.IdAlumno == idAlumno))
            {
                MessageBox.Show("El alumno ya se encuentra agregado a la lista de este retiro.", "Alumno Duplicado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var alumnoObj = _alumnosAutorizados.FirstOrDefault(a => a.IdAlumno == idAlumno);
            string nombreCompleto = alumnoObj != null ? $"{alumnoObj.Nombre} {alumnoObj.Apellido}" : $"Alumno #{idAlumno}";
            TimeSpan horaSalida = dtpHoraSalida.Value.TimeOfDay;
            string estado = cmbEstado.SelectedItem?.ToString() ?? "Retirado";

            _detalles.Add(new DetalleRetiroDTO
            {
                IdDetalleRetiro = 0,
                IdRetiro = _retiroExistente?.IdRetiro ?? 0,
                IdAlumno = idAlumno,
                AlumnoNombreCompleto = nombreCompleto,
                HoraSalida = horaSalida,
                Estado = estado
            });
        }

        private void btnQuitarAlumno_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una línea de detalle para quitar.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvDetalles.SelectedRows[0].DataBoundItem as DetalleRetiroDTO;
            if (selected != null)
            {
                _detalles.Remove(selected);
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbTutor.SelectedValue is not int idTutor || idTutor <= 0)
            {
                MessageBox.Show("Debe seleccionar un tutor válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPersonal.SelectedValue is not int idPersonal || idPersonal <= 0)
            {
                MessageBox.Show("Debe seleccionar el personal que entrega al alumno.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_detalles.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un alumno al detalle del retiro.", "Validación Maestro/Detalle",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new RetiroDTO
            {
                IdRetiro = _isEditMode && _retiroExistente != null ? _retiroExistente.IdRetiro : 0,
                IdTutor = idTutor,
                IdPersonal = idPersonal,
                FechaHora = dtpFechaHora.Value,
                Observaciones = txtObservaciones.Text.Trim(),
                Detalles = _detalles.ToList()
            };

            try
            {
                if (_isEditMode)
                {
                    var actualizado = await _apiClient.UpdateAsync(dto);
                    if (actualizado != null)
                    {
                        MessageBox.Show("Retiro actualizado correctamente con sus líneas de detalle.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    var creado = await _apiClient.AddAsync(dto);
                    if (creado != null)
                    {
                        MessageBox.Show($"Retiro #{creado.IdRetiro} registrado con éxito con {creado.Detalles.Count} alumno(s).",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores de negocio sin cerrar el formulario
                MessageBox.Show(ex.Message, "Error de Validación de Retiro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
