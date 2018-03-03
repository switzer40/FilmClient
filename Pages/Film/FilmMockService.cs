using FilmAPI.Common.DTOs;
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
        public async Task<OperationResult<FilmDto>> GetByTitleAndYearAsync(string title, short year)
        {
            FilmDto retVal = default;
            var key = _keyService.ConstructFilmKey(title, year);
            var res = await GetByKeyAsync(key);
            var dto = (KeyedFilmDto)res.Value;
            return new OperationResult<FilmDto>(OKStatus, retVal);
        }

        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        protected override IKeyedDto RetrieveKeyedDto(FilmDto t)
        {
            return new KeyedFilmDto(t.Title, t.Year, t.Length, KeyFrom(t));
        }

        protected override void SpecificCopy(IKeyedDto target, FilmDto source)
        {
            KeyedFilmDto specificTarget = (KeyedFilmDto)target;
            // Title and Year must be immutable.
            specificTarget.Length = source.Length;
        }
    }
}
