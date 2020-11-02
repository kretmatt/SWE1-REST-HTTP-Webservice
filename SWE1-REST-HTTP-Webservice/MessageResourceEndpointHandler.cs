using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SWE1_REST_HTTP_Webservice
{
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
}