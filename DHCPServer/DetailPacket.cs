using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHCPPacketNamespace;
using networkconfignamespace;

namespace DHCPServer
{
    public partial class DetailPacket : Form
    {

        public DetailPacket(DHCPPacket packet)
        {
            InitializeComponent();
            rtbMessage.Text = packet.ToText();
        }
    }
}
