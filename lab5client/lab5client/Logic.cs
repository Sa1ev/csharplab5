using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5client
{
    class Logic
    {
        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;
        static RichTextBox output;
        static TextBox input;

        public static void startClient(String userName, RichTextBox output, TextBox input)
        {
            {
                Logic.userName = userName;
                Logic.output = output;
                Logic.input = input;
                client = new TcpClient();
                try
                {
                    client.Connect(host, port); //подключение клиента
                    stream = client.GetStream(); // получаем поток
                    byte[] data = Encoding.Unicode.GetBytes(userName);
                    stream.Write(data, 0, data.Length);
                    // запускаем новый поток для получения данных
                    Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                    receiveThread.Start(); //старт потока
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //Disconnect();
                }
            }
        }
        public static void SendMessage()
        {
           
            if (stream == null)
            {
                return;
            }
            while (true)
            {
                if (input.Text == ""){
                    return;
                }
                byte[] data = Encoding.Unicode.GetBytes(input.Text);
                input.Text = "";
                stream.Write(data, 0, data.Length);
            }
        }

        public static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    output.AppendText(message+"\n");//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        public static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }
    }
}
