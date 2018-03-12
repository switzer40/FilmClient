using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        public FilmPersonAPIService()
        {
            _controller = "FilmPerson";
        }
        public override async Task<IKeyedDto> AddAsync(FilmPersonDto dto)
        {
            var response = await ResponseForAddAsync(dto);
            if (!string.IsNullOrEmpty(response))
            {
                return ResultFromResponse(response);
            }
            else
            {
                return null;
            }             
        }

        public override async Task<int> CountAsync()
        {
            var response = await ResponseForCountAsync();
            return IntResultFromResponse(response);
        }

        public override async Task DeleteAsync(string key)
        {
            var response = await ResponseForDeleteAsync(key);
            return;
        }

        public override async Task<List<IKeyedDto>> GetAbsolutelyAllAsync()
        {
            var response = await ResponseForGetAbsolutelyAllAsync();
            return ListResultFromResponse(response);
        }

        private List<IKeyedDto> MapList(List<KeyedFilmPersonDto> list)
        {
            List<IKeyedDto> result = new List<IKeyedDto>();
            foreach (var k in list)
            {
                result.Add((IKeyedDto)k);
            }
            return result;
        }

        public override async Task<List<IKeyedDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var respone = await ResponseForGetAllAsync(pageIndex, pageSize);
            return ListResultFromResponse(respone);
        }

        public override async Task<IKeyedDto> GetByKeyAsync(string key)
        {
            var response = await ResponseForGetByKey(key);
            return ResultFromResponse(response);
        }

        public override async Task<IKeyedDto> GetLastEntryAsync()
        {
            var response = await ResponseForLastEntryAsync();
            return ResultFromResponse(response);
        }
        
        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }

        public override async Task UpdateAsync(FilmPersonDto dto)
        {
            var response = await ResponseForUpdateAsync(dto);
            return;
        }

        protected override IBaseDto RecoverBaseDto(BaseDto dto)
        {
            var fp = (FilmPersonDto)dto;
            return new BaseFilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
        }

        protected override IKeyedDto ResultFromResponse(string response)
        {
            return (IKeyedDto)(JsonConvert.DeserializeObject<OperationResult<KeyedFilmPersonDto>>(response)).Value;
        }

        protected override List<IKeyedDto> ListResultFromResponse(string response)
        {
            List<IKeyedDto> result = new List<IKeyedDto>();
            var res = JsonConvert.DeserializeObject<OperationResult<List<KeyedFilmPersonDto>>>(response);
            if (res.Status == OKStatus)
            {
                var rawList = res.Value;
                result = MapList(rawList);
            }
            else
            {
                throw new Exception(BuildExplanation(res.Status));
            }
            return result;
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetByTitleYearAndRoleAsync(string title, short year, string role)
        {
            List<IKeyedDto> retVal = new List<IKeyedDto>();
            var list = await GetAbsolutelyAllAsync();
            foreach (var k in list)
            {
                var fp = (KeyedFilmPersonDto)k;
                if (fp.Title == title && fp.Year == year && fp.Role == role)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(OKStatus, retVal);
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role)
        {
            List<IKeyedDto> retVal = new List<IKeyedDto>();
            var list = await GetAbsolutelyAllAsync();
            foreach (var k in list)
            {
                var fp = (KeyedFilmPersonDto)k;
                if (fp.LastName == lastName && fp.Birthdate== birthdate & fp.Role == role)
                {
                    retVal.Add(k);
                }
            }
            return new OperationResult<List<IKeyedDto>>(OKStatus, retVal);
        }
    }
}
