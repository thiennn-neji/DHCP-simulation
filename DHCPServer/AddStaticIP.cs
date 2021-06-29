using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using networkconfignamespace;

namespace DHCPServer
{
    public partial class AddStaticIP : Form
    {
        public AddStaticIP()
        {
            InitializeComponent();
            mac = new byte[6];
            ok = false;
        }

        public bool ok { get; set; }

        public byte[] mac { get; set; }

        public IPAddress ip { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ip = IPAddress.Parse(tb_IP.Text.Trim());
            }
            catch (Exception z)
            {
                MessageBox.Show(z.Message);
                return;
            }
            
            string[] f = tb_Mac.Text.Trim().Split(':');
            if (f.Length < 6)
            {
                MessageBox.Show("Mac addr not valid");
                return;
            }
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    mac[i] = (byte)Convert.ToInt32(f[i], 16);
                }
            } catch (Exception z)
            {
                MessageBox.Show(z.Message);
                return;
            }
            
            ok = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
