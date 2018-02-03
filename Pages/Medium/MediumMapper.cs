using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;

namespace FilmClient.Pages.Medium
{
    public class MediumMapper : IMediumMapper
    {
        private readonly IKeyService _keyService;
        public MediumMapper()
        {
            _keyService = new KeyService();
        }
        public IKeyedDto Map(BaseDto dto)
        {
            KeyedMediumDto result = null;
            if (dto.GetType() ==typeof(MediumDto))
            {
                var m = (MediumDto)dto;
                var key = _keyService.ConstructMediumKey(m.Title, m.Year, m.MediumType);
                result = new KeyedMediumDto(m.Title,
                                            m.Year,
                                            m.MediumType,
                                            m.Location,
                                            m.GermanSubtitles,
                                            key);
            }
            return result;
        }

        public BaseDto Mapback(IKeyedDto k)
        {

            MediumDto result = null;
            if (k.GetType() == typeof(KeyedMediumDto))
            {
                var dto = (KeyedMediumDto)k;
                result = new MediumDto(dto.Title,
                                       dto.Year,
                                       dto.MediumType,
                                       dto.Location,
                                       dto.HasGermanSubtitles);
            }
            return result;
        }
    }
}
