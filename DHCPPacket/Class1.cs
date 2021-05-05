using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHCPPacket
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
	public struct DHCPpacket
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
	}
}
