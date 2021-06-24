using GudelIdService;
using GudelIdService.Implementation.Extensions;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUnitAPITestProject.Setup
{
    public class Factory : WebApplicationFactory<Startup>
    {
        public int TestServicePort { get; set; } = 5000;
        public static string URL { get; set; }
        public static readonly Factory factory = new Factory("v1");
        private static readonly InMemoryDatabaseRoot root = new InMemoryDatabaseRoot();

        protected Factory(string version)
        {
            URL = $"http://localhost:{TestServicePort}";
            version = version ?? string.Empty;
            if (!version.EndsWith('/'))
            {
                version += '/';
            }

            ClientOptions.BaseAddress = new Uri($"{URL}/{version.TrimStart('/')}");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseIISIntegration().UseUrls(URL);

            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider(true);

            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // using an in-memory database for testing.
                services.AddDatabase(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDBForTesting", root);
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }
    }
}
