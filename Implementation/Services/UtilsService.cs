using GudelIdService.Domain.Services;
using System;
using System.Text;

namespace GudelIdService.Implementation.Services
{
    public class UtilsService : IUtilsService
    {
        static readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        public string GenerateRandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            
            for (var i = 0; i < size; i++)
            {
                builder.Append(random.NextDouble() < 0.5 ? genLetter() : genNumber());
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        private Char genLetter()
        {
            return Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        }
        private Char genNumber()
        {
            return Convert.ToChar(Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48)));
        }
    }
}
