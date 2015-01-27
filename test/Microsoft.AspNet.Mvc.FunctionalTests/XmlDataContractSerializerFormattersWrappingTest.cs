// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;
using Xunit;

namespace Microsoft.AspNet.Mvc.FunctionalTests
{
    public class XmlDataContractSerializerFormattersWrappingTest
    {
        private readonly IServiceProvider _services = TestHelper.CreateServices(nameof(XmlFormattersWebSite));
        private readonly Action<IApplicationBuilder> _app = new XmlFormattersWebSite.Startup().Configure;

        [Theory]
        [InlineData("http://localhost/Wrapper/IEnumerableOfValueTypes")]
        [InlineData("http://localhost/Wrapper/IQueryableOfValueTypes")]
        public async Task CanWrite_ValueTypes(string url)
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfint xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                        " xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">" +
                        "<int>10</int><int>20</int></ArrayOfint>",
                         result);
        }
        
        [Theory]
        [InlineData("http://localhost/Wrapper/IEnumerableOfNonWrappedTypes")]
        [InlineData("http://localhost/Wrapper/IQueryableOfNonWrappedTypes")]
        public async Task CanWrite_NonWrappedTypes(string url)
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfstring xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                        " xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">" +
                        "<string>value1</string><string>value2</string></ArrayOfstring>",
                         result);
        }

        [Theory]
        [InlineData("http://localhost/Wrapper/IEnumerableOfNonWrappedTypes_NullInstance")]
        [InlineData("http://localhost/Wrapper/IQueryableOfNonWrappedTypes_NullInstance")]
        public async Task CanWrite_NonWrappedTypes_NullInstance(string url)
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfstring i:nil=\"true\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                        " xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\" />",
                         result);
        }

        [Theory]
        [InlineData("http://localhost/Wrapper/IEnumerableOfWrappedTypes")]
        [InlineData("http://localhost/Wrapper/IQueryableOfWrappedTypes")]
        public async Task CanWrite_WrappedTypes(string url)
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfPersonWrapper xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                " xmlns=\"http://schemas.datacontract.org/2004/07/XmlFormattersWebSite\"><PersonWrapper>" +
                "<Age>35</Age><Id>10</Id><Name>Mike</Name></PersonWrapper><PersonWrapper><Age>35</Age><Id>" +
                "11</Id><Name>Jimmy</Name></PersonWrapper></ArrayOfPersonWrapper>",
                result);
        }

        [Theory]
        [InlineData("http://localhost/Wrapper/IEnumerableOfWrappedTypes_NullInstance")]
        [InlineData("http://localhost/Wrapper/IQueryableOfWrappedTypes_NullInstance")]
        public async Task CanWrite_WrappedTypes_NullInstance(string url)
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfPersonWrapper i:nil=\"true\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                " xmlns=\"http://schemas.datacontract.org/2004/07/XmlFormattersWebSite\" />",
                result);
        }

        [Fact]
        public async Task PostedSerializableError_IsBound()
        {
            // Arrange
            var mediaType = "application/xml-dcs";
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            var expectedXml = "<Error><key1>key1-error</key1><key2>The input was not valid.</key2></Error>";
            var requestContent = new StringContent(expectedXml, Encoding.UTF8, mediaType);

            // Act
            var response = await client.PostAsync("http://localhost/Wrapper/LogSerializableError", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal(mediaType, response.Content.Headers.ContentType.MediaType);
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedXml, responseData);
        }

        [Fact]
        public async Task CanWrite_IEnumerableOf_SerializableErrors()
        {
            // Arrange
            var server = TestServer.Create(_services, _app);
            var client = server.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Wrapper/IEnumerableOfSerializableErrors");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml-dcs"));

            // Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("<ArrayOfSerializableErrorWrapper xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                " xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.AspNet.Mvc.Xml\"><SerializableErrorWrapper>" +
                "<key1>key1-error</key1><key2>key2-error</key2></SerializableErrorWrapper><SerializableErrorWrapper>" +
                "<key3>key1-error</key3><key4>key2-error</key4></SerializableErrorWrapper>" +
                "</ArrayOfSerializableErrorWrapper>",
                result);
        }
    }
}