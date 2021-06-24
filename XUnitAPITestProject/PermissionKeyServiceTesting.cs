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
using AutoMapper;
using PasswordGenerator;
using System.Text;

namespace XUnitAPITestProject
{
    public class PermissionKeyServiceTesting
    {
        private PermissionKeyService _permissionKeyService;
        private IPermissionKeyRepository _permissionKeyRepository;
        private AppDbContext context;

        public PermissionKeyServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var _gudelIdRepository = scope.ServiceProvider.GetService<IGudelIdRepository>();
            _permissionKeyRepository = scope.ServiceProvider.GetService<IPermissionKeyRepository>();
            context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var profiles = from t in typeof(PermissionKeyServiceTesting).Assembly.GetTypes()
                           where typeof(Profile).IsAssignableFrom(t)
                           select (Profile)Activator.CreateInstance(t);


            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
            IMapper mapper = config.CreateMapper();

            _permissionKeyService = new PermissionKeyService(_permissionKeyRepository, _gudelIdRepository, context, mapper);
        }

        /// <summary>
        /// Test Gets the key hints for the given gudelId 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetKeyHintsForGudelIdAsyncTest()
        {
            //arrange
            string gudelId = "012CNP19Z8EE";

            //act
            var result = await _permissionKeyService.GetKeyHintsForGudelIdAsync(gudelId);

            //assert
            Assert.IsAssignableFrom<ICollection<Tuple<PermissionKeyType, string>>>(result);
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// Test return permissionkeytype default when its not valid the default clr type value is 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsyncTest()
        {
            //arrange
            string gudelId = "012CNP19Z8EE";
            string key = "0";

            //act
            var result = await _permissionKeyService.CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsync(gudelId, key);

            //assert
            Assert.IsAssignableFrom<Tuple<bool, PermissionKeyType>>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Generates a permission key
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GeneratePermissionKeyTest()
        {
            //arrange
            string gudelId = "012CNP19Z8EE";
            string password = new Password().IncludeLowercase()
                             .IncludeNumeric()
                             .IncludeUppercase()
                             .LengthRequired(11).Next();
            byte[] gudelIdByteArray = Encoding.ASCII.GetBytes(gudelId);

            //act
            var result = await _permissionKeyService.GeneratePermissionKey(password, gudelId, gudelIdByteArray, 1);

            //assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test Generates or updates the permissionkey for the given gudelId for every existing permission key type
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePermissionKeysTest()
        {
            //arrange
            string gudelId = "012CNP19Z8EE";

            //act
            var result = await _permissionKeyService.CreatePermissionKeys(gudelId);

            //assert
            Assert.IsAssignableFrom<ICollection<PermissionKey>>(result);
            Assert.NotNull(result);
        }
    }
}
