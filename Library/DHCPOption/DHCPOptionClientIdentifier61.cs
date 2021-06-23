using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryDHCP
{
    public class DHCPOptionClientIdentifier61 : DHCPOption
    {
        // Ref [RFC2132] section 9.14
        // For instance, it MAY consist of a hardware type and hardware address. 
        // In this case the type field SHOULD be one of the ARP hardware types defined in STD2[22].
        // A hardware type of 0 (zero) should be used when the value field contains an identifier other than a hardware address (e.g.a fully qualified domain name).
        private EHardwareType _HardwareType;
        private byte[] _OtherData;

        public EHardwareType HardwareType
        {
            get { return _HardwareType; }
            set { _HardwareType = value; }
        }
        public byte[] OtherData
        {
            get { return _OtherData; }
            set { _OtherData = value; }
        }

        #region IDHCPOption elements

        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of this Option is N octet

            var obj = new DHCPOptionClientIdentifier61();
            obj._HardwareType = (EHardwareType)stream.ReadByte();
            obj._OtherData = new byte[stream.Length - stream.Position];
            stream.Read(obj._OtherData, 0, obj._OtherData.Length);
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            stream.WriteByte((byte)_HardwareType);
            stream.Write(_OtherData, 0, _OtherData.Length);
        }

        #endregion

        public DHCPOptionClientIdentifier61() : base(EDHCPOption.Client_Identifier)
        {
            _HardwareType = EHardwareType.Unknown;
            _OtherData = new byte[0];
        }

        public DHCPOptionClientIdentifier61(EHardwareType hardwareType, byte[] otherData) : base(EDHCPOption.Client_Identifier)
        {
            _HardwareType = hardwareType;
            _OtherData = otherData;
        }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},hardwareType={1},value={2})", _optionType, _HardwareType, Utilities.Converter.ByteToHexString(_OtherData, ":"));
        }
    }
}
