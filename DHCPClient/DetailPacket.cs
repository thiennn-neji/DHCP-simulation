using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using DHCPPacketNamespace;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;

namespace DHCPClient
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
