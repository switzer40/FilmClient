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
        public override IKeyedDto Add(PersonDto t)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public override List<IKeyedDto> GetAbsolutelyAll()
        {
            throw new NotImplementedException();
        }

        public override IKeyedDto GetByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<PersonDto>> GetByLastNameAndBirthdateAsync(string lastName, string birthdate)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(PersonDto dto)
        {
            throw new NotImplementedException();
        }

        public override void Update(PersonDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
