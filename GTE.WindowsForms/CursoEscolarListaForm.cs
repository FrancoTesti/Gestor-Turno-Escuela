using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTE.DTOs;
using GTE.Clients;

namespace GTE.WindowsForms
{
    public partial class CursoEscolarListaForm : Form
    {
        private readonly CursoEscolarApiClient _apiClient = new CursoEscolarApiClient();

        public CursoEscolarListaForm()
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
            btnNuevo.FlatAppearance.BorderSize = 0;

            btnEditar.BackColor = Color.FromArgb(23, 162, 184);
            btnEditar.ForeColor = Color.White;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;

            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;

            Tema.Grilla(dgvCursos);
        }

        private async void CursoEscolarListaForm_Load(object sender, EventArgs e)
        {
            await AplicarPermisosSegunRol();
            await RefreshGrid();
        }

        private async Task AplicarPermisosSegunRol()
        {
            string? rol = await AuthServiceProvider.Instance.GetRoleAsync();
            bool puedeAdministrar = PermisosDeUsuario.PuedeAdministrarCursos(rol);

            btnNuevo.Visible = puedeAdministrar;
            btnEditar.Visible = puedeAdministrar;
            btnEliminar.Visible = puedeAdministrar;
        }

        private async Task RefreshGrid()
        {
            try
            {
                var cursos = await _apiClient.GetAllAsync();
                dgvCursos.DataSource = cursos;
                ConfigureColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cursos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureColumns()
        {
            if (dgvCursos.Columns.Count == 0)
                return;

            if (dgvCursos.Columns["IdCurso"] != null)
            {
                dgvCursos.Columns["IdCurso"].HeaderText = "ID";
                dgvCursos.Columns["IdCurso"].Width = 50;
                dgvCursos.Columns["IdCurso"].DisplayIndex = 0;
            }
            if (dgvCursos.Columns["Grado"] != null)
            {
                dgvCursos.Columns["Grado"].HeaderText = "Grado";
                dgvCursos.Columns["Grado"].Width = 80;
                dgvCursos.Columns["Grado"].DisplayIndex = 1;
            }
            if (dgvCursos.Columns["Curso"] != null)
            {
                dgvCursos.Columns["Curso"].HeaderText = "División";
                dgvCursos.Columns["Curso"].Width = 80;
                dgvCursos.Columns["Curso"].DisplayIndex = 2;
            }
            if (dgvCursos.Columns["Turno"] != null)
            {
                dgvCursos.Columns["Turno"].HeaderText = "Turno";
                dgvCursos.Columns["Turno"].Width = 80;
                dgvCursos.Columns["Turno"].DisplayIndex = 3;
            }
            if (dgvCursos.Columns["HorarioSalidaLunes"] != null)
            {
                dgvCursos.Columns["HorarioSalidaLunes"].HeaderText = "Lun";
                dgvCursos.Columns["HorarioSalidaLunes"].Width = 65;
                dgvCursos.Columns["HorarioSalidaLunes"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalidaLunes"].DisplayIndex = 4;
            }
            if (dgvCursos.Columns["HorarioSalidaMartes"] != null)
            {
                dgvCursos.Columns["HorarioSalidaMartes"].HeaderText = "Mar";
                dgvCursos.Columns["HorarioSalidaMartes"].Width = 65;
                dgvCursos.Columns["HorarioSalidaMartes"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalidaMartes"].DisplayIndex = 5;
            }
            if (dgvCursos.Columns["HorarioSalidaMiercoles"] != null)
            {
                dgvCursos.Columns["HorarioSalidaMiercoles"].HeaderText = "Mié";
                dgvCursos.Columns["HorarioSalidaMiercoles"].Width = 65;
                dgvCursos.Columns["HorarioSalidaMiercoles"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalidaMiercoles"].DisplayIndex = 6;
            }
            if (dgvCursos.Columns["HorarioSalidaJueves"] != null)
            {
                dgvCursos.Columns["HorarioSalidaJueves"].HeaderText = "Jue";
                dgvCursos.Columns["HorarioSalidaJueves"].Width = 65;
                dgvCursos.Columns["HorarioSalidaJueves"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalidaJueves"].DisplayIndex = 7;
            }
            if (dgvCursos.Columns["HorarioSalidaViernes"] != null)
            {
                dgvCursos.Columns["HorarioSalidaViernes"].HeaderText = "Vie";
                dgvCursos.Columns["HorarioSalidaViernes"].Width = 65;
                dgvCursos.Columns["HorarioSalidaViernes"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalidaViernes"].DisplayIndex = 8;
            }
            if (dgvCursos.Columns["HorarioSalida"] != null)
            {
                dgvCursos.Columns["HorarioSalida"].HeaderText = "Salida Hoy";
                dgvCursos.Columns["HorarioSalida"].Width = 90;
                dgvCursos.Columns["HorarioSalida"].DefaultCellStyle.Format = @"hh\:mm";
                dgvCursos.Columns["HorarioSalida"].DisplayIndex = 9;
            }
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            var detailForm = new CursoEscolarDetalleForm();
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un curso de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvCursos.SelectedRows[0].DataBoundItem as CursoEscolarDTO;
            if (selected == null) return;

            var detailForm = new CursoEscolarDetalleForm(selected);
            if (detailForm.ShowDialog() == DialogResult.OK)
            {
                await RefreshGrid();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCursos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un curso de la grilla.", "Seleccionar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dgvCursos.SelectedRows[0].DataBoundItem as CursoEscolarDTO;
            if (selected == null) return;

            if (MessageBox.Show($"¿Está seguro de que desea eliminar el curso {selected.Grado} {selected.Curso}?",
                "Eliminar Curso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool ok = await _apiClient.DeleteAsync(selected.IdCurso);
                    if (ok)
                    {
                        MessageBox.Show("Curso eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el curso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
