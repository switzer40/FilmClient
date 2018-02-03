using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmMapper : IFilmMapper
    {
        private readonly IKeyService _keyService;
        public FilmMapper()
        {
            _keyService = new KeyService();
        }
        public IKeyedDto Map(BaseDto b)
        {
            KeyedFilmDto result = null;
            if (b.GetType() == typeof(FilmDto))
            {
                var dto = (FilmDto)b;
                var key = _keyService.ConstructFilmKey(dto.Title, dto.Year);
                result = new KeyedFilmDto(dto.Title, dto.Year, dto.Length, key);
            }
            return result;
        }

        public BaseDto Mapback(IKeyedDto k)
        {
            FilmDto result = null;
            if (k.GetType() == typeof(KeyedFilmDto))
            {
                var dto = (KeyedFilmDto)k;
                result = new FilmDto(dto.Title, dto.Year, dto.Length);
            }
            return result;
        }
    }
}
