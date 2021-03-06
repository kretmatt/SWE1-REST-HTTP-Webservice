using System;
using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    
    /*
        RouteAction - Basically a route. Consists of a regex, a func (RequestContext parameter and returns ResponseContext) and a HTTPVerb.
        The regex and the HTTP verb are used to determine, whether a HTTP request is for this route. If so, the func is executed.
     */
    public class RouteAction
    {
        public RouteAction(Func<RequestContext,ResponseContext> pathAction, String pathRegex,EHTTPVerbs requestType)
        {
            PathAction = pathAction;
            PathRegex = pathRegex;
            RequestType = requestType;
        }
        
        public String PathRegex { get; set; }
        public Func<RequestContext,ResponseContext> PathAction{ get; set; }
        public EHTTPVerbs RequestType { get; set; }
    }
}