using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        ICRUDHandler - Defines methods for resource endpoint handlers.
        C -> Create
        R -> Read
        U -> Update
        D -> Delete
        In addition, I added a List method.
     */
    public interface ICRUDHandler
    {
        ResponseContext ListHandler(RequestContext requestContext);
        ResponseContext CreateHandler(RequestContext requestContext);
        ResponseContext ReadHandler(RequestContext requestContext);
        ResponseContext UpdateHandler(RequestContext requestContext);
        ResponseContext DeleteHandler(RequestContext requestContext);
    }
}