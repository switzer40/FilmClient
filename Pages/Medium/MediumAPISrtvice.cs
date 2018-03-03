using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Film;
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
    public class MediumAPISrtvice : BaseService<MediumDto>, IMediumService
    {
        private readonly IFilmService _filmService;
        public MediumAPISrtvice(IErrorService eservice,
                                IFilmService fservice) : base(eservice)
        {
            _controller = "Medium";
            _filmService = fservice;
        }
        public override async Task<OperationResult<IKeyedDto>> AddAsync(MediumDto dto)
        {
            KeyedMediumDto retVal = default;
            var stringResponse = await StringResponseForAddAsync(dto);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status ==OKStatus)
            {
                retVal = (KeyedMediumDto)result.Value;
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
                retVal = result.Value;
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
            var result = JsonConvert.DeserializeObject<OperationResult<List<IKeyedDto>>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                var list = result.Value;
                foreach (var k in list)
                {
                    KeyedMediumDto m = (KeyedMediumDto)k;
                    retVal.Add(m);
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
            KeyedMediumDto retVal = default;
            var stringResponse = await StringResponseForGetByKeyAsync(key);
            var result = JsonConvert.DeserializeObject<OperationResult<IKeyedDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedMediumDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public async Task<MediumDto> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            MediumDto result = default;
            var key = _keyService.ConstructMediumKey(title, year, mediumType);
            var res = await GetByKeyAsync(key);
            var status = res.Status;
            if (status == OKStatus)
            { var k = (KeyedMediumDto)res.Value;
                result = new MediumDto(k.Title, k.Year, k.MediumType, k.Location, k.HasGermanSubtitles);
            }
            return result;
        }

        public override OperationResult<IKeyedDto> GetLastEntry()
        {
            KeyedMediumDto retVal = default;
            var res = GetAbsolutelyAll();
            var status = res.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedMediumDto)res.Value.LastOrDefault();
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }
        

        protected override StringContent ContentFromDto(BaseDto dto)
        {
            var m = (MediumDto)dto;
            var b = new BaseMediumDto(m.Title, m.Year, m.MediumType);
            return new StringContent(JsonConvert.SerializeObject(b),
                                     Encoding.UTF8,
                                     "application/json");
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        Task<OperationResult<MediumDto>> IMediumService.GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            throw new NotImplementedException();
        }
    }
}
