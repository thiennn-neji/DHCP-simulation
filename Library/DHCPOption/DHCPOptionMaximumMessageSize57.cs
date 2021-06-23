using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryDHCP
{
    public class DHCPOptionMaximumMessageSize57 : DHCPOption
    {
        private UInt16 _MaxMsgSize;
        public ushort MaxMsgSize
        {
            get { return _MaxMsgSize; }
        }

        #region IDHCPOption Members
        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of this option is 2 octet
            if (stream.Length != 2) throw new IOException("Invalid DHCP option length");

            var obj = new DHCPOptionMaximumMessageSize57();
            obj._MaxMsgSize = Utilities.ReadWriteStream.Read2Bytes(stream);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            Utilities.ReadWriteStream.Write2Bytes(stream, _MaxMsgSize);
        }

        #endregion

        public DHCPOptionMaximumMessageSize57() : base(EDHCPOption.DHCP_Max_Message_Size) { }
        public DHCPOptionMaximumMessageSize57(UInt16 maxSize) : base(EDHCPOption.DHCP_Max_Message_Size) { _MaxMsgSize = maxSize; }

        public override string ToString()
        {
            
            return string.Format("Option(name=[{0}],value=[{1}])", OptionType, _MaxMsgSize);
        }
    }
}
