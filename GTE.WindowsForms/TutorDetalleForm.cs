using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class TutorDetalleForm : Form
    {
        private readonly TutorApiClient _apiClient = new TutorApiClient();
        private readonly TutorDTO? _tutorExistente;
        private readonly bool _isEditMode;

        public TutorDetalleForm()
        {
            InitializeComponent();
            _isEditMode = false;
            ApplyStyles();
        }

        public TutorDetalleForm(TutorDTO tutor) : this()
        {
            _tutorExistente = tutor;
            _isEditMode = true;
            CargarDatos();
        }

        private void ApplyStyles()
        {
            this.BackColor = Color.FromArgb(33, 37, 41);

            foreach (var etiqueta in new[] { lblTitle, lblNombre, lblApellido, lblDni,
                                             lblParentesco, lblTelefono, lblUsuario, lblContrasena })
            {
                etiqueta.ForeColor = etiqueta == lblTitle
                    ? Color.White
                    : Color.FromArgb(206, 212, 218);
            }

            foreach (var campo in new[] { txtNombre, txtApellido, txtDni, txtParentesco,
                                          txtTelefono, txtUsuario, txtContrasena })
            {
                campo.BackColor = Color.FromArgb(49, 53, 56);
                campo.ForeColor = Color.White;
                campo.BorderStyle = BorderStyle.FixedSingle;
            }

            chkRestriccion.ForeColor = Color.FromArgb(206, 212, 218);

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
            if (_tutorExistente == null) return;

            lblTitle.Text = "Editar Tutor";
            txtNombre.Text = _tutorExistente.Nombre;
            txtApellido.Text = _tutorExistente.Apellido;
            txtDni.Text = _tutorExistente.Dni;
            txtParentesco.Text = _tutorExistente.Parentesco;
            txtTelefono.Text = _tutorExistente.Telefono;
            txtUsuario.Text = _tutorExistente.NombreUsuario;
            chkRestriccion.Checked = _tutorExistente.TieneRestriccion;

            // El usuario y la contraseña se definen al crear el tutor; la
            // modificación solo cambia los datos del tutor.
            txtUsuario.Enabled = false;
            txtContrasena.Enabled = false;
            txtContrasena.PlaceholderText = "No se modifica desde acá";
        }

        private void TutorDetalleForm_Load(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                lblTitle.Text = "Nuevo Tutor";
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string dni = txtDni.Text.Trim();
            string parentesco = txtParentesco.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(parentesco) || string.IsNullOrEmpty(telefono))
            {
                Avisar("Por favor complete todos los campos.");
                return;
            }

            if (!Regex.IsMatch(dni, @"^\d{7,8}$"))
            {
                Avisar("El DNI debe tener entre 7 y 8 dígitos numéricos.");
                return;
            }

            if (!_isEditMode && string.IsNullOrEmpty(usuario))
            {
                Avisar("Debe indicar el nombre de usuario del tutor.");
                return;
            }

            if (!_isEditMode && contrasena.Length < 4)
            {
                Avisar("La contraseña debe tener al menos 4 caracteres.");
                return;
            }

            var dto = new TutorDTO
            {
                IdTutor = _tutorExistente?.IdTutor ?? 0,
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Parentesco = parentesco,
                Telefono = telefono,
                TieneRestriccion = chkRestriccion.Checked,
                NombreUsuario = usuario,
                Contrasena = contrasena
            };

            try
            {
                if (_isEditMode)
                {
                    bool ok = await _apiClient.UpdateAsync(dto);
                    if (!ok)
                    {
                        Avisar("No se pudo actualizar el tutor.", MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Tutor actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _apiClient.AddAsync(dto);
                    MessageBox.Show("Tutor creado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // La API devuelve acá los errores de negocio, como el DNI repetido.
                Avisar(ex.Message, MessageBoxIcon.Error);
            }
        }

        private static void Avisar(string mensaje, MessageBoxIcon icono = MessageBoxIcon.Warning)
        {
            MessageBox.Show(mensaje, "Tutor", MessageBoxButtons.OK, icono);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
