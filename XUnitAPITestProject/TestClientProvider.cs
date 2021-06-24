using GudelIdService;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace XUnitAPITestProject
{
    public class TestClientProvider
    {
        public HttpClient Client { get; private set; }
        public WebApplicationFactory<Startup> Server { get; set; }
        public TestClientProvider()
        {
            Server = new WebApplicationFactory<Startup>();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJsb2dpblRlbmFudElkIjoiMWZjNjhmMzUtZWE2OC00OWM3LThmMzctNWI0ODdkMGVmZGM5IiwiaWQiOiIwN2U1MzliMS01YzRkLTQ0MzgtOTMwYi1jYTAxOGRjNzdkYjgiLCJuYW1lIjoiYmVuamFtaW4uZnJhbmtsaW5AZ3VkZWxzdGFnZWRldm9yZy5vbm1pY3Jvc29mdC5jb20iLCJsYXN0TmFtZSI6IkZyYW5rbGluIiwiZmlyc3ROYW1lIjoiQmVuamFtaW4iLCJkaXNwbGF5TmFtZSI6IkJlbmphbWluIEZyYW5rbGluIiwibmJmIjoxNTk4MDIzNDU4LCJleHAiOjE1OTg2MjgyNTgsImlhdCI6MTU5ODAyMzQ1OH0.TsS-WO2uJa_pZiGj823Cd_OxnXr9NbDuF5rZ5AgzIOI";
            Client = Server.CreateClient();
            Client.DefaultRequestHeaders.Add("Cookie", "__session_key=" + token);
        }
    }
}
