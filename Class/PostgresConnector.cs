using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace ApplicativoEcofil.Class
{
	public class PostgresConnector : IDisposable
	{
        private readonly int timerTimeOut;

		public string connection;

		public Hashtable hashSqlReader = new();

		private NpgsqlCommand msqlCommand;

		private readonly NpgsqlConnection msqlConnection;

		public readonly bool mStatus;

		private readonly int mtimeOut;

		public static bool Busy { get; set; }

		public PostgresConnector(string connectionString, int timeOut = 0)
		{
			try
			{
				connection = connectionString;
				msqlConnection = new NpgsqlConnection(connectionString);
				msqlConnection.Open();
				Busy = true;
				mStatus = true;
				mtimeOut = timeOut;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Connesione database", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				mStatus = false;
				Busy = false;
			}
		}

		public bool AddReader(string tableName, string sql)
		{
			try
			{
				if (hashSqlReader.Count > 0)
				{
					foreach (DictionaryEntry item in hashSqlReader)
					{
						NpgsqlDataReader npgsqlDataReader = (NpgsqlDataReader)item.Value;
					}
				}
				if (!hashSqlReader.ContainsKey(tableName))
				{
					msqlCommand = new NpgsqlCommand(sql, msqlConnection);
					if (timerTimeOut > 0)
					{
						msqlCommand.CommandTimeout = timerTimeOut;
					}
					NpgsqlDataReader npgsqlDataReader2 = msqlCommand.ExecuteReader();
					if (npgsqlDataReader2.HasRows)
					{
						hashSqlReader = new Hashtable { { tableName, npgsqlDataReader2 } };
						return true;
					}
					npgsqlDataReader2.Close();
					return false;
				}
				return false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "DataBase", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
		}

		public void DelReader(string tableName)
		{
			try
			{
				if (hashSqlReader.ContainsKey(tableName))
				{
					((NpgsqlDataReader)hashSqlReader[tableName]).Close();
					hashSqlReader.Remove(tableName);
				}
			}
			catch (Exception)
			{
			}
		}

		public NpgsqlDataReader GetData(string tableName)
		{
			try
			{
				if (hashSqlReader.ContainsKey(tableName))
				{
					return (NpgsqlDataReader)hashSqlReader[tableName];
				}
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public int ExecuteScalar(string sqlString)
		{
			int result = 0;
			if (mStatus)
			{
				try
				{
					if (hashSqlReader.Count > 0)
					{
						foreach (DictionaryEntry item in hashSqlReader)
						{
							((NpgsqlDataReader)item.Value).Close();
						}
					}
					msqlCommand = new NpgsqlCommand(sqlString, msqlConnection);
					if (mtimeOut > 0)
					{
						msqlCommand.CommandTimeout = mtimeOut;
					}
					result = msqlCommand.ExecuteScalar()!.GetHashCode();
					msqlCommand.Dispose();
					return result;
				}
				catch (Exception ex)
				{
					if (!sqlString.Contains("id"))
					{
						MessageBox.Show(ex.Message, "DataBase", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					return result;
				}
			}
			return result;
		}

		public bool ExecuteCommand(string sqlString)
		{
			if (mStatus)
			{
				try
				{
					if (hashSqlReader.Count > 0)
					{
						foreach (DictionaryEntry item in hashSqlReader)
						{
							((NpgsqlDataReader)item.Value).Close();
						}
					}
					msqlCommand = new NpgsqlCommand(sqlString, msqlConnection);
					msqlCommand.ExecuteNonQuery();
					msqlCommand.Dispose();
					return true;
				}
				catch (Exception ex)
				{
					string message = ex.Message;
					string text = message;
					if (text != null)
					{
						if (text.Contains("duplicate key"))
						{
							return true;
						}
						string text2 = text;
						if (text2.Contains("open"))
						{
							return true;
						}
					}
					MessageBox.Show(ex.Message, "DataBase", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return false;
				}
			}
			return false;
		}

		public virtual void Dispose()
		{
			try
			{
				if (msqlConnection.State != ConnectionState.Open)
				{
					return;
				}
				if (hashSqlReader.Count > 0)
				{
					foreach (DictionaryEntry item in hashSqlReader)
					{
						try
						{
							((NpgsqlDataReader)item.Value).Close();
						}
						catch (Exception)
						{
						}
					}
					hashSqlReader.Clear();
				}
				msqlCommand.Connection!.Close();
				msqlCommand.Dispose();
				msqlConnection.Close();
				NpgsqlConnection.ClearPool(msqlConnection);
				msqlConnection.Dispose();
				Busy = false;
				GC.SuppressFinalize(this);
			}
			catch (Exception)
			{
				GC.SuppressFinalize(this);
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}
	}
}
