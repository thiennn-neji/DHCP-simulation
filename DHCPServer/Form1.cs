using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHCPPacketNamespace;

namespace DHCPServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            running();
        }

        public class item
        {
            public string macaddress;
            public string ip;
            public Int64 time;
        }

        List<item> table;

        void running()
        {
            table = new List<item>();
            UdpClient udpclient = new UdpClient(68);
            Thread t = new Thread(new ThreadStart(time));
            t.Start();
            while (true)
            {
                IPEndPoint IpEnd = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recvBytes = udpclient.Receive(ref IpEnd);
                udpclient.Close();
                DHCPPacket d = new DHCPPacket();
                d.BytesToDHCPPacket(recvBytes);
                solve(d);
                display(d);
                udpclient = new UdpClient(68);
            }
        }

        void time()
        {
            while (true)
            {
                Int64 epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                for (int i = 0; i < table.Count(); i++)
                {                    
                    if (table[i].time < epoch)
                    {
                        table.RemoveAt(i);
                        i--;
                    }
                }
                ShowTime(epoch);
                Thread.Sleep(5000);
            }
        }

        void ShowTime(Int64 ti)
        {
            listView1.Items.Clear();
            for (int i = 0; i < table.Count(); i++)
            {
                ListViewItem lvItem = new ListViewItem(table[i].macaddress);
                lvItem.SubItems.Add(table[i].ip);
                lvItem.SubItems.Add((table[i].time - ti).ToString() + "s");
                listView1.Items.Add(lvItem);
            }
        }

        void solve(DHCPPacket d)
        {
            // Xu li goi tin dhcp
        }

        void sendack()
        {

        }

        void sendoffer()
        {

        }

        void display(DHCPPacket d)
        {
            // Hien thi goi tin dhcp vua nhan duoc len man hinh
        }
    }
}
