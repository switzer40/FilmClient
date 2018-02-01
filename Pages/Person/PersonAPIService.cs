﻿using FilmAPI.Common.Constants;
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
            var res = ResultFromResponse(response);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var key = _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
                var response1 = await _client.GetAsync($"{_route}/{key}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<KeyedPersonDto>(stringResponse);
                var val = new KeyedPersonDto(p.LastName, p.Birthdate, p.FirstMidName);
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
            var people = await GetAllAsync();
            return people.Count();
        }

        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute(key);
            var response = await _client.DeleteAsync(route);            
            return ResultFromResponse(response);
        }

        public override async Task<List<PersonDto>> GetAllAsync()
        {
            _action = "GetAll";
            var route = ComputeRoute();
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
            var res = ResultFromResponse(response);
            var s = res.Status;
            var retVal = new List<IKeyedDto>();
            if (s == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<KeyedPersonDto>(stringResponse);                
                retVal.Add(p);                
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

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        public override async Task<OperationResult> UpdateAsync(PersonDto dto)
        {
            var b = new BasePersonDto(dto.LastName, dto.BirthdateString, dto.FirstMidName);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(_route, jsonContent);
            return ResultFromResponse(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            var b = (PersonDto)dto;
            return new BasePersonDto(b.LastName, b.BirthdateString);
        }
    }
}
