using System;
using System.Collections.Generic;
using System.Text;
using GudelIdService.Implementation.Services;
using Xunit;

namespace XUnitAPITestProject
{

    public class UtilsServiceTesting
    {

        private UtilsService _utilsService;

        public UtilsServiceTesting()
        {
            _utilsService = new UtilsService();
        }


        /// <summary>
        /// Test Utils method Generate random string
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GenerateRandomStringShouldEqualItsEqual()
        {
            //arrange
            var hash = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                var len = 12;

                //act
                var s = _utilsService.GenerateRandomString(len, false);
                var s1 = _utilsService.GenerateRandomString(len, true);

                if (hash.Contains(s) || hash.Contains(s1))
                {
                    //assert if its generate same random string
                    Assert.True(false, "Duplicate string found"); // random string should not Repeated
                }
                else
                {
                    hash.Add(s);
                    hash.Add(s1);
                }
            }
        }
    }
}
