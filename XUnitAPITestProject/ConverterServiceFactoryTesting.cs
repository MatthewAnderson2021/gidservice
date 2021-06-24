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
using GudelIdService.Domain.Services;
using GudelIdService;
using Microsoft.Extensions.Configuration;

namespace XUnitAPITestProject
{
    public class ConverterServiceFactoryTesting
    {
        private ConverterServiceFactory<int> _converterServiceFactory;
        private ConverterService<int> _converterService;
        private const string PORT = nameof(PORT);
        private IConfiguration _config;
        public ConverterServiceFactoryTesting()
        {
            _converterServiceFactory = new ConverterServiceFactory<int>();
            _converterService = (ConverterService<int>)_converterServiceFactory.GetConverter();
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Test WithDefaultValue
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void WithDefaultValueTest()
        {
            // arrange 
            int temp = 10;

            // act
            var result = _converterService.WithDefaultValue(temp);

            // assert
            Assert.IsAssignableFrom<IConverterService<int>>(result);
        }

        /// <summary>
        /// Test WithConvertAction
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void WithConvertActionTest()
        {
            var result = _converterService.WithConvertAction(() => int.Parse(Environment.GetEnvironmentVariable(PORT) ?? _config[PORT]));

            Assert.IsAssignableFrom<IConverterService<int>>(result);
        }

        /// <summary>
        /// Test Execute
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void ExecuteTest()
        {
            var result = _converterServiceFactory
                .GetConverter()
                .WithDefaultValue(3003)
                .WithConvertAction(() => int.Parse(Environment.GetEnvironmentVariable(PORT) ?? _config[PORT]))
                .Execute();
            Assert.IsType<int>(result);
        }
    }
}
