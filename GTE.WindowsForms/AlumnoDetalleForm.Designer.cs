namespace GTE.WindowsForms
{
    partial class AlumnoDetalleForm
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblTurno = new System.Windows.Forms.Label();
            this.cmbTurno = new System.Windows.Forms.ComboBox();
            this.lblCursoEscolar = new System.Windows.Forms.Label();
            this.cmbCursoEscolar = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(176, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Detalle Alumno";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNombre.Location = new System.Drawing.Point(30, 85);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(68, 21);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNombre.Location = new System.Drawing.Point(30, 110);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(320, 30);
            this.txtNombre.TabIndex = 1;
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblApellido.Location = new System.Drawing.Point(30, 160);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(67, 21);
            this.lblApellido.TabIndex = 0;
            this.lblApellido.Text = "Apellido";
            // 
            // txtApellido
            // 
            this.txtApellido.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtApellido.Location = new System.Drawing.Point(30, 185);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(320, 30);
            this.txtApellido.TabIndex = 2;
            // 
            // lblTurno
            // 
            this.lblTurno.AutoSize = true;
            this.lblTurno.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTurno.Location = new System.Drawing.Point(30, 235);
            this.lblTurno.Name = "lblTurno";
            this.lblTurno.Size = new System.Drawing.Size(50, 21);
            this.lblTurno.TabIndex = 0;
            this.lblTurno.Text = "Turno";
            // 
            // cmbTurno
            // 
            this.cmbTurno.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbTurno.Location = new System.Drawing.Point(30, 260);
            this.cmbTurno.Name = "cmbTurno";
            this.cmbTurno.Size = new System.Drawing.Size(140, 31);
            this.cmbTurno.TabIndex = 3;
            this.cmbTurno.SelectedIndexChanged += new System.EventHandler(this.cmbTurno_SelectedIndexChanged);
            // 
            // lblCursoEscolar
            // 
            this.lblCursoEscolar.AutoSize = true;
            this.lblCursoEscolar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCursoEscolar.Location = new System.Drawing.Point(210, 235);
            this.lblCursoEscolar.Name = "lblCursoEscolar";
            this.lblCursoEscolar.Size = new System.Drawing.Size(50, 21);
            this.lblCursoEscolar.TabIndex = 0;
            this.lblCursoEscolar.Text = "Curso";
            // 
            // cmbCursoEscolar
            // 
            this.cmbCursoEscolar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbCursoEscolar.Location = new System.Drawing.Point(210, 260);
            this.cmbCursoEscolar.Name = "cmbCursoEscolar";
            this.cmbCursoEscolar.Size = new System.Drawing.Size(140, 31);
            this.cmbCursoEscolar.TabIndex = 4;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEstado.Location = new System.Drawing.Point(30, 310);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(56, 21);
            this.lblEstado.TabIndex = 0;
            this.lblEstado.Text = "Estado";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "Presente",
            "Retirado",
            "Ausente"});
            this.cmbEstado.Location = new System.Drawing.Point(30, 335);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(320, 31);
            this.cmbEstado.TabIndex = 5;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGuardar.Location = new System.Drawing.Point(30, 400);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(150, 40);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancelar.Location = new System.Drawing.Point(200, 400);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(150, 40);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // AlumnoDetalleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 470);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cmbCursoEscolar);
            this.Controls.Add(this.lblCursoEscolar);
            this.Controls.Add(this.cmbTurno);
            this.Controls.Add(this.lblTurno);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlumnoDetalleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalle del Alumno";
            this.Load += new System.EventHandler(this.AlumnoDetalleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblTurno;
        private System.Windows.Forms.ComboBox cmbTurno;
        private System.Windows.Forms.Label lblCursoEscolar;
        private System.Windows.Forms.ComboBox cmbCursoEscolar;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
