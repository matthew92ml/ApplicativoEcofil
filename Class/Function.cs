using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ApplicativoEcofil.Properties;
using Npgsql;
using Renci.SshNet;

namespace ApplicativoEcofil.Class
{
	internal class Function
	{
		private static readonly string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		public static int IndexMax(Dictionary<int, string> reader)
		{
			int result = 0;
			int count = reader.Count;
			int num = count;
			if ((uint)(num - 6) <= 1u)
			{
				result = 4;
			}
			else if (num > 7)
			{
				for (int i = 4; i < reader.Count - 2; i += 3)
				{
					result = ((Math.Max(Convert.ToInt32(reader[i]), Convert.ToInt32(reader[i + 3])) != Convert.ToInt32(reader[i])) ? (i + 3) : i);
				}
			}
			return result;
		}
		public static void EndElaboration(Chart chart, CheckedListBox check, Button button, ProgressBar progress, ComboBox combo, DataGridView data)
		{
			data.Enabled = true;
			combo.Enabled = true;
			foreach (Series item in chart.Series)
			{
				item.Enabled = true;
				item.IsVisibleInLegend = true;
			}
			check.Enabled = true;
			button.Enabled = true;
			progress.Value = 0;
			chart.Annotations["1"].Width = 2 * chart.ChartAreas[0].AxisX.LineWidth;
			chart.Annotations["2"].Width = 2 * chart.ChartAreas[0].AxisX.LineWidth;
		}
		public static void Add(Dictionary<int, Hashtable> result, DataGridView dataGridViewCity)
		{
			foreach (KeyValuePair<int, Hashtable> resultString in result)
			{
				try
				{
					dataGridViewCity.Rows.Add(resultString.Value["idstation"].ToString(), resultString.Value["plastic"].ToString().Replace("Plastica: ", ""), resultString.Value["glass"].ToString().Replace("Vetro: ", ""), resultString.Value["paper"].ToString().Replace("Carta: ", ""), resultString.Value["aluminium"].ToString().Replace("Barattolame: ", ""), resultString.Value["garbage"].ToString().Replace("Indifferenziato: ", ""), resultString.Value["organic"].ToString().Replace("Organico: ", ""), resultString.Value["nappies"].ToString().Replace("Pannolini: ", ""), resultString.Value["oil"].ToString().Replace("Olio: ", ""), resultString.Value["medicine"].ToString().Replace("Medicinali: ", ""), resultString.Value["battery"].ToString().Replace("Pile: ", ""));
					int index = dataGridViewCity.Rows.IndexOf(dataGridViewCity.Rows.Cast<DataGridViewRow>().First((DataGridViewRow x) => x.Cells[0].Value.ToString() == resultString.Value["idstation"].ToString()));
					KeyValuePair<int, Hashtable> keyValuePair = resultString;
					KeyValuePair<int, Hashtable> keyValuePair2 = keyValuePair;
					KeyValuePair<int, Hashtable> keyValuePair3 = keyValuePair2;
					if (keyValuePair3.Value.Values.Cast<string>().Any((string x) => x.Contains("pieno")))
					{
						dataGridViewCity.Rows[index].DefaultCellStyle.BackColor = Color.Red;
						continue;
					}
					KeyValuePair<int, Hashtable> keyValuePair4 = keyValuePair2;
					if (keyValuePair4.Value.Values.Cast<string>().Any((string x) => x.Contains("svuotamento")))
					{
						dataGridViewCity.Rows[index].DefaultCellStyle.BackColor = Color.Yellow;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			dataGridViewCity.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		}
		public static void CreationTable(string line, List<Dictionary<int, string>> table)
		{
			Dictionary<int, string> dictionary = new();
			for (int num = 0; num < line.Split(';').Length - 1; num++)
			{
				dictionary.Add(num, line.Split(';')[num]);
			}
			table.Add(dictionary);
		}
		public static double[] PsReader()
		{
			double[] array;
			using (StreamReader streamReader = new(directory + "\\Resources\\ps.csv"))
			{
				array = Array.ConvertAll<string, double>(streamReader.ReadLine().Split(';'), double.Parse) ;
				streamReader.Dispose();
				streamReader.Close();
			}
			return array;
		}
		public static void PsWriter(FlowLayoutPanel flow)
		{   
			using (StreamWriter streamWriter = new(directory + "\\Resources\\ps.csv"))
			{
				string line = string.Empty;
				TextBox last = flow.Controls.OfType<TextBox>().Last();
				foreach (TextBox support in flow.Controls.OfType<TextBox>())
				{ 
					line += support.Text + (support==last ? "": ";") ;
				}
				streamWriter.WriteLine(line);
				streamWriter.Dispose();
				streamWriter.Close();
			}
		}
		public static bool VpnTest()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.Description.Contains("TAP-Windows Adapter"))
				{
					return true;
				}
			}
			return false;
		}
		public static void ListUncheck(CheckedListBox check)
		{
			for (int i = 0; i <= check.Items.Count - 1; i++)
			{
				check.SetItemCheckState(i, CheckState.Unchecked);
			}
		}
		public static List<Dictionary<int, string>> TableElaboration(List<Dictionary<int, string>> table)
		{
			List<Dictionary<int, string>> list = new();
			string value = string.Empty;
			string value2 = string.Empty;
			foreach (Dictionary<int, string> item2 in table)
			{
				int num = 0;
				if (item2.Keys.Count <= 3)
				{
					continue;
				}
				for (int i = 4; i <= item2.Keys.Count; i += 3)
				{
					if (num < Convert.ToInt32(item2[i]))
					{
						num = Convert.ToInt32(item2[i]);
						value = item2[i + 1];
						value2 = item2[i - 1];
					}
				}
				Dictionary<int, string> item = new()
                {
					{
						0,
						item2[0]
					},
					{
						1,
						item2[1]
					},
					{
						2,
						item2[2]
					},
					{ 3, value2 },
					{
						4,
						num.ToString()
					},
					{ 5, value }
				};
				list.Add(item);
				num = 0;
				value2 = string.Empty;
				value = string.Empty;
			}
			return list;
		}
		private static string Encrypt(string password)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new();
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			bytes = mD5CryptoServiceProvider.ComputeHash(bytes);
			StringBuilder stringBuilder = new();
			byte[] array = bytes;
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2").ToLower());
			}
			return stringBuilder.ToString();
		}
		public static string CheckLogin(PostgresConnector connector, string username, string password)
		{
			string text = "";
			if (username == string.Empty || username == "Inserire Username")
			{
				return "username";
			}
			if (password == string.Empty || password == "Inserire Password")
			{
				return "password";
			}
			try
			{
				if (!connector.mStatus)
				{
					throw new Exception();
				}
				string sql = "Select id, username , password, usertype, attivato from utentiweb  where username = '" + username + "' and password = '" + Encrypt(password) + "' ;";
				if (connector.AddReader("login", sql))
				{
					NpgsqlDataReader data = connector.GetData("login");
					if (data.HasRows)
					{
						while (data.Read())
						{
							if (data.GetInt64(4) != 1)
							{
								return "error-activation";
							}
							text = data.GetInt64(0) + ";" + data.GetString(1) + ";" + data.GetString(2) + ";" + data.GetInt64(3) + ";|";
						}
						return (text.Split('|').Length == 2) ? text.Split(';')[0] : "database";
					}
					return "error-datadb";
				}
				connector.DelReader("login");
				sql = string.Empty;
				sql = "Select id, username , password, usertype from utentiweb  where username = '" + username + "' ;";
				if (connector.ExecuteScalar(sql) > 0)
				{
					return "error-password";
				}
				return "error-username";
			}
			catch (Exception)
			{
				connector.DelReader("login");
				return "error-database";
			}
		}
		public static Hashtable HashEmpty(PostgresConnector connector, int city, string garbage)
		{
			Hashtable hashtable = new();
			string sql = "Select distinct  codicestazione, MAX(dataora) as data from logoperatore where idcomune = " + city + " AND (info ILIKE '%" + garbage + "%' " + (garbage.Contains("INDIFFERENZIATO") ? "OR info ILIKE '%RIFIUTI%'" : "") + ") group by idcomune, codicestazione;";
			if (connector.AddReader("data", sql))
			{
				NpgsqlDataReader data = connector.GetData("data");
				while (data.Read())
				{
					hashtable.Add(data.GetInt32(0), data.GetDateTime(1).ToUniversalTime());
				}
				connector.DelReader("data");
				return hashtable;
			}
			return null;
		}
		private static DateTime LastEmpty(Stations station, int id, Hashtable date)
		{
			DateTime result = (DateTime)date[station.nStation[id]];
			int index = 0;
			double num = 0.0;
			GeoCoordinate geoCoordinate = new(station.latitude[id], station.longitude[id]);
			for (int i = 0; i <= station.nStation.Count - 1; i++)
			{
				if (i != id && !station.ecocentre[i])
				{
					GeoCoordinate other = new(station.latitude[i], station.longitude[i]);
					if ((num == 0.0 || num > geoCoordinate.GetDistanceTo(other)) && i != id)
					{
						num = geoCoordinate.GetDistanceTo(other);
						index = i;
					}
				}
			}
			if (date.Contains(station.nStation[index]) && date.Contains(station.nStation[id]) && ((DateTime)date[station.nStation[index]]).Day > ((DateTime)date[station.nStation[id]]).Day)
			{
				result = Convert.ToDateTime(date[station.nStation[id]]);
			}
			return result;
		}
		private static double CheckPs(PostgresConnector connector, int city, Garbage garbage, int station, int typ, string volume, double ps)
		{
			try
			{
				string sql = "SELECT AVG(ps) from (SELECT distinct idcomune, codicestazione, idrifiuto, peso as s, peso / " + volume + " AS ps, LAG( peso / " + volume + ", 1) over(order by peso) AS ps2, peso/ numerosacchetti as pesosacchetto, numerosacchetti AS ns from  cumulopesi WHERE idcomune = " + city + " AND codicestazione = " + station + " AND idrifiuto = " + garbage.id[typ] + " AND peso IS NOT NULL AND peso != 0 AND numerosacchetti != 0 group by idcomune, codicestazione, numerosacchetti, idrifiuto, nomerifiuto, peso order by ns DESC LIMIT 5) TOTAL;  ";
				if (connector.AddReader("ps", sql))
				{
					sql = string.Empty;
					try
					{
						NpgsqlDataReader data = connector.GetData("ps");
						data.Read();
						ps = data.GetDouble(0);
					}
					catch (Exception)
					{
						connector.DelReader("ps");
						sql = "SELECT MAX(ps) from (SELECT distinct ic.idcomune, ic.codicestazione, idrifiuto, peso as s, coalesce(ic.cassonetto1, 0) + coalesce(ic.cassonetto2, 0) + coalesce(ic.cassonetto3, 0) + coalesce(ic.cassonetto4, 0) + coalesce(ic.cassonetto5, 0) as volume,  peso / (coalesce(ic.cassonetto1, 0) + coalesce(ic.cassonetto2, 0) + coalesce(ic.cassonetto3, 0) + coalesce(ic.cassonetto4, 0) + coalesce(ic.cassonetto5, 0)) AS ps, LAG( peso / (coalesce(ic.cassonetto1, 0) + coalesce(ic.cassonetto2, 0) + coalesce(ic.cassonetto3, 0) + coalesce(ic.cassonetto4, 0) + coalesce(ic.cassonetto5, 0)), 1) over(order by peso) AS ps2, peso/ numerosacchetti as pesosacchetto, numerosacchetti AS ns from cumulopesi cp left join infocassonetti ic on ic.idcomune = cp.idcomune AND ic.codicestazione = cp.codicestazione AND ic.tiporifiuto = cp.idrifiuto  WHERE tiporifiuto = " + garbage.id[typ] + " AND ic.cassonetto1 IS NOT NULL AND peso IS NOT NULL AND peso != 0 AND numerosacchetti != 0 AND numerosacchetti> 5 group by ic.idcomune, ic.codicestazione, numerosacchetti, cassonetto1, cassonetto2, cassonetto3, cassonetto4, cassonetto5, idrifiuto, nomerifiuto, peso order by ns DESC ) total ; ";
						if (connector.AddReader("ps", sql))
						{
							NpgsqlDataReader data2 = connector.GetData("ps");
							data2.Read();
							ps = data2.GetDouble(0);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				AutoCloseMsg(ex2.Message, "Peso specifico " + station + " " + garbage.type[typ], 2);
			}
			connector.DelReader("ps");
			return ps;
		}
		public static string PercentualFull(PostgresConnector connector, Stations station, int city, Garbage garbage, string nameG, List<string> type, double ps, int typ, int id, Hashtable dateEmpty = null)
		{
			string text = type[id];
			string text2 = text;
			if (text2 == "0")
			{
				return "Bidone non disponibile";
			}
			DateTime dateTime = LastEmpty(station, id, dateEmpty).ToUniversalTime();
			ps = ((bool)Settings.Default["ActivePS"]) ? CheckPs(connector, city, garbage, station.nStation[id], typ, type[id], ps) : ps;
			string sql = "SELECT COALESCE( SUM(CASE WHEN  peso = peso1 THEN 0 ELSE peso END),0) , lastdatasend from (Select peso, LEAD(peso, 1) over(order by dataora) peso1, idcomune, codicestazione from conferimenti where dataora>= '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and idcomune = " + city + " and codicestazione = " + station.nStation[id] + " and tiporifiuto = " + garbage.id[typ] + " group by idcomune, codicestazione,peso, dataora) as t inner join datistazione dt on dt.idcomune = t.idcomune AND dt.idstazione = t.codicestazione GROUP BY lastdatasend ; ";
			string result;
			if (connector.AddReader("garbage", sql))
			{
				NpgsqlDataReader data = connector.GetData("garbage");
				data.Read();
				DateTime dateTime2 = data.IsDBNull(1)?dateTime:data.GetDateTime(1);
				DateTime now = DateTime.Now;
				if (Math.Abs(now.Hour - dateTime2.Hour) > 23 || (Math.Abs(now.Hour - dateTime2.Hour) > 1 && Math.Abs(now.Day - dateTime2.Day) >= 1))
				{
					result = nameG + ": dato non disponibile dal " + dateTime2.ToString("MM/dd/yyyy");
				}
				else
				{
					double @double = data.GetDouble(0);
					if (@double != 0.0)
					{
						double num = @double / ps / (Convert.ToDouble(type[id]) / 100.0);
						result = ((num > (double)(int)Settings.Default["Max"]) ? ("Bidone " + nameG + " pieno") : (nameG + ": " + num.ToString("0") + "%"));
					}
					else
					{
						result = nameG + ": svuotamento il " + dateTime.ToString("MM/dd/yyyy");
					}
				}
			}
			else
			{
				result = nameG + ": svuotamento il " + dateTime.ToString("MM/dd/yyyy");
			}
			connector.DelReader("garbage");
			return result;
		}
		public static SshClient SshConnection(int choose, uint[] ports, string pathLocation)
		{
			bool flag = true;
			IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] activeTcpConnections = iPGlobalProperties.GetActiveTcpConnections();
			ForwardedPortLocal forwardedPortLocal;
			SshClient sshClient;
			switch (choose)
			{
			case 0:
			{
				string text = "sql.ecofil.it";
				PrivateKeyFile privateKeyFile = new(pathLocation + "\\Resources\\" + text + "\\id_rsa", "Ma31051983#");
				forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", ports[0], "localhost", ports[1]);
				sshClient = new SshClient(text, 2222, "ecofil", privateKeyFile);
				break;
			}
			case 1:
			{
				string text = "vpn.ecofil.it";
				PrivateKeyFile privateKeyFile = new(pathLocation + "\\Resources\\" + text + "\\id_dsa", "Ma31051983#");
				forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", ports[2], "127.0.0.1", ports[3]);
				sshClient = new SshClient(text, 2222, "root", privateKeyFile);
				break;
			}
			default:
			{
				string text = "sql.ecofil.it";
						PrivateKeyFile privateKeyFile = new(pathLocation + "\\Resources\\" + text + "\\id_rsa", "Ma31051983#");
				forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", ports[0], "localhost", ports[1]);
				sshClient = new SshClient(text, 2222, "ecofil", privateKeyFile);
				break;
			}
			}
			try
			{
				do
				{
					TcpConnectionInformation[] array = activeTcpConnections;
					foreach (TcpConnectionInformation tcpConnectionInformation in array)
					{
						if (tcpConnectionInformation.LocalEndPoint.Port == ports[3] || tcpConnectionInformation.LocalEndPoint.Port == ports[2] || tcpConnectionInformation.LocalEndPoint.Port == ports[1] || tcpConnectionInformation.LocalEndPoint.Port == ports[0])
						{
							try
							{
								DisconnectionCheck.CloseRemotePort(ports[0]);
								DisconnectionCheck.CloseRemotePort(ports[1]);
								DisconnectionCheck.CloseRemotePort(ports[2]);
								DisconnectionCheck.CloseRemotePort(ports[3]);
								DisconnectionCheck.CloseLocalPort(ports[0]);
								DisconnectionCheck.CloseLocalPort(ports[1]);
								DisconnectionCheck.CloseLocalPort(ports[2]);
								DisconnectionCheck.CloseLocalPort(ports[3]);
								flag = true;
							}
							catch (Exception ex)
							{
								MessageBox.Show(ex.Message, "Control connection open");
								flag = false;
							}
							break;
						}
					}
					sshClient.Connect();
					sshClient.AddForwardedPort(forwardedPortLocal);
					while (flag)
					{
						if (!forwardedPortLocal.IsStarted)
						{
							forwardedPortLocal.Start();
							flag = true;
							break;
						}
					}
				}
				while (!forwardedPortLocal.IsStarted);
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
				sshClient.Disconnect();
				sshClient.Dispose();
			}
			return sshClient;
		}
		public static void AutoCloseMsg(string msg, string title, int time)
		{
			Form w = new()
            {
				Size = new Size(0, 0)
			};
			Task.Delay(TimeSpan.FromSeconds(time)).ContinueWith(delegate
			{
				w.Close();
			}, TaskScheduler.FromCurrentSynchronizationContext());
			MessageBox.Show(w, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		public static void CreateHtmlMaps(string path, string city, double latitude, double longitude, string time)
		{
			string[] array = Resources.modelMap.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			using FileStream stream = new(path + ".html", FileMode.Create);
			using StreamWriter streamWriter = new(stream, Encoding.UTF8);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text;
				string text3 = text2;
				if (text3 != null)
				{
					if (text3.Contains("$1"))
					{
						streamWriter.WriteLine(text.Replace("$1", city));
						continue;
					}
					string text4 = text3;
					if (text4.Contains("$2"))
					{
						streamWriter.WriteLine(text.Replace("$2", latitude.ToString().Replace(",", ".")).Replace("$3", longitude.ToString().Replace(",", ".")));
						continue;
					}
					string text5 = text3;
					if (text5.Contains("$4"))
					{
						streamWriter.WriteLine(text.Replace("$4", city + time));
						continue;
					}
				}
				streamWriter.WriteLine(text);
			}
			streamWriter.Close();
			streamWriter.Dispose();
		}
		public static void CheckFileBusy(string path)
		{
			while (true)
			{
				try
				{
					using FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
					if (fileStream != null)
					{
						fileStream.Close();
						break;
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
