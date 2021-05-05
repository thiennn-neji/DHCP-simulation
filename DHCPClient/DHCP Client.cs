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

namespace DHCPClient
{
    public partial class DHCP_Client : Form
    {
        public DHCP_Client()
        {
            InitializeComponent();
        }

        void SendDiscover()
        {
            UdpClient client = new UdpClient();
            byte[] mess = UTF8Encoding.UTF8.GetBytes("need ")
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {

        }
              
    }
}
