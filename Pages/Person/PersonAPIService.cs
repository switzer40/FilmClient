using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonAPIService : BaseService<PersonDto>, IPersonService
    {
        
        public PersonAPIService(IErrorService eservice) : base(eservice)
        {            
            _route = FilmConstants.PersonUri;
        }
        public override async Task<OperationStatus> AddAsync(PersonDto dto)
        {
            var result = OperationStatus.OK;
            var b = new BasePersonDto(dto.LastName, dto.BirthdateString, dto.FirstMidName);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_route, jsonContent);
            result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var key = _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
                var response1 = await _client.GetAsync($"{_route}/{key}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                _addResult = JsonConvert.DeserializeObject<PersonDto>(stringResponse);
                _addResult.Key = _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
            }
            else
            {
                _addResult = null;
            }
            return result;
        }

        public override PersonDto AddResult()
        {
            return _addResult;
        }

        public override async Task<int> CountAsync()
        {
            return (await GetAllAsync()).Count();
        }

        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            var response = await _client.DeleteAsync($"{_route}/{key}");            
            return StatusFromResponse(response);
        }

        public override async Task<List<PersonDto>> GetAllAsync()
        {
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rawPeople = JsonConvert.DeserializeObject<List<KeyedPersonDto>>(stringResponse);
            var result = new List<PersonDto>();
            foreach (var k in rawPeople)
            {
                var dto = new PersonDto(k.LastName, k.Birthdate, k.FirstMidName);
                dto.Key = _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
                result.Add(dto);
            }
            return result;
        }

        public override async Task<OperationStatus> GetByKeyAsync(string key)
        {
            var response = await _client.GetAsync($"{_route}/{key}");
            var result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<PersonDto>(stringResponse);
                p.FullName = $"{p.FirstMidName} {p.LastName}";
                p.Key = key;
                _getResults[key] = p;
                var data = _keyService.DeconstructPersonKey(key);
                _getResults[key].Key = key;
                _getResults[key].BirthdateString = data.birthdate;
            }
            else
            {
                _getResults[key] = null;
            }
            return result;
        }

        public override PersonDto GetByKeyResult(string key)
        {
            if (_getResults.ContainsKey(key))
            {
                return _getResults[key];
            }
            else
            {
                return null;
            }
        }

        public async Task<PersonDto> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var s = await GetByKeyAsync(key);
            return (s == OperationStatus.OK) ? GetByKeyResult(key) : null;
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        public override async Task<OperationStatus> UpdateAsync(PersonDto dto)
        {
            var b = new BasePersonDto(dto.LastName, dto.BirthdateString, dto.FirstMidName);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(_route, jsonContent);
            return StatusFromResponse(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (PersonDto)dto;
            return new BasePersonDto(b.LastName, b.BirthdateString);
        }
    }
}
