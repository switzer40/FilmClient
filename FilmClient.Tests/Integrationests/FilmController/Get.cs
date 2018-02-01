using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
