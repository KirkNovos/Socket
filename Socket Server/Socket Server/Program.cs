using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Socket_Server
{
    class Program
    {
        static int port = 8001;// порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получение адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            //создание сокета
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связь сокета с локальной точкой, по которой будут принимать данные
                listenSocket.Bind(ipPoint);

                // начало прослушивания
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключения");

                while (true)
                {
                    Socket arrived = listenSocket.Accept();
                    //получение сообщения
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;//кол-во полученных байтов
                    byte[] data = new byte[256];//буфер получаемых данных

                    do
                    {
                        bytes = arrived.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (arrived.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    //отправка ответа
                    string msg = "сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(msg);
                    arrived.Send(data);
                    arrived.Shutdown(SocketShutdown.Both);
                    arrived.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
