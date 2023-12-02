using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Npgsql;

namespace ApplicativoEcofil.Class
{
	public class Garbage
	{
		public int[] id;

		public string[] type;

		public string[] color;

		public Garbage()
		{
			id = new int[0];
			type = new string[0];
			color = new string[0];
		}

		public Garbage(PostgresConnector connector, int city)
		{
			int num = connector.ExecuteScalar(" Select count(codicerifiuto) from  inforifiuti where idcomune = " + city + ";");
			id = new int[num];
			type = new string[num];
			color = new string[num];
			string sql = "Select codicerifiuto, descrizione, colore from  inforifiuti where idcomune = " + city + " order by codicerifiuto ASC ;";
			if (!connector.AddReader("inforifiuti", sql))
			{
				return;
			}
			int num2 = 0;
			NpgsqlDataReader data = connector.GetData("inforifiuti");
			while (data.Read())
			{
				try
				{
					id[num2] = data.GetInt32(0);
					switch (data.GetString(1).ToUpper())
					{
					case "OLIO VEGETALE":
						type[num2] = "OLIO";
						break;
					case "PILE ALCALINE":
						type[num2] = "PILE";
						break;
					case "PANNOLINI E PANNOLONI":
						type[num2] = "PANNOLINI";
						break;
					case "MEDICINALI SCADUTI":
						type[num2] = "MEDICINALI";
						break;
					default:
						type[num2] = data.GetString(1).ToUpper();
						break;
					}
					color[num2] = (data.IsDBNull(2) ? "" : data.GetString(2));
					if (Regex.IsMatch(color[num2].ToString().Replace("#", ""), "\\A\\b[0-9a-fA-F]+\\b\\Z"))
					{
						switch (color[num2].ToUpper())
						{
						case "#063971":
							color[num2] = Color.Blue.Name;
							break;
						case "#FFFF00":
							color[num2] = Color.Yellow.Name;
							break;
						case "#7E4B26":
							color[num2] = Color.Brown.Name;
							break;
						case "#F6F6F6":
							color[num2] = Color.NavajoWhite.Name;
							break;
						case "#D7D7D7":
							color[num2] = Color.LightGray.Name;
							break;
						case "#287233":
							color[num2] = Color.DarkSeaGreen.Name;
							break;
						case "#808000":
							color[num2] = Color.Olive.Name;
							break;
						case "#666666":
						{
							string[] array = color;
							string[] array2 = array;
							if (array2 != null && array2.FirstOrDefault().Contains("pannolini"))
							{
								color[num2] = Color.SlateGray.Name;
							}
							else
							{
								color[num2] = Color.LightGray.Name;
							}
							break;
						}
						case "#B0C4DE":
							color[num2] = Color.LightSteelBlue.Name;
							break;
						case "#DC143C":
							color[num2] = Color.Crimson.Name;
							break;
						}
					}
					else
					{
						switch (color[num2])
						{
						case "greenDark":
							color[num2] = Color.DarkGreen.Name;
							break;
						case "blueDark":
							color[num2] = Color.DarkBlue.Name;
							break;
						case "grey":
							color[num2] = Color.Gray.Name;
							break;
						}
					}
					num2++;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Dati rifiuti", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					connector.DelReader("inforifiuti");
				}
			}
			connector.DelReader("inforifiuti");
		}
	}
}
