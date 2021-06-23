using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryDHCP
{
    /// <summary>
    /// Default class for all others DHCPOption that have not been declared
    /// </summary>
    class DHCPOptionGeneric :DHCPOption
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
            var obj = new DHCPOptionGeneric(_optionType);
            obj._Data = new byte[stream.Length];
            stream.Read(obj._Data, 0, obj._Data.Length);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            stream.Write(_Data, 0, _Data.Length);
        }

        #endregion

        public DHCPOptionGeneric(EDHCPOption optionType) : base(optionType) { _Data = new byte[0]; }

        public DHCPOptionGeneric(EDHCPOption optionType, byte[] data) : base(optionType) { _Data = data; }

        public override string ToString()
        {
            return string.Format("DHCPOption(type={0},value={1})", _optionType, Utilities.Converter.ByteToHexString(_Data, " "));
        }
    }
}
