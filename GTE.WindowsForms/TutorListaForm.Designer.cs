namespace GTE.WindowsForms
{
    partial class TutorListaForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFiltroNombre = new System.Windows.Forms.Label();
            this.txtFiltroNombre = new System.Windows.Forms.TextBox();
            this.lblFiltroGrado = new System.Windows.Forms.Label();
            this.lblFiltroDivision = new System.Windows.Forms.Label();
            this.lblFiltroTurno = new System.Windows.Forms.Label();
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.cmbFiltroDivision = new System.Windows.Forms.ComboBox();
            this.cmbFiltroTurno = new System.Windows.Forms.ComboBox();
            this.cmbFiltroEstado = new System.Windows.Forms.ComboBox();
            this.cmbFiltroGrado = new System.Windows.Forms.ComboBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvTutores = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTutores)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(230, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestión de Tutores";
            // 
            // lblFiltroNombre
            // 
            this.lblFiltroNombre.AutoSize = true;
            this.lblFiltroNombre.Location = new System.Drawing.Point(20, 72);
            this.lblFiltroNombre.Name = "lblFiltroNombre";
            this.lblFiltroNombre.Size = new System.Drawing.Size(170, 20);
            this.lblFiltroNombre.TabIndex = 20;
            this.lblFiltroNombre.Text = "Buscar por nombre o DNI";
            // 
            // txtFiltroNombre
            // 
            this.txtFiltroNombre.Location = new System.Drawing.Point(20, 96);
            this.txtFiltroNombre.Name = "txtFiltroNombre";
            this.txtFiltroNombre.Size = new System.Drawing.Size(180, 30);
            this.txtFiltroNombre.TabIndex = 21;
            // 
            // 
            // lblFiltroDivision
            // 
            this.lblFiltroDivision.AutoSize = true;
            this.lblFiltroDivision.Location = new System.Drawing.Point(320, 72);
            this.lblFiltroDivision.Name = "lblFiltroDivision";
            this.lblFiltroDivision.Size = new System.Drawing.Size(70, 20);
            this.lblFiltroDivision.Text = "División";
            // 
            // cmbFiltroDivision
            // 
            this.cmbFiltroDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroDivision.Location = new System.Drawing.Point(320, 96);
            this.cmbFiltroDivision.Name = "cmbFiltroDivision";
            this.cmbFiltroDivision.Size = new System.Drawing.Size(90, 30);
            // 
            // lblFiltroTurno
            // 
            this.lblFiltroTurno.AutoSize = true;
            this.lblFiltroTurno.Location = new System.Drawing.Point(420, 72);
            this.lblFiltroTurno.Name = "lblFiltroTurno";
            this.lblFiltroTurno.Size = new System.Drawing.Size(60, 20);
            this.lblFiltroTurno.Text = "Turno";
            // 
            // cmbFiltroTurno
            // 
            this.cmbFiltroTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroTurno.Location = new System.Drawing.Point(420, 96);
            this.cmbFiltroTurno.Name = "cmbFiltroTurno";
            this.cmbFiltroTurno.Size = new System.Drawing.Size(110, 30);
            // 
            // lblFiltroEstado
            // 
            this.lblFiltroEstado.AutoSize = true;
            this.lblFiltroEstado.Location = new System.Drawing.Point(540, 72);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(60, 20);
            this.lblFiltroEstado.Text = "Estado del alumno";
            // 
            // cmbFiltroEstado
            // 
            this.cmbFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroEstado.Location = new System.Drawing.Point(540, 96);
            this.cmbFiltroEstado.Name = "cmbFiltroEstado";
            this.cmbFiltroEstado.Size = new System.Drawing.Size(110, 30);
            //             // lblFiltroGrado
            // 
            this.lblFiltroGrado.AutoSize = true;
            this.lblFiltroGrado.Location = new System.Drawing.Point(210, 72);
            this.lblFiltroGrado.Name = "lblFiltroGrado";
            this.lblFiltroGrado.Size = new System.Drawing.Size(120, 20);
            this.lblFiltroGrado.TabIndex = 22;
            this.lblFiltroGrado.Text = "Grado del alumno";
            // 
            // cmbFiltroGrado
            // 
            this.cmbFiltroGrado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroGrado.Location = new System.Drawing.Point(210, 96);
            this.cmbFiltroGrado.Name = "cmbFiltroGrado";
            this.cmbFiltroGrado.Size = new System.Drawing.Size(100, 30);
            this.cmbFiltroGrado.TabIndex = 23;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNuevo.Location = new System.Drawing.Point(410, 30);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(110, 36);
            this.btnNuevo.TabIndex = 1;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEditar.Location = new System.Drawing.Point(530, 30);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(110, 36);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEliminar.Location = new System.Drawing.Point(650, 30);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(110, 36);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvTutores
            // 
            this.dgvTutores.AllowUserToAddRows = false;
            this.dgvTutores.AllowUserToDeleteRows = false;
            this.dgvTutores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTutores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTutores.Location = new System.Drawing.Point(20, 140);
            this.dgvTutores.Name = "dgvTutores";
            this.dgvTutores.ReadOnly = true;
            this.dgvTutores.RowHeadersWidth = 51;
            this.dgvTutores.Size = new System.Drawing.Size(740, 370);
            this.dgvTutores.TabIndex = 4;
            // 
            // TutorListaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 530);
            this.Controls.Add(this.dgvTutores);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbFiltroEstado);
            this.Controls.Add(this.cmbFiltroTurno);
            this.Controls.Add(this.cmbFiltroDivision);
            this.Controls.Add(this.lblFiltroEstado);
            this.Controls.Add(this.lblFiltroTurno);
            this.Controls.Add(this.lblFiltroDivision);
            this.Controls.Add(this.cmbFiltroGrado);
            this.Controls.Add(this.lblFiltroGrado);
            this.Controls.Add(this.txtFiltroNombre);
            this.Controls.Add(this.lblFiltroNombre);
            this.Name = "TutorListaForm";
            this.Text = "Tutores";
            this.Load += new System.EventHandler(this.TutorListaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTutores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFiltroNombre;
        private System.Windows.Forms.TextBox txtFiltroNombre;
        private System.Windows.Forms.Label lblFiltroGrado;
        private System.Windows.Forms.ComboBox cmbFiltroGrado;
        private System.Windows.Forms.Label lblFiltroDivision;
        private System.Windows.Forms.Label lblFiltroTurno;
        private System.Windows.Forms.Label lblFiltroEstado;
        private System.Windows.Forms.ComboBox cmbFiltroDivision;
        private System.Windows.Forms.ComboBox cmbFiltroTurno;
        private System.Windows.Forms.ComboBox cmbFiltroEstado;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvTutores;
    }
}
