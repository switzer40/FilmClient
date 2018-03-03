using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonMockService : BaseMockService<PersonDto>, IPersonService
    {
        public async Task<OperationResult<PersonDto>> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await GetByKeyAsync(key);
            var k = (KeyedPersonDto)res.Value;
            var retVal = new PersonDto(k.LastName, k.Birthdate, k.FirstMidName);
            return new OperationResult<PersonDto>(OKStatus, retVal);
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        protected override IKeyedDto RetrieveKeyedDto(PersonDto t)
        {
            return new KeyedPersonDto(t.LastName, t.BirthdateString, t.FirstMidName, KeyFrom(t));
        }

        protected override void SpecificCopy(IKeyedDto target, PersonDto source)
        {
            KeyedPersonDto specificTarget = (KeyedPersonDto)target;
            // LastName and BirthdateString must be immutable.
            specificTarget.FirstMidName = source.FirstMidName;
        }
    }
}
