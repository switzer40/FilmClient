using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public interface IFilmPersonService : IService<FilmPersonDto>
    {
        Task<OperationStatus> DeleteRangeAsync(List<FilmPersonDto> filmPeopleToDete);
        Task<int> RelationCountForPersonAsync(string lastName, string birthdate);
        Task<List<FilmPersonDto>> RelationsForPersonAsync(string lastName, string birthdate);
        Task<OperationStatus> DeleteRelationsForPersonAsync(string lastName, string birthdate);
        Task<int> RelationCountForFilmAsync(string title, short year);
        Task<List<FilmPersonDto>> RelationsForFilmAsync(string title, short year);
        Task<OperationStatus> DeleteRelationsForFilmAsync(string title, short year);
    }
}
