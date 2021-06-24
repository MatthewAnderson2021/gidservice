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
    public class ConfigServiceTesting
    {
        private ConfigService _configService;
        
        public ConfigServiceTesting()
        {
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var _converterServiceFactory = scope.ServiceProvider.GetService<IConverterServiceFactory<int>>();

            var IConfigObj = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            _configService = new ConfigService(IConfigObj, _converterServiceFactory);
        }

        /// <summary>
        /// Test Get Config from key 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetMethodTest()
        {
            // arrange
            var keyVar = "CREATE_CRON_INTERVAL";
            var valueVar = "1000";

            // act
            var result = _configService.Get(keyVar);

            // assert
            Assert.Equal(valueVar, result);
        }

        /// <summary>
        /// Test Returen HttpPort 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GetEnvironmentVariablesTest()
        {
            // arrange
            var PORT = 3003;

            // act
            var result = _configService.HttpPort();
            
            // assert
            Assert.True(result == PORT);
        }

        /// <summary>
        /// Test return environment variable CREATE_CRON_INTERVAL
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CreateCronIntervalTest()
        {
            // arrange
            var CREATE_CRON_INTERVAL = 1000;

            // act
            var result = _configService.CreateCronInterval();

            // assert
            Assert.True(result == CREATE_CRON_INTERVAL);
        }

        /// <summary>
        /// Test return environment variable CREATE_CRON_AMOUNT
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CreateCronAmountTest()
        {
            // arrange
            var CREATE_CRON_AMOUNT = 1;

            // act
            var result = _configService.CreateCronAmount();

            // assert
            Assert.True(result == CREATE_CRON_AMOUNT);
        }

        /// <summary>
        /// Test return environment variable BODY_PARSE_LIMIT
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void MaxBodyParseLimitTest()
        {
            // arrange
            var limit = 10;

            // act
            var result = _configService.MaxBodyParseLimit();

            // assert
            Assert.True(result == limit);
        }
    }
}
