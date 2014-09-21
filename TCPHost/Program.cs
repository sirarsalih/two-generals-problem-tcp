using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPHost
{
    public class Program
    {
        private static NetworkStream _networkStream;
        private static StreamWriter _streamWriter;
        private static string _message;
        private static readonly DateTime TimeOfAttack = DateTime.Now;

        public static void Main(string[] args)
        {
            try
            {
                StartTcpListenerAndWaitForConnection();
                _message = "Attack at ->" + TimeOfAttack;
                while (true)
                {
                    SendMessage(_message);
                    ReceiveMessage();
                    var timeOfAttack = _message.Split('>')[1];
                    _message = "I received your confirmation of the planned attack at ->" + timeOfAttack;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void StartTcpListenerAndWaitForConnection()
        {
            var tcpListener = new TcpListener(IPAddress.Any, 7845);
            tcpListener.Start();
            Console.WriteLine("TCP listener started on port 7845. Waiting for client.");
            var tcpClient = tcpListener.AcceptTcpClient();
            Console.WriteLine("Connected to client.");
            _networkStream = tcpClient.GetStream();
            _streamWriter = new StreamWriter(_networkStream);
        }

        private static void SendMessage(string message)
        {
            _streamWriter.WriteLine(message);
            _streamWriter.Flush();
        }

        private static void ReceiveMessage()
        {
            var data = new Byte[256];
            var bytes = _networkStream.Read(data, 0, data.Length);
            var responseData = Encoding.ASCII.GetString(data, 0, bytes);
            _message = responseData;
            Console.WriteLine(_message);
        }
    }
}
