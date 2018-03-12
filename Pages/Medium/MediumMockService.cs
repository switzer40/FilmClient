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
        public override IKeyedDto Add(MediumDto t)
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

        public Task<OperationResult<MediumDto>> GetByTitleYearAndMediumTypeAsync(string title, short year, string mediumType)
        {
            throw new NotImplementedException();
        }

        public override string KeyFrom(MediumDto dto)
        {
            throw new NotImplementedException();
        }

        public override void Update(MediumDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
