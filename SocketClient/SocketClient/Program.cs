using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class Program
    {

        //адрес и порт сервера к которым будем подлкючаться
        static int port = 8001;
        static string adress = "127.0.01";

        static void Main(string[] args)
        {

            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(adress), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //подключение к удаленному хосту
                socket.Connect(ipPoint);
                Console.WriteLine("введите сообщение:");
                string msg = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(msg);
                socket.Send(data);

                //получение ответа
                data = new byte[256];//буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                //закрытие сокета
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
