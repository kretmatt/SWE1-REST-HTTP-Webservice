using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class MessageResourceEndpointHandlerTests
    {
        private RequestContext _requestContext;

        [SetUp]
        public void SetUp()
        {
            _requestContext = RequestContext.GetBaseRequest("GET /messages HTTP/1.1");
            _requestContext.AddHeader("Content-Type: text/plain");
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
            mockResourceEndpointHandler.Object.HandleRequest(_requestContext);
            //assert
            mockResourceEndpointHandler.Verify(resourceEndpointHandler=>resourceEndpointHandler.HandleRequest(_requestContext));
        }

        [Test]
        public void ListHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.ListHandler(_requestContext);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.ListHandler(_requestContext));
        }

        [Test]
        public void CreateHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.CreateHandler(_requestContext);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.CreateHandler(_requestContext));
        }

        [Test]
        public void ReadHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.ReadHandler(_requestContext);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.ReadHandler(_requestContext));
        }

        [Test]
        public void UpdateHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.UpdateHandler(_requestContext);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.UpdateHandler(_requestContext));
        }

        [Test]
        public void DeleteHandlerMock()
        {
            //arrange
            var crudHandlerMock = new Mock<ICRUDHandler>();
            //act
            crudHandlerMock.Object.DeleteHandler(_requestContext);
            //assert
            crudHandlerMock.Verify(crudHandler=>crudHandler.DeleteHandler(_requestContext));
        }
    }
}