using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitAPITestProject
{
    public class GudelIdControllerApiTests
    {
        #region QueryGudelId

        /// <summary>
        /// Test Query on gudelIds with wrong pageSize it should return BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_WrongPageSize_When_QueryGudelIdsIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var pageSize = -1;
            //act
            var response = await client.GetAsync($"/v1/ids/query?pageSize={pageSize}");

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test Query on gudelIds with wrong pageSize it should return BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_WrongPageSize_When_QueryGudelIdsIsCalled_Then_BadRequestIsReturned1()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var pageSize = 20001;
            //act
            var response = await client.GetAsync($"/v1/ids/query?pageSize={pageSize}");

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test Query on gudelIds with wrong page it should return BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_WrongPage_When_QueryGudelIdsIsCalled_Then_BadRequestIsReturned1()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var page = -1;
            //act
            var response = await client.GetAsync($"/v1/ids/query?page={page}");

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test Query on gudelIds will return all the gudelId data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task QueryGudelIdsIsCalled_Then_allGudelIdDataIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;

            //act
            var response = await client.GetAsync($"/v1/ids/query");

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion QueryGudelId

        #region GetGudelId

        /// <summary>
        /// Test gets a Gudel with unexisting GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingGudelId_When_GetGudelIdIsCalled_Then_NotFoundIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = "313321313131";

            //act
            var response = await client.GetAsync($"/v1/ids/{gudelId}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test gets a Gudel with existing GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetGudelUsingExistingGudelId()
        {
            var client = new TestClientProvider().Client;
            var gudelId = "008801754838";

            var response = await client.GetAsync($"/v1/ids/{gudelId}");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test gets a Gudel with GudelId length is less than 12 digit
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetGudelWithGudelIdIsLessthan12Digit()
        {
            var client = new TestClientProvider().Client;
            var gudelId = "234123";

            var response = await client.GetAsync($"/v1/ids/{gudelId}");
            var value = response.Content.ReadAsStringAsync();
            Console.WriteLine(value);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test gets a Gudel with Wrong Format GudelId_
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_WrongFormatGudelId_When_GetGudelIdIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = "W";

            //act
            var response = await client.GetAsync($"/v1/ids/{gudelId}");

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion GetGudelId

        private string CreateGudelId()
        {
            var gudelId = "";
            for (int i = 0; i < 12; i++)
            {
                gudelId += new Random().Next(0, 9);
            }

            return gudelId;
        }

        #region CreateGudelId

        /// <summary>
        /// Test Create create Gudel using GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingGudelId_When_GetGudelIdIsCalled_Then_GudelIdIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            //can may lead in duplicated ids
            var newGudelId = CreateGudelId();
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = GudelIdTypes.SmartproductId
            };

            //create id
            var idCreationResponse = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);

            idCreationResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, idCreationResponse.StatusCode);

            var ids = await idCreationResponse.Content.ReadAsAsync<List<GudelIdData>>();

            var id = ids.First().Id;

            //act
            var response = await client.GetAsync($"/v1/ids/{id}");
            Assert.True(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsAsync<GudelIdData>();
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //assert
            Assert.Equal(id, content.Id);
        }

        /// <summary>
        /// Test Create  Gudel using existing GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateGudelUsingExistingGudelId()
        {
            //arrange
            var client = new TestClientProvider().Client;
            //can may lead in duplicated ids
            var newGudelId = "4ZD3IZ0R9Q0P";
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = GudelIdTypes.SmartproductId
            };

            //create id
            var response = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test Create  Gudel using invalid GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateGudelUsingInvalidGudelId()   // Yasser: bug xxxxxxxx
        {
            //arrange
            var client = new TestClientProvider().Client;
            //can may lead in duplicated ids
            var newGudelId = "4dR9Q0cP";
            var gudelIdRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = GudelIdTypes.SmartproductId
            };

            //create id
            var response = await client.PostAsJsonAsync($"/v1/ids/create/{newGudelId}", gudelIdRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// test Create GudelIds with zero Amount
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ZeroAmount_When_CreateGudelIdsIsCalled_Then_OneGudelIdIsCreated()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var response = await client.PostAsJsonAsync($"/v1/ids/create", req);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var content = await response.Content.ReadAsAsync<List<GudelIdData>>();

            //assert
            Assert.True(content.Count == 1);
        }

        /// <summary>
        /// test Create GudelIds with less than zero Amount
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_AmountLessThanZero_When_CreateGudelIdsIsCalled_Then_OneGudelIdIsCreated()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var req = new GudelIdRequest()
            {
                Amount = -99,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var response = await client.PostAsJsonAsync($"/v1/ids/create", req);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var content = await response.Content.ReadAsAsync<List<GudelIdData>>();

            //assert
            Assert.True(content.Count == 1);
        }

        /// <summary>
        /// test Create GudelIds with Amount greater than 10000
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_AmountOver10000_When_CreateGudelIdsIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var req = new GudelIdRequest()
            {
                Amount = 980000,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var response = await client.PostAsJsonAsync($"/v1/ids/create", req);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test Create GudelIds with TypeId equal zero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ZeroTypeId_When_CreateGudelIdsIsCalled_Then_TypeIdIsSetToOne()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = 0
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var response = await client.PostAsJsonAsync($"/v1/ids/create", req);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var content = await response.Content.ReadAsAsync<List<GudelIdData>>();

            //assert
            Assert.True(content.First().TypeId == 1);
        }

        /// <summary>
        /// Test CreateGudelId using existing GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingGudelId_When_CreateGudelIdIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var response = await client.PostAsJsonAsync($"/v1/ids/create", req);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var content = await response.Content.ReadAsAsync<List<GudelIdData>>();

            var gudelId = content.First().Id;

            //act
            var createGudelIdResponse = await client.PostAsJsonAsync($"/v1/ids/create/{gudelId}", req);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, createGudelIdResponse.StatusCode);
        }

        /// <summary>
        /// Test CreateGudelId using un existing GudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingGudelId_When_CreateGudelIdIsCalled_Then_GudelIdIsCreated()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = new Random().Next(03131345, 9999999).ToString() + new Random().Next(18987, 87654).ToString();
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool
            var createResponse = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var createContent = await createResponse.Content.ReadAsAsync<PoolData>();

            req.poolId = createContent.Id;

            //act
            var createGudelIdResponse = await client.PostAsJsonAsync($"/v1/ids/create/{gudelId}", req);
            Assert.True(createGudelIdResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            //fetch gudelId
            var gudelIdResponse = await client.GetAsync($"/v1/ids/{gudelId}");
            Assert.True(gudelIdResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
            var gudelIdContent = await gudelIdResponse.Content.ReadAsAsync<GudelIdData>();

            //assert
            Assert.Equal(gudelId, gudelIdContent.Id);
            Assert.Equal(req.TypeId, gudelIdContent.TypeId);
        }

        #endregion CreateGudelId

        #region AssignToPool

        /// <summary>
        ///  Test AssignToPoolIs  with existing pool, the PoolId In GudelIdIs Successfully Updated
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ValidGudelIdWithExistingPool_When_AssignToPoolIsCalled_Then_PoolIdInGudelIdIsSuccessfullyUpdated()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = new Random().Next(03131345, 9999999).ToString() + new Random().Next(18987, 87654).ToString();
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };
            var poolData = new PoolData();

            //create a pool 1
            var createPoolResponseOne = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createPoolResponseOne.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseOne.StatusCode);
            var createPoolContentOne = await createPoolResponseOne.Content.ReadAsAsync<PoolData>();

            //create a pool 2
            var createPoolResponseTwo = await client.PostAsJsonAsync("v1/pools", poolData);
            Assert.True(createPoolResponseTwo.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createPoolResponseTwo.StatusCode);
            var createPoolContentTwo = await createPoolResponseTwo.Content.ReadAsAsync<PoolData>();

            req.poolId = createPoolContentOne.Id;

            var createGudelIdResponse = await client.PostAsJsonAsync($"/v1/ids/create/{gudelId}", req);
            Assert.True(createGudelIdResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createGudelIdResponse.StatusCode);

            //act
            var assignToPoolResponse = await client.PostAsync($"/v1/ids/assign/{gudelId}/{createPoolContentTwo.Id}", null);
            Assert.True(assignToPoolResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, createGudelIdResponse.StatusCode);

            //fetch gudelId
            var gudelIdResponse = await client.GetAsync($"/v1/ids/{gudelId}");
            Assert.True(gudelIdResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, gudelIdResponse.StatusCode);
            var gudelIdContent = await gudelIdResponse.Content.ReadAsAsync<GudelIdData>();

            //assert
            Assert.Equal(createPoolContentTwo.Id, gudelIdContent.PoolId);
            Assert.Equal(gudelId, gudelIdContent.Id);
        }

        /// <summary>
        /// Test AssignToPool using invaild gudelId , gudelId length not equal 12
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_WrongFormatGudelId_When_AssignToPoolIsCalled_Then_BadRequestIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;

            //act
            var assignToPoolResponse = await client.PostAsync($"/v1/ids/assign/{321}/{1}", null);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, assignToPoolResponse.StatusCode);
        }

        /// <summary>
        /// Test AssignToPool using unexisting gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingId_When_AssignToPoolIsCalled_Then_NotFoundIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = new Random().Next(03131345, 9999999).ToString() + new Random().Next(18987, 87654).ToString();

            //act
            var assignToPoolResponse = await client.PostAsync($"/v1/ids/assign/{gudelId}/{1}", null);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, assignToPoolResponse.StatusCode);
        }

        /// <summary>
        /// st AssignToPool using unexisting PoolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_UnExistingPoolId_When_AssignToPoolIsCalled_Then_XxIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var gudelId = new Random().Next(03131345, 9999999).ToString() + new Random().Next(18987, 87654).ToString();
            var req = new GudelIdRequest()
            {
                Amount = 0,
                poolId = 1,
                TypeId = GudelIdTypes.SmartproductId
            };

            var createGudelIdResponse = await client.PostAsJsonAsync($"/v1/ids/create/{gudelId}", req);
            Assert.True(createGudelIdResponse.IsSuccessStatusCode);

            //act
            var assignToPoolResponse = await client.PostAsync($"/v1/ids/assign/{gudelId}/{999999}", null);    
            Assert.Equal(HttpStatusCode.NotFound, assignToPoolResponse.StatusCode);
            //assert
            //TODO produces an internal server error because the pool id is not valid
        }


        /// <summary>
        /// Test AssignToPool using existing gudelIds
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_ExistingIds_When_AssignToPoolIsCalled_Then_CreatedIsReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            List<PoolAssignRequest> poolAssignRequests = new List<PoolAssignRequest>();

            var mylist = new List<(string gudelId, int targetPoolId)>();
            mylist.Add(("008801754838", 1));
            mylist.Add(("00R9YF7W91NU", 2));
            mylist.Add(("012CNP19Z8EE", 3));


            for (int i = 0; i < 3; i++)
            {
                poolAssignRequests.Add(new PoolAssignRequest() { GudelId = mylist[i].gudelId, TargetPoolId = mylist[i].targetPoolId });
            }

            //act
            var assignToPoolResponse = await client.PostAsJsonAsync("/v1/ids/assign/batch", poolAssignRequests);

            //assert
            Assert.Equal(HttpStatusCode.Created, assignToPoolResponse.StatusCode);
        }

        #endregion AssignToPool
    }
}