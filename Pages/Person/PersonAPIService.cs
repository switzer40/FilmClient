using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
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
            _controller = "Person";
            _keyService = new KeyService();
        }
        public override async Task<OperationResult> AddAsync(PersonDto dto)
        {
            _action = "Add";
            var result = new OperationResult(OperationStatus.OK);
            var route = ComputeRoute();
            var b = new BasePersonDto(dto.LastName, dto.BirthdateString, dto.FirstMidName);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(route, jsonContent);
            var retVal = new List<IKeyedDto>();
            var res = await ResultFromResponseAsync(response);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var key = _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
                _action = "GetByKey";
                var route1 = ComputeRoute(key);
                var response1 = await _client.GetAsync(route1);
                var stringResponse = await response1.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<KeyedPersonDto>>(stringResponse);
                var p = list.FirstOrDefault();
                var val = new KeyedPersonDto(p.LastName, p.Birthdate, p.FirstMidName, key);
                retVal.Add(val);
            }
            else
            {
                retVal = null;
            }
            return new OperationResult(s, retVal);
        }

        public override async Task<int> CountAsync()
        {
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var people = JsonConvert.DeserializeObject<List<PersonDto>>(stringResponse);
            return people.Count();
        }

        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute(key);
            var response = await _client.DeleteAsync(route);            
            return await ResultFromResponseAsync(response);
        }

        public override async Task<List<PersonDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            _action = "GetAll";
            var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";
            var route = ComputeRoute() + queryString;
            var response = await _client.GetAsync(route);
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

        public override async Task<OperationResult> GetByKeyAsync(string key)
        {
            _action = "GetByKey";
            var route = ComputeRoute(key);
            var response = await _client.GetAsync(route);
            var res = await ResultFromResponseAsync(response);
            var s =  res.Status;
            var retVal = new List<IKeyedDto>();
            if (s == OperationStatus.OK)
            {
                var dto = (KeyedPersonDto)res.ResultValue.Single();                
                retVal.Add(dto);                
            }
            else
            {
                retVal = null;
            }
            return new OperationResult(s, retVal);
        }

        public async Task<PersonDto> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            PersonDto result = null;
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await GetByKeyAsync(key);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var p = (KeyedPersonDto)res.ResultValue.Single();
                result = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
            }
            return result;
        }

        public override async Task<PersonDto> GetLastEntryAsync()
        {           
            _action = "Count";
            var route = ComputeRoute();
            var response = await _client.GetAsync(route);
            var res = await ResultFromResponseAsync(response);
            var p = (KeyedPersonDto)res.ResultValue.SingleOrDefault();
            return new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);            
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        public override async Task<OperationResult> UpdateAsync(PersonDto dto)
        {
            var b = new BasePersonDto(dto.LastName, dto.BirthdateString, dto.FirstMidName);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(_route, jsonContent);
            return await ResultFromResponseAsync(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (PersonDto)dto;
            return new BasePersonDto(b.LastName, b.BirthdateString);
        }

        protected override async Task<List<IKeyedDto>> ExtractListFromAsync(HttpResponseMessage response)
        {
            var result = new List<IKeyedDto>();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<KeyedPersonDto>>(stringResponse);
            foreach (var item in list)
            {
                result.Add((IKeyedDto)item);
            }
            return result;
        }
    }
}
