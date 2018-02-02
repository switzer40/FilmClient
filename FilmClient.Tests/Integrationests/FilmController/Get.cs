using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FilmClient.Tests.Integrationests.FilmController
{
    public class Get : TestBase
    {
        [Fact]
        public async Task CountReturnsTwo()
        {
            // Arrange
            
            _controller = "Film";
            _action = "Count";
            var route = $"{_route}/{_controller}/{_action}";
            // Act
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(stringResponse);

            // Assert
            Assert.Equal(2, result);
        }
        [Fact]
        public async Task GetAllReturnsTiffanyAndPretty()
        {
            // Arrange
            _controller = "Film";
            _action = "GetAll";
            var route = $"{_route}/{_controller}/{_action}";

            // Act
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<KeyedFilmDto>>(stringResponse);

            // Assert
            Assert.Contains(result, f => f.Title.Contains("Tiffany"));
            Assert.Contains(result, f => f.Title.Contains("Pretty"));

        }
        [Fact]
        public async Task GetByKeyReturnsPrettyGivenValidKey()
        {
            // Arrange
            _controller = "Film";
            _action = "GetByKey";
            var title = "Pretty Woman";
            var year = (short)1990;
            var key = _keyService.ConstructFilmKey(title, year);           
            var route = $"{_route}/{_controller}/{_action}/{key}";

            // Act
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<KeyedFilmDto>>(stringResponse);

            // Assert
            Assert.Contains("Pretty", result.First().Title);
        }
        [Fact]
        public async Task GetByKeyReturnsNotFoundGivenUnknownFilmKey()
        {
            // Arrange
            _action = "GetByKey";
            var title = "Ugly Woman";
            var year = (short)1990;
            var key = _keyService.ConstructFilmKey(title, year);
            var route = $"{_route}/{_controller}/{_action}/{key}";

            // Act
            var response = await _client.GetAsync(route);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetByKeyReturnsBadRequestGivenMalformedKey()
        {
            // Arrange
            _action = "GetByKey";            
            var key = "Foo";
            var route = $"{_route}/{_controller}/{_action}/{key}";

            // Act
            var response = await _client.GetAsync(route);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
