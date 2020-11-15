using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class TcpClientTests
    {
        private Stream mockStream;
        private byte[] streamContent = Encoding.UTF8.GetBytes(RequestContext.GetBaseRequest("GET /messages HTTP/1.1").ToString());
        [SetUp]
        public void SetUp()
        {
            mockStream = new MemoryStream(streamContent.Length);
            mockStream.Write(streamContent, 0, streamContent.Length);
        }

        [Test]
        public void DisposeMock()
        {
            //arrange
            var disposeMock = new Mock<ITcpClient>();
            //act
            disposeMock.Object.Dispose();
            //assert
            disposeMock.Verify(dm => dm.Dispose());
        }
        [Test]
        public void GetStreamMock()
        {
            //arrange
            var getStreamMock = new Mock<ITcpClient>();
            getStreamMock.Setup(tclient => tclient.GetStream()).Returns(mockStream);
            Stream stream;
            //act
            stream = getStreamMock.Object.GetStream();
            byte[] streamResult = ((MemoryStream) stream).GetBuffer();
            //assert
            getStreamMock.Verify(tclient=>tclient.GetStream());
            Assert.AreEqual(Encoding.UTF8.GetString(streamContent),Encoding.UTF8.GetString(streamResult));
        }
        [Test]
        public void CloseMock()
        {
            //arrange
            var closeMock = new Mock<ITcpClient>();
            //act
            closeMock.Object.Close();
            //assert
            closeMock.Verify(cm=>cm.Close());
        }
    }
}