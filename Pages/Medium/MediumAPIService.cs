using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Film;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public class MediumAPIService : BaseService<MediumDto>, IMediumService
    {
        private readonly IFilmService _filmService;

        
        public MediumAPIService(IFilmService fservice, IErrorService eservice) : base(eservice)
        {
            _filmService = fservice;
            
            _route = FilmConstants.MediumUri;
        }
        [ValidateMediumNotDuplicate]
        public override async Task<OperationStatus> AddAsync(MediumDto dto)
        {
            var result = OperationStatus.OK;
            var b = new BaseMediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_route, jsonContent);
            result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
                var response1 = await _client.GetAsync($"{_route}/{key}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                _addResult = JsonConvert.DeserializeObject<MediumDto>(stringResponse);
                _addResult.Key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
            }
            else
            {
                _addResult = null;
            }
            return result;
        }

        public override MediumDto AddResult()
        {
            return _addResult;
        }

        public async Task CascadeDeleteForFilmAsync(string title, short year)
        {
            var media = await GetAllAsync();
            var toBeDeleted = media.Where(m => m.Title == title && m.Year == year);
            await DeleteRangeAsync(toBeDeleted);
        }

        private async Task DeleteRangeAsync(IEnumerable<MediumDto> toBeDeleted)
        {
            foreach (var m in toBeDeleted)
            {
                var key = _keyService.ConstructMediumKey(m.Title, m.Year, m.MediumType);
                var s = await DeleteAsync(key);
            }
        }

        [ValidateMediumExists]
        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            var response = await _client.DeleteAsync($"{_route}/{key}");
            return StatusFromResponse(response);
        }

        public override async Task<List<MediumDto>> GetAllAsync()
        {
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rawMedia = JsonConvert.DeserializeObject<List<KeyedMediumDto>>(stringResponse);
            var result = new List<MediumDto>();
            foreach (var k in rawMedia)
            {
                var dto = new MediumDto(k.Title, k.Year, k.MediumType, k.Location);
                dto.Key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
                result.Add(dto);
            }
            return result;
        }
        [ValidateMediumExists]
        public override async Task<OperationStatus> GetByKeyAsync(string key)
        {
            var response = await _client.GetAsync($"{_route}/{key}");
            var result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                _getResults[key] = JsonConvert.DeserializeObject<MediumDto>(stringResponse);
                _getResults[key].Key = key;
            }
            else
            {
                _getResults[key] = null;
            }
            return result;
        }

        public override MediumDto GetByKeyResult(string key)
        {
            if (_getResults.ContainsKey(key))
            {
                return _getResults[key];
            }
            return null;
        }

        public override async Task<OperationStatus> UpdateAsync(MediumDto dto)
        {
            var b = new BaseMediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(_route, jsonContent);
            return StatusFromResponse(response);
        }

        public async Task<bool> HasMediumForFilmAsync(string title, short year)
        {
            return ((await MediaForFilmAsync(title, year)).Count > 0);
        }

        public async Task<List<MediumDto>> MediaForFilmAsync(string title, short year)
        {
            var spec = new MediumForTitleAndYear(title, year);
            return (await GetAllAsync()).Where(spec.Predicate).ToList();
        }

        public async Task<OperationStatus> DeleteMediaForFilmAsync(string title, short year)
        {
            var mediaToDelete = await MediaForFilmAsync(title, year);
            return await DeleteMediaRangeAsync(mediaToDelete);
        }

        public async Task<OperationStatus> DeleteMediaRangeAsync(List<MediumDto> mediaToDelete)
        {
            var result = OperationStatus.OK;
            var media = await GetAllAsync();
            foreach (var m in mediaToDelete)
            {
                var s = await DeleteAsync(m.Key);
                if (s != OperationStatus.OK)
                {
                    result = s;
                    break;
                }
            }
            return result;
        }

        public override async Task<int> CountAsync()
        {
            return (await GetAllAsync()).Count();
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (MediumDto)dto;
            return new BaseMediumDto(b.Title, b.Year, b.MediumType);
        }
    }
}
