using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        MessageResourceEndpointHandler - Implementation of an endpoint handler. Responsible for messages.
     */
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
            //RouteActions = Possible routes for this endpoint
            RouteActions=new List<RouteAction>()
            {
                new RouteAction(
                    ListHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.GET
                    ),
                new RouteAction(
                    CreateHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.POST
                ),
                new RouteAction(
                    ReadHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    EHTTPVerbs.GET
                ),
                new RouteAction(
                    UpdateHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    EHTTPVerbs.PUT
                ),
                new RouteAction(
                    DeleteHandler,
                    String.Format(@"^\{0}\/[0-9]+$",urlBase),
                    EHTTPVerbs.DELETE
                ),
            };
        }
        public bool CheckResponsibility(RequestContext requestContext)
        {
            return requestContext.URL.StartsWith(String.Format("{0}/",urlBase))||requestContext.URL==urlBase;
        }

        private RouteAction DetermineRouteAction(RequestContext requestContext)
        {
            RouteAction endpointAction=null;
            RouteActions.ForEach(ra =>
            {
                Regex re = new Regex(ra.PathRegex);
                
                if (re.IsMatch(requestContext.URL)&&ra.RequestType==requestContext.Type)
                    endpointAction = ra;
            });
            return endpointAction;
        }

        public ResponseContext HandleRequest(RequestContext requestContext)
        {
            RouteAction routeAction = DetermineRouteAction(requestContext);
            ResponseContext responseContext;
            if (routeAction != null)
                responseContext=routeAction.PathAction(requestContext);
            else
            {
                responseContext = ResponseContext.BadRequestResponse().SetContent("No fitting endpoint could be found!", "text/plain");
            }

            return responseContext;
        }

        public ResponseContext ListHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages List - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext=ResponseContext.OKResponse().SetContent("Currently no messages exist!","text/plain").SetContent(String.Concat(_messages),"text/plain"); //default content is "Currently no messages exist!". if messages exist, default content is overwritten
            return responseContext;
        }

        public ResponseContext CreateHandler(RequestContext requestContext)
        {
            Console.WriteLine("Messages Create - {0} {1}",requestContext.Type,requestContext.URL);
            ResponseContext responseContext;
            if (String.IsNullOrEmpty(requestContext.Body))
            {
                responseContext=ResponseContext.BadRequestResponse().SetContent("To create a message, some sort of text is needed!","text/plain");
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

            return responseContext;
        }
        
        public ResponseContext ReadHandler(RequestContext requestContext)
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
                    responseContext = ResponseContext.NotFoundResponse().SetContent("The requested message was not found! Try again with another id!","text/plain");
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse().SetContent("Id could not be converted!","text/plain");
            }
            return responseContext;
        }

        public ResponseContext UpdateHandler(RequestContext requestContext)
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
                    responseContext = ResponseContext.NotFoundResponse().SetContent("The requested message was not found! Try again with another id!","text/plain");
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse().SetContent("Id could not be converted!","text/plain");
            }

            return responseContext;
        }

        public ResponseContext DeleteHandler(RequestContext requestContext)
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
                    responseContext = ResponseContext.NotFoundResponse().SetContent("The requested message was not found! Try again with another id!","text/plain");
                }
            }
            else
            {
                responseContext = ResponseContext.BadRequestResponse().SetContent("Id could not be converted!","text/plain");
            }

            return responseContext;
        }
    }
}