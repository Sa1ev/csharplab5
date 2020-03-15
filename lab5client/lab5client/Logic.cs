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
        static Form1 form;
        public static Char splitChar = '人';
        public static void startClient(String userName, Form1 form)
        {
            {
                Logic.userName = userName;
                Logic.form = form;
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
                    form.richAddText("Вы подключены к серверу\n");
                    form.setupWindow();
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

                if (client.Connected & form.getMessage() != "")
                {
                    byte[] data = Encoding.Unicode.GetBytes(form.getComboValue()+ splitChar + form.getMessage());
                    Console.WriteLine(form.getComboValue());
                    form.clearMessage();
                    stream.Write(data, 0, data.Length);
                }
                else if (form.getMessage() == "")
                {
                    return;
                }
            

               
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
                    if (isMessageConnectionList(message))
                    {
                        form.fillCombo(message);
                    }
                    else
                    {
                        form.richAddText(message + "\n");
                    }
                    
                    
                    
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                    return;
                }
            }
        }

        public static void Disconnect()
        {

            if (stream != null)
            {
                if (client.Connected){
                    byte[] data = Encoding.Unicode.GetBytes("\\disconnect");
                    stream.Write(data, 0, data.Length);
                }
                
                stream.Close();//отключение потока
            }
               
            if (client != null)
                client.Close();//отключение клиента
        }

        private static bool isMessageConnectionList(String text)
        {
            bool d= text.Substring(0, 9) == "userlist"+ splitChar;
            return d;
        }
    }
}
