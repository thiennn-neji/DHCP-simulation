using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace LibraryDHCP
{
    class DHCPOptionDomainNameServer6 : DHCPOption
    {
        private List<IPAddress> _DNSAddressList = new List<IPAddress>();
        public List<IPAddress> DefaultRouterAddress
        {
            get { return _DNSAddressList; }
            set { _DNSAddressList = value; }
        }

        #region IDHCP elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // length MUST always be a multiple of 4.
            if (stream.Length % 4 != 0) throw new IOException("Invalid DHCP option length");

            var obj = new DHCPOptionDomainNameServer6();

            for (int t = 0; t < stream.Length; t += 4) { obj._DNSAddressList.Add(Utilities.ReadWriteStream.ReadIPAddress(stream)); }
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            foreach (var ipAddr in _DNSAddressList) { Utilities.ReadWriteStream.WriteIPAddress(stream, ipAddr); }
        }
        #endregion

        public DHCPOptionDomainNameServer6() : base(EDHCPOption.Router_Option) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ipAddr in _DNSAddressList)
            {
                sb.AppendFormat("{0}, ", ipAddr.ToString());
            }
            if (sb.Length > 0) sb.Remove(sb.Length, 1);
            return String.Format("DHCPOption(type={0},value={1})", _optionType, sb.ToString());
        }
    }
}
