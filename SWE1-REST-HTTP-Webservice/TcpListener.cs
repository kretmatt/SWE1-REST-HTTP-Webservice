using System.Net;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        TcpListener - An implementation, which can only forward the methods that are used in the server.
     */
    public class TcpListener:ITcpListener
    {
        private readonly System.Net.Sockets.TcpListener _tcpListener;
        public void Start() => _tcpListener.Start();
        public void Stop() => _tcpListener.Stop();
        public ITcpClient AcceptTcpClient()=>new TcpClient(_tcpListener.AcceptTcpClient());
        public TcpListener(IPAddress ipAddress, int port)
        {
            _tcpListener=new System.Net.Sockets.TcpListener(ipAddress,port);
        }
    }
}