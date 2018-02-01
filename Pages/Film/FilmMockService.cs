using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmMockService : BaseMockService<FilmDto>, IFilmService
    {
        public override string KeyFrom(FilmDto dto)
        {
            return _keyService.ConstructFilmKey(dto.Title, dto.Year);
        }

        protected override IKeyedDto RetrieveKeyedDtoFrom(FilmDto t)
        {
            var key = KeyFrom(t);
            return new KeyedFilmDto(t.Title, t.Year, t.Length, key);
        }
    }
}
