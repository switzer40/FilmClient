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
            _controller = "Film";
            _keyService = new KeyService();
        }
        [ValidateFilmNotDuplicate]
        public override async Task<OperationResult> AddAsync(FilmDto dto)
        {
            var result = new OperationResult();
            _action = "Add";
            var route = ComputeRoute();
            var b = new BaseFilmDto(dto.Title, dto.Year, dto.Length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(route, jsonContent);
            var retVal = new List<IKeyedDto>();
            result = await ResultFromResponseAsync(response);
            var status = result.Status;
            if (status == OperationStatus.OK)
            {
                var val = (KeyedFilmDto)result.ResultValue.Single();
                retVal.Add(val);                
            }
            else
            {
                retVal = null;
            }
            return new OperationResult(status, retVal);
        }

        public override async Task<int> CountAsync()
        {
            var films = await GetAllAsync();
            return films.Count();
        }

        [ValidateFilmExists]
        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute(key);
            var response = await _client.DeleteAsync(route);
            var data = _keyService.DeconstructFilmKey(key);
            return await ResultFromResponseAsync(response);
        }

        public override async Task<List<FilmDto>> GetAllAsync()
        {
            _action = "GetAll";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var films = JsonConvert.DeserializeObject<List<FilmDto>>(stringResponse);
            foreach (var f in films)
            {
                f.Key = _keyService.ConstructFilmKey(f.Title, f.Year);
            }
            return films;
        }
        [ValidateFilmExists]
        public override async Task<OperationResult> GetByKeyAsync(string key)
        {
            _action = "GetByKey";
            var route = ComputeRoute(key);
            var status = OperationStatus.OK;
            var retVal = new List<IKeyedDto>();
            var response = await _client.GetAsync(route);
            var result = await ResultFromResponseAsync(response);
            status = result.Status;
            if (status == OperationStatus.OK)
            {
                var value = (KeyedFilmDto) result.ResultValue.Single();
                value.Key = _keyService.ConstructFilmKey(value.Title, value.Year);
                retVal.Add(value);
            }
            else
            {
                retVal = null;
            }
            return new OperationResult(status, retVal);
        }
      
        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        public override async Task<OperationResult> UpdateAsync(FilmDto dto)
        {
            _action = "Edit";
            var route = ComputeRoute();
            var b = new BaseFilmDto(dto.Title, dto.Year, dto.Length);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(route, jsonContent);
            return await ResultFromResponseAsync(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (FilmDto)dto;
            return new BaseFilmDto(b.Title, b.Year);
        }

        protected override async Task<List<IKeyedDto>> ExtractListFromAsync(HttpResponseMessage response)
        {
            var result = new List<IKeyedDto>();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<KeyedFilmDto>>(stringResponse);
            foreach (var item in list)
            {
                result.Add((IKeyedDto)item);
            }
            return result;
        }
    }
}
