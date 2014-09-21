using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    public class Program
    {
        private static NetworkStream _networkStream;
        private static StreamWriter _streamWriter;
        private static string _message;

        static void Main(string[] args)
        {
            try
            {
                StartTcpClient();
                while (true)
                {
                    ReceiveMessage();
                    var timeOfAttack = _message.Split('>')[1];
                    _message = "I received your message and will attack at ->" + timeOfAttack;
                    SendMessage(_message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void StartTcpClient()
        {
            var tcpClient = new TcpClient("127.0.0.1", 7845);
            _networkStream = tcpClient.GetStream();
            _streamWriter = new StreamWriter(_networkStream);
        }

        private static void ReceiveMessage()
        {
            var data = new Byte[256];
            var bytes = _networkStream.Read(data, 0, data.Length);
            var responseData = Encoding.ASCII.GetString(data, 0, bytes);
            var message = responseData.Replace("\r", string.Empty).Replace("\n", string.Empty);
            _message = message;
            Console.WriteLine(_message);
        }

        private static void SendMessage(string message)
        {
            _streamWriter.Write(message);
            _streamWriter.Flush();
        }
    }
}
