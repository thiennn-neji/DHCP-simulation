using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DHCPServer
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        byte[] key = new byte[16];
        byte[] iv = new byte[16] { 0x9A, 0x5A, 0xC8, 0x41, 0x8B, 0xE8, 0xB4, 0x84, 0xE4, 0x74, 0xFA, 0xF2, 0xBF, 0x2F, 0x03, 0xE9 };

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        [Serializable]
        public class save
        {
            public string username;
            public byte[] password;
            public save()
            {
                password = new byte[32];
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tb_Username.Text.Trim();
            string spassword = tb_Password.Text.Trim();
            string skey = tb_Key.Text.Trim();

            key = Encoding.Unicode.GetBytes(skey);
            byte[] pass = Encoding.Unicode.GetBytes(spassword);

            if (key.Length < 32)
            {
                MessageBox.Show("Login failed");
                return;
            }

            pass = Encrypt(pass);
            var hash = HashAlgorithm.Create("SHA256");
            byte[] hashpass = hash.ComputeHash(pass);

            save d = new save();
            
            try
            {
                FileStream fs = new FileStream("../../Login.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                d = (save)formatter.Deserialize(fs);
                fs.Close();
            }
            catch (Exception k)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + k.Message);
                throw;
            }            

            if (d.username != username || !d.password.SequenceEqual(hashpass))
            {
                MessageBox.Show("Login failed");
                return;
            }

            this.Hide();
            Form f = new Server();
            f.Closed += (s, args) => this.Close();
            f.Show();
        }

        public byte[] Encrypt(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }
    }
}
