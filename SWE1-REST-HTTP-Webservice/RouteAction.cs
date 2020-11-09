using System;
using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    public class RouteAction
    {
        public RouteAction(Action<RequestContext, NetworkStream> pathAction, String pathRegex,String requestType)
        {
            PathAction = pathAction;
            PathRegex = pathRegex;
            RequestType = requestType;
        }
        
        public String PathRegex { get; set; }
        public Action<RequestContext, NetworkStream> PathAction{ get; set; }
        public String RequestType { get; set; }
    }
}