using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using ApplicativoEcofil.Class;
using Renci.SshNet;

namespace ApplicativoEcofil
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Hikvision.NET_DVR_Init();
			string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			if (!File.Exists(directoryName + "\\Resource\\Resources.resx"))
			{
				using IResourceWriter resourceWriter = new ResourceWriter(directoryName + "\\resources\\Resources.resx");
				resourceWriter.AddResource("type", "Applicativo Stazioni");
				resourceWriter.AddResource("version", " v5.6.0(05/05/2022)");
				resourceWriter.AddResource("info1", "\r\n***Usare una connessione internet per il funzionamento***");
				resourceWriter.AddResource("description", "Applicativo del controllo stazioni");
				resourceWriter.AddResource("login", "Login");
				resourceWriter.AddResource("info2", "Livello di Riempimento dei cassonetti");
				resourceWriter.Close();
			}
			ResourceSet resx = new(directoryName + "\\Resources\\Resources.resx");
			uint[] ports = new uint[4] { 54321u, 5433u, 5432u, 27017u };
			string connectionString = "host=localhost;port=54321;username=ecofil;password=Ma31051983#;database=ecofil;MAXPOOLSIZE=200;timeout=300;";
			bool flag = false;
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			while (true)
			{
				using SshClient sshClient = Function.SshConnection(0, ports, directoryName);
				try
				{
					try
					{
						PostgresConnector postgresConnector = new(connectionString);
						if (!postgresConnector.mStatus)
						{
							throw new Exception();
						}
						Login login = new(postgresConnector, sshClient, resx);
						Application.EnableVisualStyles();
						Application.Run(login);
						string value = login.value;
						bool loggedON = login.loggedON;
						if (login.IsDisposed && value != string.Empty && loggedON)
						{
							if (!postgresConnector.mStatus)
							{
								throw new Exception();
							}
							City city = new(postgresConnector);
							FormCity formCity = new(city, postgresConnector, sshClient, value, resx);
							Application.EnableVisualStyles();
							Application.Run(formCity);
							flag = formCity.close;
						}
						else if (!loggedON && login.IsDisposed)
						{
							if (postgresConnector.mStatus)
							{
                                ((IDisposable)postgresConnector).Dispose();
							}
							sshClient.Disconnect();
							sshClient.Dispose();
							break;
						}
						if (flag)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						sshClient.Disconnect();
						sshClient.Dispose();
						MessageBox.Show(ex.Message, "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						break;
					}
				}
				catch (Exception ex2)
				{
					sshClient.Disconnect();
					sshClient.Dispose();
					MessageBox.Show(ex2.Message, "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					break;
				}
				Hikvision.NET_DVR_Cleanup();
			}
		}
	}
}