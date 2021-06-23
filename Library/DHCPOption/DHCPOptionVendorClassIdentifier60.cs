using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryDHCP
{
    public class DHCPOptionVendorClassIdentifier60 : DHCPOption
    {
        private byte[] _Data;

        public byte[] Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        #region IDHCPOption elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Length for this opion is N octet

            var obj = new DHCPOptionVendorClassIdentifier60();
            obj._Data = new byte[stream.Length];
            stream.Read(obj._Data, 0, obj._Data.Length);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            stream.Write(_Data, 0, _Data.Length);
        }
        #endregion

        public DHCPOptionVendorClassIdentifier60() : base(EDHCPOption.Vendor_class_Identifier) { _Data = new byte[0]; }
        public DHCPOptionVendorClassIdentifier60(byte[] data) : base(EDHCPOption.Vendor_class_Identifier) { _Data = data; }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", _optionType, Utilities.Converter.ByteToHexString(_Data, " "));
        }
    }
}
