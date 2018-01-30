using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Medium;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmAPIService : BaseService<FilmDto>, IFilmService
    {
        

        public FilmAPIService(IErrorService eservice) : base(eservice)
        {            
            _route = FilmConstants.FilmUri;
        }
        [ValidateFilmNotDuplicate]
        public override async Task<OperationStatus> AddAsync(FilmDto dto)
        {
            var result = OperationStatus.OK;
            var b = new BaseFilmDto(dto.Title, dto.Year, dto.Length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_route, jsonContent);
            result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var key = _keyService.ConstructFilmKey(dto.Title, dto.Year);
                var response1 = await _client.GetAsync($"{_route}/{key}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                _addResult = JsonConvert.DeserializeObject<FilmDto>(stringResponse);
                _addResult.Key = _keyService.ConstructFilmKey(_addResult.Title, _addResult.Year);
            }
            else
            {
                _addResult = null;
            }
            return result;
        }

        public override FilmDto AddResult()
        {
            return _addResult;
        }

        public override async Task<int> CountAsync()
        {
            return (await GetAllAsync()).Count();
        }

        [ValidateFilmExists]
        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            var response = await _client.DeleteAsync($"{_route}/{key}");
            var data = _keyService.DeconstructFilmKey(key);
            return StatusFromResponse(response);
        }

        public override async Task<List<FilmDto>> GetAllAsync()
        {
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var films = JsonConvert.DeserializeObject<List<FilmDto>>(stringResponse);
            foreach (var f in films)
            {
                f.Key = _keyService.ConstructFilmKey(f.Title, f.Year);
            }
            return films;
        }
        [ValidateFilmExists]
        public override async Task<OperationStatus> GetByKeyAsync(string key)
        {
            var response = await _client.GetAsync($"{_route}/{key}");
            var result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<FilmDto>(stringResponse);
                value.Key = _keyService.ConstructFilmKey(value.Title, value.Year);
                _getResults[key] = value;
            }
            else
            {
                _getResults[key] = null;
            }
            return result;
        }

        public override FilmDto GetByKeyResult(string key)
        {
            if (_getResults.ContainsKey(key))
            {
                return _getResults[key];
            }
            return null;
        }

        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        public override async Task<OperationStatus> UpdateAsync(FilmDto dto)
        {
            var b = new BaseFilmDto(dto.Title, dto.Year, dto.Length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var status = await _client.PutAsync(_route, jsonContent);
            return StatusFromResponse(status);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (FilmDto)dto;
            return new BaseFilmDto(b.Title, b.Year);
        }
    }
}
