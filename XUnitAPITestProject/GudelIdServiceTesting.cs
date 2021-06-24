using System;
using System.Linq;
using GudelIdService.Implementation.Services;
using GudelIdService.Implementation.Persistence.Context;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GudelIdService.Domain.Dto;

namespace XUnitAPITestProject
{
    public class GudelIdServiceTesting
    {
        private GudelIdService.Implementation.Services.GudelIdService _gudelIdService;
        private AppDbContext context;
        private IGudelIdRepository _gudelIdRepository;


        public GudelIdServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();
            _gudelIdRepository = scope.ServiceProvider.GetService<IGudelIdRepository>();


            var _activityRepository = scope.ServiceProvider.GetService<IActivityRepository>();
            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var _activityService = new ActivityService(_activityRepository, context);
            var _configService = scope.ServiceProvider.GetService<ConfigService>();
            var _IMapper = scope.ServiceProvider.GetService<AutoMapper.IMapper>();

            _gudelIdService = new GudelIdService.Implementation.Services.GudelIdService(_gudelIdRepository, _activityService, _configService, _IMapper, new UtilsService(), context);
        }

        /// <summary>
        /// Test findAll returns all GudelIdData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllGudelIdTest()
        {
            //arrange
            var userId = "system";

            //act
            var result = await _gudelIdService.FindAll((x) => x.CreatedBy == userId);

            //assert
            Assert.IsType<List<GudelIdData>>(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// Test find and return GudelIdData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetGudelIdDataByIdTest()
        {
            //arrange
            var Id = "01LN0V7G1T0D";
            var language = ConfigService.LANG_DEFAULT;

            //act
            GudelIdData result = await _gudelIdService.Find(Id, language);

            //assert
            Assert.IsType<GudelIdData>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test findAllCount returns counts
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllGudelIdCountTest()
        {
            //arrange
            var userId = "system";

            //act
            var result = await _gudelIdService.FindAllCount((x) => x.CreatedBy == userId);

            //assert
            Assert.IsType<int>(result);
            Assert.True(result>0);
        }

        /// <summary>
        /// Test GetCountForPool returns counts by poolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetCountForPoolTest()
        {
            //arrange
            int poolId = 1;

            //act
            var result = await _gudelIdService.GetCountForPool(poolId);

            //assert
            Assert.IsType<int>(result);
            Assert.True(result > 0);
        }

        /// <summary>
        /// Test update gudelId with poolId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePoolIdTest()
        {
            //arrange
            string Id = "ZQU8A4D5N5U3";
            int poolId = 1;

            //act
            var updatedResult = await _gudelIdService.UpdatePoolId(Id, poolId);
            GudelId result = await _gudelIdRepository.Find(x => x.Id == Id);

            //assert
            Assert.IsType<GudelId>(updatedResult);
            Assert.Same(result, updatedResult);
        }

        /// <summary>
        /// Test creating gudelIds
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateGudelIdsTest()
        {
            //arrange
            var userId = "system";
            var newGudelRequest = new GudelIdRequest()
            {
                Amount = 1,
                poolId = null,
                TypeId = 1
            };

            //act
            var createdResult = await _gudelIdService.CreateGudelIds(newGudelRequest, userId);

            //assert
            Assert.IsType<List<GudelIdData>>(createdResult);
            Assert.NotNull(createdResult);
        }

        /// <summary>
        /// Test creating gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateGudelIdTest()
        {
            //arrange
            var userId = "system";
            string gudelId = CreateGudelId();
            int poolId = 4;
            int typeId = 1;

            //act
            var newId = await _gudelIdService.CreateGudelId(gudelId, poolId, typeId, userId);

            //assert
            Assert.IsType<GudelId>(newId);
            Assert.NotNull(newId);
        }

        /// <summary>
        /// Test reserving gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReserveGudelIdTest()
        {
            //arrange
            string userId = "system";
            string gudelId = "01LN0V7JJT0D";

            //act
            var resultId = await _gudelIdService.ReserveGudelId(gudelId, userId);
            var updatedGudelId = await _gudelIdRepository.Find(x => x.Id == gudelId);

            //assert
            Assert.IsType<GudelId>(resultId);
            Assert.NotNull(resultId);
            Assert.Same(resultId, updatedGudelId);
        }

        /// <summary>
        /// Test reserving gudelIds
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReserveGudelIdsTest()
        {
            // arrange
            string userId = "system";
            List<GudelId> gudelIds = new List<GudelId>();
            int poolId = 1;
            int typeId = 1;

            gudelIds.Add(new GudelId() { Id = "008801754838" });
            gudelIds.Add(new GudelId() { Id = "00R9YF7W91NM" });
            gudelIds.Add(new GudelId() { Id = "00R9YF7W91NU" });

            // act
            var result = _gudelIdService.ReserveGudelIds(gudelIds, userId, poolId, typeId);

            // assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Generate gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GenerateGudelIdTest()
        {
            //arrange
            var userId = "system";

            //act
            var resultId = await _gudelIdService.GenerateGudelId(userId);
            var allGudelIds = await _gudelIdRepository.FindAll(1000, 0, (x) => x.CreatedBy == userId);

            //assert
            Assert.IsType<GudelId>(resultId);
            Assert.Contains(resultId, allGudelIds);
        }

        /// <summary>
        /// Test Generate gudelIds
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GenerateGudelIdsTest()
        {
            //arrange
            var userId = "system";
            int amount = 3;

            //act
            var resultIds = await _gudelIdService.GenerateGudelIds(amount, userId);
            var allGudelIds = await _gudelIdRepository.FindAll(1000, 0, (x) => x.CreatedBy == userId);

            //assert
            Assert.IsType<List<GudelId>>(resultIds);

            foreach (var gudelId in resultIds)
            {
                Assert.Contains(gudelId, allGudelIds);
            }
        }

        /// <summary>
        /// Test Changing State of gudelId
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangeStateTest()
        {
            //arrange
            string userId = "system";
            int stateId = GudelIdStates.ReservedId;
            GudelIdData olGudelId = new GudelIdData() {
                Id = "012CNP19Z8EE"
            };

            //act
            var resultId = await _gudelIdService.ChangeState(olGudelId, stateId, userId);
            var updatedGudelId = await _gudelIdRepository.Find(x => x.Id == olGudelId.Id);

            //assert
            Assert.IsType<GudelId>(resultId);
            Assert.Same(resultId, updatedGudelId);
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
