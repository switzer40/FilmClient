using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System.Text;
using FilmAPI.Common.Constants;

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
        protected string BuildRoute(string controller, string action, string key = "")
        {
            string result = "";
            if (string.IsNullOrEmpty(key))
            {
                result = $"{_route}/{controller}/{action}";
            }
            else
            {
                result = $"{_route}/{controller}/{action}/{key}";
            }
            return result;
        }
        protected async Task RepopulateDatabaseAsync()
        {
            await RepolulateFilmDBAsync();
            await RepopulateFilmPersonDBAsync();
            await RepopulateMediumDBAsync();
            await RepopulatePersonDBAsync();
        }

        private async Task RepolulateFilmDBAsync()
        {
            await ClearDBAsync("Film");
            await AddFilmAsync("Frühstück bei Tiffany", (short)1961, (short)110);
            await AddFilmAsync("Pretty Woman", (short)1990, (short)109);
        }

        private async Task AddFilmAsync(string title, short year, short length)
        {
            var route = BuildRoute("Film", "Add");
            var b = new BaseFilmDto(title, year, length);
            await SendPostRequestAsync(route, b);
        }

        private async Task SendPostRequestAsync(string route, IBaseDto b)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            await _client.PostAsync(route, jsonContent);
        }

        private async Task ClearDBAsync(string controller)
        {
            var route = BuildRoute(controller, "ClearAll");
            await _client.DeleteAsync(route);
        }

        private async Task RepopulateFilmPersonDBAsync()
        {
            await ClearDBAsync("FilmPerson");
            await AddFilmPersonAsync("Frühstück bei Tiffany",
                                     (short)1961,
                                     "Hepburn",
                                     "1929-05-04",
                                     FilmConstants.Role_Actor);
            await AddFilmPersonAsync("Pretty Woman",
                                     (short)1990,
                                     "Roberts",
                                     "1967-10-28",
                                     FilmConstants.Role_Actor);
        }

        private async Task AddFilmPersonAsync(string title, short year, string lastName, string birthdate, string role)
        {
            var route = BuildRoute("FilmPerson", "Add");
            var b = new BaseFilmPersonDto(title, year, lastName, birthdate, role);
            await SendPostRequestAsync(route, b);
        }

        private async Task RepopulateMediumDBAsync()
        {
            await ClearDBAsync("Medium");
            await AddMediumAsync("Frühstück bei Tiffany", (short)1961, FilmConstants.MediumType_DVD, FilmConstants.Location_Left);
            await AddMediumAsync("Pretty Woman", (short)1990, FilmConstants.MediumType_DVD, FilmConstants.Location_Left);
        }

        private async Task AddMediumAsync(string title, short year, string mediumType, string location)
        {
            var route = BuildRoute("Medium", "Add");
            var b = new BaseMediumDto(title, year, mediumType, location, true);
            await SendPostRequestAsync(route, b);
        }

        private async Task RepopulatePersonDBAsync()
        {
            await ClearDBAsync("Person");
            await AddPersonAsync("Audrey", "Hepburn", "1929-05-04");
            await AddPersonAsync("Julia", "Roberts", "1967-10-28");
        }

        private async Task AddPersonAsync(string firstName, string lastName, string birthdate)
        {
            var route = BuildRoute("Person", "Add");
            var b = new BasePersonDto(lastName, birthdate, firstName);
            await SendPostRequestAsync(route, b);
        }
    }
}
