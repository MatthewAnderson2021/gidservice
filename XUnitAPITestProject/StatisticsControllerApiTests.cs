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
    public class StatisticsControllerApiTests
    {
        [Fact]
        public async Task GetStatisticsPools()
        {
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync("/v1/statistics/count/pools");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test GetPoolCount using nullable username, return count of all existing pools
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NullUserName_When_GetPoolCountIsCalled_Then_CountOfAllExistingPoolsIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var poolCount = context.Pool.Count();

            //act
            var result = await client.GetAsync("/v1/statistics/count/pools");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<int>();

            //assert
            Assert.Equal(poolCount, content);
        }

        /// <summary>
        /// Test GetPoolCount using empty string for  username, return count of all existing pools
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_EmptyUserName_When_GetPoolCountIsCalled_Then_CountOfAllExistingPoolsIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var poolCount = context.Pool.Count();
            var userName = "  ";

            //act
            var result = await client.GetAsync($"/v1/statistics/count/pools/{userName}");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<int>();

            //assert
            Assert.Equal(poolCount, content);
        }

        /// <summary>
        ///  Test GetPoolCount using existing username, return Pool Count From User
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingUserName_When_GetPoolCountIsCalled_Then_PoolCountFromUserIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userName = "Hund";
            context.Pool.Add(new GudelIdService.Domain.Models.Pool()
            {
                CreatedBy = userName
            });
            await context.SaveChangesAsync();
            var poolCount = context.Pool.Count(x => x.CreatedBy == userName);

            //act
            var result = await client.GetAsync($"/v1/statistics/count/pools?username={userName}");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<int>();

            //assert
            Assert.Equal(poolCount, content);
        }

        /// <summary>
        /// Test GetIDCount using null for state, returned  GudelId Counts Are Searched By UserName
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NullbyState_When_GetIDCountIsCalled_Then_GudelIdCountsAreSearchedByUserName()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userName = "Hund";
            var poolCount = context.GudelId.Count(x => x.CreatedBy == userName);

            //act
            var result = await client.GetAsync($"/v1/statistics/count/ids?username={userName}");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<ArrayList>();

            //assert
            Assert.Contains(poolCount.ToString(), content[0].ToString());
        }

        /// <summary>
        /// Test GetIDCount using null for username, returned  GudelId Counts Are Searched By UserName
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NullUsername_When_GetIDCountIsCalled_Then_GudelIdCountsAreSearchedByUserName()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var byState = "0";

            //act
            var result = await client.GetAsync($"/v1/statistics/count/ids?byState={byState}");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<ArrayList>();

            //assert
            Assert.True(content.Count > 0);
        }

        /// <summary>
        /// Test GetIDCount using 0 for state, returned  GudelId Counts Are Searched By UserName
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_byState_When_GetIDCountIsCalled_Then_GudelIdCountsAreSearchedByUserName()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userName = "Hund";
            var byState = "0";
            var poolCount = context.GudelId.Count(x => x.CreatedBy == userName);

            //act
            var result = await client.GetAsync($"/v1/statistics/count/ids?username={userName}&byState={byState}");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<ArrayList>();

            //assert
            //Assert.Contains(poolCount.ToString(), content[0].ToString());
            Assert.True(content.Count > 0);
        }
    }
}