using System;
using System.Net.Sockets;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class RouteActionTests
    {
        private String sampleRegex = String.Format(@"^\{0}\/[0-9]+$", "/messages");
        private String sampleRequestType = "GET";
        private Action<RequestContext, NetworkStream> samplePathAction = (RequestContext requestContext, NetworkStream networkStream) => { };


        [Test]
        public void RouteActionConstructorTest()
        {
            //arrange
            RouteAction routeAction;
            //act
            routeAction=new RouteAction(samplePathAction,sampleRegex,sampleRequestType);
            //assert
            
            Assert.AreEqual(sampleRegex,routeAction.PathRegex);
            Assert.AreEqual(sampleRequestType,routeAction.RequestType);
            Assert.AreEqual(samplePathAction, routeAction.PathAction);
        }
    }
}