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

		public byte Op
		{
			get { return op;}
			set { op = value; }
		}
		public byte Htype
        {
            get { return htype; }
			set { htype = value; }
        }
		public byte Hlen
		{
			get { return hlen; }
			set { hlen = value; }
		}
		public byte Hops
		{
			get { return hops; }
			set { hops = value; }
		}
		public byte Xid
		{
			get { return xid; }
			set { xid = value; }
		}
		public byte Secs
		{
			get { return secs; }
			set { secs = value; }
		}
		public byte Flags
		{
			get { return flags; }
			set { flags = value; }
		}
		public byte Ciaddr
		{
			get { return ciaddr; }
			set { ciaddr = value; }
		}
		public byte Yiaddr
		{
			get { return yiaddr; }
			set { yiaddr = value; }
		}
		public byte Siaddr
		{
			get { return siaddr; }
			set { siaddr = value; }
		}
		public byte Giaddr
		{
			get { return giaddr; }
			set { giaddr = value; }
		}
		public byte Chaddr
		{
			get { return chaddr; }
			set { chaddr = value; }
		}
		public byte Sname
		{
			get { return sname; }
			set { sname = value; }
		}
		public byte File
		{
			get { return file; }
			set { file = value; }
		}
		public byte Options
		{
			get { return options; }
			set { options = value; }
		}
	}
}
