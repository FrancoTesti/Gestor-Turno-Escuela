namespace GTE.WindowsForms
{
    partial class AutorizacionListaForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbTutores = new System.Windows.Forms.ComboBox();
            this.lblTutor = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.cmbAlumnos = new System.Windows.Forms.ComboBox();
            this.lblAlumno = new System.Windows.Forms.Label();
            this.dgvAlumnosAutorizados = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnosAutorizados)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(18, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(395, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Administración de Autorizaciones";
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(this.cmbTutores);
            this.pnlTop.Controls.Add(this.lblTutor);
            this.pnlTop.Location = new System.Drawing.Point(24, 65);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(730, 65);
            this.pnlTop.TabIndex = 1;
            // 
            // cmbTutores
            // 
            this.cmbTutores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTutores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTutores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbTutores.FormattingEnabled = true;
            this.cmbTutores.Location = new System.Drawing.Point(150, 16);
            this.cmbTutores.Name = "cmbTutores";
            this.cmbTutores.Size = new System.Drawing.Size(555, 31);
            this.cmbTutores.TabIndex = 1;
            this.cmbTutores.SelectedIndexChanged += new System.EventHandler(this.cmbTutores_SelectedIndexChanged);
            // 
            // lblTutor
            // 
            this.lblTutor.AutoSize = true;
            this.lblTutor.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTutor.Location = new System.Drawing.Point(15, 19);
            this.lblTutor.Name = "lblTutor";
            this.lblTutor.Size = new System.Drawing.Size(129, 23);
            this.lblTutor.TabIndex = 0;
            this.lblTutor.Text = "Seleccionar Tutor:";
            // 
            // pnlActions
            // 
            this.pnlActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActions.BackColor = System.Drawing.Color.White;
            this.pnlActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActions.Controls.Add(this.btnQuitar);
            this.pnlActions.Controls.Add(this.btnAgregar);
            this.pnlActions.Controls.Add(this.cmbAlumnos);
            this.pnlActions.Controls.Add(this.lblAlumno);
            this.pnlActions.Location = new System.Drawing.Point(24, 140);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(730, 65);
            this.pnlActions.TabIndex = 2;
            // 
            // btnQuitar
            // 
            this.btnQuitar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuitar.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnQuitar.Location = new System.Drawing.Point(595, 14);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(120, 35);
            this.btnQuitar.TabIndex = 3;
            this.btnQuitar.Text = "Quitar Alumno";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAgregar.Location = new System.Drawing.Point(460, 14);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(125, 35);
            this.btnAgregar.TabIndex = 2;
            this.btnAgregar.Text = "+ Autorizar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // cmbAlumnos
            // 
            this.cmbAlumnos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAlumnos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlumnos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbAlumnos.FormattingEnabled = true;
            this.cmbAlumnos.Location = new System.Drawing.Point(150, 16);
            this.cmbAlumnos.Name = "cmbAlumnos";
            this.cmbAlumnos.Size = new System.Drawing.Size(295, 31);
            this.cmbAlumnos.TabIndex = 1;
            // 
            // lblAlumno
            // 
            this.lblAlumno.AutoSize = true;
            this.lblAlumno.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAlumno.Location = new System.Drawing.Point(15, 19);
            this.lblAlumno.Name = "lblAlumno";
            this.lblAlumno.Size = new System.Drawing.Size(129, 23);
            this.lblAlumno.TabIndex = 0;
            this.lblAlumno.Text = "Agregar Alumno:";
            // 
            // dgvAlumnosAutorizados
            // 
            this.dgvAlumnosAutorizados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlumnosAutorizados.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlumnosAutorizados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnosAutorizados.Location = new System.Drawing.Point(24, 220);
            this.dgvAlumnosAutorizados.Name = "dgvAlumnosAutorizados";
            this.dgvAlumnosAutorizados.RowHeadersWidth = 51;
            this.dgvAlumnosAutorizados.RowTemplate.Height = 29;
            this.dgvAlumnosAutorizados.Size = new System.Drawing.Size(730, 280);
            this.dgvAlumnosAutorizados.TabIndex = 3;
            // 
            // AutorizacionListaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 520);
            this.Controls.Add(this.dgvAlumnosAutorizados);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lblTitle);
            this.Name = "AutorizacionListaForm";
            this.Text = "Gestión de Autorizaciones";
            this.Load += new System.EventHandler(this.AutorizacionListaForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnosAutorizados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTutor;
        private System.Windows.Forms.ComboBox cmbTutores;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Label lblAlumno;
        private System.Windows.Forms.ComboBox cmbAlumnos;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.DataGridView dgvAlumnosAutorizados;
    }
}
