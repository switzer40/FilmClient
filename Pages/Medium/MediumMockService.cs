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
        public override Task<PaginatedList<MediumDto>> CurrentPageAsync(int pageIndex, int pageSize)
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

        public override void SetController(string controller)
        {
            throw new NotImplementedException();
        }

        protected override IKeyedDto RetrieveKeyedDto(MediumDto t)
        {
            throw new NotImplementedException();
        }

        protected override void SpecificCopy(IKeyedDto target, MediumDto source)
        {
            throw new NotImplementedException();
        }
    }
}
