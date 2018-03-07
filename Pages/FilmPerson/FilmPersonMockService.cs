using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonMockService : BaseMockService<FilmPersonModel>, IFilmPersonService
    {
        public OperationResult<IKeyedDto> Add(FilmPersonDto t)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IKeyedDto>> AddAsync(FilmPersonDto dto)
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

        public string KeyFrom(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> KeyFromAsync(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }

        public OperationStatus Update(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationStatus> UpdateAsync(FilmPersonDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
