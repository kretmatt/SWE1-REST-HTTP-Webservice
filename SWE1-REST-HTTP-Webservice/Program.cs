using System.IO;
using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BaseHTTPServer server = new BaseHTTPServer(8080);
            server.Start();
        }
    }
}