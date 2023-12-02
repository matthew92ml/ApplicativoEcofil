using System.ComponentModel;
using System.Windows.Forms;

namespace ApplicativoEcofil
{
    public partial class CameraHikvision
    {
		private IContainer components = null;
		private PictureBox Camera3;
		private PictureBox Camera4;
		private Label labelCamera3;
		private Label labelCamera4;
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.Camera3 = new System.Windows.Forms.PictureBox();
            this.Camera4 = new System.Windows.Forms.PictureBox();
            this.labelCamera3 = new System.Windows.Forms.Label();
            this.labelCamera4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Camera3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Camera4)).BeginInit();
            this.SuspendLayout();
            // 
            // Camera3
            // 
            this.Camera3.BackColor = System.Drawing.SystemColors.WindowText;
            this.Camera3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Camera3.Location = new System.Drawing.Point(2, -1);
            this.Camera3.Name = "Camera3";
            this.Camera3.Size = new System.Drawing.Size(390, 451);
            this.Camera3.TabIndex = 0;
            this.Camera3.TabStop = false;
            // 
            // Camera4
            // 
            this.Camera4.BackColor = System.Drawing.SystemColors.WindowText;
            this.Camera4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Camera4.Location = new System.Drawing.Point(390, -1);
            this.Camera4.Name = "Camera4";
            this.Camera4.Size = new System.Drawing.Size(416, 451);
            this.Camera4.TabIndex = 1;
            this.Camera4.TabStop = false;
            // 
            // labelCamera3
            // 
            this.labelCamera3.AutoSize = true;
            this.labelCamera3.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCamera3.ForeColor = System.Drawing.Color.Red;
            this.labelCamera3.Location = new System.Drawing.Point(79, 225);
            this.labelCamera3.Name = "labelCamera3";
            this.labelCamera3.Size = new System.Drawing.Size(0, 39);
            this.labelCamera3.TabIndex = 2;
            // 
            // labelCamera4
            // 
            this.labelCamera4.AutoSize = true;
            this.labelCamera4.BackColor = System.Drawing.SystemColors.WindowText;
            this.labelCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCamera4.ForeColor = System.Drawing.Color.Red;
            this.labelCamera4.Location = new System.Drawing.Point(488, 225);
            this.labelCamera4.Name = "labelCamera4";
            this.labelCamera4.Size = new System.Drawing.Size(0, 39);
            this.labelCamera4.TabIndex = 3;
            // 
            // CameraHikvision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 450);
            this.Controls.Add(this.labelCamera4);
            this.Controls.Add(this.labelCamera3);
            this.Controls.Add(this.Camera4);
            this.Controls.Add(this.Camera3);
            this.MaximizeBox = false;
            this.Name = "CameraHikvision";
            this.Text = "CameraHikvision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Closing);
            ((System.ComponentModel.ISupportInitialize)(this.Camera3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Camera4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          