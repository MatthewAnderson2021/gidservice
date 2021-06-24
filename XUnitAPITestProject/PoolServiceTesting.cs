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
    public class PoolServiceTesting
    {
        private PoolService _poolService;
        private AppDbContext context;
        private IPoolRepository _poolRepository;

        public PoolServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();

            _poolRepository = scope.ServiceProvider.GetService<IPoolRepository>();
            var _gudelIdRepository = scope.ServiceProvider.GetService<IGudelIdRepository>();

            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var _configService = scope.ServiceProvider.GetService<ConfigService>();
            var _IMapper = scope.ServiceProvider.GetService<AutoMapper.IMapper>();

            _poolService = new PoolService(_poolRepository, _gudelIdRepository, _IMapper, _configService);
        }

        /// <summary>
        /// Test findAll returns all PoolData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindAllPoolDataTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;

            //act
            var result = await _poolService.FindAll(language);

            //assert
            Assert.IsType<List<PoolData>>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test find and return PoolData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindPoolDataByIdTest()
        {
            //arrange
            int Id = 42;
            string language = ConfigService.LANG_DEFAULT;

            //act
            PoolData result = await _poolService.FindById(Id, language);

            //assert
            Assert.IsType<PoolData>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test remove PoolData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemovePoolDataByIdTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;
            PoolData newPoolData = new PoolData();
            newPoolData = await _poolService.AddAsync(newPoolData, language);

            //act
            bool result = await _poolService.Remove((int)newPoolData.Id);
            List<PoolData> allPoolData = await _poolService.FindAll(language);
            List<int> allPoolIds = new List<int>();

            foreach (var poolData in allPoolData)
            {
                allPoolIds.Add((int)poolData.Id);
            }

            //assert
            Assert.True(result);
            Assert.DoesNotContain((int)newPoolData.Id, allPoolIds);
        }

        /// <summary>
        /// Test Update PoolData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task updatePoolDataTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;
            int Id = 42;
            PoolData updatePoolData = await _poolService.FindById(Id, language);
            updatePoolData.Name = "test";

            //act
            var updatedPoolData = await _poolService.Update(updatePoolData, language);

            //assert
            Assert.NotNull(updatedPoolData);
        }

        /// <summary>
        /// Test Add PoolData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddPoolDataTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;
            PoolData newPoolData = new PoolData();

            //act
            newPoolData = await _poolService.AddAsync(newPoolData, language);
            List<PoolData> allPoolData = await _poolService.FindAll(language);
            List<int> allPoolIds = new List<int>();

            foreach (var poolData in allPoolData)
            {
                allPoolIds.Add((int)poolData.Id);
            }

            //assert
            Assert.Contains((int)newPoolData.Id, allPoolIds);
        }
    }
}
