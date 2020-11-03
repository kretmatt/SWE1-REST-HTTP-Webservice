using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Threading;

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