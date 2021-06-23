using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDHCP
{
    public enum EDHCPMessageType
    {
        Undefined = 0,              // Self-definition
        DHCPDISCOVER = 1,           // [RFC2132]
        DHCPOFFER = 2,              // [RFC2132]
        DHCPREQUEST = 3,            // [RFC2132]
        DHCPDECLINE = 4,            // [RFC2132]
        DHCPACK = 5,                // [RFC2132]
        DHCPNAK = 6,                // [RFC2132]
        DHCPRELEASE = 7,            // [RFC2132]
        DHCPINFORM = 8,             // [RFC2132]

        DHCPFORCERENEW = 9,         // [RFC3203]

        DHCPLEASEQUERY = 10,        // [RFC4388]
        DHCPLEASEUNASSIGNED = 11,   // [RFC4388]
        DHCPLEASEUNKNOWN = 12,      // [RFC4388]
        DHCPLEASEACTIVE = 13,       // [RFC4388]

        DHCPBULKLEASEQUERY = 14,    // [RFC6926]
        DHCPLEASEQUERYDONE = 15,    // [RFC6926]

        DHCPACTIVELEASEQUERY = 16,  // [RFC7724]
        DHCPLEASEQUERYSTATUS = 17,  // [RFC7724]
        DHCPTLS = 18,               // [RFC7724]
    }
    public class DHCPOptionMessageType53 : DHCPOption
    {
        private EDHCPMessageType _MessageType;
        public EDHCPMessageType MessageType
        {
            get { return _MessageType; }
        }

        #region IDHCP elements
        public override IDHCPOption FromStream(Stream stream)
        {
            // Length of Option Message Type is 1 byte
            if (stream.Length != 1) throw new IOException("Invalid DHCP Option length");
            var obj = new DHCPOptionMessageType53();
            obj._MessageType = (EDHCPMessageType)stream.ReadByte();
            return obj;
        }

        public override void ToStream(Stream stream)
        {
            stream.WriteByte((byte)_MessageType);
        }
        #endregion

        public DHCPOptionMessageType53() : base(EDHCPOption.DHCP_Message_Type) { }
        public DHCPOptionMessageType53(EDHCPMessageType messageType) : base(EDHCPOption.DHCP_Message_Type) { _MessageType = messageType; }

        public override string ToString()
        {
            return String.Format("DHCPOption(type={0},value={1})", _optionType, _MessageType.ToString());
        }
    }
}
