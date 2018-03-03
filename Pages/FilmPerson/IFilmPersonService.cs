using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public interface IFilmPersonService : IService<FilmPersonDto>
    {
        Task<OperationResult<List<IKeyedDto>>> GetByTitleYearAndRoleAsync(string title, short year, string role);
        Task<OperationResult<List<IKeyedDto>>> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role);
    }
}
