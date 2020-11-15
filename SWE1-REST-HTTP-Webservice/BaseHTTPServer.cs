using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SWE1_REST_HTTP_Webservice
{
    
    /*
        BaseHTTPServer - Concrete implementation of IHTTPServer.
     */
    public class BaseHTTPServer : IHTTPServer

    {
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "FHTW SWE HTTP Server v1.0";

        private int port;
        private bool running = false;
        private ITcpListener listener;

        public List<IResourceEndpointHandler> ResourceEndpointHandlers { get; set; }

        public BaseHTTPServer(int port)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, this.port);
            ResourceEndpointHandlers = new List<IResourceEndpointHandler>();
            ResourceEndpointHandlers.Add(new MessageResourceEndpointHandler());
        }

        public void Start()
        {
            Console.WriteLine("Starting server on port {0}", port);
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();
            while (running)
            {
                Console.WriteLine("Waiting for connection ...");
                ITcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                Thread clientThread = new Thread(new ThreadStart(()=>HandleClient(client)));
                clientThread.Start();
            }

            running = false;
            listener.Stop();
        }


        public void HandleClient(ITcpClient client)
        {
            String msg = "";
            RequestContext requestContext;
            NetworkStream networkStream = (NetworkStream) client.GetStream();
            if (networkStream != null)
            {
                
                bool requestHandled = false;
                ResponseContext responseContext=ResponseContext.BadRequestResponse();
                responseContext.SetContent("The requested resource was not found!","text/plain");
                using (StreamReader streamReader = new StreamReader(networkStream))
                {
                    //Get first request line
                    requestContext = RequestContext.GetBaseRequest(streamReader.ReadLine());
                    //Get headers
                    while ((msg = streamReader.ReadLine()) != "")
                    {
                        requestContext.AddHeader(msg);
                    }

                    msg = "";
                    //Get body
                    while (streamReader.Peek() != -1)
                    {
                        msg += (char) streamReader.Read();
                    }

                    requestContext.Body = msg;
                    
                    Console.WriteLine("\n<-- REQUEST -->\n");
                    
                    Console.WriteLine(requestContext.ToString());

                    ResourceEndpointHandlers.ForEach(reh =>
                    {
                        if (reh.CheckResponsibility(requestContext))
                        {
                            responseContext=reh.HandleRequest(requestContext);
                        }
                    });
                    /* Due to "using" the streamreader and the underlying stream (in this case networkstream of tcpclient) are closed. Afterwards you can't access the stream anymore. That's why there is a using-Statement in another using-Statement */

                    using (StreamWriter streamWriter = new StreamWriter(client.GetStream()))
                        streamWriter.Write(responseContext.ToString());
                    
                    Console.WriteLine("\n<-- RESPONSE -->\n");
                    Console.WriteLine(responseContext.ToString());
                }
            }
            client.Close();
        }
    }
}