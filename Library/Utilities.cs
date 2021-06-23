using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace LibraryDHCP
{
	public static class Utilities
	{
		public static bool isInfinityTimeSpan(TimeSpan span)
        {
			if (span < TimeSpan.FromMilliseconds(0) || span >= TimeSpan.MaxValue) return true;
			return false;
        }
		public static bool ArrayEquals<T>(T[] obj1, T[] obj2)
		{

			if (obj1 == null || obj2 == null || obj1.Length != obj2.Length)
			{
				return false;
			}
			else
			{
				for (int i = 0; i < obj1.Length; i++)
				{
					if (!EqualityComparer<T>.Default.Equals(obj1[i], obj2[i]))
						return false;
				}

				return true;
			}
		}
		public static class ReadWriteStream
		{
			#region Read from stream

			public static UInt16 Read2Bytes(Stream s)
			{
				BinaryReader br = new BinaryReader(s);
				return (UInt16)IPAddress.NetworkToHostOrder((Int16)br.ReadUInt16());
			}

			public static UInt32 Read4Bytes(Stream s)
			{
				BinaryReader br = new BinaryReader(s);
				return (UInt32)IPAddress.NetworkToHostOrder((Int32)br.ReadUInt16());
			}

			public static string ReadStringToNull(Stream s)
			{
				StringBuilder sb = new StringBuilder();
				int character = s.ReadByte();
				while (character > 0)
				{
					sb.Append((char)character);
					character = s.ReadByte();
				}
				return sb.ToString();
			}

			public static string ReadString(Stream s, int maxLength)
			{
				StringBuilder sb = new StringBuilder();
				int character = s.ReadByte();
				while (character > 0 && sb.Length < maxLength)
				{
					sb.Append((char)character);
					character = s.ReadByte();
				}
				return sb.ToString();
			}

			public static IPAddress ReadIPAddress(Stream s)
			{
				byte[] bytes = new byte[4];
				s.Read(bytes, 0, bytes.Length);
				return new IPAddress(bytes);
			}

			#endregion

			#region Write to stream

			public static void Write2Bytes(Stream s, UInt16 data)
			{
				BinaryWriter bw = new BinaryWriter(s);
				bw.Write((UInt16)IPAddress.HostToNetworkOrder((Int16)data));
			}

			public static void Write4Bytes(Stream s, UInt32 data)
			{
				BinaryWriter bw = new BinaryWriter(s);
				bw.Write((UInt32)IPAddress.HostToNetworkOrder((Int32)data));
			}

			public static void WriteNullTerminatedString(Stream s, string data)
			{
				TextWriter tw = new StreamWriter(s, Encoding.ASCII);
				tw.Write(data);
				tw.Flush();
				s.WriteByte(0); //null-terminated
			}

			public static void WriteNullTerminatedString(Stream s, string data, int length)
			{
				if (data.Length >= length)
				{
					data = data.Substring(0, length - 1);
				}

				TextWriter tw = new StreamWriter(s, Encoding.ASCII);
				tw.Write(data);
				tw.Flush();

				// null-terminated and padding trailing
				for (int t = data.Length; t < length; t++)
				{
					s.WriteByte(0);
				}
			}

			public static void WriteString(Stream s, string data, bool isNullTerminated = false)
			{
				TextWriter tw = new StreamWriter(s, Encoding.ASCII);
				tw.Write(data);
				tw.Flush();
				if (isNullTerminated) s.WriteByte(0);
			}

			public static void WriteIPAddress(Stream s, IPAddress ipAddr)
			{
				byte[] bytes = ipAddr.GetAddressBytes();
				s.Write(bytes, 0, bytes.Length);
			}

			#endregion
		}

		public static class Converter
		{
			public static string ByteToHexString(byte[] data, string separator = "")
			{
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					sb.AppendFormat("{0:X2}", data[i]); // Uppercase hex char
					if (i < (data.Length - 1))
					{
						sb.Append(separator);
					}
				}
				return sb.ToString();
			}

			public static byte[] HexStringToByte(string data)
			{

				int character;
				List<byte> result = new List<byte>();

				// Convert string to bytes in memory stream.
				// In which we can ignore the separator character (which affect hex string length)
				MemoryStream ms = new MemoryStream();
				StreamWriter sw = new StreamWriter(ms);
				sw.Write(data);
				sw.Flush();
				ms.Position = 0;
				StreamReader sr = new StreamReader(ms);

				StringBuilder hexByte = new StringBuilder();

				while ((character = sr.Read()) > 0)
				{
					// ignore the separator character
					if ((character >= '0' && character <= '9') ||
						(character >= 'a' && character <= 'f') ||
						(character >= 'A' && character <= 'F'))
					{
						hexByte.Append((char)character);

						if (hexByte.Length >= 2) // 1-byte enough
						{
							result.Add(Convert.ToByte(hexByte.ToString(), 16));
							hexByte.Length = 0;
						}
					}
				}
				return result.ToArray();
			}

			public static IPAddress UInt32ToIPAddress(UInt32 address)
			{
				return new IPAddress(new byte[] {
				(byte)((address>>24) & 0xFF) ,
				(byte)((address>>16) & 0xFF) ,
				(byte)((address>>8)  & 0xFF) ,
				(byte)( address & 0xFF)});
			}

			public static UInt32 IPAddressToUInt32(IPAddress address)
			{
				return
					(((UInt32)address.GetAddressBytes()[0]) << 24) |
					(((UInt32)address.GetAddressBytes()[1]) << 16) |
					(((UInt32)address.GetAddressBytes()[2]) << 8) |
					(((UInt32)address.GetAddressBytes()[3]));
			}
		}
	}
}
