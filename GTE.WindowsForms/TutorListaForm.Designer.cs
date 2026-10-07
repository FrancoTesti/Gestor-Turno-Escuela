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
            this.lblFiltroCurso = new System.Windows.Forms.Label();
            this.cmbFiltroCurso = new System.Windows.Forms.ComboBox();
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
            this.lblFiltroNombre.Location = new System.Drawing.Point(20, 24);
            this.lblFiltroNombre.Name = "lblFiltroNombre";
            this.lblFiltroNombre.Size = new System.Drawing.Size(170, 20);
            this.lblFiltroNombre.TabIndex = 20;
            this.lblFiltroNombre.Text = "Buscar por nombre o DNI";
            // 
            // txtFiltroNombre
            // 
            this.txtFiltroNombre.Location = new System.Drawing.Point(20, 48);
            this.txtFiltroNombre.Name = "txtFiltroNombre";
            this.txtFiltroNombre.Size = new System.Drawing.Size(180, 30);
            this.txtFiltroNombre.TabIndex = 21;
            // 
            // lblFiltroCurso
            // 
            this.lblFiltroCurso.AutoSize = true;
            this.lblFiltroCurso.Location = new System.Drawing.Point(212, 24);
            this.lblFiltroCurso.Name = "lblFiltroCurso";
            this.lblFiltroCurso.Size = new System.Drawing.Size(120, 20);
            this.lblFiltroCurso.TabIndex = 22;
            this.lblFiltroCurso.Text = "Curso del alumno";
            // 
            // cmbFiltroCurso
            // 
            this.cmbFiltroCurso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroCurso.Location = new System.Drawing.Point(212, 48);
            this.cmbFiltroCurso.Name = "cmbFiltroCurso";
            this.cmbFiltroCurso.Size = new System.Drawing.Size(190, 30);
            this.cmbFiltroCurso.TabIndex = 23;
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
            this.dgvTutores.Location = new System.Drawing.Point(20, 90);
            this.dgvTutores.Name = "dgvTutores";
            this.dgvTutores.ReadOnly = true;
            this.dgvTutores.RowHeadersWidth = 51;
            this.dgvTutores.Size = new System.Drawing.Size(740, 420);
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
            this.Controls.Add(this.cmbFiltroCurso);
            this.Controls.Add(this.lblFiltroCurso);
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
        private System.Windows.Forms.Label lblFiltroCurso;
        private System.Windows.Forms.ComboBox cmbFiltroCurso;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvTutores;
    }
}
