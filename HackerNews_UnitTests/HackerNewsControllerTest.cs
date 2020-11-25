using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using HackerNews;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace HackerNews_UnitTests
{
    public class HackerNewsControllerTest
    {
        private readonly HttpClient client;

        public HackerNewsControllerTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestGetHackerNewsAsync(string method)
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/HackerNews");

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
