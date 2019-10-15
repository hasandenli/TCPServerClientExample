using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP.Client
{
    public partial class Form1 : Form
    {
        //Burda server da tanımladıklarımızdan farklı olarak TcpClient sınıfı ile serverdan gelen bilgileri alıyoruz
        public TcpClient _client;
        private NetworkStream _network;
        private StreamReader _reader;
        private StreamWriter _writer;

        public Form1()
        {
            InitializeComponent();
        }
    

        private void SendButton_Click(object sender, EventArgs e)
        {
            //Kullanıcı butona her tıkladığında textbox'ta yazı yoksa uyarı veriyoruz
            //Sonra AkimYazici vasıtası ile AgAkımına veriyi gönderip sunucudan gelen
            //cevabı AkimOkuyucu ile alıp Mesaj la kullanıcıya gösteriyoruz
            //Tabi olası hatalara karşı, Sunucuya bağlanmada hata oluştu mesajı veriyoruz.
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please, enter your message", "Warning");
                    textBox1.Focus();
                    return;
                }

                string serverMessage;
                _writer.WriteLine(textBox1.Text);
                _writer.Flush();

                serverMessage = _reader.ReadLine();
                MessageBox.Show(serverMessage, "Server Message");
            }

            catch
            {
                MessageBox.Show("Server Connection Error!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _client = new TcpClient("localhost", 9999);
            }
            catch
            {
                Console.WriteLine("Cannot connection!");
                return;
            }
            //Server programında yaptıklarımızı burda da yapıyoruz.
            _network = _client.GetStream();
            _reader = new StreamReader(_network);
            _writer = new StreamWriter(_network);
        }

        //TVe bütün oluşturduğumuz nesneleri form kapatıldığında kapatıyoruz.
        public void form1_kapatma(object o, CancelEventArgs ec)
        {
            try
            {
                _writer.Close();
                _reader.Close();
                _network.Close();
            }

            catch
            {
                MessageBox.Show("Server cannot close correctly!");
            }
        }
    }
}
