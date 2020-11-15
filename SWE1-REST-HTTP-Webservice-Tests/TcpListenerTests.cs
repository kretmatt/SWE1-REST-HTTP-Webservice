using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class TcpListenerTests
    {
        private TcpClient _tcpClient;

        [SetUp]
        public void SetUp()
        {
            _tcpClient=new TcpClient();
        }
        
        [Test]
        public void StartTest()
        {
            //arrange
            var startMock = new Mock<ITcpListener>();
            //act
            startMock.Object.Start();
            //assert
            startMock.Verify(sm => sm.Start());
        }

        [Test]
        public void StopTest()
        {
            //arrange
            var stopMock = new Mock<ITcpListener>();
            //act
            stopMock.Object.Stop();
            //assert
            stopMock.Verify(sm => sm.Stop());
        }

        [Test]
        public void AcceptTcpClientTest()
        {
            //arrange
            var acceptTcpClientMock = new Mock<ITcpListener>();
            acceptTcpClientMock.Setup(tcplistener => tcplistener.AcceptTcpClient()).Returns(_tcpClient);
            TcpClient tcpClient;
            //act
            tcpClient = acceptTcpClientMock.Object.AcceptTcpClient();
            //assert
            acceptTcpClientMock.Verify(atcm=>atcm.AcceptTcpClient());
            Assert.AreEqual(_tcpClient,tcpClient);
        }
    }
}