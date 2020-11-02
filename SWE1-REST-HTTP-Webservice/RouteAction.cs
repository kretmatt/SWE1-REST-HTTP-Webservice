using System;

namespace SWE1_REST_HTTP_Webservice
{
    public class RouteAction
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
}