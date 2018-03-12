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
        public override IKeyedDto Add(FilmPersonDto t)
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

        public override void Update(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
