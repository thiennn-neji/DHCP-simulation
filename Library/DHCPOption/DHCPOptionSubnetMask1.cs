using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace LibraryDHCP
{
    public class DHCPOptionSubnetMask1 : DHCPOption
    {
        private IPAddress _SubnetMask;
        public IPAddress SubnetMask
        {
            get
            {
                return _SubnetMask;
            }
        }

        #region IDHCPOption elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Subnet mask has length is 4 octets
            if (stream.Length != 4) throw new IOException("Invalid DHCP option length");
            
            var obj = new DHCPOptionSubnetMask1();
            obj._SubnetMask = Utilities.ReadWriteStream.ReadIPAddress(stream);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            Utilities.ReadWriteStream.WriteIPAddress(stream, _SubnetMask);
        }

        #endregion

        public DHCPOptionSubnetMask1() : base(EDHCPOption.Subnet_Mask) { }
        public DHCPOptionSubnetMask1(IPAddress subnetMask) : base(EDHCPOption.Subnet_Mask) { _SubnetMask = subnetMask; }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", _optionType, _SubnetMask.ToString());
        }
    }
}
