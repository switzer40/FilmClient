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
        public override IKeyedDto Add(FilmDto t)
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

        public Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(FilmDto dto)
        {
            throw new NotImplementedException();
        }

        public override void Update(FilmDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
