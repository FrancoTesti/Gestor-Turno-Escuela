namespace GTE.WindowsForms
{
    partial class MisAlumnosForm
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
            this.lblSub = new System.Windows.Forms.Label();
            this.pnlAviso = new System.Windows.Forms.Panel();
            this.lblAviso = new System.Windows.Forms.Label();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.dgvAlumnos = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).BeginInit();
            this.pnlAviso.SuspendLayout();
            this.SuspendLayout();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mis Alumnos a Cargo";
            this.lblSub.AutoSize = true;
            this.lblSub.Location = new System.Drawing.Point(23, 62);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(500, 20);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Alumnos autorizados a retirar, con su curso, horario de salida y estado.";
            this.pnlAviso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAviso.Controls.Add(this.lblAviso);
            this.pnlAviso.Location = new System.Drawing.Point(20, 90);
            this.pnlAviso.Name = "pnlAviso";
            this.pnlAviso.Size = new System.Drawing.Size(740, 54);
            this.pnlAviso.TabIndex = 4;
            this.lblAviso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAviso.Location = new System.Drawing.Point(0, 0);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.lblAviso.Size = new System.Drawing.Size(740, 54);
            this.lblAviso.TabIndex = 0;
            this.lblAviso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.Location = new System.Drawing.Point(650, 26);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(110, 32);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            this.dgvAlumnos.AllowUserToAddRows = false;
            this.dgvAlumnos.AllowUserToDeleteRows = false;
            this.dgvAlumnos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlumnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnos.Location = new System.Drawing.Point(20, 158);
            this.dgvAlumnos.Name = "dgvAlumnos";
            this.dgvAlumnos.ReadOnly = true;
            this.dgvAlumnos.RowHeadersWidth = 51;
            this.dgvAlumnos.Size = new System.Drawing.Size(740, 352);
            this.dgvAlumnos.TabIndex = 2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 530);
            this.Controls.Add(this.dgvAlumnos);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.pnlAviso);
            this.Controls.Add(this.lblSub);
            this.Controls.Add(this.lblTitle);
            this.Name = "MisAlumnosForm";
            this.Text = "Mis Alumnos";
            this.Load += new System.EventHandler(this.MisAlumnosForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).EndInit();
            this.pnlAviso.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel pnlAviso;
        private System.Windows.Forms.Label lblAviso;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.DataGridView dgvAlumnos;
    }
}
