using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    public interface IResourceEndpointHandler
    {
        bool CheckResponsibility(RequestContext requestContext);

        void HandleRequest(RequestContext requestContext, NetworkStream networkStream);

    }
}