using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
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
        }
        public override async Task<OperationResult<IKeyedDto>> AddAsync(PersonDto dto)
        {
            KeyedPersonDto retVal = default;
            var stringResponse = await StringResponseForAddAsync(dto);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedPersonDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public override async Task<OperationResult<int>> CountAsync()
        {
            int retVal = 0;
            var stringResponse = await StringResponseForCountAsync();
            var result = JsonConvert.DeserializeObject<OperationResult<int>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (int)result.Value;
            }
            return new OperationResult<int>(status, retVal);
        }

        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            _action = "Delete";
            ComputeRoute(key);
            var response = await _client.DeleteAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OperationStatus>(stringResponse);
        }

        public override async Task<OperationResult<List<IKeyedDto>>> GetAbsolutelyAllAsync()
        {
            List<IKeyedDto> retVal = default;
            var stringResponse = await StringResponseForGetAbsolutelyAllAsync();
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedPersonDto>>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = result.Value;
                foreach (var k in list)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override async Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize)
        {
            List<IKeyedDto> retVal = default;
            var stringResponse = await StringResponseForGetAllAsync(pageIndex, pageSize);
            var result = JsonConvert.DeserializeObject<OperationResult<List<IKeyedDto>>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = result.Value
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();
                foreach (var k in list)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override async Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key)
        {
            KeyedPersonDto retVal = default;
            var stringResponse = await StringResponseForGetByKeyAsync(key);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedPersonDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public async Task<OperationResult<PersonDto>> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            PersonDto retVal = default;
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await GetByKeyAsync(key);
            var status = res.Status;
            if (status == OKStatus)
            {
                var k = (KeyedPersonDto)res.Value;
                retVal = new PersonDto(k.LastName, k.Birthdate, k.FirstMidName);
            }
            return new OperationResult<PersonDto>(status, retVal);
        }

        public override OperationResult<IKeyedDto> GetLastEntry()
        {
            KeyedPersonDto retVal = default;
            var res = GetAbsolutelyAll();
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedPersonDto)res.Value.LastOrDefault();
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        protected override StringContent ContentFromDto(BaseDto dto)
        {
            var p = (PersonDto)dto;
            var b = new BasePersonDto(p.LastName, p.BirthdateString, p.FirstMidName);
            return new StringContent(JsonConvert.SerializeObject(b),
                                     Encoding.UTF8,
                                     "application/json");
        }

    }
}
