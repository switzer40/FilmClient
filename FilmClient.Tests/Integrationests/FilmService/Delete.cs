using FilmAPI.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FilmClient.Tests.Integrationests.FilmController
{
    public class Delete : TestBase
    {
       
        [Fact]
        public async Task DeleteRemovesOneFilmGivenValidKey()
        {
            // Arrange
            int expected = await GetFilmCountAsync() - 1;

            // Act
            int actual =  await DeleteFilmAsync("Avanti!", (short)1972);

            // Assert
            Assert.Equal(expected, actual);

        }

        private async Task<int> GetFilmCountAsync()
        {
            var route = BuildRoute("Film", "Count");
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            return  JsonConvert.DeserializeObject<int>(stringResponse);            
        }

        private async Task<int> DeleteFilmAsync(string title, short year)
        {            
            var key = _keyService.ConstructFilmKey(title, year);
            var route = BuildRoute("Film", "Delete", key);
            var response = await _client.DeleteAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var films = JsonConvert.DeserializeObject<List<KeyedFilmDto>>(stringResponse);
            return films.Count;
        }
    }
}
