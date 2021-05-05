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

namespace DHCPClient
{
    public partial class DHCP_Client : Form
    {
        public DHCP_Client()
        {
            InitializeComponent();
        }

        IPAddress ip; // Ip hien co cua client
        IPAddress defaultgateway; // default gateway cua client
        IPAddress subnetmask; // subneskmask hien co cua client

        private void btnRenew_Click(object sender, EventArgs e)
        {
            sendrelease(); // gui dhcp relesase
            SendDiscover();  // gui dhcp discover   
            UdpClient udpclient = new UdpClient(67); // mo port 67 va cho doi dhcp offer
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
            Byte[] recvBytes = udpclient.Receive(ref IpEnd);
            udpclient.Close();
            DHCPPacket d = new DHCPPacket(); // chuyen doi dhcp offer o dang byte thanh cau truc dhcp packet
            display1(d); // hien thi thong tin goi dhcp vua nhan duoc
            sendrequest(new IPAddress(d.yiaddr)); // gui dhcp request

            udpclient = new UdpClient(67); // doi nhan dhcp ack
            recvBytes = udpclient.Receive(ref IpEnd);
            udpclient.Close();
            d = new DHCPPacket(); // chuyen doi goi dhcp ack
            display1(d); // hien thi thong tin goi dhcp vua nhan duoc
            display2(); // Cap nhat dia chi ip moi va hien thi
        }        

        void sendrelease()
        {
            // send dhcp release
            display2();
        }

        void display1(DHCPPacket d)
        {
            // Display dhcp messeage
        }

        void display2()
        {
            rtbPara.Text = "Ip address: " + ip.ToString() + "\r\n";
            rtbPara.Text += "Default Gateway: " + ip.ToString() + "\r\n";
            rtbPara.Text += "Subnet Mask: " + ip.ToString() + "\r\n";
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            sendrelease();
        }

        void SendDiscover()
        {
            // send dhcp discover
        }

        void sendrequest(IPAddress ip)
        {
            // send dhcp request
        }

    }
}
