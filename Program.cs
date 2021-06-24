using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace GudelIdService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var builder = webBuilder.UseStartup<Startup>();

                var port = Environment.GetEnvironmentVariable("PORT");
                if (!String.IsNullOrWhiteSpace(port))
                {
                    builder.UseUrls($"http://*:{port}");
                }

            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole(c => c.TimestampFormat = "dd.MM.yy HH:mm:ss ");
                logging.AddAzureWebAppDiagnostics();
            });
}
}

