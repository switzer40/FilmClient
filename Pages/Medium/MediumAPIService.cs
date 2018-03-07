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

namespace FilmClient.Pages.Medium
{
    public class MediumAPIService : BaseService<MediumDto>, IMediumService
    {
        public MediumAPIService(IErrorService eservice) : base(eservice)
        {
            SetController("Medium");
        }
        public override async Task<OperationResult<IKeyedDto>> AddAsync(MediumDto dto)
        {
            KeyedMediumDto retVal = default;
            var stringResponse = await StringResponseForAddAsync(dto);
            var res = JsonConvert.DeserializeObject<OperationResult<KeyedMediumDto>>(stringResponse);
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
            var res = JsonConvert.DeserializeObject<OperationResult<int>>(stringResponse);
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
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedMediumDto>>>(stringResponse);
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
            var result = JsonConvert.DeserializeObject<OperationResult<List<KeyedMediumDto>>>(stringResponse);
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
            KeyedMediumDto retVal = default;
            var stringResponse = await StringResponseForGetByKeyAsync(key);
            var result = JsonConvert.DeserializeObject<OperationResult<KeyedMediumDto>>(stringResponse);
            var status = result.Status;
            if (status == OKStatus)
            {
                retVal = (KeyedMediumDto)result.Value;
            }
            return new OperationResult<IKeyedDto>(status, retVal);
        }

        public async Task<OperationResult<MediumDto>> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            MediumDto retVal = default;
            var key = _keyService.ConstructMediumKey(title, year, mediumType);
            var res = await GetByKeyAsync(key);
            if (res.Status == OKStatus)
            {
                var k = (KeyedMediumDto)res.Value;
                retVal = new MediumDto(k.Title, k.Year, k.MediumType, k.Location, k.HasGermanSubtitles);
            }
            return new OperationResult<MediumDto>(res.Status, retVal);
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        protected override StringContent ContentFromDto(BaseDto dto)
        {
            var m = (MediumDto)dto;
            var b = new BaseMediumDto(m.Title, m.Year, m.MediumType, m.Location, m.GermanSubtitles);
            return new StringContent(JsonConvert.SerializeObject(b),
                                     Encoding.UTF8,
                                     "application/json");
        }
    }
}
