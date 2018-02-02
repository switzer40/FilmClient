using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;

namespace FilmClient.Tests.Integrationests
{
    public class TestBase
    {
        public TestBase()
        {
            _route = $"http://localhost:5000/api";
            _keyService = new KeyService();
        }
        protected static HttpClient _client = new HttpClient();
        protected string _route;
        protected string _controller;
        protected string _action;
        protected IKeyService _keyService;

        private static HttpClient GetClient()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseEnvironment("Testing");

            var server = new TestServer(builder);
            var client = server.CreateClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
