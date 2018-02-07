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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        private readonly IFilmService _filmService;
        private readonly IPersonService _personService;
        public FilmPersonAPIService(IFilmService fService,
                                    IPersonService pservice,
                                    IErrorService eservice) : base(eservice)
        {
            _filmService = fService;
            _personService = pservice;
            _keyService = new KeyService();
            _controller = "FilmPerson";
        }
        public override async Task<OperationResult> AddAsync(FilmPersonDto dto)
        {
            _action = "Add";
            var route = ComputeRoute();
            HttpContent jsonContent = ContentFromDto(dto);
            var response = await _client.PostAsync(route, jsonContent);
            return await ResultFromResponseAsync(response);
        }

        public override async Task<int> CountAsync()
        {
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var filmPeople = JsonConvert.DeserializeObject<List<FilmPersonDto>>(stringResponse);
            return filmPeople.Count();
        }

        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute();
            var response = await _client.DeleteAsync(route);
            return await ResultFromResponseAsync(response);
        }

        public override async Task<List<FilmPersonDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            _action = "GetAll";
            var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";
            var route = ComputeRoute() + queryString;
            var result = new List<FilmPersonDto>();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rawFilmPeople = JsonConvert.DeserializeObject<List<KeyedFilmPersonDto>>(stringResponse);
            foreach (var fp in rawFilmPeople)
            {
                var fKey = _keyService.ConstructFilmKey(fp.Title, fp.Year);
                var f = _filmService.GetByKey(fKey);
                if (f == null)
                {
                    // if this happens,
                    // the server is broken
                }
                var pKey = _keyService.ConstructPersonKey(fp.LastName, fp.Birthdate);
                var p = _personService.GetByKey(pKey);
                if (p == null)
                {
                    // if this happens,
                    // the server is broken
                }
                var dto = new FilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
                result.Add(dto);
            }
            return result;
        }

        public override async Task<OperationResult> GetByKeyAsync(string key)
        {
            _action = "GetByKey";
            var route = ComputeRoute(key);
            var list = new List<IKeyedDto>();
            var response = await _client.GetAsync(route);
            var result = await ResultFromResponseAsync(response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var retVal = JsonConvert.DeserializeObject<KeyedFilmPersonDto>(stringResponse);                
                list.Add(retVal);
            }            
            result.ResultValue = list;
            return result;

        }

        public async Task<OperationResult> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role)
        {
            OperationResult result = new OperationResult();
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var result1 = await ResultFromResponseAsync(response);
            var s = result1.Status;
            if (s == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var filmPeople = JsonConvert.DeserializeObject<List<KeyedFilmPersonDto>>(stringResponse);
                var list = filmPeople.Where(fp => fp.LastName == lastName && fp.Birthdate == birthdate && fp.Role == role);
                var retVal = new List<IKeyedDto>();
                foreach (var item in list)
                {
                    var val = (IKeyedDto)item;
                    retVal.Add(val);
                }
                result = new OperationResult(s, retVal);
            }
            return result;
        }

        public async Task<OperationResult> GetByTitleYearAndRoleAsync(string title, short year, string role)
        {
            OperationResult result = new OperationResult();
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var result1 = await ResultFromResponseAsync(response);
            var s = result1.Status;
            if (s == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var filmPeople = JsonConvert.DeserializeObject<List<KeyedFilmPersonDto>>(stringResponse);
                var list = filmPeople.Where(fp => fp.Title == title && fp.Year == year && fp.Role == role);
                var retVal = new List<IKeyedDto>();
                foreach (var item in list)
                {
                    var val = (IKeyedDto)item;
                    retVal.Add(val);
                }
                result = new OperationResult(s, retVal);
            }
            return result;
        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }

        public override async Task<OperationResult> UpdateAsync(FilmPersonDto dto)
        {
            _action = "Edit";
            var route = ComputeRoute();
            var jsonContent = ContentFromDto(dto);
            var response = await _client.PutAsync(route, jsonContent);
            return await ResultFromResponseAsync(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            
            var b = (FilmPersonDto)dto;
             return new BaseFilmPersonDto(b.Title, b.Year, b.LastName, b.Birthdate, b.Role);            
        }

        protected override async Task<List<IKeyedDto>> ExtractListFromAsync(HttpResponseMessage response)
        {
            var result = new List<IKeyedDto>();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<KeyedFilmPersonDto>>(stringResponse);
            foreach (var item in list)
            {
                result.Add((IKeyedDto)item);
            }
            return result;
        }
    }
}
