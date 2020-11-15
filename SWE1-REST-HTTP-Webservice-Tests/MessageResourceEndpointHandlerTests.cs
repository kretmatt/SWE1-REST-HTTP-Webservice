using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Moq;
using NUnit.Framework;
using SWE1_REST_HTTP_Webservice;

namespace SWE1_REST_HTTP_Webservice_Tests
{
    [TestFixture]
    public class MessageResourceEndpointHandlerTests
    {
        private RequestContext _badRequestRequestContext;
        private MessageResourceEndpointHandler _messageResourceEndpointHandler;
        private string sampleCreateBody = "Hallo das ist ein Test!";
        private static IEnumerable<TestCaseData> ReadHandlerFailureTests
        {
            get
            {
                yield return new TestCaseData(RequestContext.GetBaseRequest("GET /messages/3 HTTP/1.1"),ResponseContext.NotFoundResponse()); //resource does not exist
                yield return new TestCaseData(RequestContext.GetBaseRequest("GET /messages/hallo HTTP/1.1"), ResponseContext.BadRequestResponse()); //not possible because id needs to be a numeric value
                yield return new TestCaseData(RequestContext.GetBaseRequest("PUT /messages/3 HTTP/1.1"),ResponseContext.NotFoundResponse());//resource not found
                yield return new TestCaseData(RequestContext.GetBaseRequest("PUT /messages/hallo HTTP/1.1"), ResponseContext.BadRequestResponse());//not possible because id needs to be a numeric value
                yield return new TestCaseData(RequestContext.GetBaseRequest("DELETE /messages/3 HTTP/1.1"),ResponseContext.NotFoundResponse());//resource not found
                yield return new TestCaseData(RequestContext.GetBaseRequest("DELETE /messages/hallo HTTP/1.1"), ResponseContext.BadRequestResponse());//not possible because id needs to be a numeric value
                yield return new TestCaseData(RequestContext.GetBaseRequest("POST /messages HTTP/1.1"), ResponseContext.BadRequestResponse());//No content for creating a message
            }
        }
        
        [SetUp]
        public void SetUp()
        {
            _badRequestRequestContext = RequestContext.GetBaseRequest("DELETE /messages HTTP/1.1");
            _badRequestRequestContext.AddHeader("Content-Type: text/plain");
            _messageResourceEndpointHandler=new MessageResourceEndpointHandler();
            RequestContext createMessageRequest = RequestContext.GetBaseRequest("POST /messages HTTP/1.1");
            createMessageRequest.Body = sampleCreateBody;
            _messageResourceEndpointHandler.HandleRequest(createMessageRequest);
        }

        [Test]
        public void CheckResponsibility()
        {
            //arrange
            bool responsible;
            //act
            responsible = _messageResourceEndpointHandler.CheckResponsibility(_badRequestRequestContext);
            //assert
            Assert.IsTrue(responsible);
        }

        [Test]
        public void HandleRequestBadRequestResponseTest()
        {
            //arrange
            ResponseContext badRequestResponse;
            //act
            badRequestResponse = _messageResourceEndpointHandler.HandleRequest(_badRequestRequestContext);
            //assert -> I need to compare the values like this, because every response has a datetime string in headerpairs-List. Although the strings are equal at first glance, the assert will fail because of it. 
            Assert.AreEqual(ResponseContext.BadRequestResponse().HTTPVersion, badRequestResponse.HTTPVersion);
            Assert.AreEqual(ResponseContext.BadRequestResponse().HeaderPairs.Count+2, badRequestResponse.HeaderPairs.Count);//+2 due to default message for Bad Request
            Assert.AreEqual(ResponseContext.BadRequestResponse().StatusMessage, badRequestResponse.StatusMessage);
            Assert.AreEqual(ResponseContext.BadRequestResponse().StatusCode, badRequestResponse.StatusCode);
            Assert.AreEqual("No fitting endpoint could be found!", badRequestResponse.Content);
        }

        [Test]
        public void ListHandlerTest()
        {
            //arrange
            ResponseContext responseContext;
            RequestContext requestContext = RequestContext.GetBaseRequest("GET /messages HTTP/1.1");
            //act
            responseContext = _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.AreEqual(ResponseContext.OKResponse().StatusCode, responseContext.StatusCode);
            Assert.AreEqual(ResponseContext.OKResponse().StatusMessage, responseContext.StatusMessage);
        }

        [Test]
        [TestCaseSource(nameof(ReadHandlerFailureTests))]
        public void HandlerFailureTest(RequestContext requestContext, ResponseContext expectedResponse)
        {
            //arrange
            ResponseContext responseContext;
            //act
            responseContext = _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.StatusMessage, responseContext.StatusMessage);

        }

        [Test]
        public void CreateHandlerTest()
        {
            //arrange
            RequestContext requestContext = RequestContext.GetBaseRequest("POST /messages HTTP/1.1");
            requestContext.Body = "Sample Message!";
            ResponseContext responseContext;
            //act
            responseContext = _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.AreEqual(ResponseContext.CreatedResponse().StatusCode, responseContext.StatusCode);
            Assert.AreEqual(ResponseContext.CreatedResponse().StatusMessage, responseContext.StatusMessage);
            Assert.AreEqual("2",responseContext.Content );
        }

        [Test]
        public void ReadHandlerTest()
        {
            //arrange
            ResponseContext responseContext;
            RequestContext requestContext = RequestContext.GetBaseRequest("GET /messages/1 HTTP/1.1");
            //act
            responseContext= _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.IsNotEmpty(responseContext.Content);
            Assert.IsTrue(responseContext.Content.Contains(sampleCreateBody));
        }

        [Test]
        public void DeleteHandlerTest()
        {
            //arrange
            ResponseContext responseContext;
            RequestContext requestContext = RequestContext.GetBaseRequest("DELETE /messages/1 HTTP/1.1");
            //act
            responseContext= _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.IsNotEmpty(responseContext.Content);
            Assert.AreEqual("1",responseContext.Content);
            Assert.AreEqual(ResponseContext.OKResponse().StatusCode,responseContext.StatusCode);
            Assert.AreEqual(ResponseContext.OKResponse().StatusMessage, responseContext.StatusMessage);
        }

        [Test]
        public void UpdateHandlerTest()
        {
            //arrange
            ResponseContext responseContext;
            RequestContext requestContext = RequestContext.GetBaseRequest("PUT /messages/1 HTTP/1.1");
            string updatedMessage = "Update!";
            requestContext.Body = updatedMessage;
            //act
            responseContext= _messageResourceEndpointHandler.HandleRequest(requestContext);
            //assert
            Assert.IsNotEmpty(responseContext.Content);
            Assert.AreEqual("1",responseContext.Content);
            Assert.AreEqual(ResponseContext.OKResponse().StatusCode,responseContext.StatusCode);
            Assert.AreEqual(ResponseContext.OKResponse().StatusMessage, responseContext.StatusMessage);
        }
    }
}