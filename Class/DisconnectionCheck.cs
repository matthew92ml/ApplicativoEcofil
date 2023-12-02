using System;
using System.Runtime.InteropServices;

namespace ApplicativoEcofil.Class
{
	internal class DisconnectionCheck
	{
		public enum State
		{
			All,
			Closed,
			Listen,
			Syn_Sent,
			Syn_Rcvd,
			Established,
			Fin_Wait1,
			Fin_Wait2,
			Close_Wait,
			Closing,
			Last_Ack,
			Time_Wait,
			Delete_TCB
		}
		private struct Connection
		{
			public int State;

			public int LocalAddr { get; set; }

			public int LocalPort { get; set; }

			public int RemoteAddr { get; set; }

			public int RemotePort { get; set; }
		}
		[DllImport("iphlpapi.dll")]
		private static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);
		[DllImport("iphlpapi.dll")]
		private static extern int SetTcp(IntPtr pTcprow);
		private static Connection[] TcpTable()
		{
			IntPtr intPtr = IntPtr.Zero;
			bool flag = false;
			try
			{
				int pdwSize = 0;
                _ = GetTcpTable(IntPtr.Zero, ref pdwSize, bOrder: false);
				intPtr = Marshal.AllocCoTaskMem(pdwSize);
				flag = true;
				GetTcpTable(intPtr, ref pdwSize, bOrder: false);
				int num = Marshal.ReadInt32(intPtr);
				IntPtr intPtr2 = intPtr;
				intPtr2 = IntPtr.Add(intPtr, 4);
				Connection[] array = new Connection[num];
				int offset = Marshal.SizeOf(default(Connection));
				for (int i = 0; i < num; i++)
				{
					array[i] = (Connection)Marshal.PtrToStructure(intPtr2, typeof(Connection));
					intPtr2 = IntPtr.Add(intPtr2, offset);
				}
				return array;
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				if (flag)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
		}
		private static IntPtr GetPtrToNewObject(object obj)
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(obj));
			Marshal.StructureToPtr(obj, intPtr, fDeleteOld: false);
			return intPtr;
		}
		private static uint ConvertIPInt(string ip)
		{
			if (ip.IndexOf(".") < 0)
			{
				throw new Exception("Indirizzo Ip non valido");
			}
			string[] array = ip.Split('.');
			if (array.Length != 4)
			{
				throw new Exception("Indirizzo Ip non valido");
			}
			byte[] value = new byte[4]
			{
				Convert.ToByte(array[0]),
				Convert.ToByte(array[1]),
				Convert.ToByte(array[2]),
				Convert.ToByte(array[3])
			};
			return BitConverter.ToUInt32(value, 0);
		}
		public static void CloseLocalPort(uint port)
		{
			Connection[] array = TcpTable();
			for (int i = 0; i < array.Length; i++)
			{
				if (port == array[i].LocalPort)
				{
					array[i].State = 12;
					IntPtr ptrToNewObject = GetPtrToNewObject(array[i]);
					int num = SetTcp(ptrToNewObject);
				}
			}
		}
		public static void CloseRemotePort(uint port)
		{
			Connection[] array = TcpTable();
			for (int i = 0; i < array.Length; i++)
			{
				if (port == array[i].RemotePort)
				{
					array[i].State = 12;
					IntPtr ptrToNewObject = GetPtrToNewObject(array[i]);
					int num = SetTcp(ptrToNewObject);
				}
			}
		}
		public static void CloseLocaLIP(string ip)
		{
			Connection[] array = TcpTable();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].LocalAddr == ConvertIPInt(ip))
				{
					array[i].State = 12;
					IntPtr ptrToNewObject = GetPtrToNewObject(array[i]);
					int num = SetTcp(ptrToNewObject);
				}
			}
		}
		public static void CloseRemoteIP(string ip)
		{
			Connection[] array = TcpTable();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].RemoteAddr == ConvertIPInt(ip))
				{
					array[i].State = 12;
					IntPtr ptrToNewObject = GetPtrToNewObject(array[i]);
					int num = SetTcp(ptrToNewObject);
				}
			}
		}
	}
}
