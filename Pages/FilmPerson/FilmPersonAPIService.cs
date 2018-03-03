using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        public FilmPersonAPIService(IErrorService eservice) : base(eservice)
        {
            _controller = "FilmPerson";
        }
        public override async Task<OperationResult<IKeyedDto>> AddAsync(FilmPersonDto dto)
        {
            KeyedFilmPersonDto retVal = default;
            var stringResponse = await StringResponseForAddAsync(dto);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedFilmPersonDto)result.Value;
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
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedFilmPersonDto>>>(stringResponse);
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
                var list = result.Value;
                foreach (var k in list)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override async Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key)
        {
            KeyedFilmPersonDto retVal = default;
            var stringResponse = await StringResponseForGetByKeyAsync(key);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedFilmPersonDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role)
        {
            List<IKeyedDto> retVal = default
    ;        var res = await GetAbsolutelyAllAsync();
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = res.Value;
                foreach (var k in list)
                {
                    var val = (KeyedFilmPersonDto)k;
                    if (val.LastName == lastName && val.Birthdate == birthdate && val.Role == role)
                    {
                        retVal.Add(val);
                    }                    
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetByTitleYearAndRoleAsync(string title, short year, string role)
        {
            List<IKeyedDto> retVal = default;
            var res = await GetAbsolutelyAllAsync();
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = res.Value;
                foreach (var k in list)
                {
                    var val = (KeyedFilmPersonDto)k;
                    if (val.Title == title && val.Year == year && val.Role == role)
                    {
                        retVal.Add(val);
                    }
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override OperationResult<IKeyedDto> GetLastEntry()
        {
            KeyedFilmPersonDto retVal = default;
            var res = GetAbsolutelyAll();
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedFilmPersonDto)res.Value.LastOrDefault();
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title, dto.Year, dto.LastName, dto.Birthdate, dto.Role);
        }

        protected override StringContent ContentFromDto(BaseDto dto)
        {
            var fp = (FilmPersonDto)dto;
            var b = new BaseFilmPersonDto(fp.Title,
                                          fp.Year,
                                          fp.LastName,
                                          fp.Birthdate,
                                          fp.Role);
            return new StringContent(JsonConvert.SerializeObject(b),
                                     Encoding.UTF8,
                                     "application/json");
        }
    }
}
