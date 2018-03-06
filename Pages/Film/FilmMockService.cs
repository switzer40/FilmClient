using FilmAPI.Common.Interfaces;
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
        public override Task<PaginatedList<FilmDto>> CurrentPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(FilmDto dto)
        {
            throw new NotImplementedException();
        }

        public override void SetController(string controller)
        {
            throw new NotImplementedException();
        }

        protected override IKeyedDto RetrieveKeyedDto(FilmDto t)
        {
            throw new NotImplementedException();
        }

        protected override void SpecificCopy(IKeyedDto target, FilmDto source)
        {
            throw new NotImplementedException();
        }
    }
}
