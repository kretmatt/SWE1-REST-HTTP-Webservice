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

    public class BaseHTTPServer
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
            resourceEndpointHandlers=new List<IResourceEndpointHandler>();
            resourceEndpointHandlers.Add(new MessageResourceEndpointHandler());
        }

        public void Start()
        {
            Console.WriteLine("Starting server on port {0}",port);
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
                HandleClient(client);
                client.Close();
            }

            running = false;
            listener.Stop();
        }


        private void HandleClient(TcpClient client)
        {
            String msg = "";
            RequestContext requestContext;
            using (StreamReader streamReader = new StreamReader(client.GetStream()))
            {
                requestContext = RequestContext.GetBaseRequest(streamReader.ReadLine());
                while ((msg=streamReader.ReadLine())!="")
                {   
                    requestContext.AddHeader(msg);
                }

                msg = "";

                while (streamReader.Peek() != -1)
                {
                    msg += (char) streamReader.Read();
                }

                requestContext.Body = msg;
            }
            Console.WriteLine(requestContext.ToString());
            
            resourceEndpointHandlers.ForEach(reh =>
            {
                if(reh.CheckResponsibility(requestContext))
                    reh.HandleRequest(requestContext);
            });
        }
    }

    public class RequestContext
    {
        public String Type { get; set; }
        public String URL { get; set; }
        public String HTTPVersion { get; set; }
        public List<HttpHeaderPair> HeaderPairs { get; private set; }
        public String Body { get; set; }
        
        private RequestContext(String type,String url,String httpVersion)
        {
            Type = type;
            URL = url;
            HTTPVersion = httpVersion;
            HeaderPairs = new List<HttpHeaderPair>();
        }

        public void AddHeader(String headerLine)
        {
            String[] headerPairParts = headerLine.Split(' ');
            HeaderPairs.Add(new HttpHeaderPair(headerPairParts[0].Trim(':'),headerPairParts[1]));
        }
        
        public static RequestContext GetBaseRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
                return null;

            String[] tokens = request.Split(' ');
            String type = tokens[0];
            String url = tokens[1];
            String httpVersion = tokens[2];
            
            return new RequestContext(type,url,httpVersion);
        }

        public override string ToString()
        {
            string requestString = String.Format("{0} {1} {2}\n",Type,URL,HTTPVersion);
            HeaderPairs.ForEach(hp=> requestString+=String.Format("{0}\n",hp.ToString()));
            return String.Format("{0}\n{1}\n",requestString,Body);
        }
    }

    public class HttpHeaderPair
    {
        public String HeaderKey { get; set; }
        public String HeaderValue { get; set; }


        public HttpHeaderPair(String headerKey,String headerValue)
        {
            HeaderKey = headerKey;
            HeaderValue = headerValue;
        }
        
        public override string ToString()
        {
            return String.Format("{0}: {1}", HeaderKey, HeaderValue);
        }
    }

    interface IResourceEndpointHandler
    {
        bool CheckResponsibility(RequestContext requestContext);

        void HandleRequest(RequestContext requestContext);

    }

    interface ICRUDHandler
    {
        void ListHandler(RequestContext requestContext);
        void CreateHandler(RequestContext requestContext);
        void ReadHandler(RequestContext requestContext);
        void UpdateHandler(RequestContext requestContext);
        void DeleteHandler(RequestContext requestContext);
    }
    public class MessageResourceEndpointHandler : IResourceEndpointHandler, ICRUDHandler
    {
        private List<Message> _messages;
        private const String urlBase = "/messages";
        private List<RouteAction> RouteActions;
        public MessageResourceEndpointHandler()
        {
            _messages=new List<Message>();
            RouteActions=new List<RouteAction>()
            {
                new RouteAction(
                    ListHandler,
                    String.Format(@"^\{0}$",urlBase),
                    "GET"
                    ),
                new RouteAction(
                    CreateHandler,
                    String.Format(@"^\{0}$",urlBase),
                    "POST"
                ),
                new RouteAction(
                    ReadHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    "GET"
                ),
                new RouteAction(
                    UpdateHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    "PUT"
                ),
                new RouteAction(
                    DeleteHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    "DELETE"
                ),
            };
        }
        public bool CheckResponsibility(RequestContext requestContext)
        {
            bool responsible = false;
            
            RouteActions.ForEach(ra =>
            {
                Regex re = new Regex(ra.PathRegex);
                if (re.IsMatch(requestContext.URL)&&requestContext.Type==ra.RequestType)
                    responsible = true;
            });
            return responsible;
        }

        private RouteAction DetermineRouteAction(RequestContext requestContext)
        {
            RouteAction endpointAction=null;
            RouteActions.ForEach(ra =>
            {
                Regex re = new Regex(ra.PathRegex);
                if (re.IsMatch(requestContext.URL)&&requestContext.Type==ra.RequestType)
                    endpointAction = ra;
            });
            return endpointAction;
        }

        public void HandleRequest(RequestContext requestContext)
        {
            RouteAction routeAction = DetermineRouteAction(requestContext);
            if (routeAction != null)
                routeAction.PathAction(requestContext);
            else
            {
                Console.WriteLine("sdfdsfdsf");
            }
        }

        public void ListHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages List");
            Console.WriteLine(requestContext.Type);
            Console.WriteLine(requestContext.URL);
        }

        public void CreateHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages Create");
            Console.WriteLine(requestContext.Type);
            Console.WriteLine(requestContext.URL);
        }
        
        public void ReadHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages Read");
            Console.WriteLine(requestContext.Type);
            Console.WriteLine(requestContext.URL);
        }

        public void UpdateHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages Update");
            Console.WriteLine(requestContext.Type);
            Console.WriteLine(requestContext.URL);
        }

        public void DeleteHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages Delete");
            Console.WriteLine(requestContext.Type);
            Console.WriteLine(requestContext.URL);
        }
        
    }
    class RouteAction
    {
        public RouteAction(Action<RequestContext> pathAction, String pathRegex,String requestType)
        {
            PathAction = pathAction;
            PathRegex = pathRegex;
            RequestType = requestType;
        }
        
        public String PathRegex { get; set; }
        public Action<RequestContext> PathAction{ get; set; }
        public String RequestType { get; set; }
    }
    class Message
    {
        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime SentDate { get; set; }
    }
    /*class ResponseContext
    {
        private Byte[] data = null;
        private int statusCode;
        private String statusMessage;
        private String mime;
        
        private ResponseContext(Byte[] data,String mime,String status)
        {
            this.data = this.data;
            this.mime = mime;
            this.statusCode = statusCode;
            this.statusMessage
        }

        public static ResponseContext From(RequestContext requestContext)
        {
            if(requestContext==null)
                MakeNullRequest();

            if (requestContext.Type == "GET")
            {
                
            }

        }

        private static ResponseContext MakeNullRequest()
        {
            return new ResponseContext(new Byte[0], "text/plain","400 Bad Request");
        }

        public void Post(NetworkStream stream)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",BaseHTTPServer.VERSION,FtpStatusCode.ToString(),BaseHTTPServer.NAME,mime,data.Length));
                
                
                stream.Write(data,0,data.Length);
            }
        }
    }*/
}