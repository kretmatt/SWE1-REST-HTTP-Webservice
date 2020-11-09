using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SWE1_REST_HTTP_Webservice
{
    public class BaseHTTPServer : IHTTPServer

    {
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "FHTW SWE HTTP Server v0.1";

        private int port;
        private bool running = false;
        private TcpListener listener;

        private List<IResourceEndpointHandler> resourceEndpointHandlers;

        public BaseHTTPServer(int port)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, this.port);
            resourceEndpointHandlers = new List<IResourceEndpointHandler>();
            resourceEndpointHandlers.Add(new MessageResourceEndpointHandler());
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
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                Thread clientThread = new Thread(new ThreadStart(()=>HandleClient(client)));
                clientThread.Start();
            }

            running = false;
            listener.Stop();
        }


        private void HandleClient(TcpClient client)
        {
            String msg = "";
            RequestContext requestContext;
            NetworkStream networkStream = client.GetStream();
            bool requestHandled = false;
            using (StreamReader streamReader = new StreamReader(networkStream))
            {
                requestContext = RequestContext.GetBaseRequest(streamReader.ReadLine());
                while ((msg = streamReader.ReadLine()) != "")
                {
                    requestContext.AddHeader(msg);
                }

                msg = "";

                while (streamReader.Peek() != -1)
                {
                    msg += (char) streamReader.Read();
                }

                requestContext.Body = msg;
                Console.WriteLine(requestContext.ToString());

                /* Due to "using" the streamreader and the underlying stream (in this case networkstream of tcpclient) are closed. Afterwards you can't access the stream anymore */
                resourceEndpointHandlers.ForEach(reh =>
                {
                    if (reh.CheckResponsibility(requestContext))
                    {
                        reh.HandleRequest(requestContext, networkStream);
                        requestHandled = true;
                    }
                });
                if (requestHandled == false)
                {
                    using(StreamWriter streamWriter = new StreamWriter(networkStream))
                        streamWriter.Write(ResponseContext.BadRequestResponse().ToString());
                }
            }
            client.Close();
        }
    }
}