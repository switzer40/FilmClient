using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        public FilmPersonAPIService(IErrorService eservice) : base(eservice)
        {
        }
        public override async Task<OperationStatus> AddAsync(FilmPersonDto dto)
        {
            HttpContent jsonContent = ContentFromDto(dto);
            var response = await _client.PostAsync(_route, jsonContent);
            return StatusFromResponse(response);
        }

        public override async Task<int> CountAsync()
        {
            return (await GetAllAsync()).Count;
        }

        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            var response = await _client.DeleteAsync($"{_route}/{key}");
            return StatusFromResponse(response);
        }

        public override async Task<List<FilmPersonDto>> GetAllAsync()
        {
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FilmPersonDto>>(stringResponse);
        }

        public override async Task<OperationStatus> GetByKeyAsync(string key)
        {
            var response = await _client.GetAsync($"{_route}/{key}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FilmPersonDto>(stringResponse);
                _getResults[key] = result;
            }
            return StatusFromResponse(response);
        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }

        public override async Task<OperationStatus> UpdateAsync(FilmPersonDto dto)
        {
            var jsonContent = ContentFromDto(dto);
            var response = await _client.PutAsync(_route, jsonContent);
            return StatusFromResponse(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            
            var b = (FilmPersonDto)dto;
             return new BaseFilmPersonDto(b.Title, b.Year, b.LastName, b.Birthdate, b.Role);            
        }
    }
}
