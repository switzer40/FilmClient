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
    }
}
