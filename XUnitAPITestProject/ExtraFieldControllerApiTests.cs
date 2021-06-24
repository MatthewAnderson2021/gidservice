using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitAPITestProject
{
    public class ExtraFieldControllerApiTests
    {
        private void ClearExtraFieldDefinitionTable()
        {
            using (var scope = new TestClientProvider().Server.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.ExtraFieldDefinition.RemoveRange(context.ExtraFieldDefinition.ToList());
                context.SaveChanges();
            }
        }

        #region FindAll

        /// <summary>
        /// Test FindAll using UnExisting Extra Fields , the empty list returned
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingExtraFields_When_FindAllAsyncIsCalled_Then_EmptyListIsReturned()
        {
            //arrange
            ClearExtraFieldDefinitionTable();
            var client = new TestClientProvider().Client;

            //act
            var response = await client.GetAsync("/v1/extra-fields");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsAsync<List<ExtraFieldDefinitionData>>();

            //assert
            Assert.True(content.Count == 0);
        }

        [Fact]
        public async Task Given_ExistingExtraFields_When_FindAllAsyncIsCalled_Then_ExtraFieldIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var stateId = 0;
            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                State = new List<int>() { stateId },
            };

            //act
            var response = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinitionData);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //act
            var getResponse = await client.GetAsync("/v1/extra-fields");
            Assert.True(getResponse.IsSuccessStatusCode);
            var getContent = await getResponse.Content.ReadAsAsync<List<ExtraFieldDefinitionData>>();

            //assert
            Assert.Contains(getContent, extraField => extraField.State.Any(state => state == stateId));
        }

        #endregion FindAll

        #region GetByState

        /// <summary>
        /// Test GetByState by using unexisting stateId ,it returned empty list.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingSateId_When_GetByStateIsCalled_Then_EmptyListIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var stateId = 22;

            //act
            var response = await client.GetAsync("/v1/extra-fields/state/" + stateId);
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsAsync<List<ExtraFieldDefinitionData>>();

            //assert
            Assert.False(content.Any());
        }

        /// <summary>
        /// Test extra-fields GetByState by using existing stateId ,it returned list of ExtraFieldDefinitionData.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingSateId_When_GetByStateIsCalled_Then_ExtraFieldDefinitionDataIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var stateId = 0;
            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                State = new List<int>() { stateId },
            };

            var creationResult = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinitionData);
            Assert.Equal(HttpStatusCode.Created, creationResult.StatusCode);

            //act
            var response = await client.GetAsync("/v1/extra-fields/state/" + stateId);
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsAsync<List<ExtraFieldDefinitionData>>();

            //assert
            Assert.Contains(content, extraField => extraField.State.Any(state => state == stateId));
        }

        #endregion GetByState

        #region Find

        /// <summary>
        /// Test extra-fields Find by using unexisting key
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingKey_When_FindIsCalled_Then_NotFoundIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var key = "I do not exist";

            //act
            var response = await client.GetAsync($"/v1/extra-fields/{key}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test extra-fields Findby using existing key
        /// </summary>
        /// <returns></returns

        [Fact]
        public async Task Given_ExistingKey_When_FindIsCalled_Then_KeyIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var key = "key";
            int id = new Random().Next(10000000);

            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                State = new List<int>() { 0 },
                Key = "Key",
                Name = "name",
                IsRequired = false,
                Description = "description",
                Type = "type",
                Id = id
            };

            var createResponse = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinitionData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            //act
            var getResponse = await client.GetAsync($"/v1/extra-fields/{key}");

            //assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        #endregion Find

        //TODO Internal Server Error in Method ExtraFieldService.MapToExtraFieldDefinition
        //Yasser: Fix by check gudel foreign key before perform added
        [Fact]
        public async Task Given_UnExistingKey_When_ModifyExtraFieldDefinitionIsCalled_Then_xxIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;

            var extraFieldDefinition = new ExtraFieldDefinitionData()
            {
                State = new List<int>() { 1 },
                Key = "Key",
                Name = "name",
                IsRequired = false,
                Description = "description"
            };
            ///act
            var response = await client.PutAsJsonAsync("/v1/extra-fields/KeyNotUsed", extraFieldDefinition);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test ModifyExtraFieldDefinition by pass Existing Extra Definition Key
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingExtraDefinitionKey_When_ModifyExtraFieldDefinitionIsCalled_Then_ModifiedExtraFieldIsReturned()
        {
            //arrange
            ClearExtraFieldDefinitionTable();
            var client = new TestClientProvider().Client;
            var key = "2";
            var extraFieldDefinition = new ExtraFieldDefinitionData()
            {
                Key = key,
                IsRequired = false,
                State = new List<int>() { 0 },
                Name = "name",
                Description = "description"
            };

            var creationResponse = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinition);
            Assert.Equal(HttpStatusCode.Created, creationResponse.StatusCode);

            extraFieldDefinition.IsRequired = true;

            //act
            var updateResponse = await client.PutAsJsonAsync($"/v1/extra-fields/{key}", extraFieldDefinition);
            Assert.True(updateResponse.IsSuccessStatusCode);
            var updateContent = await updateResponse.Content.ReadAsAsync<ExtraFieldDefinitionData>();

            //assert
            Assert.Equal(extraFieldDefinition.IsRequired, updateContent.IsRequired);
            Assert.Equal(extraFieldDefinition.Key, updateContent.Key);
            Assert.Equal(extraFieldDefinition.State, updateContent.State);
            Assert.Equal(extraFieldDefinition.Type, updateContent.Type);
        }

        //TODO diese Methode sollte grün werden sobald der Bug behoben wurde
        //Yasser: bug not checking the gudelIdService as a foreign key, added checking before adding
        /// <summary>
        /// test CreateExtraFieldDefinition with un exsiting state Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingStateId_When_CreateStateIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var stateId = new Random().Next(10000000);
            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                State = new List<int>() { stateId },
            };

            //act
            var response = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinitionData);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test DeleteExtraFieldDefinition with unexistingExtraField
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingExtraField_When_DeleteExtraFieldByKeyIsCalled_Then_NotFoundIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var key = "I do not exist";

            //act
            var response = await client.DeleteAsync("/v1/extra-fields/" + key);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test DeleteExtraFieldDefinition with existingExtraField
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingExtraField_When_DeleteExtraFieldByKeyIsCalled_Then_DeletionIsPersistent()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var key = "I exist";
            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                Key = key,
                Id = new Random().Next(10000000),
                IsRequired = false,
                State = new List<int>() { 0 },
            };

            var creationResult = await client.PostAsJsonAsync("/v1/extra-fields/", extraFieldDefinitionData);
            Assert.Equal(HttpStatusCode.Created, creationResult.StatusCode);

            //act
            var response = await client.DeleteAsync("/v1/extra-fields/" + key);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //assert
            var searchResponse = await client.GetAsync("/v1/extra-fields/key");
            Assert.Equal(HttpStatusCode.NotFound, searchResponse.StatusCode);
        }

        //TODO es wird eine KeyNotFoundException im ExtraFieldService in Zeile 89 geworfen dadurch entsteht ein Internal Server Error
        //Yasser: handel and checking the ExtraField with gudelId and fieldId could not be found.
        /// <summary>
        /// Test UpdateExtraField with unexisting extrafield
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingExtraField_When_UpdateExtraFieldIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = 1;
            var fieldId = 2;
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

            //act
            var response = await client.PostAsJsonAsync("/v1/extra-fields/data/" + gudelId + "/" + fieldId, keyValuePair);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test UpdateExtraField with  Existing ExtraField
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingExtraField_When_UpdateExtraFieldIsCalled_Then_OkIsReturned()
        {
            //arrange 
            ClearExtraFieldDefinitionTable();
            var client = new TestClientProvider().Client;
            var extraFieldDefinitionData = new ExtraFieldDefinitionData()
            {
                Key = Guid.NewGuid().ToString()
            };
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null, //todo es muss ein pool erstellt werden oder die andere route genommen werden
                TypeId = GudelIdTypes.SmartproductId
            };
            var newGudelId = CreateGudelId();

            var fieldDefinitionData = new ExtraFieldDefinitionData()
            {
                Description = "Super desc",
                IsRequired = false,
                Key = "1",
                Name = "Super",
                State = new List<int>() { 0, 10, 99 },
                Type = ""
            };

            //create id
            var idCreationResponse = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);
            Assert.True(idCreationResponse.IsSuccessStatusCode);
            var ids = await idCreationResponse.Content.ReadAsAsync<List<GudelIdData>>();

            //create extra field definition
            var extraFieldCreationResponse = await client.PostAsJsonAsync("v1/extra-fields", fieldDefinitionData);
            Assert.True(extraFieldCreationResponse.IsSuccessStatusCode);
            var extraFieldContent = await extraFieldCreationResponse.Content.ReadAsAsync<ExtraFieldDefinitionData>();

            var extraFieldData = new Dictionary<string, string>();
            extraFieldData.Add(fieldDefinitionData.Key, "I am a super extra field");

            //change state => create a extraField
            var changeStateResponse = await client.PutAsJsonAsync($"v1/state/{ids.First().Id}/{99}", extraFieldData);
            Assert.True(changeStateResponse.IsSuccessStatusCode);

            var keyValue = new KeyValuePair<string, string>("value", "En-DE");

            //act
            var updateResponse = await client.PostAsJsonAsync($"v1/extra-fields/data/{ids.First().Id}/{extraFieldContent.Id}", keyValue);
            Assert.True(updateResponse.IsSuccessStatusCode);

            //fetch extra field
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var searchResult = await context.ExtraField.Where(extra => extra.GudelId == ids.First().Id).FirstOrDefaultAsync();

            //assert
            Assert.True(searchResult.GudelId == ids.First().Id);
            Assert.Contains(searchResult.Value, g => g.Value == keyValue.Value);
            // Yasser: fix the issue by validiting newGudelId
            //TODO es gibt noch einen Bug wenn es mehrere ExtrafieldDefintions mit dem selben key gibt wird immer nur das erste aus der Datenbank
            //geholt. Das versucht ein Problem beim z.b updaten oder es entstehen ungewollt falsche Daten ChangeState Method Zeile 124.
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