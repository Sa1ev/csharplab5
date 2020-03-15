using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lab5server
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        public string userName;
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;

                message = userName + " вошел в чат. Ip:"+ getIp();
                // посылаем сообщение о входе в чат всем подключенным пользователям
                server.BroadcastMessage(message, this.Id);
                server.sendUserList();
               
                    Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        
                        if (message== "\\quit")
                        {
                            server.RemoveConnection(Id);
                            throw new Exception();
                        }
                        String[] splt = message.Split(ServerObject.splitChar);
                        if (splt[0]=="Общий") 
                        {
                            message = String.Format("{0} отправил: {1}", userName, splt[1]);
                            Console.WriteLine(message);
                            server.BroadcastMessage(message, this.Id);
                        }
                        else if (splt[0] != "")
                        {
                            message = String.Format("{0} отправил {1} личное сообщение: {2}", userName, splt[0], splt[1]);
                            Console.WriteLine(message);
                            server.PrivateMessage(splt[1], splt[0]);
                        }
                        
                        
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }

        public String getIp()
        {
            return ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        }
    }
}
