using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonAPIService : BaseService<PersonDto>, IPersonService
    {        
        public PersonAPIService() : base()
        {
            _controller = "Person";
        }
        public override async Task<IKeyedDto> AddAsync(PersonDto dto)
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

        public async Task<OperationResult<PersonDto>> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var p = (KeyedPersonDto) await GetByKeyAsync(key);
            var dto = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
            return new OperationResult<PersonDto>(OKStatus, dto);
        }

        public override async Task<IKeyedDto> GetLastEntryAsync()
        {
            var response = await ResponseForLastEntryAsync();
            return ResultFromResponse(response);
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        public override async Task UpdateAsync(PersonDto dto)
        {
            var response = await ResponseForUpdateAsync(dto);
            VoidResultFromResponse(response);
            return;
        }

        protected override List<IKeyedDto> ListResultFromResponse(string response)
        {
            List<IKeyedDto> result = new List<IKeyedDto>();
            var res = JsonConvert.DeserializeObject<OperationResult<List<KeyedPersonDto>>>(response);
            var status = res.Status;
            if (status == OKStatus)
            {
                var rawList = res.Value;
                foreach (var k in rawList)
                {
                    result.Add((IKeyedDto)k);
                }
            }
            else
            {
                HandleError(status);
            }            
            return result;
        }

        protected override IBaseDto RecoverBaseDto(BaseDto dto)
        {
            var b = (PersonDto)dto;
            return new BasePersonDto(b.LastName, b.BirthdateString, b.FirstMidName);
        }

        protected override IKeyedDto ResultFromResponse(string response)
        {
            return (IKeyedDto)(JsonConvert.DeserializeObject<OperationResult<KeyedPersonDto>>(response)).Value;
        }
    }
}
