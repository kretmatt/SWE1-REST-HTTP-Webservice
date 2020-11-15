using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    public interface ICRUDHandler
    {
        ResponseContext ListHandler(RequestContext requestContext);
        ResponseContext CreateHandler(RequestContext requestContext);
        ResponseContext ReadHandler(RequestContext requestContext);
        ResponseContext UpdateHandler(RequestContext requestContext);
        ResponseContext DeleteHandler(RequestContext requestContext);
    }
}