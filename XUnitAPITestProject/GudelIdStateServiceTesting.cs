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
    public class GudelIdStateServiceTesting
    {
        private GudelIdStateService _gudelIdStateService;
        private IGudelIdStateRepository _gudelIdStateRepository;

        public GudelIdStateServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();

            _gudelIdStateRepository = scope.ServiceProvider.GetService<IGudelIdStateRepository>();
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GudelIdState, GudelIdStateData>();
                cfg.CreateMap<ExtraFieldDefinition, ExtraFieldDefinitionData>();
            });
            IMapper mapper = config.CreateMapper();

            _gudelIdStateService = new GudelIdStateService(_gudelIdStateRepository, mapper);
        }

        /// <summary>
        /// Test findAll returns all GudelIdStateData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindAllGudelIdStateDataTest()
        {
            //arrange
            var language = ConfigService.LANG_DEFAULT;

            //act
            var result = await _gudelIdStateService.FindAll(language);

            //assert
            Assert.IsType<List<GudelIdStateData>>(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// Test find and return GudelIdStateData 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindGudelIdStateDataByIdTest()
        {
            //arrange
            int Id = 10;
            string language = ConfigService.LANG_DEFAULT;

            //act
            GudelIdStateData result = await _gudelIdStateService.FindById(Id, language);

            //assert
            Assert.IsType<GudelIdStateData>(result);
            Assert.NotNull(result);
            Assert.Equal("Reserviert", result.Name);
        }
    }
}
