using GudelIdService.Implementation.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitAPITestProject
{
    public class PermissionControllerApiTests
    {
        /// <summary>
        /// test GetKeyHints with un exsiting gudel Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingGudelId_When_GetKeyHintsIsCalled_Then_NotFoundIsReturned()
        {
            string gudelId = "012CNP19Z8E";
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync($"v1/permission/{gudelId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// test GetKeyHints with exsiting gudel Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingGudelId_When_GetKeyHintsIsCalled_Then_ResponseIsReturned()
        {
            string gudelId = "012CNP19Z8EE";
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync($"v1/permission/{gudelId}");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// test CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsync with gudel Id and key
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_GudelIdAndKey_When_CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsyncIsCalled_Then_ResponseIsReturned()
        {
            string gudelId = "012CNP19Z8EE";
            string key = "0";
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync($"v1/permission/{gudelId}/{key}");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// test Create Permission key with gudel Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_GudelId_When_GenerateKeysIsCalled_Then_ResponseIsReturned()
        {
            string gudelId = "008801754838";
            var client = new TestClientProvider().Client;
            var response = await client.PostAsync($"v1/permission/{gudelId}", null);
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
