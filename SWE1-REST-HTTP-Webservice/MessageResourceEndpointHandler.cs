using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace SWE1_REST_HTTP_Webservice
{
    public class MessageResourceEndpointHandler : IResourceEndpointHandler, ICRUDHandler
    {
        private List<Message> _messages;
        private const String urlBase = "/messages";
        private List<RouteAction> RouteActions;
        private int nextId;
        public MessageResourceEndpointHandler()
        {
            nextId = 1;
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

        public void HandleRequest(RequestContext requestContext, NetworkStream networkStream)
        {
            RouteAction routeAction = DetermineRouteAction(requestContext);
            if (routeAction != null)
                routeAction.PathAction(requestContext, networkStream);
            else
            {
                Console.WriteLine("sdfdsfdsf");
            }
        }

        public void ListHandler(RequestContext requestContext,NetworkStream networkStream)
        {
            Console.WriteLine("Messages List - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext=ResponseContext.OKResponse().SetContent(String.Concat(_messages),"text/plain");
            SendResponse(responseContext,networkStream);
        }

        public void CreateHandler(RequestContext requestContext,NetworkStream networkStream)
        {
            Console.WriteLine("Messages Create - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext;
            if (String.IsNullOrEmpty(requestContext.Body))
            {
                responseContext=ResponseContext.BadRequestResponse();
            }
            else
            {
                Message message = new Message();
                message.Id = nextId;
                message.Content = requestContext.Body;
                message.SentDate=DateTime.Now;
                _messages.Add(message);
                nextId++;
                responseContext=ResponseContext.CreatedResponse().SetContent(
                    message.Id.ToString(),
                    "text/plain"
                    );
            }
            SendResponse(responseContext,networkStream);
        }
        
        public void ReadHandler(RequestContext requestContext,NetworkStream networkStream)
        {
            Console.WriteLine("Messages Read - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext;
            String[] urlParts = requestContext.URL.Split('/');
            int requestedResourceId;
            if (int.TryParse(urlParts[2], out requestedResourceId))
            {
                Message message = _messages.SingleOrDefault(m => m.Id == requestedResourceId);
                if (message!=null)
                {
                    responseContext = ResponseContext.OKResponse().SetContent(message.ToString(), "text/plain");
                }
                else
                {
                    responseContext = ResponseContext.NotFoundResponse();
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse();
            }
            SendResponse(responseContext,networkStream);
        }

        public void UpdateHandler(RequestContext requestContext,NetworkStream networkStream)
        {
            Console.WriteLine("Messages Update - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext;
            String[] urlParts = requestContext.URL.Split('/');
            int requestedResourceId;
            if (int.TryParse(urlParts[2], out requestedResourceId))
            {
                Message message = _messages.SingleOrDefault(m => m.Id == requestedResourceId);
                if (message!=null)
                {
                    message.Content = requestContext.Body;
                    responseContext = ResponseContext.OKResponse().SetContent(message.Id.ToString(), "text/plain");
                }
                else
                {
                    responseContext = ResponseContext.NotFoundResponse();
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse();
            }
            SendResponse(responseContext,networkStream);
        }

        public void DeleteHandler(RequestContext requestContext,NetworkStream networkStream)
        {
            Console.WriteLine("Messages Delete - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext;
            String[] urlParts = requestContext.URL.Split('/');
            int requestedResourceId;
            if (int.TryParse(urlParts[2], out requestedResourceId))
            {
                Message message = _messages.SingleOrDefault(m => m.Id == requestedResourceId);
                if (message!=null)
                {
                    _messages.Remove(message);
                    responseContext = ResponseContext.OKResponse().SetContent(message.Id.ToString(), "text/plain");
                }
                else
                {
                    responseContext = ResponseContext.NotFoundResponse();
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse();
            }
            SendResponse(responseContext,networkStream);
        }

        private void SendResponse(ResponseContext responseContext, NetworkStream networkStream)
        {
            using(StreamWriter streamWriter= new StreamWriter(networkStream))
                streamWriter.Write(responseContext.ToString());
        }
        
    }
}