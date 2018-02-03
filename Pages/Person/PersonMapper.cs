using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonMapper : IPersonMapper
    {
        private readonly IKeyService _keyService;
        public PersonMapper()
        {
            _keyService = new KeyService();
        }
        public IKeyedDto Map(BaseDto dto)
        {
            KeyedPersonDto result = null;
            if (dto.GetType() == typeof(PersonDto))
            {
                var p = (PersonDto)dto;
                var key = _keyService.ConstructPersonKey(p.LastName, p.BirthdateString);
                result = new KeyedPersonDto(p.LastName, p.BirthdateString, p.FirstMidName, key);
            }
            return result;
        }

        public BaseDto Mapback(IKeyedDto k)
        {
            PersonDto result = null;
            if (k.GetType() == typeof(KeyedPersonDto))
            {
                var dto = (KeyedPersonDto)k;
                result = new PersonDto(dto.LastName, dto.Birthdate, dto.FirstMidName);
            }
            return result;
        }
    }
}
