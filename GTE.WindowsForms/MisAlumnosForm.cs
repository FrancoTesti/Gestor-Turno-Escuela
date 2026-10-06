using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.Clients;

namespace GTE.WindowsForms
{
    /// <summary>
    /// Pantalla del tutor: los alumnos que tiene autorizados a retirar, con su
    /// curso, el horario de salida y el estado.
    /// </summary>
    public partial class MisAlumnosForm : Form
    {
        private readonly TutorApiClient _apiClient = new TutorApiClient();

        public MisAlumnosForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            Tema.Ventana(this);
            Tema.Titulo(lblTitle);
            Tema.Etiqueta(lblSub);
            Tema.BotonSecundario(btnActualizar);
            Tema.Grilla(dgvAlumnos);
        }

        private async void MisAlumnosForm_Load(object sender, EventArgs e)
        {
            await CargarAsync();
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                var alumnos = await _apiClient.GetMisAlumnosAsync();

                dgvAlumnos.DataSource = alumnos;

                if (dgvAlumnos.Columns.Count > 0)
                {
                    dgvAlumnos.Columns["IdAlumno"].Visible = false;

                    dgvAlumnos.Columns["Nombre"].HeaderText = "Nombre";
                    dgvAlumnos.Columns["Nombre"].Width = 150;
                    dgvAlumnos.Columns["Apellido"].HeaderText = "Apellido";
                    dgvAlumnos.Columns["Apellido"].Width = 150;
                    dgvAlumnos.Columns["Grado"].HeaderText = "Grado";
                    dgvAlumnos.Columns["Grado"].Width = 80;
                    dgvAlumnos.Columns["Curso"].HeaderText = "División";
                    dgvAlumnos.Columns["Curso"].Width = 80;
                    dgvAlumnos.Columns["Turno"].HeaderText = "Turno";
                    dgvAlumnos.Columns["Turno"].Width = 100;
                    dgvAlumnos.Columns["HorarioSalida"].HeaderText = "Horario de salida";
                    dgvAlumnos.Columns["HorarioSalida"].Width = 140;
                    dgvAlumnos.Columns["Estado"].HeaderText = "Estado";
                    dgvAlumnos.Columns["Estado"].Width = 100;
                    dgvAlumnos.Columns["EstaSaliendo"].HeaderText = "¿Está saliendo?";
                    dgvAlumnos.Columns["EstaSaliendo"].Width = 130;
                }

                if (alumnos.Count == 0)
                {
                    MessageBox.Show(
                        "Todavía no tenés alumnos autorizados a tu cargo. Consultá con la secretaría.",
                        "Mis alumnos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar tus alumnos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
