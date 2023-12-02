using System.ComponentModel;
using System.Windows.Forms;

namespace ApplicativoEcofil
{
    public partial class ProgressWork
    {
		private IContainer components = null;
		private ProgressBar pUD;
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
            this.pUD = new System.Windows.Forms.ProgressBar();
            this.menuStripTitle = new System.Windows.Forms.MenuStrip();
            this.Title = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pUD
            // 
            this.pUD.BackColor = System.Drawing.Color.Snow;
            this.pUD.ForeColor = System.Drawing.Color.Lime;
            this.pUD.Location = new System.Drawing.Point(0, 27);
            this.pUD.Name = "pUD";
            this.pUD.Size = new System.Drawing.Size(329, 31);
            this.pUD.TabIndex = 0;
            // 
            // menuStripTitle
            // 
            this.menuStripTitle.BackColor = System.Drawing.Color.ForestGreen;
            this.menuStripTitle.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripTitle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Title});
            this.menuStripTitle.Location = new System.Drawing.Point(0, 0);
            this.menuStripTitle.Name = "menuStripTitle";
            this.menuStripTitle.Size = new System.Drawing.Size(329, 28);
            this.menuStripTitle.TabIndex = 1;
            this.menuStripTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Progress_MouseDown);
            this.menuStripTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Progress_MouseMove);
            this.menuStripTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Progress_MouseUp);
            // 
            // Title
            // 
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(14, 24);
            // 
            // ProgressWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.ClientSize = new System.Drawing.Size(329, 58);
            this.Controls.Add(this.pUD);
            this.Controls.Add(this.menuStripTitle);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStripTitle;
            this.Name = "ProgressWork";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.ProgressWork_show);
            this.menuStripTitle.ResumeLayout(false);
            this.menuStripTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        private MenuStrip menuStripTitle;
        private ToolStripMenuItem Title;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              