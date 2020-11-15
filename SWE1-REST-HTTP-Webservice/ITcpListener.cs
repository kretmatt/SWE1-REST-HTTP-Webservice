using System;

namespace SWE1_REST_HTTP_Webservice
{
    public interface ITcpListener
    {
        void Start();
        void Stop();
        TcpClient AcceptTcpClient();
    }
}