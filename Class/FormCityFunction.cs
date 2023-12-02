using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using stt=System.Threading.Tasks;
using System.Windows.Forms;
using Chilkat;
using GMap.NET.WindowsForms;
using excel = Microsoft.Office.Interop.Excel;
using Renci.SshNet;
using System.Windows.Forms.DataVisualization.Charting;

namespace ApplicativoEcofil.Class
{
	internal class FormCityFunction
	{
		public static void CheckSsh (SshClient client)
		{
			if (client.IsConnected)
			{
				return;
			}
			try
			{
				do
				{
					client.Connect();
				}
				while (!client.IsConnected);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				client.Disconnect();
				client.Dispose();
			}
		}
		public static void SaveExcel (DataGridView data, int nstations)
		{
			if (data.RowCount == 0 || data.RowCount < nstations)
			{
				MessageBox.Show("Dati non presenti o non elaborati correttamente", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			SaveFileDialog saveFileDialog = new()
            {
				InitialDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
				Title = "Sceglere la cartella in cui esportare i dati",
				DefaultExt = "xlsx",
				Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
				FilterIndex = 2,
				CheckPathExists = true,
				RestoreDirectory = true
			};
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			try
			{
				Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
				excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
				excel.Worksheet worksheet = (excel.Worksheet)(dynamic)workbook.ActiveSheet;
				worksheet.Name = "Report";
				worksheet.Cells.Font.Size = 15;
				for (int i = 0; i <= data.Columns.Count - 1; i++)
				{
					worksheet.Cells[1, i + 1] = data.Columns[i].HeaderText;
				}
				foreach (DataGridViewRow item in (IEnumerable)data.Rows)
				{
					if (item.Cells[0].Value != null)
					{
						int num = data.Rows.IndexOf(item);
						for (int j = 0; j <= data.Columns.Count - 1; j++)
						{
							worksheet.Cells[num + 2, j + 1] = data.Rows[num].Cells[j].Value.ToString();
						}
					}
				}
				dynamic val = worksheet.Range[(dynamic)worksheet.Cells[1, 1], (dynamic)worksheet.Cells[data.Rows.Count, data.Columns.Count]];
				val.EntireColumn.AutoFit();
				excel.Borders borders = val.Borders;
				borders.LineStyle = excel.XlLineStyle.xlContinuous;
				borders.Weight = 2.0;
				workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
				workbook.Close(Type.Missing, Type.Missing, Type.Missing);
				application.Quit();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			saveFileDialog.Dispose();
		}
		public static void SaveKMZ (GMapControl gMap, DataGridView data, Stations nstations, Garbage garbage)
		{
			if (data.RowCount == 0 || data.RowCount < nstations.nStation.Count)
			{
				MessageBox.Show("Dati non presenti o non elaborati correttamente", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			SaveFileDialog saveFileDialog = new()
            {
				InitialDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
				Title = "Sceglere la cartella in cui esportare la mappa",
				DefaultExt = "kml",
				Filter = "kmz files (*.kmz)|*.kmz|Kml files (*.kml)|*.kml|All files (*.*)|*.*",
				FilterIndex = 2,
				CheckPathExists = true,
				RestoreDirectory = true
			};
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Function.AutoCloseMsg("Attendere 5 secondi per il salvataggio del file", "Information", 5);
			try
			{
				if (gMap.Overlays.Count <= 0)
				{
					throw new Exception("Mappa vuota");
				}
                _ = new KMLZ(saveFileDialog.FileName, nstations, gMap, garbage);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			saveFileDialog.Dispose();
		}		
		public static async stt.Task OpenOption(Garbage garb , string[] garbage)
		{
			await stt.Task.Factory.StartNew(()=>
			{
				Option mainForm = new(garb,garbage);
				Application.EnableVisualStyles();
				Application.Run(mainForm);
			});
		}
		public static void SendScpFile (GMapControl gMap, Stations nstations, string city, double latitude, double longitude, Garbage garbage)
		{
            stt.Task.Run(() =>
			{
				bool busy = false;
				string text = DateTime.Now.ToString("yyyyMMddHHmmss");
				string text2 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\temp\\";
				if (gMap.Overlays.Count <= 0)
				{
					throw new Exception("Mappa vuota");
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				Function.CreateHtmlMaps(text2 + city, city, latitude, longitude, text);
                new KMLZ(text2 + city + text + ".kml", nstations, gMap, garbage);
				stt.Parallel.ForEach(new DirectoryInfo(text2).GetFiles(), new stt.ParallelOptions
				{
					MaxDegreeOfParallelism = 3
				}, (FileInfo file)=>
				{
					try
					{
						Function.CheckFileBusy(file.FullName);
                        SshKey sshKey = new();
						string keyStr = sshKey.LoadText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Resources\\ecofil.it\\cloud-hetzner-private-key.ppk");
						Ssh ssh = new();
						while (true)
						{
							try
							{
								sshKey.Password = "Ma31051983#";
								if (!sshKey.FromPuttyPrivateKey(keyStr))
								{
									MessageBox.Show(sshKey.LastErrorText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									return;
								}
								ssh.Connect("ecofil.it", 2222);
								ssh.AuthenticatePk("root", sshKey);
							}
							catch (Exception)
							{
								MessageBox.Show(ssh.LastErrorText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								continue;
							}
							break;
						}
						if (!busy)
						{
							ssh.QuickCommand("rm -r /var/www/ecofil.it/maps/" + city + "*", "utf-8");
							busy = true;
						}
						Scp scp = new()
                        {
							EnableEvents = true
						};
						scp.UseSsh(ssh);
						if (scp.UploadFile(file.FullName, "/var/www/ecofil.it/maps/" + file.Name))
						{
							scp.Dispose();
							ssh.Dispose();
							if (File.Exists(file.FullName))
							{
								Function.CheckFileBusy(file.FullName);
								File.Delete(file.FullName);
							}
						}
						else
						{
							MessageBox.Show(ssh.LastErrorText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							scp.Dispose();
							ssh.Dispose();
							if (File.Exists(file.FullName))
							{
								Function.CheckFileBusy(file.FullName);
								File.Delete(file.FullName);
							}
						}
					}
					catch (Exception ex2)
					{
						if (File.Exists(file.FullName))
						{
							Function.CheckFileBusy(file.FullName);
							File.Delete(file.FullName);
						}
						MessageBox.Show(ex2.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				});
			}).Wait();
		}
		public static void SendDriveFile (GMapControl gMap, Stations nstations, string city, double latitude, double longitude, Garbage garbage)
		{
				if (gMap.Overlays.Count <= 0)
				{
					throw new Exception("Mappa vuota");
				}
				string text = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\temp\\";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
                 new KMLZ(text + city + ".kmz", nstations, gMap, garbage);
                 new Drive(text + city + ".kmz", city);
		}
		public static Image ResizeImage (Image stem, int width, int height)
		{
			Bitmap bitmap = new(width, height);
			bitmap.SetResolution(stem.HorizontalResolution, stem.VerticalResolution);
			Graphics graphics = Graphics.FromImage(stem);
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			Rectangle destRect = new(0, 0, width, height);
			ImageAttributes imageAttributes = new();
			imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
			graphics.DrawImage(stem, destRect, 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);
			return stem;
		}
		public static GraphicsPath GetRoundPath (Rectangle Rect, int radius)
		{
			float num = (float)radius / 2f;
			GraphicsPath graphicsPath = new();
			graphicsPath.AddArc(Rect.X, Rect.Y, radius, radius, 180f, 90f);
			graphicsPath.AddLine((float)Rect.X + num, Rect.Y, (float)Rect.Width - num, Rect.Y);
			graphicsPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270f, 90f);
			graphicsPath.AddLine(Rect.Width, (float)Rect.Y + num, Rect.Width, (float)Rect.Height - num);
			graphicsPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y + Rect.Height - radius, radius, radius, 0f, 90f);
			graphicsPath.AddLine((float)Rect.Width - num, Rect.Height, (float)Rect.X + num, Rect.Height);
			graphicsPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90f, 90f);
			graphicsPath.AddLine(Rect.X, (float)Rect.Height - num, Rect.X, (float)Rect.Y + num);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
		public static void ChartCleans (Chart chart)
		{
			foreach (Series item in chart.Series)
			{
				item.Points.Clear();
				item.Enabled = false;
				item.IsVisibleInLegend = false;
			}
		}
		public static void SetForms (Chart chart, DataGridView dataGridViewCity, ProgressBar progress, Button button, CheckedListBox check, ComboBox combo, Label label1, string[] garb, Garbage garbage)
		{
			label1.Text = "";
			dataGridViewCity.Enabled = false;
			combo.Enabled = false;
			button.Enabled = false;
			check.Enabled = false;
			check.ClearSelected();
			for (int i = 0; i < check.Items.Count; i++)
			{
				check.SetItemChecked(i, value: false);
			}
			chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
			chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
			chart.ChartAreas[0].AxisY.Maximum = 150.0;
			progress.Minimum = 0;
			progress.Step = 1;
			dataGridViewCity.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			chart.Annotations["1"].Visible = true;
			chart.Annotations["2"].Visible = true;
			chart.Annotations["1"].Width = 0.0;
			chart.Annotations["2"].Width = 0.0;
			ChartCleans(chart);
			dataGridViewCity.Rows.Clear();
			if (dataGridViewCity.Columns.Count == 0 || chart.Series.Count == 0)
			{
				dataGridViewCity.Columns.Clear();
				dataGridViewCity.Columns.Add("station", "Stazione");

				for (int index = 0; index < 10; index++)
				{
					Color color = Color.FromName(garbage.color[Array.IndexOf(garbage.type, garb[10 + index].ToUpper())]);
					chart.Series.Add(new Series { Name = garb[10 + index], Color = color, IsVisibleInLegend = false });
					dataGridViewCity.Columns.Add(garb[index], garb[10 + index]);
				}
			}
		}
	}
}
