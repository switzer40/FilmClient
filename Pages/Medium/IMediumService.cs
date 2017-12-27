using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public interface IMediumService : IService<MediumDto>
    {
        Task<OperationStatus> DeleteMediaRangeAsync(List<MediumDto> mediaToDelete);
        Task<bool> HasMediumForFilmAsync(string title, short year);
        Task<List<MediumDto>> MediaForFilmAsync(string title, short year);
        Task<OperationStatus> DeleteMediaForFilmAsync(string title, short year);
    }
}
