using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
//using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ApplicativoEcofil.Class;
using ApplicativoEcofil.Properties;
using GMap.NET;
using Renci.SshNet;
using Cur=System.Windows.Forms.Cursor;

namespace ApplicativoEcofil
{
	public partial class FormCity : Form
	{
		[DllImport("kernel32.dll")]
		static extern int GetCurrentThreadId();

		public delegate void delPassData(DataGridView data, int operation, List<Dictionary<int, string>> table, KeyValuePair<int, string> combo);
		private readonly ResourceSet res;
		private static delPassData data2;
		private Stopwatch timer;
        private readonly System.Windows.Forms.Timer evUpdate = new();
		private readonly System.Windows.Forms.Timer evAlarm = new();
		private readonly System.Windows.Forms.Timer evListEmpty = new();
		private readonly System.Windows.Forms.Timer evSsh = new();
		private readonly List<Dictionary<int, string>> table = new();
		private object setGarbage = null;
		private readonly int zero = 0;
		public List<PointLatLng> points;
		public string userForm;
		public Garbage garbage;
		public SshClient client;
		public int contator = 0;
		public int th = 0;
		public Stations stationForm;
		public City cityForm;
		public PostgresConnector connectorForm;
		public bool close = true;
		public HorizontalLineAnnotation line;
		public HorizontalLineAnnotation line2;
		private static int garbageClick, nThread;

		private bool drag = false;
		private Point pmouse;
		private Point pform;
		private bool select = false;
		//private readonly bool notFirst = false;
		private static List<Hashtable> listEmpty;
		private static List<Hashtable> listEmpty2;
		private static readonly string[] garb = new string[20]
		{
		"plastic", "glass", "paper", "aluminium", "garbage", "organic", "nappies", "oil", "medicine", "battery",
		"Plastica", "Vetro", "Carta", "Barattolame", "Indifferenziato", "Organico", "Pannolini", "Olio", "Medicinali", "Pile"
		};
		private static List<string>[] listGarbage;	
		public FormCity()
		{
			InitializeComponent();
		}
		public FormCity(string message)
		{
			InitializeComponent();
			labelCity.TextAlign = ContentAlignment.TopCenter;
			labelCity.Text = message;
			pictureBoxCity.Image = Resources.LogoEcofil2020;
			pictureBoxCity.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBoxCity.Scale(new SizeF(1.2f, 0.8f));
		}
		public FormCity(City city, PostgresConnector connector, SshClient com, string user, ResourceSet resx)
		{
			res = resx;
			Text = resx.GetObject("description").ToString() + resx.GetObject("version").ToString();
			userForm = user;
			client = com;
			connectorForm = connector;
			cityForm = city;
			short num = Convert.ToInt16(user);
			short num2 = num;
			if (num2 == 0)
			{
				InitializeComponent();
				line = new HorizontalLineAnnotation
				{
					IsSizeAlwaysRelative = false,
					Name = "1",
					AxisX = chartCity.ChartAreas[0].AxisX,
					AxisY = chartCity.ChartAreas[0].AxisY,
					Y = 100.0,
					X = 0.0,
					AnchorX = 0.0,
					AnchorY = 100.0,
					LineWidth = 4,
					Visible = true,
					LineColor = Color.Red
				};
				line.IsSizeAlwaysRelative = false;
				chartCity.Annotations.Add(line);
				line2 = new HorizontalLineAnnotation
				{
					IsSizeAlwaysRelative = false,
					Name = "2",
					AxisX = chartCity.ChartAreas[0].AxisX,
					AxisY = chartCity.ChartAreas[0].AxisY,
					Y = 85.0,
					X = 0.0,
					AnchorX = 0.0,
					AnchorY = 85.0,
					LineWidth = 4,
					Visible = true,
					LineColor = Color.Orange
				};
				line2.IsSizeAlwaysRelative = false;
				chartCity.Annotations.Add(line2);
				ComboBox.ObjectCollection items = comboBoxNumberCity.Items;
				object[] nameCity = city.nameCity;
				items.AddRange(nameCity);
				labelState.TextAlign = ContentAlignment.TopCenter;
				labelCity.TextAlign = ContentAlignment.TopCenter;
				labelCity.Text = "Scegliere il comune da elaborare";
				labelTitle2.Visible = false;
				pictureBoxCity.Image = Resources.LogoEcofil2020;
				pictureBoxCity.SizeMode = PictureBoxSizeMode.StretchImage;
				pictureBoxCity.Scale(new SizeF(1.2f, 1f));
			}
			else if (num2 > 1)
			{
				InitializeComponent();
				garbage = new Garbage(connectorForm, cityForm.idCity[Convert.ToUInt32(userForm)]);
				stationForm = new Stations(connectorForm, cityForm.idCity[Convert.ToInt32(user)], Convert.ToInt32(user), garbage);
				OpenFormMaps();
				Task.Run(()=>
				{
					CheckEmpty(this, new EventArgs());
				});
				zero = (int)Math.Floor(Math.Log10(stationForm.nStation.Max()) + 1.0);
				string[] type = garbage.type;
				foreach (string item in type)
				{
					checkListMenuGarbage.Items.Add(item, isChecked: false);
				}
				Label label = labelTitleGarbage;
				label.Text = label.Text + " di " + cityForm.nameCity[Convert.ToInt32(user)];
				line = new HorizontalLineAnnotation
				{
					IsSizeAlwaysRelative = false,
					Name = "1",
					AxisX = chartCity.ChartAreas[0].AxisX,
					AxisY = chartCity.ChartAreas[0].AxisY,
					Y = 100.0,
					X = 0.0,
					AnchorX = 0.0,
					AnchorY = 100.0,
					LineWidth = 4,
					Visible = true,
					LineColor = Color.Red
				};
				line.IsSizeAlwaysRelative = false;
				chartCity.Annotations.Add(line);
				line2 = new HorizontalLineAnnotation
				{
					IsSizeAlwaysRelative = false,
					Name = "2",
					AxisX = chartCity.ChartAreas[0].AxisX,
					AxisY = chartCity.ChartAreas[0].AxisY,
					Y = 85.0,
					X = 0.0,
					AnchorX = 0.0,
					AnchorY = 85.0,
					LineWidth = 4,
					Visible = true,
					LineColor = Color.Orange
				};
				line2.IsSizeAlwaysRelative = false;
				chartCity.Annotations.Add(line2);
				comboBoxNumberCity.Text = "Stazione di partenza...";
				foreach (int item2 in stationForm.nStation)
				{
					flowCheckStations.Controls.Add(new CheckBox() {Checked=true,Width=40,Height=20,
						Text= item2.ToString().PadLeft(zero, '0'), Name= item2.ToString().PadLeft(zero, '0')});
					if (!stationForm.ecocentre[stationForm.nStation.IndexOf(item2)])
					{
						comboBoxNumberCity.Items.Add(item2.ToString().PadLeft(zero, '0'));
					}
				}
				labelState.TextAlign = ContentAlignment.TopCenter;
				labelCity.TextAlign = ContentAlignment.TopCenter;
				labelCity.Font = (cityForm.nameCity[Convert.ToInt32(user)].Length <= 11) ? labelCity.Font : new Font(labelCity.Font.FontFamily, labelCity.Font.Size - (float)((cityForm.nameCity[Convert.ToInt32(user)].Length - 11) / 3), labelCity.Font.Style);
				labelCity.Text = "Comune di " + cityForm.nameCity[Convert.ToInt32(user)];
				pictureBoxCity.Image = (Image) new Bitmap((Image)Resources.ResourceManager.GetObject("comuni." + cityForm.idCity[Convert.ToInt32(user)].ToString()), new Size(pictureBoxCity.Width, pictureBoxCity.Height));
				pictureBoxCity.SizeMode = PictureBoxSizeMode.AutoSize;
				checkListMenuGarbage.Enabled = false;
				evSsh.Interval = 1200000;
				evSsh.Tick += CheckSsh;
				evSsh.Start();
				if ((bool)Settings.Default["updateData2"])
				{
					evListEmpty.Interval = (int)Settings.Default["timeMinutes"] * 60000;
					evListEmpty.Tick += CheckEmpty;
					evListEmpty.Start();
				}
				if ((bool)Settings.Default["updateData"])
				{
					evUpdate.Tick += ButtonCity_Click;
					evUpdate.Interval = (int)Settings.Default["timeSeconds"] * 1000;
					evUpdate.Start();
				}
				if (Settings.Default["AlarmClock"].ToString() != "")
				{
					evAlarm.Tick += CheckClock;
					evAlarm.Interval = 1000;
					evAlarm.Start();
				}
				listGarbage = new List<string>[10] { stationForm.garbage5, stationForm.garbage7, stationForm.garbage6, stationForm.garbage4, stationForm.garbage8, stationForm.garbage10, stationForm.garbage9, stationForm.garbage1, stationForm.garbage2, stationForm.garbage3 };
			}
			Region = new Region(FormCityFunction.GetRoundPath(DisplayRectangle, Height / 15));
			GraphicsPath graphicsPath = new();
			graphicsPath.AddEllipse(pictureBoxCity.DisplayRectangle);
			pictureBoxCity.Region = new Region(graphicsPath);
			buttonCity.Region = new Region(FormCityFunction.GetRoundPath(buttonCity.DisplayRectangle, buttonCity.Height / 2));
			panelUser.BackColor = Color.Transparent;
			panelStation.BackColor = Color.Transparent;
			chartCity.BackColor = Color.Transparent;
			chartCity.ChartAreas[0].BackColor = Color.Transparent;
			BackgroundImageLayout = ImageLayout.Stretch;
			panelCity.BackColor = Color.Transparent;
			panelCityDown.BackColor = Color.Transparent;
			flowCheckStations.Enabled = checkBoxStation.Checked;
			Region= new Region(FormCityFunction.GetRoundPath(DisplayRectangle, Height / 15));
			fileToolStripMenuItem.Image = Resources.faviconj;
			Icon = Resources.favicon;
			BackgroundImage = Resources.background1;
		}
		private void CheckSsh(object sender, EventArgs e)
		{
			FormCityFunction.CheckSsh(client);
		}
		private void CheckEmpty(object sender, EventArgs e)
		{
			listEmpty2 = listEmpty;
			buttonCity.Enabled = false;
			using (PostgresConnector postgresConnector = new(connectorForm.connection))
			{
				listEmpty = new List<Hashtable>();
				int index;
				for (index = 0; index < 10; index++)
				{
					timer = new Stopwatch();
					Hashtable hashtable;
					while (true)
					{
						int num = Array.FindIndex(garbage.type, (string x) => x == garb[10 + index].ToUpper());
						hashtable = Function.HashEmpty(postgresConnector, cityForm.idCity[Convert.ToUInt32(userForm)], garbage.type[num]);
						if (hashtable.Count == listGarbage[index].Where((string x) => x != "0").Count() || timer.Elapsed.TotalSeconds > 5.0)
						{
							break;
						}
						if (!timer.IsRunning)
						{
							timer.Start();
						}
					}
					if (timer.IsRunning)
					{
						timer.Stop();
						timer.Reset();
					}
					listEmpty.Add(hashtable);
					if (dataGridViewCity.RowCount <= 1)
					{
						continue;
					}
					foreach (DataGridViewRow item in (IEnumerable)dataGridViewCity.Rows)
					{
						DateTime dateTime = Convert.ToDateTime(listEmpty[index][Convert.ToInt32(item.Cells[0].Value)]);
						DateTime dateTime2 = Convert.ToDateTime(listEmpty2[index][Convert.ToInt32(item.Cells[0].Value)]);
						if (dateTime > dateTime2)
						{
							item.Cells[index + 1].Value = "svuotamento il " + dateTime.ToString("MM/dd/yyyy");
						}
					}
				}
                ((IDisposable)postgresConnector).Dispose();
			}
			Invoke((Action)delegate
			{
				buttonCity.Enabled = true;
			});
		}
		private void CheckClock(object sender, EventArgs e)
		{
			Hashtable hashtable = new();
			foreach (string item in from x in Settings.Default["AlarmClock"].ToString().Split(';')
									where x != ""
									select x)
			{
				string[] array = item.Split('-');
				hashtable.Add(array[0], array[1]);
			}
			string key = DateTime.Now.ToString("dddHH:mm:ss");
			if (hashtable.Contains(key))
			{
				evAlarm.Stop();
				setGarbage = hashtable[key];
				buttonCity.PerformClick();
			}
		}
		/*private void EndAsyncEvent(IAsyncResult execute)
		{
			((EventHandler)((AsyncResult)execute).AsyncDelegate).EndInvoke(execute);
		}*/
		private async Task Buttonexecute(double[] gb, int[] i, object setGarbage = null)
		{
			try
			{
				double[] ps = Function.PsReader();
                ((IDisposable)connectorForm).Dispose();
				Dictionary<int, Hashtable> result = new();
				Exception ex2;
				timer.Start();
                await Task.Factory.StartNew(() =>
				{
					try
					{
						Parallel.For(0, stationForm.nStation.Count, new ParallelOptions
						{
							MaxDegreeOfParallelism = Environment.ProcessorCount
						}, 
						(counter)=>
						{
							try
							{
								string support = stationForm.nStation[counter].ToString().PadLeft(zero, '0');
							if (flowCheckStations.Controls.ContainsKey(support) &&
							!((CheckBox)flowCheckStations.Controls[support]).Checked)
							{
								return;
							}
							double num = 0.0;
							int num2 = 0;
							string empty = string.Empty;
							string value = stationForm.nStation[counter].ToString().PadLeft(zero, '0');
							Hashtable hashtable = new() { { "idstation", value } };
							empty = string.Format("{0};{1};{2};", value, stationForm.latitude[counter].ToString("0.0000000"), stationForm.longitude[counter].ToString("0.0000000"));
                            using PostgresConnector postgresConnector = new(connectorForm.connection);
							for (int index = 0; index < 10; index++)
                                {
                                    try
                                    {
                                        num = 0.0;
                                        num2 = Array.FindIndex(garbage.type, (string x) => x == garb[10 + index].ToUpper());
                                        hashtable.Add(garb[index], Function.PercentualFull(postgresConnector, stationForm, cityForm.idCity[Convert.ToUInt32(userForm)], garbage, garb[10 + index], listGarbage[index], ps[index], num2, counter, listEmpty[index]));
                                        if (!hashtable[garb[index]].ToString().Contains("Bidone") && !hashtable[garb[index]].ToString().Contains("dato") && !hashtable[garb[index]].ToString().Contains("svuotamento") && hashtable[garb[index]].ToString() != "")
                                        {
                                            num = Convert.ToDouble(hashtable[garb[index]].ToString().Replace(garb[10 + index] + ": ", "").Replace("%", ""));
                                        }
                                        gb[index] += num;
                                        if (!hashtable[garb[index]].ToString().Contains("Bidone"))
                                        {
                                            i[index]++;
                                        }
                                        if (Enumerable.Range((int)Settings.Default["Min"], (int)Settings.Default["Max"] - (int)Settings.Default["Min"]).Contains(Convert.ToInt32(num)))
                                        {
                                            empty += $"{garbage.type[num2]};{num};{garbage.color[num2]};";
                                        }
									BeginInvoke(() =>
									{
										WorkProgress();
									});
								}
                                    catch (Exception ex)
                                    {
                                    }
                                } 
								result.Add(counter, hashtable);
					       Function.CreationTable(empty, table);
                           postgresConnector.Dispose();
							}
							catch (Exception ex) { }
						});
						timer.Stop();
						connectorForm = new PostgresConnector(connectorForm.connection);
						BeginInvoke(()=>
						{
							Function.Add(result, dataGridViewCity);
							Function.EndElaboration(chartCity, checkListMenuGarbage, buttonCity, progressBarCity, comboBoxNumberCity, dataGridViewCity);
							foreach (DataGridViewCell cell in dataGridViewCity.Rows[0].Cells)
							{
								int columnIndex = cell.ColumnIndex;
								string headerText = dataGridViewCity.Columns[columnIndex].HeaderText;
								if (!headerText.Contains("Stazione"))
								{
									columnIndex--;
									chartCity.Series[headerText].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], (columnIndex != 6) ? (gb[columnIndex] / (double)i[columnIndex]) : ((gb[columnIndex] > 3150.0) ? 0.0 : (gb[columnIndex] / (double)i[columnIndex])));
								}
							}
							data2(dataGridViewCity, 3, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
							labelState.Text = "Clicca qui per la mappa online (Elaborato il " + DateTime.Now.ToString() + " in " + timer.Elapsed.TotalSeconds.ToString("0") + " secondi)";
							labelTitle2.Text = res.GetObject("info2").ToString();
							buttonCity.Text = "Aggiorna";
							evListEmpty.Start();
							if (setGarbage != null)
							{
								checkListMenuGarbage.SetItemChecked(checkListMenuGarbage.Items.IndexOf(setGarbage), value: true);
							}
							if ((bool)Settings.Default["updateData"])
							{
								evUpdate.Interval = (int)Settings.Default["timeSeconds"] * 1000;
								evUpdate.Start();
							}
							if (Settings.Default["AlarmClock"].ToString() != "")
							{
								evAlarm.Start();
							}
						});
					}
					catch (Exception ex3)
					{
						ex2 = ex3;
						Invoke(()=>
						{
							MessageBox.Show(ex2.Message, "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							buttonCity.Enabled = true;
							buttonCity.Text = "Aggiorna";
							progressBarCity.Value = 0;
							evListEmpty.Start();
							if ((bool)Settings.Default["updateData"])
							{
								evUpdate.Interval = (int)Settings.Default["timeSeconds"] * 1000;
								evUpdate.Start();
							}
							if (Settings.Default["AlarmClock"].ToString() != "")
							{
								evUpdate.Start();
							}
						});
					}
				});
			}
			catch (Exception)
			{
				Invoke(()=>
				{
					MessageBox.Show("Errore di elaborazione dati", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					buttonCity.Enabled = true;
					buttonCity.Text = "Aggiorna";
					progressBarCity.Value = 0;
				});
			}
		}
		private void WorkProgress()
		{
			progressBarCity.PerformStep();
		}
		private void DataGridViewCity_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1)
			{
				return;
			}
			string idStation = dataGridViewCity.Rows[e.RowIndex].Cells["station"].Value.ToString();
			int index = stationForm.nStation.IndexOf(Convert.ToInt32(idStation));
			if (stationForm.hikvision[index])
			{
				BeginInvoke(()=>
				{
					new CameraHikvision(dataGridViewCity.Rows[e.RowIndex].Cells["station"].Value.ToString(), stationForm.ipvpn[index]).ShowDialog();
				});
			}
			if (chartCity.Series["Plastica"].Points.Any((DataPoint x) => x.AxisLabel.Contains(idStation)))
			{
				return;
			}
			if (chartCity.Series["Plastica"].Points.Count <= 3)
			{
				if (userForm == "0")
				{
					labelCity.Text = "Comune di " + cityForm.nameCity[comboBoxNumberCity.SelectedIndex];
				}
				try
				{
					foreach (DataGridViewCell cell in dataGridViewCity.Rows[e.RowIndex].Cells)
					{
						string text = cell.Value.ToString();
						if (!(text == idStation))
						{
							string headerText = dataGridViewCity.Columns[cell.ColumnIndex].HeaderText;
							chartCity.Series[headerText].Points.AddXY("Stazione " + idStation, (!text.ToString().Contains("Bidone") && !text.ToString().Contains("svuotamento") && !text.ToString().Contains("dato")) ? Convert.ToInt32(text.Replace(" ", "").Replace("%", "")) : 0);
						}
					}
					int num = chartCity.Series["Pile"].Points.Count * chartCity.ChartAreas[0].AxisX.LineWidth;
					chartCity.Annotations["1"].Width = Convert.ToInt32(0.33 * (double)num) + num;
					chartCity.Annotations["2"].Width = Convert.ToInt32(0.33 * (double)num) + num;
					return;
				}
				catch (Exception)
				{
					MessageBox.Show("Errore nei dati da visualizzare", "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			MessageBox.Show("Numero massimo di stazioni selezionabili inserite", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		private void FormCity_Load(object sender, EventArgs e)
		{
		}
		private void CheckListMenuGarbage_SelectedValueChanged(object sender, EventArgs e)
		{
			int count = checkListMenuGarbage.CheckedItems.Count;
			int num = count;
			switch (num)
			{
				case 0:
					data2(dataGridViewCity, 1, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
					break;
				case 1:
					if (checkListMenuGarbage.GetItemCheckState(garbageClick) == CheckState.Checked && !select)
					{
						data2(dataGridViewCity, 1, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
						Function.ListUncheck(checkListMenuGarbage);
					}
					break;
				case 2:
					if (userForm != "3")
					{
						Function.ListUncheck(checkListMenuGarbage);
						MessageBox.Show("Selezionare solo un rifiuto!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					break;
				default:
					if (num > 2 && userForm == "3")
					{
						Function.ListUncheck(checkListMenuGarbage);
						MessageBox.Show("Selezionare solo 2 rifiuti!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					break;
			}
		}
		/*private void FormCity_SizeChanged(object sender, EventArgs e)
		{
			if (notFirst)
			{
				dataGridViewCity.Height = base.Height / 100 * ((base.WindowState == FormWindowState.Maximized) ? 40 : 38);
			}
		}
		*/
		public void ButtonCity_Click(object sender, EventArgs e)
		{
			evListEmpty.Stop();
			if ((bool)Settings.Default["updateData"])
			{
				evUpdate.Stop();
			}
			if (Settings.Default["AlarmClock"].ToString() != "")
			{
				evAlarm.Stop();
			}
			double[] gb = new double[this.garbage.color.Where((string x) => x != null).Count()];
			int[] i = new int[garbage.color.Where((string x) => x != null).Count()];
			timer = new Stopwatch();
			short num = Convert.ToInt16(userForm);
			short num2 = num;
			if (num2 == 0)
			{
				dataGridViewCity.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				chartCity.Annotations["1"].Visible = true;
				chartCity.Annotations["2"].Visible = true;
				chartCity.Annotations["1"].Width = 0.0;
				chartCity.Annotations["2"].Width = 0.0;
				try
				{
					buttonCity.Enabled = false;
					comboBoxNumberCity.Enabled = false;
					using (StreamReader streamReader = new(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Resources\\ps.csv"))
					{
						while (!streamReader.EndOfStream)
						{
							string[] array = streamReader.ReadLine().Split(';');
							if (!array[0].Contains("Plastica"))
							{
								(new double[10])[0] = Convert.ToDouble(array[0]);
								(new double[10])[1] = Convert.ToDouble(array[1]);
								(new double[10])[2] = Convert.ToDouble(array[2]);
								(new double[10])[3] = Convert.ToDouble(array[3]);
								(new double[10])[4] = Convert.ToDouble(array[4]);
								(new double[10])[5] = Convert.ToDouble(array[5]);
								(new double[10])[6] = Convert.ToDouble(array[6]);
								(new double[10])[6] = Convert.ToDouble(array[7]);
								(new double[10])[8] = Convert.ToDouble(array[8]);
								(new double[10])[9] = Convert.ToDouble(array[9]);
							}
						}
						streamReader.Dispose();
						streamReader.Close();
					}
					foreach (Series item in chartCity.Series)
					{
						item.Points.Clear();
						item.Enabled = false;
						item.IsVisibleInLegend = false;
					}
					if (comboBoxNumberCity.SelectedIndex <= 9)
					{
						if (chartCity.Series.IndexOf("Plastica") == -1)
						{
							chartCity.Series.Add("Plastica");
							chartCity.Series.Add("Vetro");
							chartCity.Series.Add("Carta");
							chartCity.Series.Add("Barattolame");
							chartCity.Series.Add("Indifferenziato");
							chartCity.Series.Add("Organico");
							chartCity.Series.Add("Pannolini");
							chartCity.Series.Add("Olio");
							chartCity.Series.Add("Medicinali");
							chartCity.Series.Add("Pile");
						}
						Stations stations = new(connectorForm, cityForm.idCity[comboBoxNumberCity.SelectedIndex], comboBoxNumberCity.SelectedIndex, this.garbage);
						labelCity.Text = "Comune di " + cityForm.nameCity[comboBoxNumberCity.SelectedIndex];
						pictureBoxCity.Image = Image.FromFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/img/comuni/" + cityForm.idCity[comboBoxNumberCity.SelectedIndex] + ".jpg");
						pictureBoxCity.SizeMode = PictureBoxSizeMode.Zoom;
						int num3 = 0;
						double num4 = 0.0;
						double num5 = 0.0;
						double num6 = 0.0;
						double num7 = 0.0;
						double num8 = 0.0;
						double num9 = 0.0;
						double num10 = 0.0;
						double num11 = 0.0;
						double num12 = 0.0;
						double num13 = 0.0;
						chartCity.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
						chartCity.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
						chartCity.ChartAreas[0].AxisY.Maximum = 150.0;
						string path = "D:/App/repos/ApplicativoEcofil/ApplicativoEcofil/resource/" + cityForm.nameCity[comboBoxNumberCity.SelectedIndex] + ".csv";
						if (File.Exists(path))
						{
							File.Delete(path);
						}
						Hashtable hashtable = new();
						StreamWriter streamWriter = new(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write));
						try
						{
							Garbage garbage = new(connectorForm, cityForm.idCity[comboBoxNumberCity.SelectedIndex]);
							progressBarCity.Minimum = 0;
							progressBarCity.Step = 1;
							progressBarCity.Maximum = (stations.nStation.Count - 1) * 10;
							string value = string.Format("{0};{1};{2};{3};{4};{5}", "Stazione", "Latitudine", "Longitutide", cityForm.nameCity[comboBoxNumberCity.SelectedIndex], cityForm.latitude[comboBoxNumberCity.SelectedIndex], cityForm.longitude[comboBoxNumberCity.SelectedIndex]);
							streamWriter.WriteLine(value);
							dataGridViewCity.Rows.Clear();
							if (dataGridViewCity.Columns.Count == 0)
							{
								dataGridViewCity.Columns.Add("station", "Stazione");
								dataGridViewCity.Columns.Add("plastic", "Plastica");
								dataGridViewCity.Columns.Add("glass", "Vetro");
								dataGridViewCity.Columns.Add("paper", "Carta");
								dataGridViewCity.Columns.Add("aluminium", "Barattolame");
								dataGridViewCity.Columns.Add("garbage", "Indifferenziato");
								dataGridViewCity.Columns.Add("organic", "Organico");
								dataGridViewCity.Columns.Add("nappies", "Pannolini");
								dataGridViewCity.Columns.Add("oil", "Olio");
								dataGridViewCity.Columns.Add("medicine", "Medicinali");
								dataGridViewCity.Columns.Add("battery", "Pile");
							}
							timer.Start();
							while (num3 <= stations.nStation.Count - 1)
							{
								hashtable.Clear();
								hashtable.Add("idstation", stations.nStation[num3 + 1]);
								value = string.Format("{0};{1};{2};", stations.nStation[num3 + 1], stations.latitude[num3].ToString("0.0000000"), stations.longitude[num3].ToString("0.0000000"));
								hashtable.Add("plastic", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Plastica", stations.garbage5, (new double[10])[0], 3, num3));
								if (!hashtable["plastic"].ToString().Contains("Bidone") && !hashtable["plastic"].ToString().Contains("svuotamento") && !hashtable["plastic"].ToString().Contains("dato") && hashtable["plastic"].ToString() != "")
								{
									num4 = Convert.ToDouble(hashtable["plastic"].ToString().Replace("Plastica: ", "").Replace("%", ""));
								}
								gb[0] += num4;
								if (!hashtable["plastic"].ToString().Contains("Bidone"))
								{
									i[0]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num4)))
								{
									value += $"{garbage.type[3]};{num4};{garbage.color[3]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("glass", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Vetro", stations.garbage7, (new double[10])[1], 15, num3));
								if (!hashtable["glass"].ToString().Contains("Bidone") && !hashtable["glass"].ToString().Contains("svuotamento") && !hashtable["glass"].ToString().Contains("dato") && hashtable["glass"].ToString() != "")
								{
									num5 = Convert.ToDouble(hashtable["glass"].ToString().Replace("Vetro: ", "").Replace("%", ""));
								}
								gb[1] += num5;
								if (!hashtable["glass"].ToString().Contains("Bidone"))
								{
									i[1]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num5)))
								{
									value += $"{garbage.type[15]};{num5};{garbage.color[15]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("paper", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Carta", stations.garbage6, (new double[10])[2], 1, num3));
								if (!hashtable["paper"].ToString().Contains("Bidone") && !hashtable["paper"].ToString().Contains("svuotamento") && !hashtable["paper"].ToString().Contains("dato") && hashtable["paper"].ToString() != "")
								{
									num6 += Convert.ToDouble(hashtable["paper"].ToString().Replace("Carta: ", "").Replace("%", ""));
								}
								gb[2] += num6;
								if (!hashtable["paper"].ToString().Contains("Bidone"))
								{
									i[2]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num6)))
								{
									value += $"{garbage.type[1]};{num6};{garbage.color[1]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("aluminium", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Barattolame", stations.garbage4, (new double[10])[3], 16, num3));
								if (!hashtable["aluminium"].ToString().Contains("Bidone") && !hashtable["aluminium"].ToString().Contains("svuotamento") && !hashtable["aluminium"].ToString().Contains("dato") && hashtable["aluminium"].ToString() != "")
								{
									num7 = Convert.ToDouble(hashtable["aluminium"].ToString().Replace("Barattolame: ", "").Replace("%", ""));
								}
								gb[3] += num7;
								if (!hashtable["aluminium"].ToString().Contains("Bidone"))
								{
									i[3]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num7)))
								{
									value += $"{garbage.type[16]};{num7};{garbage.color[16]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("garbage", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Indifferenziato", stations.garbage8, (new double[10])[4], 0, num3));
								if (!hashtable["garbage"].ToString().Contains("Bidone") && !hashtable["garbage"].ToString().Contains("svuotamento") && !hashtable["garbage"].ToString().Contains("dato") && hashtable["garbage"].ToString() != "")
								{
									num8 = Convert.ToDouble(hashtable["garbage"].ToString().Replace("Indifferenziato: ", "").Replace("%", ""));
								}
								gb[4] += num8;
								if (!hashtable["garbage"].ToString().Contains("Bidone"))
								{
									i[4]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num8)))
								{
									value += $"{garbage.type[0]};{num8};{garbage.color[0]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("organic", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Organico", stations.garbage10, (new double[10])[5], 4, num3));
								if (!hashtable["organic"].ToString().Contains("Bidone") && !hashtable["organic"].ToString().Contains("svuotamento") && !hashtable["organic"].ToString().Contains("dato") && hashtable["organic"].ToString() != "")
								{
									num9 = Convert.ToDouble(hashtable["organic"].ToString().Replace("Organico: ", "").Replace("%", ""));
								}
								gb[5] += num9;
								if (!hashtable["organic"].ToString().Contains("Bidone"))
								{
									i[5]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num9)))
								{
									value += $"{garbage.type[4]};{num9};{garbage.color[4]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("nappies", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Pannolini", stations.garbage9, (new double[10])[6], 17, num3));
								if (!hashtable["nappies"].ToString().Contains("Bidone") && !hashtable["nappies"].ToString().Contains("svuotamento") && !hashtable["nappies"].ToString().Contains("dato") && hashtable["nappies"].ToString() != "")
								{
									num10 = Convert.ToDouble(hashtable["nappies"].ToString().Replace("Pannolini: ", "").Replace("%", ""));
								}
								gb[6] += num10;
								if (!hashtable["nappies"].ToString().Contains("Bidone"))
								{
									i[6]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num10)))
								{
									value += $"{garbage.type[17]};{num10};{garbage.color[17]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("oil", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Olio", stations.garbage1, (new double[10])[7], 9, num3));
								if (!hashtable["oil"].ToString().Contains("Bidone") && !hashtable["oil"].ToString().Contains("svuotamento") && !hashtable["oil"].ToString().Contains("dato") && hashtable["oil"].ToString() != "")
								{
									num11 = Convert.ToDouble(hashtable["oil"].ToString().Replace("Olio: ", "").Replace("%", ""));
								}
								gb[7] += num11;
								if (!hashtable["oil"].ToString().Contains("Bidone"))
								{
									i[7]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num11)))
								{
									value += $"{garbage.type[9]};{num11};{garbage.color[9]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("medicine", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Medicinali", stations.garbage2, (new double[10])[8], 7, num3));
								if (!hashtable["medicine"].ToString().Contains("Bidone") && !hashtable["medicine"].ToString().Contains("svuotamento") && !hashtable["medicine"].ToString().Contains("dato") && hashtable["medicine"].ToString() != "")
								{
									num12 = Convert.ToDouble(hashtable["medicine"].ToString().Replace("Medicinali: ", "").Replace("%", ""));
								}
								gb[8] += num12;
								if (!hashtable["medicine"].ToString().Contains("Bidone"))
								{
									i[8]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num12)))
								{
									value += $"{garbage.type[7]};{num12};{garbage.color[7]};";
								}
								progressBarCity.PerformStep();
								hashtable.Add("battery", Function.PercentualFull(connectorForm, stations, cityForm.idCity[comboBoxNumberCity.SelectedIndex], garbage, "Pile", stations.garbage3, (new double[10])[9], 6, num3));
								if (!hashtable["battery"].ToString().Contains("Bidone") && !hashtable["battery"].ToString().Contains("svuotamento") && !hashtable["battery"].ToString().Contains("dato") && hashtable["battery"].ToString() != "")
								{
									num13 = Convert.ToDouble(hashtable["battery"].ToString().Replace("Pile: ", "").Replace("%", ""));
								}
								gb[9] += num13;
								if (!hashtable["battery"].ToString().Contains("Bidone"))
								{
									i[9]++;
								}
								if (Enumerable.Range(80, 70).Contains(Convert.ToInt32(num13)))
								{
									value += $"{garbage.type[6]};{num13};{garbage.color[6]};";
								}
								progressBarCity.PerformStep();
								dataGridViewCity.Rows.Add(hashtable["idstation"].ToString(), hashtable["plastic"].ToString().Replace("Plastica: ", ""), hashtable["glass"].ToString().Replace("Vetro: ", ""), hashtable["paper"].ToString().Replace("Carta: ", ""), hashtable["aluminium"].ToString().Replace("Barattolame: ", ""), hashtable["garbage"].ToString().Replace("Indifferenziato: ", ""), hashtable["organic"].ToString().Replace("Organico: ", ""), hashtable["nappies"].ToString().Replace("Pannolini: ", ""), hashtable["oil"].ToString().Replace("Olio: ", ""), hashtable["medicine"].ToString().Replace("Medicinali: ", ""), hashtable["battery"].ToString().Replace("Pile: ", ""));
								num3++;
								streamWriter.WriteLine(value);
							}
							timer.Stop();
							streamWriter.Flush();
							streamWriter.Close();
							labelState.Text = "";
							gb[0] = gb[0] / (double)i[0];
							gb[1] = gb[1] / (double)i[1];
							gb[2] = gb[2] / (double)i[2];
							gb[3] = gb[3] / (double)i[3];
							gb[4] = gb[4] / (double)i[4];
							gb[5] = gb[5] / (double)i[5];
							gb[6] = ((gb[6] > 3150.0) ? 0.0 : (gb[6] / (double)i[6]));
							gb[7] = gb[7] / (double)i[7];
							gb[8] = gb[8] / (double)i[8];
							gb[9] = gb[9] / (double)i[9];
							FormCityFunction.ChartCleans(chartCity);
							chartCity.Series["Plastica"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[0]);
							chartCity.Series["Plastica"].Color = ColorTranslator.FromHtml(garbage.color[3]);
							chartCity.Series["Vetro"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[1]);
							chartCity.Series["Vetro"].Color = Color.FromName(garbage.color[15]);
							chartCity.Series["Carta"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[2]);
							chartCity.Series["Carta"].Color = Color.FromName(garbage.color[1]);
							chartCity.Series["Barattolame"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[3]);
							chartCity.Series["Barattolame"].Color = Color.FromName(garbage.color[16]);
							chartCity.Series["Indifferenziato"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[4]);
							chartCity.Series["Indifferenziato"].Color = Color.FromName(garbage.color[0]);
							chartCity.Series["Organico"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[5]);
							chartCity.Series["Organico"].Color = Color.FromName(garbage.color[4]);
							chartCity.Series["Pannolini"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[6]);
							chartCity.Series["Pannolini"].Color = Color.FromName(garbage.color[17]);
							chartCity.Series["Olio"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[7]);
							chartCity.Series["Olio"].Color = Color.FromName(garbage.color[9]);
							chartCity.Series["Medicinali"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[8]);
							chartCity.Series["Medicinali"].Color = Color.FromName(garbage.color[7]);
							chartCity.Series["Pile"].Points.AddXY("Comune di " + cityForm.nameCity[Convert.ToUInt32(userForm)], gb[9]);
							chartCity.Series["Pile"].Color = Color.FromName(garbage.color[6]);
							comboBoxNumberCity.Enabled = true;
							buttonCity.Enabled = true;
							labelState.Text = "Elaborato il " + DateTime.Now.ToString() + " in " + timer.Elapsed.TotalSeconds.ToString("0") + " secondi";
							data2(dataGridViewCity, 2, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
							buttonCity.Text = "Aggiorna";
							progressBarCity.Value = 0;
							chartCity.Annotations["1"].Width = 2 * chartCity.ChartAreas[0].AxisX.LineWidth;
							chartCity.Annotations["2"].Width = 2 * chartCity.ChartAreas[0].AxisX.LineWidth;
							streamWriter.Dispose();
							streamWriter.Close();
							return;
						}
						catch (Exception)
						{
							streamWriter.Flush();
							streamWriter.Close();
							labelState.Text = "Elaborazione annullata per errore";
							comboBoxNumberCity.Enabled = true;
							buttonCity.Enabled = true;
							buttonCity.Text = "Aggiorna";
							progressBarCity.Value = 0;
							return;
						}
					}
					labelCity.Text = "Comune non valido";
					buttonCity.Enabled = true;
					buttonCity.Text = "Aggiorna";
					chartCity.Annotations["1"].Visible = false;
					chartCity.Annotations["2"].Visible = false;
					return;
				}
				catch (Exception)
				{
					labelState.Text = "Elaborazione annullata per errore";
					labelCity.Text = "Selezionare un comune corretto";
					buttonCity.Enabled = true;
					comboBoxNumberCity.Enabled = true;
					return;
				}
			}
			if (!(num2 > 0 && num2 < 99))
			{
				return;
			}
			try
			{
				if (comboBoxNumberCity.SelectedItem != null)
				{
					try
					{
						labelState.Text = "Il programma si bloccherà durante l'elaborazione dati...";
						table.Clear();
						FormCityFunction.SetForms(chartCity, dataGridViewCity, progressBarCity, buttonCity, checkListMenuGarbage, comboBoxNumberCity, labelTitle2, garb, garbage);
						progressBarCity.Maximum = flowCheckStations.Controls.OfType<CheckBox>().Where(x => x.Checked).Count()* 10;
						progressBarCity.Maximum = flowCheckStations.Controls.OfType<CheckBox>().Where(x => x.Checked).Count()* 10;
						timer.Start();
						data2(dataGridViewCity, 1, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
						Task.Run(async()=>
						{
							await Buttonexecute(gb, i, setGarbage);
						});
						return;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						comboBoxNumberCity.Enabled = true;
						buttonCity.Enabled = true;
						buttonCity.Text = "Aggiorna";
						progressBarCity.Value = 0;
						return;
					}
				}
				MessageBox.Show("Stazione non selezionata!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				comboBoxNumberCity.Enabled = true;
				buttonCity.Enabled = true;
				buttonCity.Text = "Aggiorna";
				chartCity.Annotations["1"].Visible = false;
				chartCity.Annotations["2"].Visible = false;
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.Message, "Critical", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				buttonCity.Enabled = true;
				buttonCity.Text = "Aggiorna";
				progressBarCity.Value = 0;
			}
		}
		/*private void LoadSet()
		{
			notFirst = true;
		}*/
		private void EsportaExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormCityFunction.SaveExcel(dataGridViewCity, stationForm.nStation.Count);
		}
		private void EsciToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void LabelState_Click(object sender, EventArgs e)
		{
			if (dataGridViewCity.RowCount >= stationForm.nStation.Count)
			{
				try
				{
					Process.Start("https://www.ecofil.it/maps/" + cityForm.nameCity[Convert.ToInt32(userForm)] + ".html");
				}
				catch (Exception)
				{
				}
			}
		}
		private void OptionToolStripMenuItem_ClickAsync(object sender, EventArgs e)
		{
			if ((bool)Settings.Default["updateData"])
			{
				evUpdate.Stop();
			}
			if (Settings.Default["AlarmClock"].ToString() != "")
			{
				evUpdate.Stop();
			}
			Task.Run(async ()=>
			{
				await FormCityFunction.OpenOption(garbage,garb);
			}).Wait();
			if ((bool)Settings.Default["updateData"])
			{
				evUpdate.Interval = (int)Settings.Default["timeSeconds"] * 1000;
				evUpdate.Start();
			}
			if (Settings.Default["AlarmClock"].ToString() != "")
			{
				evUpdate.Start();
			}
		}
		private void CheckBoxStation_CheckedChanged(object sender, EventArgs e)
		{
			flowCheckStations.Enabled = checkBoxStation.Checked;
			foreach (CheckBox checkBox in flowCheckStations.Controls)
			{
				checkBox.Checked =!checkBoxStation.Checked;
			}
			if(comboBoxNumberCity.SelectedItem is not null) ((CheckBox)flowCheckStations.Controls[comboBoxNumberCity.SelectedItem.ToString()]).Checked = true;
		}
		private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			close = false;
			DialogResult dialogResult = MessageBox.Show("Logout?", "Ecofil", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				Close();
			}
		}
		private void CheckListMenuGarbage_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (checkListMenuGarbage.GetItemCheckState(e.Index) != 0 || dataGridViewCity.Rows.Count <= 0)
			{
				return;
			}
			garbageClick = e.Index;
			data2(dataGridViewCity, 1, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
			Function.ListUncheck(checkListMenuGarbage);
			table.Clear();
			string garbageSelected = string.Empty;
			int objects = flowCheckStations.Controls.OfType<CheckBox>().Where(x=>x.Checked).Count();
			if (Enumerable.Range(objects - 2, objects + 2).Contains(dataGridViewCity.Rows.Count))
			{
				try
				{
					garbageSelected = char.ToUpper(checkListMenuGarbage.Items[e.Index].ToString()[0]) + checkListMenuGarbage.Items[e.Index].ToString().Substring(1).ToLower();
					DataGridViewColumn dataGridViewColumn = (from DataGridViewColumn x in dataGridViewCity.Columns
															 where x.HeaderText == garbageSelected
															 select x).FirstOrDefault();
					if (garbageSelected == string.Empty && dataGridViewColumn == null)
					{
						throw new Exception();
					}
					int index = dataGridViewColumn.Index;
					for (int i = 0; i <= dataGridViewCity.RowCount - 1; i++)
					{
						try
						{
							string value = Regex.Replace(dataGridViewCity.Rows[i].Cells[0].Value.ToString(), "%", "");
							int num = Convert.ToInt32(value);
							if (num == stationForm.nStation[comboBoxNumberCity.SelectedIndex])
							{
								int index2 = num - 1;
								table.Add(new Dictionary<int, string>
							{
								{ 0, value },
								{
									1,
									stationForm.latitude[index2].ToString()
								},
								{
									2,
									stationForm.longitude[index2].ToString()
								}
							});
							}
							else
							{
								string value2 = Regex.Replace(dataGridViewCity.Rows[i].Cells[index].Value.ToString(), "%", "");
								int num2 = Convert.ToInt32(value2);
								if (num2 > (int)Settings.Default["Min"])
								{
									int index3 = num - 1;
									table.Add(new Dictionary<int, string>
								{
									{ 0, value },
									{
										1,
										stationForm.latitude[index3].ToString()
									},
									{
										2,
										stationForm.longitude[index3].ToString()
									},
									{ 3, garbageSelected },
									{ 4, value2 },
									{
										5,
										garbage.color[e.Index]
									},
									{
										6,
										stationForm.ecocentre[index3].ToString()
									}
								});
								}
							}
						}
						catch (Exception)
						{
						}
					}
					select = true;
					data2(dataGridViewCity, 3, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
					return;
				}
				catch (Exception)
				{
					garbageClick = e.Index;
					select = false;
					data2(dataGridViewCity, 1, table, new KeyValuePair<int, string>(comboBoxNumberCity.SelectedIndex, comboBoxNumberCity.Text));
					Function.ListUncheck(checkListMenuGarbage);
					MessageBox.Show("Rifiuto non presente", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			select = false;
			MessageBox.Show("Dati non validi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		private void FormCity_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(close)
			{
				DialogResult dialogResult = MessageBox.Show("Vuoi veramente uscire?", "Ecofil", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.No)
				{
					e.Cancel = true;
					return;
				}
			}
			foreach(ProcessThread th in Process.GetCurrentProcess().Threads)
            {
				if (th.Id==nThread)
				{
			     data2(null, 4, null, new KeyValuePair<int, string>());
					break;
				}
			}
			evAlarm.Stop();
			evAlarm.Dispose();
			evUpdate.Stop();
			evUpdate.Dispose();
			evSsh.Stop();
			evSsh.Dispose();
			if (connectorForm.mStatus)
			{
                ((IDisposable)connectorForm).Dispose();
			}
			client.Disconnect();
			client.Dispose();
		}
		private void OpenFormMaps()
		{
			Task.Run(async()=>
			{
				await OpenMaps(cityForm, userForm, stationForm, garbage);
			});
		}
        private void RiduciToolStripMenuItem_Click(object sender, EventArgs e)
        {
			WindowState = FormWindowState.Minimized;
        }
        private void MassimaToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (WindowState == FormWindowState.Normal)
			{
				Region = new Region(Screen.PrimaryScreen.WorkingArea) ;
				WindowState = FormWindowState.Maximized;
				MaxToolStripMenuItem.Text = "Normale";
			}
			else 
			{
				WindowState = FormWindowState.Normal;
				Region = new Region(FormCityFunction.GetRoundPath(DisplayRectangle, Height / 15));
				MaxToolStripMenuItem.Text = "Intera";
			}
		}
        private static async Task OpenMaps(City city, string user, Stations station, Garbage garb)
		{
			await Task.Factory.StartNew(()=>
			{
			         MapCity mapCity = new(city, user, station, garb);
					nThread = GetCurrentThreadId();
					data2 = mapCity.FunData;
					Application.EnableVisualStyles();
					Application.Run(mapCity);
			});
		}
		private void Form_MouseDown(object sender, MouseEventArgs e)
		{
			drag = true;
			pmouse = Cur.Position;
			pform = Location;
		}
		private void Form_MouseMove(object sender, MouseEventArgs e)
		{
			if (drag)
			{
				Point pt = Point.Subtract(Cur.Position, new Size(pmouse));
				base.Location = Point.Add(pform, new Size(pt));
			}
		}
        private void ComboBoxNumberCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CheckBox)flowCheckStations.Controls[comboBoxNumberCity.SelectedItem.ToString()]).Checked = true;
		}
        private void Form_MouseUp(object sender, MouseEventArgs e)
		{
			drag = false;
		}
	}
}