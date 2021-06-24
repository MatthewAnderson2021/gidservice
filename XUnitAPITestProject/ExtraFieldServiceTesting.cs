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
using GudelIdService.Implementation.Mappers;
using GudelIdService;

namespace XUnitAPITestProject
{
    public class ExtraFieldServiceTesting
    {
        private ExtraFieldService _extraFieldService;
        private IExtraFieldRepository _extraFieldRepository;

        public ExtraFieldServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();

            _extraFieldRepository = scope.ServiceProvider.GetService<IExtraFieldRepository>();

            var profiles = from t in typeof(Startup).Assembly.GetTypes()
                           where typeof(Profile).IsAssignableFrom(t)
                           select (Profile)Activator.CreateInstance(t);


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GudelIdState, GudelIdStateData>();
                cfg.CreateMap<ExtraFieldDefinition, ExtraFieldDefinitionData>();
                cfg.CreateMap<ExtraFieldDefinitionData, ExtraFieldDefinition>();
                //cfg.AddMaps(System.Reflection.Assembly.GetExecutingAssembly());
                // first way
                //cfg.AddProfile(Activator.CreateInstance<ExtraFieldDefinitionToExtraFieldDefinitionDataMapperProfile>());
                //cfg.AddProfile(Activator.CreateInstance<GudelIdStateToGudelIdStateDataMapperProfile>());
                //cfg.AddProfile(Activator.CreateInstance<GudelIdToGudelIdDataMapperProfile>());
                //cfg.AddProfile(Activator.CreateInstance<PoolToPoolDataMapperProfile>());

                // second way
                //foreach (var profile in profiles)
                //{
                //    cfg.AddProfile(profile);
                //}

                // options
                //cfg.CreateMap<ExtraFieldDefinitionData, ExtraFieldDefinition>();
            });
            IMapper mapper = config.CreateMapper();

            _extraFieldService = new ExtraFieldService(_extraFieldRepository, mapper);
        }

        /// <summary>
        /// Test findAll returns all ExtraFieldDefinitionData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindAllDefinitionsTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;

            //act
            var result = await _extraFieldService.FindAllDefinitions(language);

            //assert
            Assert.IsType<List<ExtraFieldDefinitionData>>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test find and return ExtraFieldDefinitionData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindPoolDataByIdTest()
        {
            //arrange
            string key = "2";
            string language = ConfigService.LANG_DEFAULT;

            //act
            var result = await _extraFieldService.FindDefinitionByKey(key, language);

            //assert
            Assert.IsType<ExtraFieldDefinitionData>(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test remove ExtraFieldDefinitionData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveExtraFieldDefinitionDataByIdTest()
        {
            //arrange
            string language = ConfigService.LANG_DEFAULT;
            ExtraFieldDefinitionData extraFieldDefinitionData = new ExtraFieldDefinitionData();
            extraFieldDefinitionData.Key = "2";
            extraFieldDefinitionData.Name = "Test";
            extraFieldDefinitionData = await _extraFieldService.AddDefinition(extraFieldDefinitionData, language);

            //act
            int result = await _extraFieldService.RemoveDefinition(extraFieldDefinitionData.Key);
            List<ExtraFieldDefinitionData> allDefinitionData = await _extraFieldService.FindAllDefinitions(language);
            List<int> allExtraIds = new List<int>();

            foreach (var extraFieldDefinition in allDefinitionData)
            {
                allExtraIds.Add((int)extraFieldDefinition.Id);
            }

            //assert
            Assert.True(result>0);
            Assert.DoesNotContain((int)extraFieldDefinitionData.Id, allExtraIds);
        }
    }
}
