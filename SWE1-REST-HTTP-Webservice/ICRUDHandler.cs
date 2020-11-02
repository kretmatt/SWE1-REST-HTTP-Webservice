namespace SWE1_REST_HTTP_Webservice
{
    public interface ICRUDHandler
    {
        void ListHandler(RequestContext requestContext);
        void CreateHandler(RequestContext requestContext);
        void ReadHandler(RequestContext requestContext);
        void UpdateHandler(RequestContext requestContext);
        void DeleteHandler(RequestContext requestContext);
    }
}