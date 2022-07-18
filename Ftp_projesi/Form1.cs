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

namespace Ftp_projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21);

        FtpSoket ftp;

        private void button1_Click(object sender, EventArgs e)
        { 
            bool sonuc = ftp.Baglan();

            if(sonuc == true)
            {
                ekle("baglanti kuruldu");
            }
            else
            {
                ekle("baglanti kurulamadi");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ftp = new FtpSoket(ip);
            ekle("soket olusturuldu");
        }

        private void ekle(string s)
        {
            listBox1.Items.Add(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool sonuc = ftp.BaglantiKes();

            if (sonuc == true)
            {
                ekle("baglanti kesildi");
            }
            else
            {
                ekle("baglanti kesilemedi");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Int32 g = ftp.VeriGonder(textBox1.Text, true);

            if(g>0)
            {
                ekle("veri gonderildi -> " + textBox1.Text);
            }
            else
            {
                ekle("veri gonderilemedi");
            }          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = ftp.VeriAl(true);

            if(s.Length>0)
            {
                ekle("veri alindi -> " + s);
            }
            else
            {
                ekle("veri alinamadi");
            }
        }
    }
}
