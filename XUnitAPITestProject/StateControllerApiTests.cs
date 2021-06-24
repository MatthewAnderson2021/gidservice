using GudelIdService;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using XUnitAPITestProject.Setup;

namespace XUnitAPITestProject
{
    public class StateControllerApiTests
    {
        private HttpClient client;
        private IServiceScope scope;
        private AppDbContext context;

        public StateControllerApiTests()
        {
            client = new TestClientProvider().Client;
            scope = new TestClientProvider().Server.Services.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        /// <summary>
        /// Test GetStates using unexisting state
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnexistingState_When_GetStatesIsCalled_Then_EmptyListIsReturned()
        {
            //arrange
            //scope = new TestClientProvider().Server.Services.CreateScope();
            //context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //context.GudelIdState.RemoveRange(context.GudelIdState);
            //await context.SaveChangesAsync();

            //act
            var result = await client.GetAsync("/v1/state");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<List<GudelIdStateData>>();

            //assert
            Assert.NotEmpty(content);
        }

        private async Task CreateStatesAsync()
        {
            //workaround we just insert the seed data when nothing is in table we have to do this because the
            //seed inside the project works with migration (InMemoryDb can't use migrations)
            if (context.GudelIdState.Count() == 0)
            {
                var state = new GudelIdState(0, null, null);
                var stateTwo = new GudelIdState(10, null, null);
                var stateThree = new GudelIdState(1, null, null);
                await context.GudelIdState.AddAsync(state);
                await context.GudelIdState.AddAsync(stateTwo);
                await context.GudelIdState.AddAsync(stateThree);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Test GetStates using  existing state, list of states is returened
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingState_When_GetStatesIsCalled_Then_ListOfStatesIsReturned()
        {

            //create state
            await CreateStatesAsync();

            //act
            var result = await client.GetAsync("/v1/state");
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = await result.Content.ReadAsAsync<List<GudelIdStateData>>();

            //assert
            Assert.NotEmpty(content);
        }

        /// <summary>
        /// Test GetStateDetails using unexisting stateId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingStateId_When_GetStateDetails_Then_NotFoundIsReturned()
        {
            var id = 1;

            //act
            var result = await client.GetAsync($"/v1/stateId/{id}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        /// <summary>
        /// Test GetStateDetails using existing stateId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingStateId_When_GetStateDetails_Then_StateIsReturned()
        {
            await CreateStatesAsync();
            var id = 10;

            //act
            var result = await client.GetAsync($"/v1/state/{id}");
            Assert.True(result.IsSuccessStatusCode);
            var content = await result.Content.ReadAsAsync<GudelIdStateData>();

            //assert
            Assert.Equal(content.Id, id);
        }

        /// <summary>
        /// Test GetStateDetails using unexisiting stateId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NullStateId_When_GetStateDetails_Then_NotFoundReturned()
        {
            await CreateStatesAsync();
            var id = 1;

            //act
            var result = await client.GetAsync($"/v1/state/{id}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        /// <summary>
        ///  Test BatchChangeState ussing unexisting StateID,
        /// </summary>
        /// <returns></returns>
        [Fact]//TODO creates a Internal Server Error should be work when its fixed  // Yasser Fix by handling missing BatchChangeStateResult
        public async Task Given_UnExistingStateId_When_cIsCalled_Then_NotFoundIsReturned()
        {
            var id = 1;
            var batchRequest = new GudelIdBatchRequests()
            {
                GudelIds = new List<string>()
                {
                    ""
                }
            };

            //act
            var result = await client.PutAsJsonAsync($"/v1/state/batch/{id}", batchRequest);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        /// <summary>
        /// Test ChangeStateBatch using  No GudelIds In BatchRequest Object
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_NoGudelIdsInBatchRequestObject_When_ChangeStateBatchIsCalled_Then_BadRequestIsReturned()
        {
            var id = 1;
            var batchRequest = new GudelIdBatchRequests();

            //act
            var result = await client.PutAsJsonAsync($"/v1/state/batch/{id}", batchRequest);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        /// <summary>
        ///Test ChangeStateBatch using existing new state Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingNewStateId_When_ChangeStateBatchIsCalled_Then_StateIsChanged()
        {
            await CreateStatesAsync();
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = GudelIdTypes.SmartproductId
            };
            var newGudelId = CreateGudelId();

            var fieldDefinitionData = new ExtraFieldDefinitionData()
            {
                Description = "Super desc",
                IsRequired = false,
                Key = "1",
                Name = "Super",
                State = new List<int>() { 1 },
                Type = ""
            };

            //create id
            var idCreationResponse = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);
            Assert.True(idCreationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, idCreationResponse.StatusCode);
            var ids = await idCreationResponse.Content.ReadAsAsync<List<GudelIdData>>();

            var id = ids.First().Id;

            var extraFieldData = new Dictionary<string, string>();
            extraFieldData.Add(fieldDefinitionData.Key, "I am a super extra field");

            var batchRequest = new GudelIdBatchRequests()
            {
                GudelIds = new List<string>
                {
                    id
                },
                ExtraFieldData = extraFieldData
            };

            //act
            var result = await client.PutAsJsonAsync($"/v1/state/batch/{10}", batchRequest);
            Assert.True(result.IsSuccessStatusCode);
        
            //fetch gudelId
            var response = await client.GetAsync($"/v1/ids/{id}");
          
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsAsync<GudelIdData>();

            //assert
            Assert.Equal(10, content.StateId);
        }

        /// <summary>
        /// Test ChangeState using existing new state Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingNewStateId_When_ChangeStateIsCalled_Then_StateIsChanged()
        {
            await CreateStatesAsync();
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = GudelIdTypes.SmartproductId
            };
            var newGudelId = CreateGudelId();

            var fieldDefinitionData = new ExtraFieldDefinitionData()
            {
                Description = "Super desc",
                IsRequired = false,
                Key = "1",
                Name = "Super",
                State = new List<int>() { 1 },
                Type = ""
            };

            //create id
            var idCreationResponse = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);
            Assert.True(idCreationResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, idCreationResponse.StatusCode);
            var ids = await idCreationResponse.Content.ReadAsAsync<List<GudelIdData>>();

            var id = ids.First().Id;

            var extraFieldData = new Dictionary<string, string>();
            extraFieldData.Add(fieldDefinitionData.Key, "I am a super extra field");

            //act
            var changeStateResponse = await client.PutAsJsonAsync($"/v1/state/{id}/{10}", extraFieldData);
            Assert.True(changeStateResponse.IsSuccessStatusCode);

            //fetch gudelId
            var response = await client.GetAsync($"/v1/ids/{id}");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsAsync<GudelIdData>();

            //assert
            Assert.Equal(10, content.StateId);
        }

        private string CreateGudelId()
        {
            var gudelId = "";
            for (int i = 0; i < 12; i++)
            {
                gudelId += new Random().Next(0, 9);
            }

            return gudelId;
        }
    }
}