using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace LibraryDHCP
{
	public class DHCPOptionRequestedIPAddress50 : DHCPOption
	{
		private IPAddress _IPAddress;

		public IPAddress IPAdress
		{
			get { return _IPAddress; }
		}

		#region IDHCPOption elements

		public override IDHCPOption FromStream(Stream stream)
		{
			// Number bytes of IPAddress
			if (stream.Length != 4) { throw new IOException("Invalid DHCP Option length"); }
			var obj = new DHCPOptionRequestedIPAddress50();
			obj._IPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);
			return obj;
			
		}

		public override void ToStream(Stream stream)
		{
			Utilities.ReadWriteStream.WriteIPAddress(stream, _IPAddress);
		}

		#endregion

		// Non-parameter Constructor
		public DHCPOptionRequestedIPAddress50() : base(EDHCPOption.Requested_IP_Address) { }

		// Constructor
		public DHCPOptionRequestedIPAddress50(IPAddress ipAddr) : base(EDHCPOption.Requested_IP_Address)
		{
			_IPAddress = ipAddr;
		}

		// To-string
		public override string ToString()
		{
			return String.Format("DHCPOption(type={0},value={1})", this._optionType, this._IPAddress.ToString());
		}

	}
}
