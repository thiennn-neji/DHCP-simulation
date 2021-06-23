using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace LibraryDHCP
{
	public enum EOpcode
	{
		Unknown = 0,
		BootRequest = 1,
		BootReply = 2
	}

	public enum EHardwareType   //RFC 1700 page 163,164
	{
		Unknown = 0,
		Ethernet_10Mb = 1,
		Experimental_Ethernet_3Mb = 2,
		Amateur_Radio_AX_25 = 3,
		Proteon_ProNET_Token_Ring = 4,
		Chaos = 5,
		IEEE_802_Networks = 6,
		ARCNET = 7,
		Hyperchannel = 8,
		Lanstar = 9,
		Autonet_Short_Address = 10,
		LocalTalk = 11,
		LocalNet = 12,
		Ultra_link = 13,
		SMDS = 14,
		Frame_Relay = 15,
		Asynchronous_Transmission_Mode_1 = 16,
		HDLC = 17,
		Fibre_Channel_ = 18,
		Asynchronous_Transmission_Mode_2 = 19,
		Serial_Line = 20,
		Asynchronous_Transmission_Mode_3 = 21,
	}

	class DHCPMessage
	{
		private static IDHCPOption[] optionsTemplates;

		#region DHCP Message fields definition

		private EOpcode _Opcode;
		private EHardwareType _HardwareType;
		// _HardwareLength phụ thuộc vào _HardwareType
		// và có thể truy xuất từ _ClientHardwareAddress nên không cần lưu
		private byte _Hops;
		private uint _XID;
		private ushort _Secs;
		private bool _Flags_Broadcast;
		private IPAddress _ClientIPAddress;
		private IPAddress _YourIPAddress;
		private IPAddress _NextServerIPAddress;
		private IPAddress _RelayAgentIPAddress;
		private byte[] _ClientHardwareAddress;
		private string _ServerHostname; // null-terminated string
		private string _BootFileName; // null-terminated string
		private List<IDHCPOption> _Options;

		#endregion

		#region DHCP Message Property definition
		// Public get, set for private Field

		public EOpcode Opcode
		{
			get { return this._Opcode; }
			set { this._Opcode = value; }
		}

		public EHardwareType HardwareType
		{
			get { return this._HardwareType; }
			set { this._HardwareType = value; }
		}

		public byte Hops
		{
			get { return this._Hops; }
			set { this._Hops = value; }
		}

		public uint XID
		{
			get { return this._XID; }
			set { this._XID = value; }
		}

		public ushort Secs
		{
			get { return this._Secs; }
			set { this._Secs = value; }
		}

		public bool Flags_Broadcast
		{
			get { return this._Flags_Broadcast; }
			set { this._Flags_Broadcast = value; }
		}

		public IPAddress ClientIPAddress
		{
			get { return this._ClientIPAddress; }
			set { this._ClientIPAddress = value; }
		}

		public IPAddress YourIPAddress
		{
			get { return this._YourIPAddress; }
			set { this._YourIPAddress = value; }
		}

		public IPAddress NextServerIPAddress
		{
			get { return this._NextServerIPAddress; }
			set { this._NextServerIPAddress = value; }
		}

		public IPAddress RelayAgentIPAddress
		{
			get { return this._RelayAgentIPAddress; }
			set { this._RelayAgentIPAddress = value; }
		}

		public byte[] ClientHardwareAddress
		{
			get { return this._ClientHardwareAddress; }
			set { this._ClientHardwareAddress = value; }
		}

		public string ServerHostname
		{
			get { return this._ServerHostname; }
			set { this._ServerHostname = value; }
		}

		public string BootFileName
		{
			get { return this._BootFileName; }
			set { this._BootFileName = value; }
		}

		public List<IDHCPOption> Options
		{
			get { return this._Options; }
			set { this._Options = value; }
		}
		
		/// <summary>
		/// Thuộc tính cho biết Type của Message này.
		/// Đọc từ DHCPOption, phần DHCPOptionMessageType
		/// </summary>
		public EDHCPMessageType MessageType
		{
			get
			{
				var messageTypeDHCPOption = (DHCPOptionMessageType53)GetOption(EDHCPOption.DHCP_Message_Type);
				if (messageTypeDHCPOption != null)
				{
					return messageTypeDHCPOption.MessageType;
				}
				else
				{
					return EDHCPMessageType.Undefined;
				}
			}
			set
			{
				EDHCPMessageType currentMessageType = MessageType;
				if (currentMessageType != value)
				{
					_Options.Add(new DHCPOptionMessageType53(value));
				}
			}
		}

		#endregion

		#region Static Methods
		/// <summary>
		/// Phương thức khởi tạo tĩnh. Khởi tạo các template cho từng từng DHCPOption Code. Những DHCPOption chưa tạo class thì dùng class DHCPOptionGeneric
		/// </summary>
		static DHCPMessage()
		{
			optionsTemplates = new IDHCPOption[256];
			for (int t = 1; t < 255; t++)
			{
				optionsTemplates[t] = new DHCPOptionGeneric((EDHCPOption)t);
			}

			RegisterOption(new DHCPOptionFixedLength(EDHCPOption.Pad));
			RegisterOption(new DHCPOptionFixedLength(EDHCPOption.End));
			RegisterOption(new DHCPOptionSubnetMask1());
			RegisterOption(new DHCPOptionDefaultRouter3());
			RegisterOption(new DHCPOptionDomainNameServer6());
			RegisterOption(new DHCPOptionRequestedIPAddress50());
			RegisterOption(new DHCPOptionIPAddressLeaseTime51());
			RegisterOption(new DHCPOptionMessageType53());
			RegisterOption(new DHCPOptionServerIdentifier54());
			RegisterOption(new DHCPOptionParameterRequestList55());
			RegisterOption(new DHCPOptionMessage56());
			RegisterOption(new DHCPOptionMaximumMessageSize57());
			RegisterOption(new DHCPOptionVendorClassIdentifier60());
			RegisterOption(new DHCPOptionClientIdentifier61());

			// HAVE TO DO
			// RegisterOption(new DHCPOptionHostName());
			// RegisterOption(new DHCPOptionOptionOverload());
			// RegisterOption(new DHCPOptionTFTPServerName());
			// RegisterOption(new DHCPOptionBootFileName());
			// RegisterOption(new DHCPOptionRenewalTimeValue());
			// RegisterOption(new DHCPOptionRebindingTimeValue());
		}

		private static void RegisterOption(IDHCPOption option)
		{
			optionsTemplates[(int)option.OptionType] = option;
		}

		/// <summary>
		/// [RFC2132] section 9.3
		/// Locate the overload option value in the passed stream.
		/// Overload has 3 values
		/// </summary>
		/// <param name="stream"></param>
		/// <returns>Returns the overload option value, or 0 if it wasn't found</returns>
		private static byte ScanOverload(Stream stream)
		{
			byte result = 0;

			while (true)
			{
				int code = stream.ReadByte();
				if (code == -1 || code == (int)EDHCPOption.End) break;
				else if (code == (int)EDHCPOption.Pad) continue;
				else if (code == (int)EDHCPOption.Overload)
				{
					if (stream.ReadByte() != 1) throw new IOException("Invalid length of DHCP option 'Option Overload'");
					result = (byte)stream.ReadByte();
				}
				else
				{
					int lengthOption = stream.ReadByte();
					if (lengthOption == -1) break;
					stream.Position += lengthOption;
				}
			}
			return result;
		}

		private static List<IDHCPOption> ReadOptions(byte[] buffer1, byte[] buffer2, byte[] buffer3)
		{
			var result = new List<IDHCPOption>();
			ReadOptions(result, new MemoryStream(buffer1, true), new MemoryStream(buffer2, true), new MemoryStream(buffer3, true));
			ReadOptions(result, new MemoryStream(buffer2, true), new MemoryStream(buffer3, true));
			ReadOptions(result, new MemoryStream(buffer3, true));
			return result;
		}

		/// <summary>
		/// Copy <paramref name="length"/> bytes from <paramref name="source"/> stream to <paramref name="target"/> stream.
		/// And advances the position of the streams
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="length"></param>
		private static void CopyBytes(Stream source, Stream target, int length)
		{
			byte[] buffer = new byte[length];
			source.Read(buffer, 0, length);
			target.Write(buffer, 0, length);
		}

		/// <summary>
		/// Đọc các DHCP Option từ MemoryStream (mảng bytes) đưa vào List <paramref name="options"/>
		/// </summary>
		/// <param name="options"></param>
		/// <param name="mainSourceStream"></param>
		/// <param name="otherSourceStream"></param>
		private static void ReadOptions(List<IDHCPOption> options, MemoryStream mainSourceStream, params MemoryStream[] otherSourceStream)
		{
			while (true)
			{
				int code = mainSourceStream.ReadByte();
				if (code == -1 || code == (int)EDHCPOption.End) break;
				else if (code == (int)EDHCPOption.Pad) continue;
				else
				{
					MemoryStream resultMemoryStream = new MemoryStream();
					int len = mainSourceStream.ReadByte();
					if (len == -1) break;
					CopyBytes(mainSourceStream, resultMemoryStream, len);
					AppendOverflow(code, mainSourceStream, resultMemoryStream);
					foreach (MemoryStream stream in otherSourceStream)
					{
						AppendOverflow(code, stream, resultMemoryStream);
					}
					resultMemoryStream.Position = 0;
					options.Add(optionsTemplates[code].FromStream(resultMemoryStream));
				}
			}
		}

		/// <summary>
		/// Tìm tất cả các phần của 1 DHCPOption có Opcode = <paramref name="code"/> thêm vào <paramref name="source"/> Stream
		/// </summary>
		/// <param name="code"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		private static void AppendOverflow(int code, MemoryStream source, MemoryStream target)
		{
			long initPosition = source.Position;
			try
			{
				while (true)
				{
					int c = source.ReadByte();
					if (c == -1 || c == 255) break;
					else if (c == 0) continue;
					else
					{
						int l = source.ReadByte();
						if (l == -1) break;

						if (c == code)
						{
							long startPosition = source.Position - 2;
							CopyBytes(source, target, l);
							source.Position = startPosition;
							for (int t = 0; t < (l + 2); t++)
							{
								source.WriteByte(0);
							}
						}
						else
						{
							source.Seek(l, SeekOrigin.Current);
						}
					}
				}
			}
			finally
			{
				source.Position = initPosition;
			}
		}
        #endregion

        #region Constructors
        public DHCPMessage()
		{
			_HardwareType = EHardwareType.Ethernet_10Mb;
			_ClientIPAddress = IPAddress.Any;
			_YourIPAddress = IPAddress.Any;
			_NextServerIPAddress = IPAddress.Any;
			_RelayAgentIPAddress = IPAddress.Any;
			// _Options = new List<IDHCPOption>();
		}

		private DHCPMessage(Stream stream) : this()
		{
			_Opcode = (EOpcode)stream.ReadByte();
			_HardwareType = (EHardwareType)stream.ReadByte();

			// Đọc phần Hlen (HardwareLength) để xác định độ dài mảng lưu ClientHardwareAddress
			_ClientHardwareAddress = new byte[stream.ReadByte()];
			
			_Hops = (byte)stream.ReadByte();
			_XID = Utilities.ReadWriteStream.Read4Bytes(stream);
			_Secs = Utilities.ReadWriteStream.Read2Bytes(stream);

			// Ref: [RFC2131] page 11
			// Flag 16 bit, chỉ có bit 0 là đang được dùng (to specify server MUST send broadcast to client or not)
			_Flags_Broadcast = ((Utilities.ReadWriteStream.Read2Bytes(stream) & 0x8000) == 0x8000);
			
			_ClientIPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);
			_YourIPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);
			_NextServerIPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);
			_RelayAgentIPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);

			// ClientHardwareAddress được pad cho đủ 16 bytes, sau khi đọc ClientHardwareAddress cần đọc tiếp phần còn lạiđể loại bỏ bytes padding
			stream.Read(_ClientHardwareAddress, 0, _ClientHardwareAddress.Length);
			for (int i = _ClientHardwareAddress.Length; i < 16; i++) stream.ReadByte();

			// Ref: [RFC2132] Section 9.3, page 26
			// ServerHostname (sname) và BootFileName (file) có thể được overload để lưu DHCPOption.
			// Chuyển thành mảng byte buffer để xử lí sau
			byte[] serverHostnameBuffer = new byte[64];
			byte[] bootFileNameBuffer = new byte[128];
			stream.Read(serverHostnameBuffer, 0, serverHostnameBuffer.Length);
			stream.Read(bootFileNameBuffer, 0, bootFileNameBuffer.Length);

			// Ref: [RFC1048], [https://community.cisco.com/t5/switching/why-dhcp-option-has-quot-magic-cookie-quot/td-p/1764244]
			// Ban đầu
			// Vì DHCP message có format gần như giống hoàn toàn với BOOTP message.
			// Nên magic cookie dùng để phân biệt DHCP message với BOOTP.
			// Nếu như tồn tại magic cookie, thì mọi thứ phía sau đó được xử lí như DHCP Options.
			// Bây giờ
			// magic cookie được gán cố định theo RFC1048 và các RFC sau chấp nhận nó, nên đây xem như trường hiển nhiên
			// nếu như không có giá trị như được đề cập ở dưới, thì đã có lỗi trong quá trình IO hoặc truyền
			// Magic cookies được cố định là 4 bytes [0x63 0x82 0x53 0x63] hay [99 130 83 99]
			if (stream.ReadByte() != 0x63) throw new IOException();
			if (stream.ReadByte() != 0x82) throw new IOException();
			if (stream.ReadByte() != 0x53) throw new IOException();
			if (stream.ReadByte() != 0x63) throw new IOException();

			// Phần còn lại là DHCPOption. Phần này cần xử lí
			byte[] optionsBuffer = new byte[stream.Length - stream.Position];
			stream.Read(optionsBuffer, 0, optionsBuffer.Length);

			// Xác định thông tin trong DHCPOptionOverload
			byte overload = ScanOverload(new MemoryStream(optionsBuffer));

			switch (overload)
			{
				default:
					_ServerHostname = Utilities.ReadWriteStream.ReadStringToNull(new MemoryStream(serverHostnameBuffer));
					_BootFileName = Utilities.ReadWriteStream.ReadStringToNull(new MemoryStream(bootFileNameBuffer));
					_Options = ReadOptions(optionsBuffer, new byte[0], new byte[0]);
					break;

				case 1:
					_ServerHostname = Utilities.ReadWriteStream.ReadStringToNull(new MemoryStream(serverHostnameBuffer));
					_Options = ReadOptions(optionsBuffer, bootFileNameBuffer, new byte[0]);
					break;

				case 2:
					_BootFileName = Utilities.ReadWriteStream.ReadStringToNull(new MemoryStream(bootFileNameBuffer));
					_Options = ReadOptions(optionsBuffer, serverHostnameBuffer, new byte[0]);
					break;

				case 3:
					_Options = ReadOptions(optionsBuffer, bootFileNameBuffer, serverHostnameBuffer);
					break;
			}
		}
		#endregion

		/// <summary>
		/// Lấy ra 1 object DHCP Option có DHCPOptionType là <paramref name="optionType"/>
		/// </summary>
		/// <param name="optionType"></param>
		/// <returns></returns>
		public IDHCPOption GetOption(EDHCPOption optionType)
		{
			return _Options.Find((IDHCPOption v) => { return v.OptionType == optionType; });
		}

		/// <summary>
		/// Tạo 1 DHCPMessage từ MemoryStream
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static DHCPMessage FromStream(Stream s)
		{
			return new DHCPMessage(s);
		}

		/// <summary>
		/// Chuyển DHCPMessage hiện tại vào MemoryStream. (Sau đó có thể chuyển sang ByteArray)
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="minimumPacketSize"></param>
		public void ToStream(Stream stream, int minimumPacketSize)
		{
			stream.WriteByte((byte)_Opcode);
			stream.WriteByte((byte)_HardwareType);
			stream.WriteByte((byte)_ClientHardwareAddress.Length);
			stream.WriteByte((byte)_Hops);

			Utilities.ReadWriteStream.Write4Bytes(stream, _XID);
			Utilities.ReadWriteStream.Write2Bytes(stream, _Secs);
			Utilities.ReadWriteStream.Write2Bytes(stream, _Flags_Broadcast ? (UInt16)0x8000 : (UInt16)0x0);

			Utilities.ReadWriteStream.WriteIPAddress(stream, _ClientIPAddress);
			Utilities.ReadWriteStream.WriteIPAddress(stream, _YourIPAddress);
			Utilities.ReadWriteStream.WriteIPAddress(stream, _NextServerIPAddress);
			Utilities.ReadWriteStream.WriteIPAddress(stream, _RelayAgentIPAddress);
			stream.Write(_ClientHardwareAddress, 0, _ClientHardwareAddress.Length);
			for (int t = _ClientHardwareAddress.Length; t < 16; t++) stream.WriteByte(0);
			Utilities.ReadWriteStream.WriteNullTerminatedString(stream, _ServerHostname, 64);  
			Utilities.ReadWriteStream.WriteNullTerminatedString(stream, _BootFileName, 128);
			stream.Write(new byte[] { 99, 130, 83, 99 }, 0, 4);  // magic cookie

			// DHCP Options
			foreach (IDHCPOption option in _Options)
			{
				MemoryStream optionStream = new MemoryStream();
				option.ToStream(optionStream);
				stream.WriteByte((byte)option.OptionType);
				stream.WriteByte((byte)optionStream.Length);
				optionStream.Position = 0;
				CopyBytes(optionStream, stream, (int)optionStream.Length);
			}
			
			// DHCP END
			stream.WriteByte((byte)EDHCPOption.End);
			stream.Flush();

			// Padding if still not enough msgSize
			while (stream.Length < minimumPacketSize) 
			{
				stream.WriteByte(0);
			}

			stream.Flush();
		}
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("Opcode (op)                    : {0}{1}", _Opcode, Environment.NewLine);
			sb.AppendFormat("HardwareType (htype)           : {0}{1}", _HardwareType, Environment.NewLine);
			sb.AppendFormat("Hops                           : {0}{1}", _Hops, Environment.NewLine);
			sb.AppendFormat("XID                            : {0}{1}", _XID, Environment.NewLine);
			sb.AppendFormat("Secs                           : {0}{1}", _Secs, Environment.NewLine);
			sb.AppendFormat("BroadCast (flags)              : {0}{1}", _Flags_Broadcast, Environment.NewLine);
			sb.AppendFormat("ClientIPAddress (ciaddr)       : {0}{1}", _ClientIPAddress, Environment.NewLine);
			sb.AppendFormat("YourIPAddress (yiaddr)         : {0}{1}", _YourIPAddress, Environment.NewLine);
			sb.AppendFormat("NextServerIPAddress (siaddr)   : {0}{1}", _NextServerIPAddress, Environment.NewLine);
			sb.AppendFormat("RelayAgentIPAddress (giaddr)   : {0}{1}", _RelayAgentIPAddress, Environment.NewLine);
			sb.AppendFormat("ClientHardwareAddress (chaddr) : {0}{1}", Utilities.Converter.ByteToHexString(_ClientHardwareAddress, "-"), Environment.NewLine);
			sb.AppendFormat("ServerHostName (sname)         : {0}{1}", _ServerHostname, Environment.NewLine);
			sb.AppendFormat("BootFileName (file)            : {0}{1}", _BootFileName, Environment.NewLine);

			foreach (IDHCPOption option in _Options)
			{
				sb.AppendFormat("Option                         : {0}{1}", option.ToString(), Environment.NewLine);
			}

			return sb.ToString();
		}

	}
}
