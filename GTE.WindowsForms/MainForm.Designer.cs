namespace GTE.WindowsForms
{
    partial class MainForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.lblUserSub = new System.Windows.Forms.Label();
            this.lblUserTitle = new System.Windows.Forms.Label();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnOtros = new System.Windows.Forms.Button();
            this.btnCursos = new System.Windows.Forms.Button();
            this.btnAlumnos = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnLogOut);
            this.pnlHeader.Controls.Add(this.lblUserSub);
            this.pnlHeader.Controls.Add(this.lblUserTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnLogOut
            // 
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOut.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogOut.Location = new System.Drawing.Point(860, 15);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(120, 40);
            this.btnLogOut.TabIndex = 2;
            this.btnLogOut.Text = "Cerrar Sesión";
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // lblUserSub
            // 
            this.lblUserSub.AutoSize = true;
            this.lblUserSub.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUserSub.Location = new System.Drawing.Point(20, 38);
            this.lblUserSub.Name = "lblUserSub";
            this.lblUserSub.Size = new System.Drawing.Size(95, 20);
            this.lblUserSub.TabIndex = 1;
            this.lblUserSub.Text = "Rol: -";
            // 
            // lblUserTitle
            // 
            this.lblUserTitle.AutoSize = true;
            this.lblUserTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUserTitle.Location = new System.Drawing.Point(18, 10);
            this.lblUserTitle.Name = "lblUserTitle";
            this.lblUserTitle.Size = new System.Drawing.Size(81, 28);
            this.lblUserTitle.TabIndex = 0;
            this.lblUserTitle.Text = "Usuario";
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Controls.Add(this.btnOtros);
            this.pnlSidebar.Controls.Add(this.btnCursos);
            this.pnlSidebar.Controls.Add(this.btnAlumnos);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 70);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 530);
            this.pnlSidebar.TabIndex = 1;
            // 
            // btnOtros
            // 
            this.btnOtros.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOtros.Location = new System.Drawing.Point(0, 100);
            this.btnOtros.Name = "btnOtros";
            this.btnOtros.Size = new System.Drawing.Size(220, 50);
            this.btnOtros.TabIndex = 2;
            this.btnOtros.Text = "Otras Opciones";
            this.btnOtros.UseVisualStyleBackColor = true;
            this.btnOtros.Visible = false;
            this.btnOtros.Click += new System.EventHandler(this.btnOtros_Click);
            // 
            // btnCursos
            // 
            this.btnCursos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCursos.Location = new System.Drawing.Point(0, 50);
            this.btnCursos.Name = "btnCursos";
            this.btnCursos.Size = new System.Drawing.Size(220, 50);
            this.btnCursos.TabIndex = 1;
            this.btnCursos.Text = "Gestionar Cursos";
            this.btnCursos.UseVisualStyleBackColor = true;
            this.btnCursos.Click += new System.EventHandler(this.btnCursos_Click);
            // 
            // btnAlumnos
            // 
            this.btnAlumnos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAlumnos.Location = new System.Drawing.Point(0, 0);
            this.btnAlumnos.Name = "btnAlumnos";
            this.btnAlumnos.Size = new System.Drawing.Size(220, 50);
            this.btnAlumnos.TabIndex = 0;
            this.btnAlumnos.Text = "Gestionar Alumnos";
            this.btnAlumnos.UseVisualStyleBackColor = true;
            this.btnAlumnos.Click += new System.EventHandler(this.btnAlumnos_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(220, 70);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(780, 530);
            this.pnlContent.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gestión - Gestor Turno Escuela";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblUserSub;
        private System.Windows.Forms.Label lblUserTitle;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnOtros;
        private System.Windows.Forms.Button btnCursos;
        private System.Windows.Forms.Button btnAlumnos;
        private System.Windows.Forms.Panel pnlContent;
    }
}
