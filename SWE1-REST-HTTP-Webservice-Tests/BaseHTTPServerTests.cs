using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;
using TcpClient = SWE1_REST_HTTP_Webservice.TcpClient;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class BaseHTTPServerTests
    {
        [Test]
        public void StartMock()
        {
            //arrange
            var baseServerMock = new Mock<IHTTPServer>();
            //act
            baseServerMock.Object.Start();
            //assert
            baseServerMock.Verify(baseServer => baseServer.Start());
        }

        [Test]
        public void HandleClientMock()
        {
            //arrange
            var mockServer = new Mock<BaseHTTPServer>(8080);
            var clientMock = new Mock<ITcpClient>();
            //act
            mockServer.Object.HandleClient(clientMock.Object);
            //assert
            clientMock.Verify(cm=>cm.Close());
            clientMock.Verify(cm=>cm.GetStream());
        }
    }
}