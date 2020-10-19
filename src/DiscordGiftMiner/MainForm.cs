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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DiscordGiftMiner
{
    public partial class MainForm : Form
    {
        private static Random random = new Random();

        public static bool deneniyor = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            deneniyor = true;
            whiledene();
        }

        public static string generategift(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async void dene()
        {
            string gift = generategift(19);

            try
            {
                WebClient wc = new WebClient();
                string data = wc.DownloadString("https://discordapp.com/api/v6/entitlements/gift-codes/" + gift);
                dynamic jo = JObject.Parse(data);
                textBox1.AppendText("Success: " + gift + "\r\n");

                if (!File.Exists("nitro.txt"))
                {
                    var create = File.Create("nitro.txt");
                    create.Close();
                }

                using (StreamWriter sw = File.AppendText("nitro.txt"))
                {
                    sw.WriteLine("https://discord.com/gifts/" + gift);
                }

                await Task.Delay(10);
            }
            catch (WebException ex)
            {
                textBox1.AppendText("Failed: " + gift + "\r\n");
                await Task.Delay(10);
            }
        }

        public async void whiledene()
        {
            while (true)
            {
                if (deneniyor)
                {
                    dene();
                    await Task.Delay(10);
                }
                else
                {
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            deneniyor = false;
        }
    }
}
