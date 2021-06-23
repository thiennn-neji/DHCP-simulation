using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDHCP
{
    public class DHCPOptionMessage56 : DHCPOption
    {
        private string _Message;
        public string Message
        {
            get { return _Message; }
        }

        #region IDHCP elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of this option is N bytes

            var obj = new DHCPOptionMessage56();
            obj._Message = Utilities.ReadWriteStream.ReadStringToNull(stream);
            return obj;
        }
        public override void ToStream(Stream stream)
        {
            Utilities.ReadWriteStream.WriteString(stream, _Message, NullTerminatedStrings);
        }
        #endregion

        public DHCPOptionMessage56() : base(EDHCPOption.DHCP_Message) { this.NullTerminatedStrings = true; }
        public DHCPOptionMessage56(string message) : base(EDHCPOption.DHCP_Message)
        {
            this.NullTerminatedStrings = true;
            _Message = message;
        }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", _optionType, _Message);
        }
    }
}
