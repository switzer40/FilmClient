using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonMockService : BaseMockService<PersonDto>, IPersonService
    {
        public async Task<PersonDto> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            return (await GetAllAsync()).Where(p => p.LastName == lastName && p.BirthdateString == birthdate).SingleOrDefault();
        }

        public override string KeyFrom(PersonDto dto)
        {
            return _keyService.ConstructPersonKey(dto.LastName, dto.BirthdateString);
        }

        protected override IKeyedDto RetrieveKeyedDtoFrom(PersonDto t)
        {
            var key = KeyFrom(t);
            return new KeyedPersonDto(t.LastName, t.BirthdateString, t.FirstMidName, key);
        }
    }
}
