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
        public override Task<PaginatedList<PersonDto>> CurrentPageAsync(int pageIndex, int pageSize)
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

        public override void SetController(string controller)
        {
            throw new NotImplementedException();
        }

        protected override IKeyedDto RetrieveKeyedDto(PersonDto t)
        {
            throw new NotImplementedException();
        }

        protected override void SpecificCopy(IKeyedDto target, PersonDto source)
        {
            throw new NotImplementedException();
        }
    }
}
