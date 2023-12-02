using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicativoEcofil.Properties;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;

namespace ApplicativoEcofil
{
	public partial class ProgressWork : Form
	{
		private readonly long max;
		private readonly FilesResource.CreateMediaUpload local;
		private readonly FilesResource.UpdateMediaUpload uplocal;
        private readonly System.Windows.Forms.Timer wait = new();
		private bool drag = false;
		private Point pmouse, pform;

		public ProgressWork(FilesResource.CreateMediaUpload request, long length)
		{
			InitializeComponent();
			Set();
			local = request;
			max = length;
		}
		public ProgressWork(FilesResource.UpdateMediaUpload request, long length)
		{
			InitializeComponent();
			Set();
			uplocal = request;
			max = length;
		}
		private void Set()
		{
			Icon = Resources.favicon;
			Text = "Upload ";
			Title.Text = "Invio File Inzio";
			Title.Image = Resources.faviconj;
            pUD.Maximum = 100;
		}
		private void ProgressWork_show(object sender, EventArgs e)
		{
			wait.Tick += CheckUpload;
			wait.Interval = 3000;
			wait.Start();
		}
		private async void CheckUpload(object sender, EventArgs e)
		{
			wait.Stop();
			await Task.Run(() =>
			{
				if (local != null)
				{
					local.SupportsTeamDrives = true;
					local.ProgressChanged += (IUploadProgress progress) =>
					{
						Request_ProgressChanged(progress, max, this);
					};
					local.ResponseReceived += (File status) =>
					{
						Request_ResponseReceived(status, this);
					};
					local.Upload();
					Thread.Sleep(5000);
				}
				else
				{
					uplocal.SupportsTeamDrives = true;
					uplocal.ProgressChanged += (IUploadProgress progress) =>
					{
						Request_ProgressChanged(progress, max, this);
					};
					uplocal.ResponseReceived += (File status) =>
					{
						Request_ResponseReceived(status, this);
					};
					uplocal.Upload();
					Thread.Sleep(5000);
				}
			});
			Close();
		}
		private void Request_ProgressChanged(IUploadProgress obj, long length, ProgressWork work)
		{
			work.BeginInvoke(()=>
			{
				switch (obj.Status)
				{
					case UploadStatus.Starting:
					case UploadStatus.Uploading:
						pUD.Value = Convert.ToInt32(obj.BytesSent / length * 100);
						work.pUD.Refresh();
						work.Title.Text = "Invio " + pUD.Value.ToString() + " % ";
						break;
					case UploadStatus.Failed:
						work.Title.Text = "Invio file " + obj.Status;
						Thread.Sleep(5000);
						work.Close();
						break;
				}
			});
		}
		private static void Request_ResponseReceived(File obj, ProgressWork work)
		{
			if (obj != null)
			{
				work.BeginInvoke(() =>
                {   
					work.pUD.Value = 100;
					work.Title.Text = "File caricato con successo";
				});
			}
		}
		private void Progress_MouseDown(object sender, MouseEventArgs e)
		{
			drag = true;
			pmouse = Cursor.Position;
			pform = Location;
		}
		private void Progress_MouseMove(object sender, MouseEventArgs e)
		{
			if (drag)
			{
				Point dif = Point.Subtract(Cursor.Position, new Size(pmouse));
				Location = Point.Add(pform, new Size(dif));
			}
		}
		private void Progress_MouseUp(object sender, MouseEventArgs e)
		{
			drag = false;
		}
	}
}
