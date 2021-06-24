using GudelIdService;
using GudelIdService.Domain.Dto;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using XUnitAPITestProject.Setup;

namespace XUnitAPITestProject
{
    public class PoolControllerApiTests
    {
        private HttpClient client;
        private IServiceScope scope;
        private AppDbContext context;

        public PoolControllerApiTests()
        {
            client = new TestClientProvider().Client;
            scope = new TestClientProvider().Server.Services.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        #region CreatePool

        /// <summary>
        /// Test CreatePool using existing pool.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingPool_When_FindAllIsCalled_Then_PoolIsReturned()
        {
            //arrange
            var poolData = new PoolData()
            {
                CreatedBy = "f32a23f5-95a3-4234-9dce-663a58f759cd",
                CreationDate = DateTime.Now,
                Description = "",
                GudelIds = new List<GudelIdData>(),
                Name = "",
                Size = 0
            };

            //create a pool 1
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContent = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            //act
            var response = await client.GetAsync($"v1/pools");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsAsync<List<PoolData>>();

            //assert
            Assert.Contains(content, p => p.Id == createPoolContent.Id);
        }

        /// <summary>
        /// Test CreatePool using valid pool data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ValidPoolData_When_CreatePoolIsCalled_Then_PoolIsCreated()
        {
            //arrange
            var pool = new PoolData()
            {
                CreatedBy = "f32a23f5-95a3-4234-9dce-663a58f759cd",
                Description = "",
                GudelIds = new List<GudelIdData>(),
                Name = "",
                Size = 0
            };

            //act
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", pool);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContent = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            //fetch from db
            var response = await client.GetAsync($"v1/pools/{createPoolContent.Id}");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var content = await response.Content.ReadAsAsync<PoolData>();

            //assert
            Assert.NotNull(content);
            Assert.Equal(pool.Description, content.Description);
            Assert.Equal(pool.CreatedBy, content.CreatedBy);
            Assert.Equal(pool.GudelIds, content.GudelIds);
            Assert.Equal(pool.Name, content.Name);
        }

        #endregion CreatePool

        #region FindAll

        /// <summary>
        /// Test FindAll using unexisting pool.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NoExistingPools_When_FindAllIsCalled_Then_NoPoolIsReturned()
        {
            //arrange
            //await ClearPoolTableAsync();

            //act
            var response = await client.GetAsync($"v1/pools");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsAsync<List<PoolData>>();

            //assert
            Assert.True(content.Count > 0);
        }

        #endregion FindAll

        #region FindById

        /// <summary>
        /// Test FindById using UnExisting PoolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingPoolId_When_FindByIdIsCalled_Then_NotFoundIsReturned()
        {

            //act
            var response = await client.GetAsync($"pools/{22}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test FindById using Existing PoolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingPool_When_FindByIdIsCalled_Then_PoolIsReturned()
        {
            var poolData = new PoolData()
            {
                CreatedBy = "f32a23f5-95a3-4234-9dce-663a58f759cd",
                Description = "",
                GudelIds = new List<GudelIdData>(),
                Name = "",
                Size = 0
            };

            //create a pool
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContent = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            //act
            var response = await client.GetAsync($"v1/pools/{createPoolContent.Id}");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsAsync<PoolData>();

            //assert
            Assert.Equal(createPoolContent.Id, content.Id);
            Assert.Equal(createPoolContent.Name, content.Name);
            Assert.Equal(createPoolContent.Size, content.Size);
            Assert.Equal(createPoolContent.GudelIds, content.GudelIds);
            Assert.Equal(createPoolContent.ExternalId, content.ExternalId);
            Assert.Equal(createPoolContent.Description, content.Description);
            Assert.Equal(createPoolContent.CreationDate, content.CreationDate);
            Assert.Equal(createPoolContent.CreatedBy, content.CreatedBy);
        }

        #endregion FindById

        #region ModifyPool

        /// <summary>
        /// Test ModifyPool using UnExisting PoolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingPool_When_ModifyPoolIsCalled_Then_NotFoundIsReturned()
        {

            //act
            var response = await client.PutAsync($"pool/{2}", null);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test ModifyPool using existing PoolId to perform update sucessfully
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingPool_When_ModifyPoolIsCalled_Then_PoolUpdateIsPersistent()
        {
            //arrange
            //await ClearPoolTableAsync();
            var updatePoolData = new PoolData()
            {
                CreatedBy = "WWW",
                Description = "Wil",
                ExternalId = "gg",
                GudelIds = null,
                Name = "Hans",
                Size = 1
            };

            var creationPoolData = new PoolData()
            {
                CreatedBy = "g",
                Description = "c",
                ExternalId = "ccc",
                GudelIds = new List<GudelIdData>(),
                Name = "GG",
                Size = 1
            };

            //create a pool
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", creationPoolData);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContent = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            var poolId = createPoolContent.Id;
            updatePoolData.Id = poolId;

            //act
            var response = await client.PutAsJsonAsync($"v1/pools/{poolId}", updatePoolData);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //fech updated pool
            var fetchResponse = await client.GetAsync($"v1/pools/{poolId}");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, fetchResponse.StatusCode);
            var fetchContent = await fetchResponse.Content.ReadAsAsync<PoolData>();

            //assert
            Assert.Equal(updatePoolData.Id, fetchContent.Id);
            Assert.Null(updatePoolData.GudelIds);
            Assert.Equal(updatePoolData.Name, fetchContent.Name);
            Assert.Equal(updatePoolData.Description, fetchContent.Description);
            Assert.Equal(updatePoolData.CreatedBy, fetchContent.CreatedBy);
        }

        #endregion ModifyPool

        #region DeletePool

        /// <summary>
        /// Test DeletePool  using unexisting poolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingId_When_DeletePoolIsCalled_Then_NotFoundIsReturned()
        {
            //arrange
            var poolId = 923813;

            //act
            var result = await client.DeleteAsync($"pools/{poolId}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Given_ExistingId_When_DeletePoolIsCalled_Then_PoolDeletionIsPersistent()
        {
            //arrange
            var pool = new PoolData()
            {
                CreatedBy = "f32a23f5-95a3-4234-9dce-663a58f759cd",
                CreationDate = DateTime.Now,
                Description = "",
                GudelIds = new List<GudelIdData>(),
                Name = "",
                Size = 0
            };

            //create pool
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", pool);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContent = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            //act
            var result = await client.DeleteAsync($"v1/pools/{createPoolContent.Id}");
            Assert.True(result.IsSuccessStatusCode);

            //try to fetch from db
            var response = await client.GetAsync($"pools/{createPoolContent.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
        }

        #endregion DeletePool

        private async Task ClearPoolTableAsync()
        {
            context.Pool.RemoveRange(context.Pool);
            await context.SaveChangesAsync();
        }
    }
}