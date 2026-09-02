namespace GTE.WindowsForms
{
    partial class CursoEscolarDetalleForm
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
            this.lblGrado = new System.Windows.Forms.Label();
            this.txtGrado = new System.Windows.Forms.TextBox();
            this.lblCurso = new System.Windows.Forms.Label();
            this.txtCurso = new System.Windows.Forms.TextBox();
            this.lblTurno = new System.Windows.Forms.Label();
            this.cmbTurno = new System.Windows.Forms.ComboBox();
            this.lblHorario = new System.Windows.Forms.Label();
            this.txtHorario = new System.Windows.Forms.TextBox();
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
            this.lblTitle.Size = new System.Drawing.Size(248, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Detalle Curso Escolar";
            // 
            // lblGrado
            // 
            this.lblGrado.AutoSize = true;
            this.lblGrado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGrado.Location = new System.Drawing.Point(30, 85);
            this.lblGrado.Name = "lblGrado";
            this.lblGrado.Size = new System.Drawing.Size(53, 21);
            this.lblGrado.TabIndex = 0;
            this.lblGrado.Text = "Grado";
            // 
            // txtGrado
            // 
            this.txtGrado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGrado.Location = new System.Drawing.Point(30, 110);
            this.txtGrado.Name = "txtGrado";
            this.txtGrado.Size = new System.Drawing.Size(320, 30);
            this.txtGrado.TabIndex = 1;
            // 
            // lblCurso
            // 
            this.lblCurso.AutoSize = true;
            this.lblCurso.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCurso.Location = new System.Drawing.Point(30, 160);
            this.lblCurso.Name = "lblCurso";
            this.lblCurso.Size = new System.Drawing.Size(119, 21);
            this.lblCurso.TabIndex = 0;
            this.lblCurso.Text = "Curso / División";
            // 
            // txtCurso
            // 
            this.txtCurso.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCurso.Location = new System.Drawing.Point(30, 185);
            this.txtCurso.Name = "txtCurso";
            this.txtCurso.Size = new System.Drawing.Size(320, 30);
            this.txtCurso.TabIndex = 2;
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
            this.cmbTurno.FormattingEnabled = true;
            this.cmbTurno.Items.AddRange(new object[] { "Mañana", "Tarde", "Noche" });
            this.cmbTurno.Location = new System.Drawing.Point(30, 260);
            this.cmbTurno.Name = "cmbTurno";
            this.cmbTurno.Size = new System.Drawing.Size(320, 31);
            this.cmbTurno.TabIndex = 3;
            // 
            // lblHorario
            // 
            this.lblHorario.AutoSize = true;
            this.lblHorario.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHorario.Location = new System.Drawing.Point(30, 310);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(183, 21);
            this.lblHorario.TabIndex = 0;
            this.lblHorario.Text = "Horario Salida (Ej: 12:15)";
            // 
            // txtHorario
            // 
            this.txtHorario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHorario.Location = new System.Drawing.Point(30, 335);
            this.txtHorario.Name = "txtHorario";
            this.txtHorario.Size = new System.Drawing.Size(320, 30);
            this.txtHorario.TabIndex = 4;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGuardar.Location = new System.Drawing.Point(30, 395);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(150, 40);
            this.btnGuardar.TabIndex = 5;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancelar.Location = new System.Drawing.Point(200, 395);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(150, 40);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // CursoEscolarDetalleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 465);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.txtHorario);
            this.Controls.Add(this.lblHorario);
            this.Controls.Add(this.cmbTurno);
            this.Controls.Add(this.lblTurno);
            this.Controls.Add(this.txtCurso);
            this.Controls.Add(this.lblCurso);
            this.Controls.Add(this.txtGrado);
            this.Controls.Add(this.lblGrado);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CursoEscolarDetalleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalle del Curso Escolar";
            this.Load += new System.EventHandler(this.CursoEscolarDetalleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblGrado;
        private System.Windows.Forms.TextBox txtGrado;
        private System.Windows.Forms.Label lblCurso;
        private System.Windows.Forms.TextBox txtCurso;
        private System.Windows.Forms.Label lblTurno;
        private System.Windows.Forms.ComboBox cmbTurno;
        private System.Windows.Forms.Label lblHorario;
        private System.Windows.Forms.TextBox txtHorario;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
