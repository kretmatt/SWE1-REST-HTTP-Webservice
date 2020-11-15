using System;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        ITcpListener - Used for mocking and used in the server instead of the "normal" TcpListener.
     */
    public interface ITcpListener
    {
        void Start();
        void Stop();
        ITcpClient AcceptTcpClient();
    }
}