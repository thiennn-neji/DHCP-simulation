using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace LibraryDHCP
{
    public class DHCPOptionServerIdentifier54 : DHCPOption
    {
        private IPAddress _ServerIPAddress;

        public IPAddress ServerIPAddress
        {
            get { return _ServerIPAddress; }
        }

        #region IDHCPOption elements

        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of this option is 4 octets
            if (stream.Length != 4) throw new IOException("Invalid DHCP option length");
            
            var obj = new DHCPOptionServerIdentifier54();
            obj._ServerIPAddress = Utilities.ReadWriteStream.ReadIPAddress(stream);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            Utilities.ReadWriteStream.WriteIPAddress(stream, _ServerIPAddress);
        }

        #endregion

        public DHCPOptionServerIdentifier54() : base(EDHCPOption.DHCP_Server_Identifier) { }
        public DHCPOptionServerIdentifier54(IPAddress ipAddr) : base(EDHCPOption.DHCP_Server_Identifier) { _ServerIPAddress = ipAddr; }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", _optionType, _ServerIPAddress.ToString());
        }
    }
}
