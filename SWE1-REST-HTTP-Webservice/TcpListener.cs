using System.Net;

namespace SWE1_REST_HTTP_Webservice
{
    public class TcpListener:ITcpListener
    {
        private readonly System.Net.Sockets.TcpListener _tcpListener;
        public void Start() => _tcpListener.Start();
        public void Stop() => _tcpListener.Stop();
        public TcpClient AcceptTcpClient()=>new TcpClient(_tcpListener.AcceptTcpClient());
        public TcpListener(IPAddress ipAddress, int port)
        {
            _tcpListener=new System.Net.Sockets.TcpListener(ipAddress,port);
        }
    }
}