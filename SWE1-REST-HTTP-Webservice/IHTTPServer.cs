namespace SWE1_REST_HTTP_Webservice
{
    public interface IHTTPServer
    {
        void Start();

        void HandleClient(ITcpClient tcpClient);
    }
}