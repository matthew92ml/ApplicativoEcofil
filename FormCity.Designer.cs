using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ApplicativoEcofil
{
	partial class FormCity
	{
		private IContainer components = null;

		private Panel panelCity;

		private Button buttonCity;

		private PictureBox pictureBoxCity;

		private ProgressBar progressBarCity;

		private Label labelCity;

		private ComboBox comboBoxNumberCity;

		private Panel panelCityDown;

		private Label labelState;

		private DataGridView dataGridViewCity;

		private CheckedListBox checkListMenuGarbage;

		private Label labelTitleGarbage;

		private MenuStrip menuStripFile;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem esportaFile;

		private ToolStripMenuItem OptionToolStripMenuItem;

		private ToolStripMenuItem esportaExcelToolStripMenuItem;

		private ToolStripMenuItem esciToolStripMenuItem1;

		private ToolStripMenuItem logoutToolStripMenuItem;

		private Panel panelUser;

		private Label labelTitle2;

		private BackgroundWorker backgroundWorker1;

		private Panel panelStation;

		private CheckBox checkBoxStation;

		private Label labelTitleS;



		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		public void InitializeComponent()
		{
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.panelCity = new System.Windows.Forms.Panel();
            this.panelStation = new System.Windows.Forms.Panel();
            this.flowCheckStations = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxStation = new System.Windows.Forms.CheckBox();
            this.labelTitleS = new System.Windows.Forms.Label();
            this.panelUser = new System.Windows.Forms.Panel();
            this.pictureBoxCity = new System.Windows.Forms.PictureBox();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.comboBoxNumberCity = new System.Windows.Forms.ComboBox();
            this.buttonCity = new System.Windows.Forms.Button();
            this.dataGridViewCity = new System.Windows.Forms.DataGridView();
            this.progressBarCity = new System.Windows.Forms.ProgressBar();
            this.panelCityDown = new System.Windows.Forms.Panel();
            this.labelTitle2 = new System.Windows.Forms.Label();
            this.labelTitleGarbage = new System.Windows.Forms.Label();
            this.checkListMenuGarbage = new System.Windows.Forms.CheckedListBox();
            this.chartCity = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStripFile = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.esportaFile = new System.Windows.Forms.ToolStripMenuItem();
            this.esportaExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.esciToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dimensioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReduxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelCity.SuspendLayout();
            this.panelStation.SuspendLayout();
            this.panelUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCity)).BeginInit();
            this.panelCityDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCity)).BeginInit();
            this.menuStripFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCity
            // 
            this.panelCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCity.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelCity.Controls.Add(this.panelStation);
            this.panelCity.Controls.Add(this.panelUser);
            this.panelCity.Controls.Add(this.dataGridViewCity);
            this.panelCity.Controls.Add(this.progressBarCity);
            this.panelCity.Location = new System.Drawing.Point(-1, 28);
            this.panelCity.Margin = new System.Windows.Forms.Padding(4);
            this.panelCity.Name = "panelCity";
            this.panelCity.Size = new System.Drawing.Size(1410, 558);
            this.panelCity.TabIndex = 0;
            this.panelCity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.panelCity.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.panelCity.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // panelStation
            // 
            this.panelStation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelStation.Controls.Add(this.flowCheckStations);
            this.panelStation.Controls.Add(this.checkBoxStation);
            this.panelStation.Controls.Add(this.labelTitleS);
            this.panelStation.Location = new System.Drawing.Point(663, 3);
            this.panelStation.Name = "panelStation";
            this.panelStation.Size = new System.Drawing.Size(744, 165);
            this.panelStation.TabIndex = 9;
            this.panelStation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.panelStation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.panelStation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // flowCheckStations
            // 
            this.flowCheckStations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowCheckStations.AutoScroll = true;
            this.flowCheckStations.Location = new System.Drawing.Point(3, 24);
            this.flowCheckStations.Name = "flowCheckStations";
            this.flowCheckStations.Size = new System.Drawing.Size(738, 141);
            this.flowCheckStations.TabIndex = 6;
            this.flowCheckStations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.flowCheckStations.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.flowCheckStations.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // checkBoxStation
            // 
            this.checkBoxStation.AutoSize = true;
            this.checkBoxStation.Location = new System.Drawing.Point(172, -2);
            this.checkBoxStation.Name = "checkBoxStation";
            this.checkBoxStation.Size = new System.Drawing.Size(205, 20);
            this.checkBoxStation.TabIndex = 5;
            this.checkBoxStation.Text = "Non elaborare tutte le stazioni";
            this.checkBoxStation.UseVisualStyleBackColor = true;
            this.checkBoxStation.CheckedChanged += new System.EventHandler(this.CheckBoxStation_CheckedChanged);
            // 
            // labelTitleS
            // 
            this.labelTitleS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitleS.AutoSize = true;
            this.labelTitleS.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitleS.ForeColor = System.Drawing.Color.Red;
            this.labelTitleS.Location = new System.Drawing.Point(8, 0);
            this.labelTitleS.Name = "labelTitleS";
            this.labelTitleS.Size = new System.Drawing.Size(141, 21);
            this.labelTitleS.TabIndex = 4;
            this.labelTitleS.Text = "Scelta Stazioni";
            this.labelTitleS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelUser
            // 
            this.panelUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUser.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelUser.Controls.Add(this.pictureBoxCity);
            this.panelUser.Controls.Add(this.labelCity);
            this.panelUser.Controls.Add(this.labelState);
            this.panelUser.Controls.Add(this.comboBoxNumberCity);
            this.panelUser.Controls.Add(this.buttonCity);
            this.panelUser.Location = new System.Drawing.Point(3, 3);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(826, 196);
            this.panelUser.TabIndex = 5;
            this.panelUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.panelUser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.panelUser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // pictureBoxCity
            // 
            this.pictureBoxCity.Location = new System.Drawing.Point(4, -3);
            this.pictureBoxCity.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxCity.Name = "pictureBoxCity";
            this.pictureBoxCity.Size = new System.Drawing.Size(148, 199);
            this.pictureBoxCity.TabIndex = 6;
            this.pictureBoxCity.TabStop = false;
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Font = new System.Drawing.Font("Perpetua Titling MT", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCity.Location = new System.Drawing.Point(170, 34);
            this.labelCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCity.Name = "labelCity";
            this.labelCity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelCity.Size = new System.Drawing.Size(0, 33);
            this.labelCity.TabIndex = 4;
            this.labelCity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelState.Location = new System.Drawing.Point(152, 174);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(0, 16);
            this.labelState.TabIndex = 7;
            this.labelState.Click += new System.EventHandler(this.LabelState_Click);
            // 
            // comboBoxNumberCity
            // 
            this.comboBoxNumberCity.FormattingEnabled = true;
            this.comboBoxNumberCity.Location = new System.Drawing.Point(170, 94);
            this.comboBoxNumberCity.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxNumberCity.Name = "comboBoxNumberCity";
            this.comboBoxNumberCity.Size = new System.Drawing.Size(249, 24);
            this.comboBoxNumberCity.TabIndex = 3;
            this.comboBoxNumberCity.SelectedIndexChanged += new System.EventHandler(this.ComboBoxNumberCity_SelectedIndexChanged);
            // 
            // buttonCity
            // 
            this.buttonCity.Location = new System.Drawing.Point(455, 94);
            this.buttonCity.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCity.Name = "buttonCity";
            this.buttonCity.Size = new System.Drawing.Size(168, 46);
            this.buttonCity.TabIndex = 1;
            this.buttonCity.Text = "Conferma";
            this.buttonCity.UseVisualStyleBackColor = true;
            this.buttonCity.Click += new System.EventHandler(this.ButtonCity_Click);
            // 
            // dataGridViewCity
            // 
            this.dataGridViewCity.AllowUserToOrderColumns = true;
            this.dataGridViewCity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCity.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewCity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewCity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCity.Location = new System.Drawing.Point(0, 232);
            this.dataGridViewCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewCity.Name = "dataGridViewCity";
            this.dataGridViewCity.RowHeadersWidth = 51;
            this.dataGridViewCity.RowTemplate.Height = 24;
            this.dataGridViewCity.Size = new System.Drawing.Size(1407, 320);
            this.dataGridViewCity.TabIndex = 8;
            this.dataGridViewCity.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewCity_CellClick);
            this.dataGridViewCity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.dataGridViewCity.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.dataGridViewCity.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // progressBarCity
            // 
            this.progressBarCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarCity.Location = new System.Drawing.Point(0, 199);
            this.progressBarCity.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarCity.Name = "progressBarCity";
            this.progressBarCity.Size = new System.Drawing.Size(1407, 27);
            this.progressBarCity.TabIndex = 5;
            // 
            // panelCityDown
            // 
            this.panelCityDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCityDown.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelCityDown.Controls.Add(this.labelTitle2);
            this.panelCityDown.Controls.Add(this.labelTitleGarbage);
            this.panelCityDown.Controls.Add(this.checkListMenuGarbage);
            this.panelCityDown.Controls.Add(this.chartCity);
            this.panelCityDown.Location = new System.Drawing.Point(-1, 586);
            this.panelCityDown.Margin = new System.Windows.Forms.Padding(4);
            this.panelCityDown.Name = "panelCityDown";
            this.panelCityDown.Size = new System.Drawing.Size(1411, 392);
            this.panelCityDown.TabIndex = 1;
            // 
            // labelTitle2
            // 
            this.labelTitle2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle2.AutoSize = true;
            this.labelTitle2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle2.ForeColor = System.Drawing.Color.Red;
            this.labelTitle2.Location = new System.Drawing.Point(587, 4);
            this.labelTitle2.Name = "labelTitle2";
            this.labelTitle2.Size = new System.Drawing.Size(0, 26);
            this.labelTitle2.TabIndex = 4;
            this.labelTitle2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelTitleGarbage
            // 
            this.labelTitleGarbage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitleGarbage.AutoSize = true;
            this.labelTitleGarbage.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitleGarbage.ForeColor = System.Drawing.Color.Red;
            this.labelTitleGarbage.Location = new System.Drawing.Point(-3, 0);
            this.labelTitleGarbage.Name = "labelTitleGarbage";
            this.labelTitleGarbage.Size = new System.Drawing.Size(147, 21);
            this.labelTitleGarbage.TabIndex = 3;
            this.labelTitleGarbage.Text = "Tipologia rifiuti ";
            this.labelTitleGarbage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkListMenuGarbage
            // 
            this.checkListMenuGarbage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkListMenuGarbage.CheckOnClick = true;
            this.checkListMenuGarbage.FormattingEnabled = true;
            this.checkListMenuGarbage.IntegralHeight = false;
            this.checkListMenuGarbage.Location = new System.Drawing.Point(0, 25);
            this.checkListMenuGarbage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkListMenuGarbage.Name = "checkListMenuGarbage";
            this.checkListMenuGarbage.Size = new System.Drawing.Size(260, 276);
            this.checkListMenuGarbage.TabIndex = 1;
            this.checkListMenuGarbage.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckListMenuGarbage_ItemCheck);
            this.checkListMenuGarbage.SelectedValueChanged += new System.EventHandler(this.CheckListMenuGarbage_SelectedValueChanged);
            // 
            // chartCity
            // 
            this.chartCity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartCity.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCity.Legends.Add(legend1);
            this.chartCity.Location = new System.Drawing.Point(225, 4);
            this.chartCity.Margin = new System.Windows.Forms.Padding(4);
            this.chartCity.Name = "chartCity";
            this.chartCity.Size = new System.Drawing.Size(1177, 275);
            this.chartCity.TabIndex = 0;
            this.chartCity.Text = "chart1";
            this.chartCity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.chartCity.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.chartCity.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // menuStripFile
            // 
            this.menuStripFile.BackColor = System.Drawing.Color.ForestGreen;
            this.menuStripFile.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dimensioniToolStripMenuItem});
            this.menuStripFile.Location = new System.Drawing.Point(0, 0);
            this.menuStripFile.Name = "menuStripFile";
            this.menuStripFile.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStripFile.Size = new System.Drawing.Size(1407, 28);
            this.menuStripFile.TabIndex = 4;
            this.menuStripFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.menuStripFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.menuStripFile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.esportaFile,
            this.OptionToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.esciToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.fileToolStripMenuItem.Text = "Menù";
            // 
            // esportaFile
            // 
            this.esportaFile.BackColor = System.Drawing.Color.Green;
            this.esportaFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.esportaExcelToolStripMenuItem});
            this.esportaFile.Name = "esportaFile";
            this.esportaFile.Size = new System.Drawing.Size(169, 26);
            this.esportaFile.Text = "Esporta File";
            // 
            // esportaExcelToolStripMenuItem
            // 
            this.esportaExcelToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.esportaExcelToolStripMenuItem.Name = "esportaExcelToolStripMenuItem";
            this.esportaExcelToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.esportaExcelToolStripMenuItem.Text = "Esporta dati in Excel";
            this.esportaExcelToolStripMenuItem.Click += new System.EventHandler(this.EsportaExcelToolStripMenuItem_Click);
            // 
            // OptionToolStripMenuItem
            // 
            this.OptionToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.OptionToolStripMenuItem.Name = "OptionToolStripMenuItem";
            this.OptionToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.OptionToolStripMenuItem.Text = "Preferenze";
            this.OptionToolStripMenuItem.Click += new System.EventHandler(this.OptionToolStripMenuItem_ClickAsync);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.LogoutToolStripMenuItem_Click);
            // 
            // esciToolStripMenuItem1
            // 
            this.esciToolStripMenuItem1.BackColor = System.Drawing.Color.Green;
            this.esciToolStripMenuItem1.Name = "esciToolStripMenuItem1";
            this.esciToolStripMenuItem1.Size = new System.Drawing.Size(169, 26);
            this.esciToolStripMenuItem1.Text = "Esci";
            this.esciToolStripMenuItem1.Click += new System.EventHandler(this.EsciToolStripMenuItem1_Click);
            // 
            // dimensioniToolStripMenuItem
            // 
            this.dimensioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MaxToolStripMenuItem,
            this.ReduxToolStripMenuItem});
            this.dimensioniToolStripMenuItem.Name = "dimensioniToolStripMenuItem";
            this.dimensioniToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.dimensioniToolStripMenuItem.Text = "Dimensione Finestra";
            // 
            // MaxToolStripMenuItem
            // 
            this.MaxToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.MaxToolStripMenuItem.Name = "MaxToolStripMenuItem";
            this.MaxToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.MaxToolStripMenuItem.Text = "Intera";
            this.MaxToolStripMenuItem.Click += new System.EventHandler(this.MassimaToolStripMenuItem_Click);
            // 
            // ReduxToolStripMenuItem
            // 
            this.ReduxToolStripMenuItem.BackColor = System.Drawing.Color.Green;
            this.ReduxToolStripMenuItem.Name = "ReduxToolStripMenuItem";
            this.ReduxToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            this.ReduxToolStripMenuItem.Text = "Riduci";
            this.ReduxToolStripMenuItem.Click += new System.EventHandler(this.RiduciToolStripMenuItem_Click);
            // 
            // FormCity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1407, 941);
            this.Controls.Add(this.panelCityDown);
            this.Controls.Add(this.panelCity);
            this.Controls.Add(this.menuStripFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCity";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCity_FormClosing);
            this.Load += new System.EventHandler(this.FormCity_Load);
            this.panelCity.ResumeLayout(false);
            this.panelStation.ResumeLayout(false);
            this.panelStation.PerformLayout();
            this.panelUser.ResumeLayout(false);
            this.panelUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCity)).EndInit();
            this.panelCityDown.ResumeLayout(false);
            this.panelCityDown.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCity)).EndInit();
            this.menuStripFile.ResumeLayout(false);
            this.menuStripFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private FlowLayoutPanel flowCheckStations;
        private ToolStripMenuItem dimensioniToolStripMenuItem;
        private ToolStripMenuItem MaxToolStripMenuItem;
        private ToolStripMenuItem ReduxToolStripMenuItem;
        private Chart chartCity;
    }
}