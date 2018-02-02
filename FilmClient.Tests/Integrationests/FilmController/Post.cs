using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FilmClient.Tests.Integrationests.FilmController
{
    public class Post : TestBase
    {
        [Fact]
        public async Task AddReturnsBadRequestGivenDuplicateFilm()
        {
            // Arrange
            _controller = "Film";
            _action = "Add";
            var route = $"{_route}/{_controller}/{_action}";
            var title = "Pretty Woman";
            var year = (short)1990;
            var length = (short)109;
            var b = new BaseFilmDto(title, year, length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(route, jsonContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task AddReturnsAvantiGivenValidData()
        {
            // Arrange
            _controller = "Film";
            _action = "Add";
            var route = $"{_route}/{_controller}/{_action}";
            var title = "Avanti";
            var year = (short)1972;
            var length = (short)140;
            var b = new BaseFilmDto(title, year, length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b),
                                                Encoding.UTF8,
                                                "application/json");

            // Act
            var response = await _client.PostAsync(route, jsonContent);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var films = JsonConvert.DeserializeObject<List<KeyedFilmDto>>(stringResponse);
            var film = films.FirstOrDefault();

            // Assert
            Assert.Equal(title, film.Title);
        }
    }
}
