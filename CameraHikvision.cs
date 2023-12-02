using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ApplicativoEcofil.Class;

namespace ApplicativoEcofil
{
	public partial class CameraHikvision : Form
	{
		private static int mUser = -1;

		private static int RealHandle = -1;

		private static int RealHandle2 = -1;

		private Hikvision.REALDATACALLBACK RealData = null;

		private Hikvision.REALDATACALLBACK RealData2 = null;

		public CameraHikvision(string station, string ip, string username = "admin", string password = "Ecofil215", int port = 9101)
		{
			InitializeComponent();
			Icon = Properties.Resources.favicon;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Text = "Telecamere cassonetti della stazione " + station;
			Connection(ip, username, password, port);
		}

		private void Connection(string ip, string username, string password, int port)
		{
			if (!Function.VpnTest())
			{
				MessageBox.Show("VPN non connessa", "DVR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				labelCamera3.Text = "OFFLINE";
				labelCamera4.Text = "OFFLINE";
				return;
			}
			Hikvision.NET_DVR_DEVICEINFO_V30 lpDeviceInfo = default;
			mUser = Hikvision.NET_DVR_Login_V30(ip, port, username, password, ref lpDeviceInfo);
			if (mUser < 0)
			{
				MessageBox.Show("DVR non connesso Error code =" + Hikvision.NET_DVR_GetLastError(), "DVR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Hikvision.NET_DVR_Cleanup();
				labelCamera3.Text = "OFFLINE";
				labelCamera4.Text = "OFFLINE";
				return;
			}
			if (RealData == null)
			{
				RealData = RealDataCallBack;
			}
			IntPtr zero = IntPtr.Zero;
			Hikvision.NET_DVR_PREVIEWINFO nET_DVR_PREVIEWINFO = default;
			nET_DVR_PREVIEWINFO.hPlayWnd = Camera3.Handle;
			nET_DVR_PREVIEWINFO.lChannel = 3;
			nET_DVR_PREVIEWINFO.dwStreamType = 0u;
			nET_DVR_PREVIEWINFO.dwLinkMode = 0u;
			nET_DVR_PREVIEWINFO.bBlocked = true;
			nET_DVR_PREVIEWINFO.dwDisplayBufNum = 1u;
			nET_DVR_PREVIEWINFO.byProtoType = 0;
			nET_DVR_PREVIEWINFO.byPreviewMode = 0;
			Hikvision.NET_DVR_PREVIEWINFO lpPreviewInfo = nET_DVR_PREVIEWINFO;
			RealHandle = Hikvision.NET_DVR_RealPlay_V40(mUser, ref lpPreviewInfo, RealData, zero);
			if (RealHandle < 0)
			{
				labelCamera3.Text = "OFFLINE";
			}
			IntPtr zero2 = IntPtr.Zero;
			if (RealData2 == null)
			{
				RealData2 = RealDataCallBack;
			}
			nET_DVR_PREVIEWINFO = default;
			nET_DVR_PREVIEWINFO.hPlayWnd = Camera4.Handle;
			nET_DVR_PREVIEWINFO.lChannel = 4;
			nET_DVR_PREVIEWINFO.dwStreamType = 0u;
			nET_DVR_PREVIEWINFO.dwLinkMode = 0u;
			nET_DVR_PREVIEWINFO.bBlocked = true;
			nET_DVR_PREVIEWINFO.dwDisplayBufNum = 1u;
			nET_DVR_PREVIEWINFO.byProtoType = 0;
			nET_DVR_PREVIEWINFO.byPreviewMode = 0;
			Hikvision.NET_DVR_PREVIEWINFO lpPreviewInfo2 = nET_DVR_PREVIEWINFO;
			RealHandle2 = Hikvision.NET_DVR_RealPlay_V40(mUser, ref lpPreviewInfo2, RealData2, zero2);
			if (RealHandle2 < 0)
			{
				labelCamera4.Text = "OFFLINE";
			}
		}

		private void RealDataCallBack(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser)
		{
			if (dwBufSize == 0)
			{
				return;
			}
			if (lRealHandle == RealHandle)
			{
				byte[] array = new byte[dwBufSize];
				Marshal.Copy(pBuffer, array, 0, (int)dwBufSize);
				string path = "channel3.ps";
				FileStream fileStream = new(path, FileMode.OpenOrCreate);
				int count = (int)dwBufSize;
				fileStream.Write(array, 0, count);
				fileStream.Close();
				return;
			}
			int num = lRealHandle;
			if (num == RealHandle2)
			{
				byte[] array = new byte[dwBufSize];
				Marshal.Copy(pBuffer, array, 0, (int)dwBufSize);
				string path = "channel4.ps";
				FileStream fileStream = new(path, FileMode.OpenOrCreate);
				int count = (int)dwBufSize;
				fileStream.Write(array, 0, count);
				fileStream.Close();
			}
		}

		private new void Closing(object sender, FormClosingEventArgs e)
		{
			if (RealHandle >= 0 && !Hikvision.NET_DVR_StopRealPlay(RealHandle))
			{
				MessageBox.Show("DVR Error code =" + Hikvision.NET_DVR_GetLastError(), "DVR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (RealHandle2 >= 0 && !Hikvision.NET_DVR_StopRealPlay(RealHandle2))
			{
				MessageBox.Show("DVRError code =" + Hikvision.NET_DVR_GetLastError(), "DVR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (mUser >= 0 && !Hikvision.NET_DVR_Logout(mUser))
			{
				MessageBox.Show("DVRError code =" + Hikvision.NET_DVR_GetLastError(), "DVR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
    }
}
