using System.IO;
using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        TcpClient - An implementation, which forwards methods used by the server.
     */
    public class TcpClient:ITcpClient
    {
        private readonly System.Net.Sockets.TcpClient _client;
        public Stream GetStream() => _client.GetStream();

        public void Dispose() => _client.Dispose();
        public void Close() => _client.Close();

        public TcpClient(System.Net.Sockets.TcpClient tcpClient)
        {
            _client = tcpClient;
        }

        public TcpClient()
        {
            _client=new System.Net.Sockets.TcpClient();
        }
    }
}