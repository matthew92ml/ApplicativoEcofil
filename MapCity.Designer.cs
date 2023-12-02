using GMap.NET.WindowsForms;
using System.ComponentModel;
using System.Windows.Forms;
using ApplicativoEcofil.Properties;

namespace ApplicativoEcofil
{
    public partial class MapCity
    {


		private IContainer components = null;

		private MenuStrip menuStripMap;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem MapToolStripMenuItem;

		private GMapControl gMapweb;

		private ToolStripContainer toolStripContainerMenu;

		private PictureBox pictureLogo;

		private RichTextBox richTextBoxRoute;

		private ToolStripMenuItem CloseStripMenuItem;

		private ToolStripMenuItem ZoomPStripMenuItem;

		private ToolStripMenuItem ZoomMStripMenuItem;



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
            this.menuStripMap = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomPStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomMStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gMapweb = new GMap.NET.WindowsForms.GMapControl();
            this.toolStripContainerMenu = new System.Windows.Forms.ToolStripContainer();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.richTextBoxRoute = new System.Windows.Forms.RichTextBox();
            this.menuStripMap.SuspendLayout();
            this.toolStripContainerMenu.TopToolStripPanel.SuspendLayout();
            this.toolStripContainerMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripMap
            // 
            this.menuStripMap.BackColor = System.Drawing.Color.ForestGreen;
            this.menuStripMap.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStripMap.Location = new System.Drawing.Point(0, 0);
            this.menuStripMap.Name = "menuStripMap";
            this.menuStripMap.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStripMap.Size = new System.Drawing.Size(954, 28);
            this.menuStripMap.TabIndex = 5;
            this.menuStripMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.menuStripMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            this.menuStripMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MapToolStripMenuItem,
            this.ZoomPStripMenuItem,
            this.ZoomMStripMenuItem,
            this.CloseStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.fileToolStripMenuItem.Text = "Menù";
            // 
            // MapToolStripMenuItem
            // 
            this.MapToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.MapToolStripMenuItem.Name = "MapToolStripMenuItem";
            this.MapToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.MapToolStripMenuItem.Text = "Esporta Mappa";
            this.MapToolStripMenuItem.Click += new System.EventHandler(this.MapToolStripMenuItem_Click);
            // 
            // ZoomPStripMenuItem
            // 
            this.ZoomPStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.ZoomPStripMenuItem.Name = "ZoomPStripMenuItem";
            this.ZoomPStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.ZoomPStripMenuItem.Text = "Zoom +";
            this.ZoomPStripMenuItem.Click += new System.EventHandler(this.ZoomPStripMenuItem_Click);
            // 
            // ZoomMStripMenuItem
            // 
            this.ZoomMStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.ZoomMStripMenuItem.Name = "ZoomMStripMenuItem";
            this.ZoomMStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.ZoomMStripMenuItem.Text = "Zoom -";
            this.ZoomMStripMenuItem.Click += new System.EventHandler(this.ZoomMStripMenuItem_Click);
            // 
            // CloseStripMenuItem
            // 
            this.CloseStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.CloseStripMenuItem.Name = "CloseStripMenuItem";
            this.CloseStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.CloseStripMenuItem.Text = "Chiudi";
            this.CloseStripMenuItem.Click += new System.EventHandler(this.CloseStripMenuItem_Click);
            // 
            // gMapweb
            // 
            this.gMapweb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapweb.Bearing = 0F;
            this.gMapweb.CanDragMap = true;
            this.gMapweb.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapweb.GrayScaleMode = false;
            this.gMapweb.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapweb.LevelsKeepInMemory = 5;
            this.gMapweb.Location = new System.Drawing.Point(417, 28);
            this.gMapweb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gMapweb.MarkersEnabled = true;
            this.gMapweb.MaxZoom = 2;
            this.gMapweb.MinZoom = 2;
            this.gMapweb.MouseWheelZoomEnabled = true;
            this.gMapweb.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapweb.Name = "gMapweb";
            this.gMapweb.NegativeMode = false;
            this.gMapweb.PolygonsEnabled = true;
            this.gMapweb.RetryLoadTile = 0;
            this.gMapweb.RoutesEnabled = true;
            this.gMapweb.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapweb.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapweb.ShowTileGridLines = false;
            this.gMapweb.Size = new System.Drawing.Size(949, 632);
            this.gMapweb.TabIndex = 6;
            this.gMapweb.Zoom = 0D;
            // 
            // toolStripContainerMenu
            // 
            // 
            // toolStripContainerMenu.ContentPanel
            // 
            this.toolStripContainerMenu.ContentPanel.Size = new System.Drawing.Size(954, 0);
            this.toolStripContainerMenu.Location = new System.Drawing.Point(412, 1);
            this.toolStripContainerMenu.Name = "toolStripContainerMenu";
            this.toolStripContainerMenu.Size = new System.Drawing.Size(954, 27);
            this.toolStripContainerMenu.TabIndex = 7;
            this.toolStripContainerMenu.Text = "toolStripContainer1";
            // 
            // toolStripContainerMenu.TopToolStripPanel
            // 
            this.toolStripContainerMenu.TopToolStripPanel.Controls.Add(this.menuStripMap);
            // 
            // pictureLogo
            // 
            this.pictureLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureLogo.BackColor = System.Drawing.Color.ForestGreen;
            this.pictureLogo.Location = new System.Drawing.Point(-2, 1);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(421, 112);
            this.pictureLogo.TabIndex = 10;
            this.pictureLogo.TabStop = false;
            this.pictureLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.pictureLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            this.pictureLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            // 
            // richTextBoxRoute
            // 
            this.richTextBoxRoute.Location = new System.Drawing.Point(0, 119);
            this.richTextBoxRoute.Name = "richTextBoxRoute";
            this.richTextBoxRoute.Size = new System.Drawing.Size(411, 362);
            this.richTextBoxRoute.TabIndex = 11;
            this.richTextBoxRoute.Text = "";
            // 
            // MapCity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 660);
            this.Controls.Add(this.richTextBoxRoute);
            this.Controls.Add(this.pictureLogo);
            this.Controls.Add(this.toolStripContainerMenu);
            this.Controls.Add(this.gMapweb);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "MapCity";
            this.Text = "Maps";
            this.Load += new System.EventHandler(this.MapCity_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Map_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
            this.menuStripMap.ResumeLayout(false);
            this.menuStripMap.PerformLayout();
            this.toolStripContainerMenu.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainerMenu.TopToolStripPanel.PerformLayout();
            this.toolStripContainerMenu.ResumeLayout(false);
            this.toolStripContainerMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.ResumeLayout(false);

		}
	}
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               