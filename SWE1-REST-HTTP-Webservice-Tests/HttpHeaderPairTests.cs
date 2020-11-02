using System;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class HttpHeaderPairTests
    {

        private String sampleHttpHeaderKey = "Content-Type";
        private String sampleHttpHeaderValue = "text/plain";
        [Test]
        public void ConstructorTest()
        {
            //arrange
            HttpHeaderPair httpHeaderPair;
            //assert
            httpHeaderPair=new HttpHeaderPair(sampleHttpHeaderKey,sampleHttpHeaderValue);
            //act
            Assert.AreEqual(sampleHttpHeaderKey,httpHeaderPair.HeaderKey);
            Assert.AreEqual(sampleHttpHeaderValue, httpHeaderPair.HeaderValue);
        }
    }
}