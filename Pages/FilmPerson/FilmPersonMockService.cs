using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonMockService : BaseMockService<FilmPersonDto>, IFilmPersonService
    {
        public OperationResult GetByLastNameBirthdateAndRole(string lastName, string birthdate, string role)
        {
            var list = _store.Values.ToList();
            var filmPeople = list.Where(fp => fp.LastName == lastName && fp.Birthdate == birthdate && fp.Role == role);
            var retVal = new List<IKeyedDto>();
            foreach (var fp in filmPeople)
            {
                var key = _keyService.ConstructFilmPersonKey(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
                var val = new KeyedFilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role, key);
                retVal.Add(val);
            }
            return new OperationResult(OperationStatus.OK, retVal);
        }

        public async Task<OperationResult> GetByLastNameBirthdateAndRoleAsync(string lastName, string birthdate, string role)
        {
            return await Task.Run(() => GetByLastNameBirthdateAndRole(lastName, birthdate, role));

        }

        public OperationResult GetByTitleYearAndRole(string title, short year, string role)
        {
            var list = _store.Values.ToList();
            var filmPeople = list.Where(fp => fp.Title == title && fp.Year == year && fp.Role == role);
            var retVal = new List<IKeyedDto>();
            foreach (var fp in filmPeople)
            {
                var key = _keyService.ConstructFilmPersonKey(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
                var val = new KeyedFilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role, key);
                retVal.Add(val);
            }
            return new OperationResult(OperationStatus.OK, retVal);
        }

        public async Task<OperationResult> GetByTitleYearAndRoleAsync(string title, short year, string role)
        {
            return await Task.Run(() => GetByTitleYearAndRole(title, year, role));
        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }

       

        protected override IKeyedDto RetrieveKeyedDtoFrom(FilmPersonDto t)
        {
            var key = KeyFrom(t);
            return new KeyedFilmPersonDto(t.Title,
                                          t.Year,
                                          t.LastName,
                                          t.Birthdate,
                                          t.Role,
                                          key);
        }
    }
}
