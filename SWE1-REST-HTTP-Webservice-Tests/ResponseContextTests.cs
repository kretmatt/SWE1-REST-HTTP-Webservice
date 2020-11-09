using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class ResponseContextTests
    {

        private string headerKey = "Content-Type";
        private string headerValue = "application/json";
        private string content = "123";
        private string contentType = "text/plain";
        
        
        [Test]
        public void AddHeaderTest()
        {
            //arrange
            ResponseContext responseContext = ResponseContext.OKResponse();
            HttpHeaderPair httpHeaderPair = new HttpHeaderPair(headerKey,headerValue);
            //act
            responseContext = responseContext.AddHeader(httpHeaderPair);
            //assert
            Assert.AreEqual(headerKey,responseContext.HeaderPairs[2].HeaderKey);
            Assert.AreEqual(headerValue,responseContext.HeaderPairs[2].HeaderValue);
        }

        [Test]
        public void SetContent()
        {
            //arrange + act
            ResponseContext responseContext = ResponseContext.OKResponse().SetContent(content,contentType);
            //assert
            Assert.AreEqual(contentType,responseContext.HeaderPairs[3].HeaderValue);
            Assert.AreEqual(content.Length.ToString(), responseContext.HeaderPairs[2].HeaderValue);
        }
    }
}