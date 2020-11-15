namespace SWE1_REST_HTTP_Webservice
{
    /*
        IHTTPServer - Defines which methods need to be implemented by HTTP server.
     */
    public interface IHTTPServer
    {
        void Start();

        void HandleClient(ITcpClient tcpClient);
    }
}