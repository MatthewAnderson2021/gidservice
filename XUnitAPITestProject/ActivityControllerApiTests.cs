using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace XUnitAPITestProject
{
    public class ActivityControllerApiTests
    {
        /// <summary>
        ///Test get a user with existing userid
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUser()
        {
            var client = new TestClientProvider().Client;
            var userId = "f32a23f5-95a3-4234-9dce-663a58f759cd";
            var response = await client.GetAsync("/v1/activity/user/" + userId);
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test get a user with unexisting userid
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUser_NotFound()
        {
            var client = new TestClientProvider().Client;
            var userId = 10;
            var response = await client.GetAsync("/v1/activity/user/" + userId);
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test get a activity with existing gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetGudelId()
        {
            var client = new TestClientProvider().Client;
            var gudelId = 568874565801;
            var response = await client.GetAsync("/v1/activity/id/" + gudelId);
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test get a activity with unexisting  gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetGudelId_NotFound()
        {
            var client = new TestClientProvider().Client;
            var gudelId = 1533;
            var response = await client.GetAsync("/v1/activity/id/" + gudelId);
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}