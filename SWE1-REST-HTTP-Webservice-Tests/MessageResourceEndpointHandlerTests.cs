using System.Net.Sockets;
using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class MessageResourceEndpointHandlerTests
    {
        private RequestContext _requestContext;
        private NetworkStream _networkStream;

        [SetUp]
        public void SetUp()
        {
            _requestContext = RequestContext.GetBaseRequest("GET /messages HTTP/1.1");
            _requestContext.AddHeader("Content-Type: text/plain");
            _networkStream=null;
        }

        [Test]
        public void CheckResponsibility()
        {
            //arrange
            MessageResourceEndpointHandler messageResourceEndpointHandler = new MessageResourceEndpointHandler();
            bool responsible;
            //act
            responsible = messageResourceEndpointHandler.CheckResponsibility(_requestContext);
            //assert
            Assert.IsTrue(responsible);
        }

        [Test]
        public void HandleRequestMock()
        {   //arrange
            var mockResourceEndpointHandler = new Mock<IResourceEndpointHandler>();
            //act
            mockResourceEndpointHandler.Object.HandleRequest(_requestContext,_networkStream);
            //assert
            mockResourceEndpointHandler.Verify(resourceEndpointHandler=>resourceEndpointHandler.HandleRequest(_requestContext,_networkStream));
        }

        [Test]
        public void ListHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.ListHandler(_requestContext, _networkStream);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.ListHandler(_requestContext, _networkStream));
        }

        [Test]
        public void CreateHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.CreateHandler(_requestContext, _networkStream);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.CreateHandler(_requestContext, _networkStream));
        }

        [Test]
        public void ReadHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.ReadHandler(_requestContext, _networkStream);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.ReadHandler(_requestContext, _networkStream));
        }

        [Test]
        public void UpdateHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.UpdateHandler(_requestContext,_networkStream);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.UpdateHandler(_requestContext, _networkStream));
        }

        [Test]
        public void DeleteHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.DeleteHandler(_requestContext, _networkStream);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.DeleteHandler(_requestContext, _networkStream));
        }
    }
}