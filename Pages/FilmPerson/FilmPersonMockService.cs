using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonMockService : BaseMockService<FilmPersonDto>, IFilmPersonService
    {
        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }
    }
}
