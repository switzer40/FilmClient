using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public class MediumAPIService : BaseService<MediumDto>, IMediumService
    {
        public MediumAPIService() : base()
        {
            _controller = "Medium";
        }
        public override async Task<IKeyedDto> AddAsync(MediumDto dto)
        {
            var response = await ResponseForAddAsync(dto);
            return ResultFromResponse(response);
        }

        public override async Task<int> CountAsync()
        {
            var response = await ResponseForCountAsync();
            return IntResultFromResponse(response);
        }

        public override async Task DeleteAsync(string key)
        {
            var response = await ResponseForDeleteAsync(key);
            VoidResultFromResponse(response);
            return;
        }

        public override async Task<List<IKeyedDto>> GetAbsolutelyAllAsync()
        {
            var response = await ResponseForGetAbsolutelyAllAsync();
            return ListResultFromResponse(response);
        }

        public override async Task<List<IKeyedDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var response = await ResponseForGetAllAsync(pageIndex, pageSize);
            return ListResultFromResponse(response);
        }

        public override async Task<IKeyedDto> GetByKeyAsync(string key)
        {
            var response = await ResponseForGetByKey(key);
            return ResultFromResponse(response);
        }

        public async Task<OperationResult<MediumDto>> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            var key = _keyService.ConstructMediumKey(title, year, mediumType);
            var m = (KeyedMediumDto)await GetByKeyAsync(key);
            var val = new MediumDto(m.Title, m.Year, m.MediumType, m.Location, m.HasGermanSubtitles);
            return new OperationResult<MediumDto>(OKStatus, val);
        }

        public override async Task<IKeyedDto> GetLastEntryAsync()
        {
            var response = await ResponseForLastEntryAsync();
            return ResultFromResponse(response);
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        public override async Task UpdateAsync(MediumDto dto)
        {
            var response = await ResponseForUpdateAsync(dto);
            VoidResultFromResponse(response);
            return;
        }

        protected override List<IKeyedDto> ListResultFromResponse(string response)
        {
            List<IKeyedDto> result = default;
            var res = JsonConvert.DeserializeObject<OperationResult<List<KeyedMediumDto>>>(response);
            var s = res.Status;
            if (s == OKStatus)
            {
                result = new List<IKeyedDto>();
                var list = res.Value;
                foreach (var k in list)
                {
                    result.Add((IKeyedDto)k);
                }
            }
            return result;
        }

        protected override IBaseDto RecoverBaseDto(BaseDto dto)
        {
            var m = (MediumDto)dto;
            return new BaseMediumDto(m.Title, m.Year, m.MediumType, m.Location, m.GermanSubtitles);
        }

        protected override IKeyedDto ResultFromResponse(string response)
        {
            var res = JsonConvert.DeserializeObject<OperationResult<KeyedMediumDto>>(response);
            return res.Value;                        
        }
    }
}
