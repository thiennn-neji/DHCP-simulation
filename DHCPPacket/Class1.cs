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

	public class DHCPPacket
	{
		private byte[1] op;
		private byte[1] htype;
		private byte[1] hlen;
		private byte[1] hops;
		private byte[4] xid;
		private byte[2] secs;
		private byte[2] flags;
		private byte[4] ciaddr;
		private byte[4] yiaddr;
		private byte[4] siaddr;
		private byte[4] giaddr;
		private byte[16] chaddr;
		private byte[64] sname;
		private byte[128] file;
		private List<byte> options;

		public string Op
		{
			get { return Encoding.UTF8.GetString(op);}
			set { op = Encoding.UTF8.GetBytes(value); }
		}
		public string Htype
        {
            get { return Encoding.UTF8.GetString(htype); }
			set { htype = Encoding.UTF8.GetBytes(value); }
        }
		public string Hlen
		{
			get { return Encoding.UTF8.GetString(hlen); }
			set { hlen = Encoding.UTF8.GetBytes(value); }
		}
		public string Hops
		{
			get { return Encoding.UTF8.GetString(hops); }
			set { hops = Encoding.UTF8.GetBytes(value); }
		}
		public string Xid
		{
			get { return Encoding.UTF8.GetString(xid); }
			set { xid = Encoding.UTF8.GetBytes(value); }
		}
		public string Secs
		{
			get { return Encoding.UTF8.GetString(secs); }
			set { secs = Encoding.UTF8.GetBytes(value); }
		}
		public string Flags
		{
			get { return Encoding.UTF8.GetString(flags); }
			set { flags = Encoding.UTF8.GetBytes(value); }
		}
		public string Ciaddr
		{
			get { return Encoding.UTF8.GetString(ciaddr); }
			set { ciaddr = Encoding.UTF8.GetBytes(value); }
		}
		public string Yiaddr
		{
			get { return Encoding.UTF8.GetString(yiaddr); }
			set { yiaddr = Encoding.UTF8.GetBytes(value); }
		}
		public string Siaddr
		{
			get { return Encoding.UTF8.GetString(siaddr); }
			set { siaddr = Encoding.UTF8.GetBytes(value); }
		}
		public string Giaddr
		{
			get { return Encoding.UTF8.GetString(giaddr); }
			set { giaddr = Encoding.UTF8.GetBytes(value); }
		}
		public string Chaddr
		{
			get { return Encoding.UTF8.GetString(chaddr); }
			set { chaddr = Encoding.UTF8.GetBytes(value); }
		}
		public string Sname
		{
			get { return Encoding.UTF8.GetString(sname); }
			set { sname = Encoding.UTF8.GetBytes(value); }
		}
		public string File
		{
			get { return Encoding.UTF8.GetString(file); }
			set { file = Encoding.UTF8.GetBytes(value); }
		}
		public string Options
		{
			get { return Encoding.UTF8.GetString(options); }
			set { options = Encoding.UTF8.GetBytes(value); }
		}
	}
}
