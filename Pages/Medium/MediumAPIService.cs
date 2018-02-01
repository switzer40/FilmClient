using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
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
        public override async Task<OperationResult> AddAsync(MediumDto dto)
        {
            var result = new OperationResult(OperationStatus.OK);
            var b = new BaseMediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_route, jsonContent);
            result = ResultFromResponse(response);
            KeyedMediumDto addResult;
            if (result.Status == OperationStatus.OK)
            {
                var key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
                var response1 = await _client.GetAsync($"{_route}/{key}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                addResult = JsonConvert.DeserializeObject<KeyedMediumDto>(stringResponse);
                addResult.Key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
            }
            else
            {
                addResult = null;
            }
            result.ResultValue = new List<IKeyedDto>();
            result.ResultValue.Add(addResult);
            return result;
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
        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute(key);
            var response = await _client.DeleteAsync(route);
            return ResultFromResponse(response);
        }

        public override async Task<List<MediumDto>> GetAllAsync()
        {
            _action = "GetAll";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
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
        public override async Task<OperationResult> GetByKeyAsync(string key)
        {
            _action = "GetByKey";
            var route = ComputeRoute(key);
            var response = await _client.GetAsync(route);
            var result = ResultFromResponse(response);
            KeyedMediumDto retVal = null;
            var list = new List<IKeyedDto>();
            if (result.Status == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                retVal = JsonConvert.DeserializeObject<KeyedMediumDto>(stringResponse);
                retVal.Key = key;                
                list.Add(retVal); 
            }
            result.ResultValue = list;
            return result;
        }

        public override async Task<OperationResult> UpdateAsync(MediumDto dto)
        {
            _action = "Edit";
            var route = ComputeRoute();
            var b = new BaseMediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(route, jsonContent);
            return ResultFromResponse(response);
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
        
        public override async Task<int> CountAsync()
        {
            var media = await GetAllAsync();
            return media.Count();
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
