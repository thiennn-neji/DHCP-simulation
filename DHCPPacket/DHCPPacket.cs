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
		/// <para>See HardwareAddLengthEnums</para>
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

		public bool BytesToDHCPPacket(byte[] data)
		{
			System.IO.MemoryStream stm = new System.IO.MemoryStream(data, 0, data.Length);
			System.IO.BinaryReader rdr = new System.IO.BinaryReader(stm);
			try
			{
				//read data
				this.op = rdr.ReadByte();
				this.htype = rdr.ReadByte();
				this.hlen = rdr.ReadByte();
				this.hops = rdr.ReadByte();
				this.xid = rdr.ReadBytes(4);
				this.secs = rdr.ReadBytes(2);
				this.flags = rdr.ReadBytes(2);
				this.ciaddr = rdr.ReadBytes(4);
				this.yiaddr = rdr.ReadBytes(4);
				this.siaddr = rdr.ReadBytes(4);
				this.giaddr = rdr.ReadBytes(4);
				this.chaddr = rdr.ReadBytes(16);
				this.sname = rdr.ReadBytes(64);
				this.file = rdr.ReadBytes(128);
				//read the rest of the data, which shall determine the dhcp

				//options
				byte temp;
				do
				{
					temp = rdr.ReadByte();
					this.options.Append<byte>(temp);
				} while ((int)temp != 255);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			return true;
		}

		private void AddOptionElement(byte[] FromValue, ref byte[] TargetArray)
		{
			try
			{
				//resize the array accoringly

				if (TargetArray != null)
					Array.Resize(ref TargetArray,
					   TargetArray.Length + FromValue.Length);
				else
					Array.Resize(ref TargetArray, FromValue.Length);
				//copy the data over

				Array.Copy(FromValue, 0, TargetArray,
					TargetArray.Length - FromValue.Length,
					FromValue.Length);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		public byte[] DHCPPacketToBytes()
		{
			byte[] returnValue;

			try
			{
				returnValue = new byte[0];
				AddOptionElement(new byte[] { this.op }, ref returnValue);
				AddOptionElement(new byte[] { this.htype }, ref returnValue);
				AddOptionElement(new byte[] { this.hlen }, ref returnValue);
				AddOptionElement(new byte[] { this.hops }, ref returnValue);
				AddOptionElement(this.xid, ref returnValue);
				AddOptionElement(this.secs, ref returnValue);
				AddOptionElement(this.flags, ref returnValue);
				AddOptionElement(this.ciaddr, ref returnValue);
				AddOptionElement(this.yiaddr, ref returnValue);
				AddOptionElement(this.siaddr, ref returnValue);
				AddOptionElement(this.giaddr, ref returnValue);
				AddOptionElement(this.chaddr, ref returnValue);
				AddOptionElement(this.sname, ref returnValue);
				AddOptionElement(this.file, ref returnValue);
				AddOptionElement(this.options, ref returnValue);
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

		public string ToText()
		{
			string text = "";
			// Chuyen sang text de hien thi
			text += "op(1): " + op.ToString("X") + " htype(1): " + htype.ToString("X") + "op(1): " + op.ToString("X") + "op(1): " + op.ToString("X") + "\r\n";
			return text;
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

	public enum HardwareAddLengthEnums
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

	public class option
    {
		public byte[] data;
		public int size;

		public option()
        {
			data = new byte[376];
			size = 0;
        }

		public void add(byte[] d)
        {
			for (int i = 0; i < d.Length; i++)
            {
				data[size++] = d[i];
            }
        }

    }
	
}
