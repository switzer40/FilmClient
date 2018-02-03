using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FilmClient.Tests.Integrationests.PersonController
{
    public class Get : TestBase
    {
        [Fact]
        public async Task CountReturnsTwo()
        {
            // Arrange
            var route = BuildRoute("Person", "Count");

            // Act
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var count = JsonConvert.DeserializeObject<int>(stringResponse);

            // Assert
            Assert.Equal(2, count);
        }
        [Fact]
        public async Task GetAllReturnsHepburnAndRobertsAsync()
        {
            // Arrange
            var route = BuildRoute("Person", "GetAll");
            // Act
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<KeyedPersonDto>>(stringResponse);

            // Assert
            Assert.Contains(result, p => p.LastName.Contains("Hepburn"));
            Assert.Contains(result, p => p.LastName.Contains("Roberts"));
        }
    }
}
