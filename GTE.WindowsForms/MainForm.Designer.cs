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
            this.btnMisAlumnos = new System.Windows.Forms.Button();
            this.btnOtros = new System.Windows.Forms.Button();
            this.btnAutorizaciones = new System.Windows.Forms.Button();
            this.btnReporteAlumnos = new System.Windows.Forms.Button();
            this.btnReporteRetiros = new System.Windows.Forms.Button();
            this.btnRetiros = new System.Windows.Forms.Button();
            this.btnTutores = new System.Windows.Forms.Button();
            this.btnCursos = new System.Windows.Forms.Button();
            this.btnAlumnos = new System.Windows.Forms.Button();
            this.btnSalidas = new System.Windows.Forms.Button();
            this.btnInicio = new System.Windows.Forms.Button();
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
            this.pnlSidebar.Controls.Add(this.btnMisAlumnos);
            this.pnlSidebar.Controls.Add(this.btnReporteRetiros);
            this.pnlSidebar.Controls.Add(this.btnReporteAlumnos);
            this.pnlSidebar.Controls.Add(this.btnAutorizaciones);
            this.pnlSidebar.Controls.Add(this.btnRetiros);
            this.pnlSidebar.Controls.Add(this.btnTutores);
            this.pnlSidebar.Controls.Add(this.btnCursos);
            this.pnlSidebar.Controls.Add(this.btnAlumnos);
            this.pnlSidebar.Controls.Add(this.btnSalidas);
            this.pnlSidebar.Controls.Add(this.btnInicio);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 70);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 530);
            this.pnlSidebar.TabIndex = 1;
            // 
            // btnInicio
            // 
            this.btnInicio.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInicio.Location = new System.Drawing.Point(0, 0);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(220, 50);
            this.btnInicio.TabIndex = 0;
            this.btnInicio.Text = "Inicio";
            this.btnInicio.UseVisualStyleBackColor = true;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            // 
            // btnSalidas
            // 
            this.btnSalidas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSalidas.Location = new System.Drawing.Point(0, 50);
            this.btnSalidas.Name = "btnSalidas";
            this.btnSalidas.Size = new System.Drawing.Size(220, 50);
            this.btnSalidas.TabIndex = 1;
            this.btnSalidas.Text = "Salidas de Curso";
            this.btnSalidas.UseVisualStyleBackColor = true;
            this.btnSalidas.Visible = false;
            this.btnSalidas.Click += new System.EventHandler(this.btnSalidas_Click);
            // 
            // btnMisAlumnos
            // 
            this.btnMisAlumnos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMisAlumnos.Location = new System.Drawing.Point(0, 500);
            this.btnMisAlumnos.Name = "btnMisAlumnos";
            this.btnMisAlumnos.Size = new System.Drawing.Size(220, 50);
            this.btnMisAlumnos.TabIndex = 10;
            this.btnMisAlumnos.Text = "Mis Alumnos";
            this.btnMisAlumnos.UseVisualStyleBackColor = true;
            this.btnMisAlumnos.Visible = false;
            this.btnMisAlumnos.Click += new System.EventHandler(this.btnMisAlumnos_Click);
            // 
            // btnOtros
            // 
            this.btnOtros.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOtros.Location = new System.Drawing.Point(0, 450);
            this.btnOtros.Name = "btnOtros";
            this.btnOtros.Size = new System.Drawing.Size(220, 50);
            this.btnOtros.TabIndex = 4;
            this.btnOtros.Text = "Otras Opciones";
            this.btnOtros.UseVisualStyleBackColor = true;
            this.btnOtros.Visible = false;
            this.btnOtros.Click += new System.EventHandler(this.btnOtros_Click);
            // 
            // btnAutorizaciones
            // 
            this.btnAutorizaciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAutorizaciones.Location = new System.Drawing.Point(0, 300);
            this.btnAutorizaciones.Name = "btnAutorizaciones";
            this.btnAutorizaciones.Size = new System.Drawing.Size(220, 50);
            this.btnAutorizaciones.TabIndex = 7;
            this.btnAutorizaciones.Text = "Autorizaciones";
            this.btnAutorizaciones.UseVisualStyleBackColor = true;
            this.btnAutorizaciones.Click += new System.EventHandler(this.btnAutorizaciones_Click);
            // 
            // btnReporteAlumnos
            // 
            this.btnReporteAlumnos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReporteAlumnos.Location = new System.Drawing.Point(0, 350);
            this.btnReporteAlumnos.Name = "btnReporteAlumnos";
            this.btnReporteAlumnos.Size = new System.Drawing.Size(220, 50);
            this.btnReporteAlumnos.TabIndex = 8;
            this.btnReporteAlumnos.Text = "Reporte de Alumnos";
            this.btnReporteAlumnos.UseVisualStyleBackColor = true;
            this.btnReporteAlumnos.Click += new System.EventHandler(this.btnReporteAlumnos_Click);
            // 
            // btnReporteRetiros
            // 
            this.btnReporteRetiros.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReporteRetiros.Location = new System.Drawing.Point(0, 400);
            this.btnReporteRetiros.Name = "btnReporteRetiros";
            this.btnReporteRetiros.Size = new System.Drawing.Size(220, 50);
            this.btnReporteRetiros.TabIndex = 9;
            this.btnReporteRetiros.Text = "Reporte de Retiros";
            this.btnReporteRetiros.UseVisualStyleBackColor = true;
            this.btnReporteRetiros.Click += new System.EventHandler(this.btnReporteRetiros_Click);
            // 
            // btnRetiros
            // 
            this.btnRetiros.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRetiros.Location = new System.Drawing.Point(0, 250);
            this.btnRetiros.Name = "btnRetiros";
            this.btnRetiros.Size = new System.Drawing.Size(220, 50);
            this.btnRetiros.TabIndex = 2;
            this.btnRetiros.Text = "Gestionar Retiros";
            this.btnRetiros.UseVisualStyleBackColor = true;
            this.btnRetiros.Click += new System.EventHandler(this.btnRetiros_Click);
            // 
            // btnTutores
            // 
            this.btnTutores.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTutores.Location = new System.Drawing.Point(0, 200);
            this.btnTutores.Name = "btnTutores";
            this.btnTutores.Size = new System.Drawing.Size(220, 50);
            this.btnTutores.TabIndex = 4;
            this.btnTutores.Text = "Gestionar Tutores";
            this.btnTutores.UseVisualStyleBackColor = true;
            this.btnTutores.Click += new System.EventHandler(this.btnTutores_Click);
            // 
            // btnCursos
            // 
            this.btnCursos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCursos.Location = new System.Drawing.Point(0, 150);
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
            this.btnAlumnos.Location = new System.Drawing.Point(0, 100);
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
        private System.Windows.Forms.Button btnMisAlumnos;
        private System.Windows.Forms.Button btnOtros;
        private System.Windows.Forms.Button btnAutorizaciones;
        private System.Windows.Forms.Button btnReporteAlumnos;
        private System.Windows.Forms.Button btnReporteRetiros;
        private System.Windows.Forms.Button btnRetiros;
        private System.Windows.Forms.Button btnTutores;
        private System.Windows.Forms.Button btnCursos;
        private System.Windows.Forms.Button btnAlumnos;
        private System.Windows.Forms.Button btnSalidas;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.Panel pnlContent;
    }
}
