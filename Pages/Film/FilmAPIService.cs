using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmAPIService : BaseService<FilmDto>, IFilmService
    {
        public FilmAPIService() : base()
        {
            _controller = "Film";
        }
        public override async Task<IKeyedDto> AddAsync(FilmDto dto)
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
            await ResponseForDeleteAsync(key);
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

        public async Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            var key = _keyService.ConstructFilmKey(title, year);
            var f = (KeyedFilmDto)await GetByKeyAsync(key);
            var val = new FilmDto(f.Title, f.Year, f.Length);
            return new OperationResult<FilmDto>(OperationStatus.OK, val);

        }

        public override async Task<IKeyedDto> GetLastEntryAsync()
        { 
            var response = await ResponseForLastEntryAsync();
            return ResultFromResponse(response);
        }

        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        public override async Task UpdateAsync(FilmDto dto)
        {
            await ResponseForUpdateAsync(dto);
            return;
        }
        protected override List<IKeyedDto> ListResultFromResponse(string response)
        {
            List<IKeyedDto> result = default;
            var res = JsonConvert.DeserializeObject<OperationResult<List<KeyedFilmDto>>>(response);
            var status = res.Status;
            if (status == OKStatus)
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
            var f = (FilmDto)dto;
            return new BaseFilmDto(f.Title, f.Year, f.Length);
        }

        protected override IKeyedDto ResultFromResponse(string response)
        {
            IKeyedDto result = default;
            var res = JsonConvert.DeserializeObject<OperationResult<KeyedFilmDto>>(response);
            var s = res.Status;
            if (s == OKStatus)
            {
                result = (IKeyedDto)res.Value;
            }
            else
            {
                HandleError(s);
            }
            return result;
        }
    }
}
