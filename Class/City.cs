using System;
using Npgsql;

namespace ApplicativoEcofil.Class
{
	public class City
	{
		public int[] idCity;

		public string[] nameCity;

		public double[] latitude;

		public double[] longitude;

		public City()
		{
			idCity = new int[0];
			nameCity = new string[0];
			latitude = new double[0];
			longitude = new double[0];
		}

		public City(PostgresConnector pQuery)
		{
			int num = pQuery.ExecuteScalar("Select Count(*) from  infocomune;");
			idCity = new int[num];
			nameCity = new string[num];
			latitude = new double[num];
			longitude = new double[num];
			string sql = "Select id, nome, latitudine, longitudine from infocomune group by id, nome order by id, nome ASC ;";
			if (!pQuery.AddReader("infocomune", sql))
			{
				return;
			}
			NpgsqlDataReader data = pQuery.GetData("infocomune");
			int num2 = 0;
			while (data.Read())
			{
				try
				{
					idCity[num2] = data.GetInt16(0);
					nameCity[num2] = data.GetString(1);
					if (nameCity[num2].Contains("test") || nameCity[num2].Contains("Test"))
					{
						nameCity[num2] = "Dato non disponibile";
					}
					latitude[num2] = (data.IsDBNull(2) ? 0.0 : data.GetDouble(2));
					longitude[num2] = (data.IsDBNull(3) ? 0.0 : data.GetDouble(3));
					num2++;
				}
				catch (Exception)
				{
				}
			}
			pQuery.DelReader("infocomune");
		}
	}
}
