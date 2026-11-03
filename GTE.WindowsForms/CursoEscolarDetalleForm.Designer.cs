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
            this.lblHorariosSemana = new System.Windows.Forms.Label();
            this.btnCopiarSemana = new System.Windows.Forms.Button();
            this.lblLunes = new System.Windows.Forms.Label();
            this.dtpLunes = new System.Windows.Forms.DateTimePicker();
            this.lblMartes = new System.Windows.Forms.Label();
            this.dtpMartes = new System.Windows.Forms.DateTimePicker();
            this.lblMiercoles = new System.Windows.Forms.Label();
            this.dtpMiercoles = new System.Windows.Forms.DateTimePicker();
            this.lblJueves = new System.Windows.Forms.Label();
            this.dtpJueves = new System.Windows.Forms.DateTimePicker();
            this.lblViernes = new System.Windows.Forms.Label();
            this.dtpViernes = new System.Windows.Forms.DateTimePicker();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(25, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(248, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Detalle Curso Escolar";
            this.lblGrado.AutoSize = true;
            this.lblGrado.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGrado.Location = new System.Drawing.Point(25, 85);
            this.lblGrado.Name = "lblGrado";
            this.lblGrado.Size = new System.Drawing.Size(53, 21);
            this.lblGrado.TabIndex = 0;
            this.lblGrado.Text = "Grado";
            this.txtGrado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGrado.Location = new System.Drawing.Point(25, 110);
            this.txtGrado.Name = "txtGrado";
            this.txtGrado.Size = new System.Drawing.Size(470, 30);
            this.txtGrado.TabIndex = 1;
            this.lblCurso.AutoSize = true;
            this.lblCurso.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCurso.Location = new System.Drawing.Point(25, 160);
            this.lblCurso.Name = "lblCurso";
            this.lblCurso.Size = new System.Drawing.Size(119, 21);
            this.lblCurso.TabIndex = 0;
            this.lblCurso.Text = "Curso / División";
            this.txtCurso.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCurso.Location = new System.Drawing.Point(25, 185);
            this.txtCurso.Name = "txtCurso";
            this.txtCurso.Size = new System.Drawing.Size(470, 30);
            this.txtCurso.TabIndex = 2;
            this.lblTurno.AutoSize = true;
            this.lblTurno.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTurno.Location = new System.Drawing.Point(25, 235);
            this.lblTurno.Name = "lblTurno";
            this.lblTurno.Size = new System.Drawing.Size(50, 21);
            this.lblTurno.TabIndex = 0;
            this.lblTurno.Text = "Turno";
            this.cmbTurno.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbTurno.FormattingEnabled = true;
            this.cmbTurno.Items.AddRange(new object[] { "Mañana", "Tarde", "Noche" });
            this.cmbTurno.Location = new System.Drawing.Point(25, 260);
            this.cmbTurno.Name = "cmbTurno";
            this.cmbTurno.Size = new System.Drawing.Size(470, 31);
            this.cmbTurno.TabIndex = 3;
            // lblHorariosSemana
            this.lblHorariosSemana.AutoSize = true;
            this.lblHorariosSemana.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHorariosSemana.Location = new System.Drawing.Point(25, 305);
            this.lblHorariosSemana.Name = "lblHorariosSemana";
            this.lblHorariosSemana.Size = new System.Drawing.Size(262, 21);
            this.lblHorariosSemana.TabIndex = 4;
            this.lblHorariosSemana.Text = "Horarios de salida (Lunes a Viernes)";
            // btnCopiarSemana
            this.btnCopiarSemana.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCopiarSemana.Location = new System.Drawing.Point(310, 298);
            this.btnCopiarSemana.Name = "btnCopiarSemana";
            this.btnCopiarSemana.Size = new System.Drawing.Size(185, 30);
            this.btnCopiarSemana.TabIndex = 5;
            this.btnCopiarSemana.Text = "Copiar a toda la semana";
            this.btnCopiarSemana.UseVisualStyleBackColor = true;
            this.btnCopiarSemana.Click += new System.EventHandler(this.btnCopiarSemana_Click);
            // lblLunes
            this.lblLunes.AutoSize = true;
            this.lblLunes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLunes.Location = new System.Drawing.Point(25, 337);
            this.lblLunes.Name = "lblLunes";
            this.lblLunes.Size = new System.Drawing.Size(46, 20);
            this.lblLunes.TabIndex = 6;
            this.lblLunes.Text = "Lunes";
            // dtpLunes
            this.dtpLunes.CustomFormat = "HH:mm";
            this.dtpLunes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLunes.Location = new System.Drawing.Point(25, 360);
            this.dtpLunes.Name = "dtpLunes";
            this.dtpLunes.ShowUpDown = true;
            this.dtpLunes.Size = new System.Drawing.Size(88, 27);
            this.dtpLunes.TabIndex = 7;
            // lblMartes
            this.lblMartes.AutoSize = true;
            this.lblMartes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMartes.Location = new System.Drawing.Point(121, 337);
            this.lblMartes.Name = "lblMartes";
            this.lblMartes.Size = new System.Drawing.Size(54, 20);
            this.lblMartes.TabIndex = 8;
            this.lblMartes.Text = "Martes";
            // dtpMartes
            this.dtpMartes.CustomFormat = "HH:mm";
            this.dtpMartes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMartes.Location = new System.Drawing.Point(121, 360);
            this.dtpMartes.Name = "dtpMartes";
            this.dtpMartes.ShowUpDown = true;
            this.dtpMartes.Size = new System.Drawing.Size(88, 27);
            this.dtpMartes.TabIndex = 9;
            // lblMiercoles
            this.lblMiercoles.AutoSize = true;
            this.lblMiercoles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMiercoles.Location = new System.Drawing.Point(217, 337);
            this.lblMiercoles.Name = "lblMiercoles";
            this.lblMiercoles.Size = new System.Drawing.Size(73, 20);
            this.lblMiercoles.TabIndex = 10;
            this.lblMiercoles.Text = "Miércoles";
            // dtpMiercoles
            this.dtpMiercoles.CustomFormat = "HH:mm";
            this.dtpMiercoles.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMiercoles.Location = new System.Drawing.Point(217, 360);
            this.dtpMiercoles.Name = "dtpMiercoles";
            this.dtpMiercoles.ShowUpDown = true;
            this.dtpMiercoles.Size = new System.Drawing.Size(88, 27);
            this.dtpMiercoles.TabIndex = 11;
            // lblJueves
            this.lblJueves.AutoSize = true;
            this.lblJueves.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblJueves.Location = new System.Drawing.Point(313, 337);
            this.lblJueves.Name = "lblJueves";
            this.lblJueves.Size = new System.Drawing.Size(52, 20);
            this.lblJueves.TabIndex = 12;
            this.lblJueves.Text = "Jueves";
            // dtpJueves
            this.dtpJueves.CustomFormat = "HH:mm";
            this.dtpJueves.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpJueves.Location = new System.Drawing.Point(313, 360);
            this.dtpJueves.Name = "dtpJueves";
            this.dtpJueves.ShowUpDown = true;
            this.dtpJueves.Size = new System.Drawing.Size(88, 27);
            this.dtpJueves.TabIndex = 13;
            // lblViernes
            this.lblViernes.AutoSize = true;
            this.lblViernes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblViernes.Location = new System.Drawing.Point(409, 337);
            this.lblViernes.Name = "lblViernes";
            this.lblViernes.Size = new System.Drawing.Size(57, 20);
            this.lblViernes.TabIndex = 14;
            this.lblViernes.Text = "Viernes";
            // dtpViernes
            this.dtpViernes.CustomFormat = "HH:mm";
            this.dtpViernes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpViernes.Location = new System.Drawing.Point(409, 360);
            this.dtpViernes.Name = "dtpViernes";
            this.dtpViernes.ShowUpDown = true;
            this.dtpViernes.Size = new System.Drawing.Size(88, 27);
            this.dtpViernes.TabIndex = 15;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardar.Location = new System.Drawing.Point(205, 420);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(140, 36);
            this.btnGuardar.TabIndex = 16;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(355, 420);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(140, 36);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 480);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.dtpViernes);
            this.Controls.Add(this.lblViernes);
            this.Controls.Add(this.dtpJueves);
            this.Controls.Add(this.lblJueves);
            this.Controls.Add(this.dtpMiercoles);
            this.Controls.Add(this.lblMiercoles);
            this.Controls.Add(this.dtpMartes);
            this.Controls.Add(this.lblMartes);
            this.Controls.Add(this.dtpLunes);
            this.Controls.Add(this.lblLunes);
            this.Controls.Add(this.btnCopiarSemana);
            this.Controls.Add(this.lblHorariosSemana);
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
        private System.Windows.Forms.Label lblHorariosSemana;
        private System.Windows.Forms.Button btnCopiarSemana;
        private System.Windows.Forms.Label lblLunes;
        private System.Windows.Forms.DateTimePicker dtpLunes;
        private System.Windows.Forms.Label lblMartes;
        private System.Windows.Forms.DateTimePicker dtpMartes;
        private System.Windows.Forms.Label lblMiercoles;
        private System.Windows.Forms.DateTimePicker dtpMiercoles;
        private System.Windows.Forms.Label lblJueves;
        private System.Windows.Forms.DateTimePicker dtpJueves;
        private System.Windows.Forms.Label lblViernes;
        private System.Windows.Forms.DateTimePicker dtpViernes;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
