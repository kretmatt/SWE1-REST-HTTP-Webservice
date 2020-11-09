using System.Net.Sockets;

namespace SWE1_REST_HTTP_Webservice
{
    public interface ICRUDHandler
    {
        void ListHandler(RequestContext requestContext,NetworkStream networkStream);
        void CreateHandler(RequestContext requestContext,NetworkStream networkStream);
        void ReadHandler(RequestContext requestContext,NetworkStream networkStream);
        void UpdateHandler(RequestContext requestContext,NetworkStream networkStream);
        void DeleteHandler(RequestContext requestContext,NetworkStream networkStream);
    }
}