using System;
using System.IO;
using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    public interface ITcpClient:IDisposable
    {
        Stream GetStream();
        void Close();
    }
}