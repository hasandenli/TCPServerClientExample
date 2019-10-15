using System;
using System.Net;  
using System.Net.Sockets;
using System.IO;

namespace TCP.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bilgi alisverisi için bilgi almak istedigimiz port numarasini TcpListener sinifi ile gerçeklestiriyoruz

            TcpListener tcpListener = new TcpListener(IPAddress.Any, 9999);
            tcpListener.Start();

            Console.WriteLine("Server has been started...");

            //Soket baglantimizi yapiyoruz.Bunu TcpListener sinifinin AcceptSocket metodu ile yaptigimiza dikkat edin
            Socket clientSocket = tcpListener.AcceptSocket();


            // Baglantının olup olmadığını kontrol ediyoruz
            if (!clientSocket.Connected)
            {
                Console.WriteLine("Cannot start server....");
            }
            else
            {
                //Sonsuz döngü sayesinde AgAkimini sürekli okuyoruz
                while (true)
                {
                    Console.WriteLine("Client connection is provided...");

                    //IstemciSoketi verilerini NetworkStream sinifi türünden nesneye aktariyoruz.
                    NetworkStream networkStream = new NetworkStream(clientSocket);

                    //Soketteki bilgilerle islem yapabilmek için StreamReader ve StreamWriter siniflarini kullaniyoruz
                    StreamWriter networkWriter = new StreamWriter(networkStream);
                    StreamReader networkReader = new StreamReader(networkStream);

                    //StreamReader ile String veri tipine aktarma islemi önceden bir hata olursa bunu handle etmek gerek
                    try
                    {
                        string clientMessage = networkReader.ReadLine();

                        Console.WriteLine("Client Message : " + clientMessage);

                        //Istemciden gelen bilginin uzunlugu hesaplaniyor
                        int messageLength = clientMessage.Length;

                        //AgAkimina, AkimYazını ile IstemciString inin uzunluğunu yazıyoruz
                        networkWriter.WriteLine(messageLength.ToString());

                        networkWriter.Flush();
                    }
                    catch
                    {
                        Console.WriteLine("Error : Server is shutting down!");
                        return;
                    }
                }
            }

            clientSocket.Close();

            Console.WriteLine("Server is shutting down...");
        }
    }
}

