using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class RequestContextUnitTests
    {
        private String sampleHttpHeaderKey = "Content-Type";
        private String sampleHttpHeaderValue = "text/plain";
        private String sampleMethod = "GET";
        private String sampleURL = "/messages";
        private String sampleHttpVersion = "HTTP/1.1";
        
        [Test]
        public void AddHeaderTest()
        {
            //arrange
            RequestContext requestContext;
            //act
            requestContext = RequestContext.GetBaseRequest(String.Format("{0} {1} {2}", sampleMethod,sampleURL,sampleHttpVersion));
            requestContext.AddHeader(String.Format("{0}: {1}",sampleHttpHeaderKey,sampleHttpHeaderValue));
            //assert
            Assert.AreEqual(1,requestContext.HeaderPairs.Count);
            Assert.AreEqual(sampleHttpHeaderKey,requestContext.HeaderPairs.First().HeaderKey);
            Assert.AreEqual(sampleHttpHeaderValue, requestContext.HeaderPairs.First().HeaderValue);
        }

        [Test]
        public void GetBaseRequestTest()
        {
            //arrange
            RequestContext requestContext;
            //act
            requestContext= RequestContext.GetBaseRequest(String.Format("{0} {1} {2}", sampleMethod,sampleURL,sampleHttpVersion));
            //assert
            Assert.AreEqual(sampleURL, requestContext.URL);
            Assert.AreEqual(sampleMethod, requestContext.Type);
            Assert.AreEqual(sampleHttpVersion, requestContext.HTTPVersion);
        }
    }
}