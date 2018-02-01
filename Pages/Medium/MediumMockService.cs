using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Film;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public class MediumMockService : BaseMockService<MediumDto>, IMediumService
    {        
        public MediumMockService() : base()
        {            
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        protected override IKeyedDto RetrieveKeyedDtoFrom(MediumDto t)
        {
            var key = KeyFrom(t);
            return new KeyedMediumDto(t.Title, t.Year, t.MediumType, t.Location, t.GermanSubtitles, key);
        }
    }
}
