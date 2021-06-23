using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDHCP
{
    public class DHCPOptionIPAddressLeaseTime51 : DHCPOption
    {
        private TimeSpan _LeaseTime;

        public TimeSpan LeaseTime
        {
            get { return _LeaseTime; }
        }

        #region IDHCP elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Number byte of LeaseTime : 4 bytes
            if (stream.Length != 4) throw new IOException("Invalid DHCP Option length");
            var obj = new DHCPOptionIPAddressLeaseTime51();
            obj._LeaseTime = TimeSpan.FromSeconds(Utilities.ReadWriteStream.Read4Bytes(stream));
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            Utilities.ReadWriteStream.Write4Bytes(stream, (UInt32)_LeaseTime.TotalSeconds);
        }
        #endregion

        public DHCPOptionIPAddressLeaseTime51() : base(EDHCPOption.IP_Address_Lease_Time) { }
        
        public DHCPOptionIPAddressLeaseTime51(TimeSpan leaseTime) : base(EDHCPOption.IP_Address_Lease_Time) 
        {
            if (leaseTime >= TimeSpan.MaxValue)
            {
                _LeaseTime = TimeSpan.MaxValue;
            }
            else
            {
                _LeaseTime = leaseTime;
            }
        }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", this._optionType, Utilities.isInfinityTimeSpan(_LeaseTime) ? "Infinity" : _LeaseTime.ToString());
        }
    }
}
