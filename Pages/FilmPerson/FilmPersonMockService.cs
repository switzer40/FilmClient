using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonMockService : BaseMockService<FilmPersonDto>, IFilmPersonService
    {
        public Task<OperationResult<List<IKeyedDto>>> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<IKeyedDto>>> GetByTitleYearAndRoleAsync(string title, short year, string role)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }

        protected override IKeyedDto RetrieveKeyedDto(FilmPersonDto t)
        {
            throw new NotImplementedException();
        }

        protected override void SpecificCopy(IKeyedDto target, FilmPersonDto source)
        {
            throw new NotImplementedException();
        }
    }
}
