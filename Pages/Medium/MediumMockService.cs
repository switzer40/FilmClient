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
    }
}
