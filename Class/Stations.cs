using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace ApplicativoEcofil.Class
{
	public class Stations
	{
		public List<int> nStation;

		public List<double> latitude;

		public List<double> longitude;

		public List<string> garbage1;

		public List<string> garbage2;

		public List<string> garbage3;

		public List<string> garbage4;

		public List<string> garbage5;

		public List<string> garbage6;

		public List<string> garbage7;

		public List<string> garbage8;

		public List<string> garbage9;

		public List<string> garbage10;

		public List<string> ipvpn;

		public List<bool> ecocentre;

		public List<bool> hikvision;

		public Stations()
		{
			nStation = new List<int>();
			latitude = new List<double>();
			longitude = new List<double>();
			ipvpn = new List<string>();
			garbage1 = new List<string>();
			garbage2 = new List<string>();
			garbage3 = new List<string>();
			garbage4 = new List<string>();
			garbage5 = new List<string>();
			garbage6 = new List<string>();
			garbage7 = new List<string>();
			garbage8 = new List<string>();
			garbage9 = new List<string>();
			garbage10 = new List<string>();
			ecocentre = new List<bool>();
			hikvision = new List<bool>();
		}

		public Stations(PostgresConnector connector, int city, int id, Garbage garbage)
		{
			Stations stations = this;
			nStation = new List<int>();
			latitude = new List<double>();
			longitude = new List<double>();
			ipvpn = new List<string>();
			ecocentre = new List<bool>();
			hikvision = new List<bool>();
			garbage1 = new List<string>();
			garbage2 = new List<string>();
			garbage3 = new List<string>();
			garbage4 = new List<string>();
			garbage5 = new List<string>();
			garbage6 = new List<string>();
			garbage7 = new List<string>();
			garbage8 = new List<string>();
			garbage9 = new List<string>();
			garbage10 = new List<string>();
			string sqlString = "Select codicestazione, latitudine, longitudine,CASE nome WHEN 'Ecocentro' THEN true ELSE false END , CASE dvr WHEN 'Hikvision' THEN true ELSE false END, ipvpn from infostazioni left join datistazione on datistazione.idstazione = infostazioni.codicestazione AND datistazione.idcomune = infostazioni.idcomune  where infostazioni.idcomune = " + city + " order by codicestazione ASC; ";
			if (connector.AddReader("infostazioni", sqlString))
			{
				NpgsqlDataReader data = connector.GetData("infostazioni");
				while (data.Read())
				{
					try
					{
						nStation.Add(data.GetInt16(0));
						latitude.Add(data.IsDBNull(1) ? 0.0 : data.GetDouble(1));
						longitude.Add(data.IsDBNull(2) ? 0.0 : data.GetDouble(2));
						ecocentre.Add(data.GetBoolean(3));
						hikvision.Add(data.GetBoolean(4));
						ipvpn.Add(data.IsDBNull(5) ? string.Empty : data.GetString(5));
						garbage1.Add("0");
						garbage2.Add("0");
						garbage3.Add("0");
						garbage4.Add("0");
						garbage5.Add("0");
						garbage6.Add("0");
						garbage7.Add("0");
						garbage8.Add("0");
						garbage9.Add("0");
						garbage10.Add("0");
					}
					catch (Exception)
					{
						connector.DelReader("infostazioni");
					}
				}
				connector.DelReader("infostazioni");
			}
            ((IDisposable)connector).Dispose();
			Parallel.For(0, garbage.id.Length - 1, new ParallelOptions
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount
			}, delegate(int index)
			{
				using PostgresConnector postgresConnector = new PostgresConnector(connector.connection);
				sqlString = string.Empty;
				sqlString = "SELECT distinct ic.codicestazione,  coalesce(ic.cassonetto1, 0) + coalesce(ic.cassonetto2, 0) + coalesce(ic.cassonetto3, 0) + coalesce(ic.cassonetto4, 0) + coalesce(ic.cassonetto5, 0) as total from infocassonetti ic inner join inforifiuti ir on ic.idcomune = ir.idcomune and ic.tiporifiuto = ir.codicerifiuto  where ic.idcomune = " + city + " and ir.codicerifiuto = " + garbage.id[index] + " order by codicestazione ASC ";
				if (postgresConnector.AddReader("data", sqlString))
				{
					NpgsqlDataReader data2 = postgresConnector.GetData("data");
					while (data2.Read())
					{
						try
						{
							int index2 = stations.nStation.IndexOf(data2.GetInt32(0));
							long num = (data2.IsDBNull(1) ? 0 : data2.GetInt64(1));
							switch (garbage.type[index])
							{
							case "OLIO":
								stations.garbage1[index2] = num.ToString();
								break;
							case "MEDICINALI":
								stations.garbage2[index2] = num.ToString();
								break;
							case "PILE":
								stations.garbage3[index2] = num.ToString();
								break;
							case "BARATTOLAME":
								stations.garbage4[index2] = num.ToString();
								break;
							case "PLASTICA":
								stations.garbage5[index2] = num.ToString();
								break;
							case "CARTA":
								stations.garbage6[index2] = num.ToString();
								break;
							case "VETRO":
							case "VETRO E BARATTOLAME":
								stations.garbage7[index2] = num.ToString();
								break;
							case "INDIFFERENZIATO":
								stations.garbage8[index2] = num.ToString();
								break;
							case "PANNOLINI":
								stations.garbage9[index2] = num.ToString();
								break;
							case "ORGANICO":
								stations.garbage10[index2] = num.ToString();
								break;
							}
						}
						catch (Exception)
						{
                            ((IDisposable)postgresConnector).Dispose();
						}
					}
				}
                ((IDisposable)postgresConnector).Dispose();
			});
		}
	}
}
