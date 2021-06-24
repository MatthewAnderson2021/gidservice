using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitAPITestProject
{
    public class StatusControllerApiTests
    {
        /// <summary>
        /// Test get status API to make sure its returns OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetStatus()
        {
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync("/v1/status");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
