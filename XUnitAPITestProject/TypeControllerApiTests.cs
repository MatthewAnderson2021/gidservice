using GudelIdService.Domain.Dto;
using GudelIdService.Implementation.Persistence.Context;
using GudelIdService.Implementation.Services;
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
    public class TypeControllerApiTests
    {
        /// <summary>
        /// Test GetTypes api to verify it return actual data from the database 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Given_GudelIdTypes_When_GetTypesIsCalled_Then_TypesAreReturned()
        {
            //arrange
            var client = new TestClientProvider().Client;
            var scope = new TestClientProvider().Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var language = ConfigService.LANG_DEFAULT;
            var types = context.GudelIdTypes.Select(x => new GudelIdTypeDto(x.Id, x.Name[language], x.Description[language])).ToList();

            //act
            var result = await client.GetAsync("v1/type");
            Assert.True(result.IsSuccessStatusCode);
            var content = await result.Content.ReadAsAsync<List<GudelIdTypeDto>>();

            //assert
            foreach (var item in types)
            {
                var match = content.Where(gudelIdType => gudelIdType.Id == item.Id).FirstOrDefault();
                Assert.Equal(item.Description, match.Description);
                Assert.Equal(item.Name, match.Name);
            }
        }
    }
}
