using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHCPPacketNamespace
{

	/*
				   0                   1                   2                   3
				   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
				   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
				   |     op (1)    |   htype (1)   |   hlen (1)    |   hops (1)    |
				   +---------------+---------------+---------------+---------------+
				   |                            xid (4)                            |
				   +-------------------------------+-------------------------------+
				   |           secs (2)            |           flags (2)           |
				   +-------------------------------+-------------------------------+
				   |                          ciaddr  (4)                          |
				   +---------------------------------------------------------------+
				   |                          yiaddr  (4)                          |
				   +---------------------------------------------------------------+
				   |                          siaddr  (4)                          |
				   +---------------------------------------------------------------+
				   |                          giaddr  (4)                          |
				   +---------------------------------------------------------------+
				   |                                                               |
				   |                          chaddr  (16)                         |
				   |                                                               |
				   |                                                               |
				   +---------------------------------------------------------------+
				   |                                                               |
				   |                          sname   (64)                         |
				   +---------------------------------------------------------------+
				   |                                                               |
				   |                          file    (128)                        |
				   +---------------------------------------------------------------+
				   |                                                               |
				   |                          options (variable)                   |
				   +---------------------------------------------------------------+
	*/

	/// <summary>
	/// <para>Khởi tạo 1 đối tượng có cấu trúc tương tự gói tin DHCP</para>
	/// </summary>
	public class DHCPPacket
	{
		/// <summary> 
		/// <para>Octet: 1</para>
		/// <para>Message op code / message type.</para>
		/// <para>1 = BOOTREQUEST, 2 = BOOTREPLY</para>
		/// </summary>
		public byte op;

		/// <summary>
		/// <para>Octet: 1</para>
		/// <para>Hardware address type. Eg: 1 = 10MB ethernet</para>
		/// <para>See ARPparamEnums</para>
		/// </summary>
		public byte htype;

		/// <summary>
		/// <para>Octet: 1</para>
		/// <para>Hardware address length. Eg: length of MACID</para>
		/// <para>See HardwareAddressLengthEnums</para>
		/// </summary>
		public byte hlen;

		/// <summary>
		/// <para>Octet: 1</para>
		/// <para>Client sets to zero, optionally used by relay agents when booting via a relay agent.</para>
		/// </summary>
		public byte hops;

		/// <summary>
		/// <para>Octet: 4</para>
		/// <para>Transaction ID, a random number chosen by the client, used by the client and server to associate messages and responses between a client and a server.</para>
		/// </summary>
		public byte[] xid;

		/// <summary>
		/// <para>Octet: 2</para>
		/// <para>Filled in by client, seconds elapsed since client began address acquisition or renewal process</para>
		/// <para>On the other way, elapsed time from trying to boot</para>
		/// </summary>
		public byte[] secs;

		/// <summary>
		/// <para>Octet: 2</para>
		/// <para>Flags</para>
		/// </summary>
		public byte[] flags;

		/// <summary>
		/// <para>Octet: 4</para>
		/// <para>Client IP address; only filled in if client is in BOUND, RENEW or REBINDING state and can respond	to ARP requests</para>
		/// <para>Client IP</para>
		/// </summary>
		public byte[] ciaddr;

		/// <summary>
		/// <para>Octet: 4</para>
		/// <para>'your' (client) IP address.</para>
		/// <para>Your client IP</para>
		/// </summary>
		public byte[] yiaddr;

		/// <summary>
		/// <para>Octet: 4</para>
		/// <para>IP address of next server to use in bootstrap; returned in DHCPOFFER, DHCPACK by server.</para>
		/// <para>Server IP</para>
		/// </summary>
		public byte[] siaddr;

		/// <summary>
		/// <para>Octet: 4</para>
		/// <para>Relay agent IP address, used in booting via a	relay agent.</para>
		/// <para>Relay agent IP</para>
		/// </summary>
		public byte[] giaddr;

		/// <summary>
		/// <para>Octet: 16</para>
		/// <para>Client hardware address</para>
		/// </summary>
		public byte[] chaddr;

		/// <summary>
		/// <para>Octet: 64</para>
		/// <para>Optional server host name, null terminated string</para>
		/// </summary>
		public byte[] sname;

		/// <summary>
		/// <para>Octet: 128</para>
		/// <para>Boot file name, null terminated string; "generic"	name or null in DHCPDISCOVER, fully qualified</para>
		/// </summary>
		public byte[] file;

		/// <summary>
		/// <para>Octet: var</para>
		/// <para>Optional parameters field.  See the options documents for a list of defined options.</para>
		/// </summary>
		public byte[] options;

		/// <summary>
		/// <para>Total length from opcode-field to file-field (thus Options-field not included) is 236 bytes</para>
		/// </summary>
		private const int DHCP_PACKET_FIXED_LENGTH_WITHOUT_OPTION = 236;

		/// <summary>
		/// <para>Khởi tạo một object DHCP Packet mới.</para>
		/// </summary> 
		public void Init()
		{
			this.xid = new byte[4];
			this.secs = new byte[2];
			this.flags = new byte[2];
			this.ciaddr = new byte[4];
			this.yiaddr = new byte[4];
			this.siaddr = new byte[4];
			this.giaddr = new byte[4];
			this.chaddr = new byte[16];
			this.sname = new byte[64];
			this.file = new byte[128];
		}

		/// <summary>
		/// <para>Hàm dùng để chuyển mảng bytes nhận được thành 1 object có kiểu DHCPPacket</para>
		/// </summary>
		/// <param name="data">Là chuỗi bytes nhận được từ client/server</param>
		/// <returns>Trả về một biến kiểu <c>bool</c> để xác định việc chuyển đổi có thành công hay không</returns>
		public bool BytesToDHCPPacket(byte[] data)
		{
			System.IO.MemoryStream stm = new System.IO.MemoryStream(data, 0, data.Length);
			System.IO.BinaryReader rdr = new System.IO.BinaryReader(stm);
			try
			{
				//read data
				this.op = rdr.ReadByte();			//độ dài 1 byte
				this.htype = rdr.ReadByte();		//độ dài 1 byte
				this.hlen = rdr.ReadByte();			//độ dài 1 byte
				this.hops = rdr.ReadByte();         //độ dài 1 byte
				this.xid = rdr.ReadBytes(4);        //độ dài 4 bytes
				this.secs = rdr.ReadBytes(2);       //độ dài 2 bytes
				this.flags = rdr.ReadBytes(2);      //độ dài 2 bytes
				this.ciaddr = rdr.ReadBytes(4);		//độ dài 4 bytes
				this.yiaddr = rdr.ReadBytes(4);     //độ dài 4 bytes
				this.siaddr = rdr.ReadBytes(4);     //độ dài 4 bytes
				this.giaddr = rdr.ReadBytes(4);     //độ dài 4 bytes
				this.chaddr = rdr.ReadBytes(16);    //độ dài 16 bytes
				this.sname = rdr.ReadBytes(64);     //độ dài 64 bytes
				this.file = rdr.ReadBytes(128);     //độ dài 128 bytes
				
				
				//The rest data is DHCP Options partition
				this.options = rdr.ReadBytes(data.Length - DHCP_PACKET_FIXED_LENGTH_WITHOUT_OPTION);
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			return true;
		}

		/// <summary>
		/// <para>Hàm dùng để copy mảng bytes từ srcArr vào destArr từ vị trí AddLocationIndex trên destArr</para>
		/// </summary>
		/// <param name="destArr">Mảng đích</param>
		/// <param name="srcArr">Mảng nguồn</param>
		/// <param name="AddLocationIndex">Ví trí trên destArr (Mảng đích) bắt đầu copy vào</param>
		/// <returns>Vị trí liền sau vị trí cuối cùng được thêm trên destArr (Mảng đích)</returns>
		private int AddArrayToArray(byte[] destArr, byte[] srcArr, int AddLocationIndex)
        {
			for (int i = 0; i < srcArr.Length; i++)
            {
				destArr[AddLocationIndex++] = srcArr[i];
            }
			return AddLocationIndex;
        }
		
		/// <summary>
		/// <para>Chuyển đối tượng kiểu DHCPPacket hiện tại thành mảng bytes để gửi đi</para>
		/// </summary>
		/// <returns>Mảng bytes sau chuyển đổi</returns>
		public byte[] DHCPPacketToBytes()
		{
			byte[] returnValue;

			try
			{
				returnValue = new byte[DHCP_PACKET_FIXED_LENGTH_WITHOUT_OPTION + this.options.Length];
				returnValue[0] = op;
				returnValue[1] = htype;
				returnValue[2] = hlen;
				returnValue[3] = hops;
				int i = AddArrayToArray(returnValue, xid, 4);
				i = AddArrayToArray(returnValue, secs, i);
				i = AddArrayToArray(returnValue, flags, i);
				i = AddArrayToArray(returnValue, ciaddr, i);
				i = AddArrayToArray(returnValue, yiaddr, i);
				i = AddArrayToArray(returnValue, siaddr, i);
				i = AddArrayToArray(returnValue, giaddr, i);
				i = AddArrayToArray(returnValue, chaddr, i);
				i = AddArrayToArray(returnValue, sname, i);
				i = AddArrayToArray(returnValue, file, i);
				i = AddArrayToArray(returnValue, options, i);
				return returnValue;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				returnValue = null;
			}
			return returnValue;
		}
		
		/// <summary>
		/// <para>Encode một mảng bytes sang chuỗi hexa để hiện thị</para>
		/// </summary>
		/// <param name="byteArr">Mảng bytes đầu vào</param>
		/// <returns>Biểu diễn hexa của mảng bytes đã cho</returns>
		private static string ByteArrayToHexString(byte[] byteArr)
		{
			StringBuilder hex = new StringBuilder(byteArr.Length * 2);
			foreach (byte b in byteArr)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		/// <summary>
		/// <para>Encode đối tượng DHCPPacket thành chuỗi để hiển thị</para>
		/// </summary>
		/// <returns>Chuỗi biểu diễn DHCPPacket object</returns>
		public override string ToString()
		{
			string text = "";
			text += "op(1): " + op.ToString("X") + " htype(1): " + htype.ToString("X") + " hlen(1): " +hlen.ToString("X") + " hops(1): " + hops.ToString("X") + "\r\n";
			text += "xid(4): " + ByteArrayToHexString(xid) + "\r\n";
			text += "secs(2): " + ByteArrayToHexString(secs) + "\r\n";
			text += "flags(2): " + ByteArrayToHexString(flags) + "\r\n";
			text += "ciaddr(4): " + ByteArrayToHexString(ciaddr) + "\r\n";
			text += "yiaddr(4): " + ByteArrayToHexString(yiaddr) + "\r\n";
			text += "siaddr(4): " + ByteArrayToHexString(siaddr) + "\r\n";
			text += "giaddr(4): " + ByteArrayToHexString(giaddr) + "\r\n";
			text += "chaddr(16): " + ByteArrayToHexString(chaddr) + "\r\n";
			text += "sname(64): " + ByteArrayToHexString(sname) + "\r\n";
			text += "file(128): " + ByteArrayToHexString(file) + "\r\n";
			text += "options(): " + ByteArrayToHexString(options) + "\r\n";
			return text;
		}

		/// <summary>
		/// <para>Tách trường DHCP Options trong đối tượng hiện tại thành danh sách các DHCP Option riêng biệt</para>
		/// </summary>
		/// <returns>Danh sách chứa các DHCP Option được tách</returns>
		public List<byte[]> DHCPOptionsSplit()
        {
			List<byte[]> returnDHCPOptionsList = new List<byte[]>();
			for (int i = 4; i < options.Length; i++)
            {
				if (options[i] == 255)
                {
					break;
                }
				int size = options[i + 1] + 2;
				byte[] tmp = new byte[size];
				for (int j = 0; j < size; j++)
                {
					tmp[j] = options[i + j];
                }
				i += (size - 1);
				returnDHCPOptionsList.Add(tmp);
            }
			return returnDHCPOptionsList;
        }
	}

	public enum ARPparamEnums
	{
		/**
					 Hardware Type (hrd)

			  Type   Description                                 
			  ----   -----------                                  
				1    Ethernet (10Mb)                                    
				2    Experimental Ethernet (3Mb)                        
				3    Amateur Radio AX.25                                
				4    Proteon ProNET Token Ring                          
				5    Chaos                                              
				6    IEEE 802 Networks                                  
				7    ARCNET                                             
				8    Hyperchannel                                       
				9    Lanstar                                             
			   10    Autonet Short Address                             
			   11    LocalTalk                                          
			   12    LocalNet (IBM PCNet or SYTEK LocalNET)             
		*/
		IEEE_8023_Ethernet = 1,
		ExperimentalEthernet = 2,
		AmateurRadioAX25 = 3,
		ProteonProNETTokenRing = 4,
		Chaos = 5,
		IEEE_802_Networks = 6,
		ARCNET = 7,
		Hyperchannel = 8,
		Lanstar = 9,
		AutonetShortAddress = 10,
		LocalTalk = 11,
		LocalNet_IBMPCNet_or_SYTEKLocalNET = 12
	}

	public enum HardwareAddressLengthEnums
	{
		DIX_Ethernet = 6,
		IEEE_8023_Ethernet = 6,
		IEEE_8025_TokenRing = 6,
		ARCNET = 1,
		FDDI = 6,
		FrameRelay2 = 2,
		FrameRelay3 = 3,
		FrameRelay4 = 4,
		SMDS = 8
	}

	public enum DHCPOptionEnums
	{
		SubnetMask = 1,
		TimeOffset = 2,
		Router = 3,
		TimeServer = 4,
		NameServer = 5,
		DomainNameServer = 6,
		LogServer = 7,
		CookieServer = 8,
		LPRServer = 9,
		ImpressServer = 10,
		ResourceLocServer = 11,
		HostName = 12,
		BootFileSize = 13,
		MeritDump = 14,
		DomainName = 15,
		SwapServer = 16,
		RootPath = 17,
		ExtensionsPath = 18,
		IpForwarding = 19,
		NonLocalSourceRouting = 20,
		PolicyFilter = 21,
		MaximumDatagramReAssemblySize = 22,
		DefaultIPTimeToLive = 23,
		PathMTUAgingTimeout = 24,
		PathMTUPlateauTable = 25,
		InterfaceMTU = 26,
		AllSubnetsAreLocal = 27,
		BroadcastAddress = 28,
		PerformMaskDiscovery = 29,
		MaskSupplier = 30,
		PerformRouterDiscovery = 31,
		RouterSolicitationAddress = 32,
		StaticRoute = 33,
		TrailerEncapsulation = 34,
		ARPCacheTimeout = 35,
		EthernetEncapsulation = 36,
		TCPDefaultTTL = 37,
		TCPKeepaliveInterval = 38,
		TCPKeepaliveGarbage = 39,
		NetworkInformationServiceDomain = 40,
		NetworkInformationServers = 41,
		NetworkTimeProtocolServers = 42,
		VendorSpecificInformation = 43,
		NetBIOSoverTCPIPNameServer = 44,
		NetBIOSoverTCPIPDatagramDistributionServer = 45,
		NetBIOSoverTCPIPNodeType = 46,
		NetBIOSoverTCPIPScope = 47,
		XWindowSystemFontServer = 48,
		XWindowSystemDisplayManager = 49,
		RequestedIPAddress = 50,
		IPAddressLeaseTime = 51,
		OptionOverload = 52,
		DHCPMessageTYPE = 53,
		ServerIdentifier = 54,
		ParameterRequestList = 55,
		Message = 56,
		MaximumDHCPMessageSize = 57,
		RenewalTimeValue_T1 = 58,
		RebindingTimeValue_T2 = 59,
		Vendorclassidentifier = 60,
		ClientIdentifier = 61,
		NetworkInformationServicePlusDomain = 64,
		NetworkInformationServicePlusServers = 65,
		TFTPServerName = 66,
		BootfileName = 67,
		MobileIPHomeAgent = 68,
		SMTPServer = 69,
		POP3Server = 70,
		NNTPServer = 71,
		DefaultWWWServer = 72,
		DefaultFingerServer = 73,
		DefaultIRCServer = 74,
		StreetTalkServer = 75,
		STDAServer = 76,
		END_Option = 255
	}

	/// <summary>
	/// <para>Message types as defined by the RFC</para>
	/// </summary>
	public enum DHCPMsgType
	{
		/// <summary>
		/// <para>A client broadcasts to locate servers</para>
		/// </summary>
		DHCPDISCOVER = 1,

		/// <summary>
		/// <para>A server offers an IP address to the device</para>
		/// </summary>
		DHCPOFFER = 2,

		/// <summary>
		/// <para>Client accepts offers from DHCP server</para>
		/// </summary>
		DHCPREQUEST = 3,

		/// <summary>
		/// <para>Client declines the offer from this DHCP server</para>
		/// </summary>
		DHCPDECLINE = 4,

		/// <summary>
		/// <para>Server to client + committed IP address</para>
		/// </summary>
		DHCPACK = 5,

		/// <summary>
		/// <para>Server to client to state net address incorrect</para>
		/// </summary>
		DHCPNAK = 6,

		/// <summary>
		/// <para>Graceful shutdown from client to Server</para>
		/// </summary>
		DHCPRELEASE = 7,

		/// <summary>
		/// <para>Client to server asking for local info</para>
		/// </summary>
		DHCPINFORM = 8
	}

	/// <summary>
	/// <para>Khởi tạo đối tượng mô tả trường Option trong DHCP packet</para>
	/// </summary>
	public class DHCPOption
    {
		private const int DHCP_OPTIONS_MINIMUM_LENGTH = 312;

		/// <summary>
		/// <para>Chuỗi bytes thể hiện các Options trong trường DHCP Options</para>
		/// </summary>
		public byte[] contents;

		/// <summary>
		/// <para>Tổng độ dài các Options</para>
		/// </summary>
		public int size;

		/// <summary>
		/// <para>Khởi tạo đối tượng với kích thước tối đa là DHCP_OPTIONS_MINIMUM_LENGTH (312 bytes). Nếu nhiều hơn chưa handle :)</para>
		/// </summary>
		public DHCPOption()
        {
			this.contents = new byte[DHCP_OPTIONS_MINIMUM_LENGTH];
			this.size = 0;
        }

		/// <summary>
		/// <para>Thêm một Option vào DHCP Options</para>
		/// </summary>
		/// <param name="DataToAddArr">Chuỗi byte thể hiện 1 option cần thêm vào</param>
		public void Add(byte[] DataToAddArr)
        {
			for (int i = 0; i < DataToAddArr.Length; i++)
            {
				this.contents[this.size++] = DataToAddArr[i];
            }
        }

    }
	
}
