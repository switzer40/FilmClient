using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmMockService : BaseMockService<FilmDto>, IFilmService
    {
        public Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            throw new NotImplementedException();
        }
    }
}
