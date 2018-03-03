using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public class MediumMockService : BaseMockService<MediumDto>, IMediumService
    {
        public Task<OperationResult<MediumDto>> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(MediumDto dto)
        {
            return _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
        }

        protected override IKeyedDto RetrieveKeyedDto(MediumDto t)
        {
            return new KeyedMediumDto(t.Title, t.Year, t.MediumType, t.Location, t.GermanSubtitles, KeyFrom(t));

        }

        protected override void SpecificCopy(IKeyedDto target, MediumDto source)
        {
            KeyedMediumDto specificTarget = (KeyedMediumDto)target;
            // Title,Year and MediumType must be immutable.
            specificTarget.Location = source.Location;
            specificTarget.HasGermanSubtitles = source.GermanSubtitles;
        }
    }
}
