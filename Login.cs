using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Resources;
using System.Windows.Forms;
using ApplicativoEcofil.Class;
using ApplicativoEcofil.Properties;
using Renci.SshNet;

namespace ApplicativoEcofil
{
	public partial class Login : Form
	{
		private readonly PostgresConnector connectorLogin;
		private readonly SshClient comLogin;
		private bool drag = false;
		private Point pmouse;
		private Point pform;
		public string value;
		public bool loggedON;
		public Login(PostgresConnector connector, SshClient com, ResourceSet resx)
		{
			comLogin = com;
			connectorLogin = connector;
			InitializeComponent();
            pictureLogin.Image = Resources.LogoEcofil2020;
			BackgroundImage = Resources.background1;
			Icon = Resources.favicon;
			Region = new Region(FormCityFunction.GetRoundPath(DisplayRectangle, base.Height / 10));
			pictureLogin.Image = Resources.LogoEcofil2020;
			pictureLogin.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureLogin.Scale(new SizeF(1.2f, 0.8f));
			Text = resx.GetObject("login").ToString();
			pictureLogin.BackColor = Color.Transparent;
			buttonConfirm.Region = new Region(FormCityFunction.GetRoundPath(buttonConfirm.DisplayRectangle, buttonConfirm.Height / 2));
			buttonConfirm.BackColor = Color.Transparent;
			buttonExit.BackColor = Color.Transparent;
			buttonExit.Region = new Region(FormCityFunction.GetRoundPath(buttonExit.DisplayRectangle, buttonExit.Height / 2));
			labelVersion.BackColor = Color.Transparent;
			checkRemember.BackColor = Color.Transparent;
			labelUsername.BackColor = Color.Transparent;
			labelPassword.BackColor = Color.Transparent;
			labelVersion.Text = resx.GetObject("type").ToString() + resx.GetObject("version").ToString() + resx.GetObject("info1").ToString();
		}
		private void Login_Load(object sender, EventArgs e)
		{
			if (Settings.Default.username.ToString() != string.Empty)
			{
				textBoxPassword.PasswordChar = '*';
				textBoxUsername.Text = Settings.Default.username;
				textBoxPassword.Text = Settings.Default.password;
			}
		}
		private void Button2_Click(object sender, EventArgs e)
		{
			IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] activeTcpConnections = iPGlobalProperties.GetActiveTcpConnections();
			TcpConnectionInformation[] array = activeTcpConnections;
			foreach (TcpConnectionInformation tcpConnectionInformation in array)
			{
				if (tcpConnectionInformation.RemoteEndPoint.Address.ToString() == comLogin.ConnectionInfo.Host)
				{
					comLogin.Disconnect();
					comLogin.Dispose();
					break;
				}
			}
			Close();
		}
		private void TextBoxPassword_MouseClick(object sender, MouseEventArgs e)
		{
			textBoxPassword.Text = "";
			textBoxPassword.PasswordChar = '*';
			textBoxUsername.BackColor = Color.White;
		}
		private void TextBoxUsername_MouseClick(object sender, MouseEventArgs e)
		{
			textBoxUsername.Text = "";
			textBoxUsername.BackColor = Color.White;
		}
		private void ButtonConfirm_Click(object sender, EventArgs e)
		{
			string text = Function.CheckLogin(connectorLogin, textBoxUsername.Text, textBoxPassword.Text);
			switch (text)
			{
				case "error-database":
					MessageBox.Show("Database Non Connesso", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				case "error-datadb":
					MessageBox.Show("Database Non Connesso", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				case "error-activation":
					MessageBox.Show("Account non attivo", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				case "error-password":
					MessageBox.Show("Password Sbagliata", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					textBoxPassword.BackColor = Color.Yellow;
					return;
				case "error-username":
					MessageBox.Show("Username Sbagliato", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					textBoxUsername.BackColor = Color.Yellow;
					return;
				case "username":
					MessageBox.Show("Campo username vuoto!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					textBoxUsername.BackColor = Color.Red;
					return;
				case "password":
					MessageBox.Show("Campo password vuoto!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					textBoxPassword.BackColor = Color.Red;
					return;
			}
			if (checkRemember.Checked)
			{
				Settings.Default.username = textBoxUsername.Text;
				Settings.Default.password = textBoxPassword.Text;
				Settings.Default.Save();
				Settings.Default.Reload();
			}
			loggedON = true;
			value = text;
			Close();
		}
		private void Login_MouseDown(object sender, MouseEventArgs e)
		{
			drag = true;
			pmouse = Cursor.Position;
			pform = base.Location;
		}
		private void Login_MouseMove(object sender, MouseEventArgs e)
		{
			if (drag)
			{
				Point pt = Point.Subtract(Cursor.Position, new Size(pmouse));
				base.Location = Point.Add(pform, new Size(pt));
			}
		}
		private void Login_MouseUp(object sender, MouseEventArgs e)
		{
			drag = false;
		}
	}
}