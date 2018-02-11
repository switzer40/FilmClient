using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Default;
using FilmClient.Pages.Error;
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
        private readonly IDefaultService _defaultService;
        
        public MediumAPIService(IFilmService fservice,
                                IDefaultService dservice,
                                IErrorService eservice) : base(eservice)
        {
            _filmService = fservice;
            _defaultService = dservice;
            _keyService = new KeyService();
            _controller = "Medium";            
        }
        [ValidateMediumNotDuplicate]
        public override async Task<OperationResult> AddAsync(MediumDto dto)
        {
            var result = new OperationResult(OperationStatus.OK);
            _action = "Add";
            var route = ComputeRoute();
            var b = new BaseMediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location, dto.GermanSubtitles);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(route, jsonContent);
            result = await ResultFromResponseAsync(response);
            KeyedMediumDto addResult;
            if (result.Status == OperationStatus.OK)
            {
                addResult = (KeyedMediumDto)result.ResultValue.SingleOrDefault();
            }
            else
            {
                addResult = null;
            }
            result.ResultValue = new List<IKeyedDto>
            {
                addResult
            };            
            return result;
        }

        [ValidateMediumExists]
        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute(key);
            var response = await _client.DeleteAsync(route);
            return await ResultFromResponseAsync(response);
        }

        public override async Task<List<MediumDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            _action = "GetAll";
            var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";
            var route = ComputeRoute() + queryString;
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rawMedia = JsonConvert.DeserializeObject<List<KeyedMediumDto>>(stringResponse);
            var result = new List<MediumDto>();
            foreach (var k in rawMedia)
            {
                var dto = new MediumDto(k.Title, k.Year, k.MediumType, k.Location, k.HasGermanSubtitles);
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
            var result = await ResultFromResponseAsync(response);            
            var list = new List<IKeyedDto>();
            if (result.Status == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var media = JsonConvert.DeserializeObject<List<KeyedMediumDto>>(stringResponse);
                var m = media.FirstOrDefault();
                list.Add(m);               
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
            return await ResultFromResponseAsync(response);
        }

     
       
        public override async Task<int> CountAsync()
        {
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var media = JsonConvert.DeserializeObject<List<MediumDto>>(stringResponse);
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

        protected override async Task<List<IKeyedDto>> ExtractListFromAsync(HttpResponseMessage response)
        {
            var result = new List<IKeyedDto>();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<KeyedMediumDto>>(stringResponse);
            foreach (var item in list)
            {
                result.Add((IKeyedDto)item);
            }
            return result;
        }

        public async Task<MediumDto> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            MediumDto result = null;
            var key = _keyService.ConstructMediumKey(title, year, mediumType);
            var res = await GetByKeyAsync(key);
            if (res.Status == OperationStatus.OK)
            {
                var m = (KeyedMediumDto)res.ResultValue.Single();
                result = new MediumDto(m.Title, m.Year, m.MediumType, m.Location, m.HasGermanSubtitles);
            }
            return result;
        }

        public override async Task<MediumDto> GetLastEntryAsync()
        {
            MediumDto result = null;
            var f = await _filmService.GetLastEntryAsync();
            var d = _defaultService.GetCurrentDefaultValues();
            
            return result;
        }
    }
}
