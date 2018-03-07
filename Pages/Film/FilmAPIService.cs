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

namespace FilmClient.Pages.Film
{
    public class FilmAPIService : BaseService<FilmDto>, IFilmService
    {
        public FilmAPIService(IErrorService eservice) : base(eservice)
        {
            SetController("Film");
        }
        public override async Task<OperationResult<IKeyedDto>> AddAsync(FilmDto dto)
        {
            IKeyedDto retVal = default;
            var stringResponse = await StringResponseForAddAsync(dto);
            var res = JsonConvert.DeserializeObject<OperationResult<KeyedFilmDto>>(stringResponse);
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = res.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public override async Task<OperationResult<int>> CountAsync()
        {
            int count = 0;
            var stringResponse = await StringResponseForCountAsync();
            var res  = JsonConvert.DeserializeObject<OperationResult<int>>(stringResponse);
            count = res.Value;
            return new OperationResult<int>(res.Status, count);
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
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedFilmDto>>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = result.Value;
                foreach (var k in list)
                {
                    retVal.Add((IKeyedDto)k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override async Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize)
        {
            List<IKeyedDto> retVal = default;
            var stringResponse = await StringResponseForGetAllAsync(pageIndex, pageSize);
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedFilmDto>>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = new List<IKeyedDto>();
                var list = result.Value.ToList();                    
                foreach (var k in list)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(status, retVal);
        }

        public override async Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key)
        {
            KeyedFilmDto retVal = default;
            var stringResponse = await StringResponseForGetByKeyAsync(key);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedFilmDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public async Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            FilmDto retVal = default;
            var key = _keyService.ConstructFilmKey(title, year);
            var res = await GetByKeyAsync(key);
            var status = res.Status;
            if (status == OKStatus)
            {
                var k = (KeyedFilmDto)res.Value;
                retVal = new FilmDto(k.Title, k.Year, k.Length);
            }
            return new OperationResult<FilmDto>(status, retVal);
        }

        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        protected override StringContent ContentFromDto(BaseDto dto)
        {
            var f = (FilmDto)dto;
            var b = new BaseFilmDto(f.Title, f.Year, f.Length);
            return new StringContent(JsonConvert.SerializeObject(b),
                                     Encoding.UTF8,
                                     "application/json");
        }
    }
}
