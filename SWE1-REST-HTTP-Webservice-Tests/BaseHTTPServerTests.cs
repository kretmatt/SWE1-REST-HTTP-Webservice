using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class BaseHTTPServerTests
    {

        [Test]
        public void StartTest()
        {
            //arrange
            var baseServerMock = new Mock<IHTTPServer>();
            //act
            baseServerMock.Object.Start();
            //assert
            baseServerMock.Verify(baseServer => baseServer.Start());
        }
    }
}