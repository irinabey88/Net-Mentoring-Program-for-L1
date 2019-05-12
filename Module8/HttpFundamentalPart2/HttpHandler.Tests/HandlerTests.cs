using System;
using System.Net.Http;
using NUnit.Framework;

namespace HttpHandler.Tests
{
    [TestFixture]
    public class HandlerTests
    {
        private readonly HttpClient _client;
        private readonly UriBuilder _uriBuilder;

        public HandlerTests()
        {
            _client = new HttpClient(new HttpClientHandler());
            _uriBuilder = new UriBuilder("http://localhost:81");
            _uriBuilder.Query = "take=30";
        }

        [Test]
        public void Get_ExcelAcceptType()
        {
            var ExcelAcceptType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", ExcelAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ExcelAcceptType, contentType);
        }

        [Test]
        public void Get_XmlAcceptType()
        {
            var TextXmlAcceptType = "text/xml";

            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", TextXmlAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;


            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(TextXmlAcceptType, contentType);
        }

        [Test]
        public void Get_ApplicationXmlAcceptType()
        {
            var ApplicationXmlAcceptType = "application/xml";

            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", ApplicationXmlAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;


            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ApplicationXmlAcceptType, contentType);
        }

        [Test]
        public void Get_DefaultAcceptType()
        {   
            var OtherAcceptType = "text/plain";
            var ExcelAcceptType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            _client.DefaultRequestHeaders.Remove("Accept");
            _client.DefaultRequestHeaders.Add("Accept", OtherAcceptType);
            var response = _client.GetAsync(_uriBuilder.Uri).Result;
            var contentType = response.Content.Headers.ContentType.MediaType;

            Assert.AreEqual(true, response.IsSuccessStatusCode);
            Assert.AreEqual(ExcelAcceptType, contentType);
        }
    }
}