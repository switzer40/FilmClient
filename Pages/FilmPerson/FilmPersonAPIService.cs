using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Core.Specifications;
using FilmClient.Pages.Film;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        private FilmPersonDto _addResult;
        private Dictionary<string, FilmPersonDto> _getResults;
        private readonly IFilmService _filmService;

        public HttpContent jsonContent { get; private set; }

        public FilmPersonAPIService(IFilmService fservice, IErrorService eservice) : base(eservice)
        {
            _filmService = fservice;
            _addResult = null;
            _getResults = new Dictionary<string, FilmPersonDto>();
            _route = FilmConstants.FilmPersonUri;
        }
        public override async Task<OperationStatus> AddAsync(FilmPersonDto dto)
        {
            var result = OperationStatus.OK;
            // Do not permit duplicates.
            var key = _keyService.ConstructFilmPersonKey(dto.Title,
                                                         dto.Year,
                                                         dto.LastName,
                                                         dto.Birthdate,
                                                         dto.Role);
            var s = await GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                result = OperationStatus.BadRequest;
                result.ReasonForFailure = "A corresponding relation exists already";
                return result;
            }

            var b = new BaseFilmPersonDto(dto.Title,
                                          dto.Year,
                                          dto.LastName,
                                          dto.Birthdate,
                                          dto.Role);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_route, jsonContent);
            result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var key1 = _keyService.ConstructFilmPersonKey(dto.Title, dto.Year, dto.LastName, dto.Birthdate, dto.Role);
                var response1 = await _client.GetAsync($"{_route}/{key1}");
                var stringResponse = await response1.Content.ReadAsStringAsync();
                _addResult = JsonConvert.DeserializeObject<FilmPersonDto>(stringResponse);
                _addResult.Key = key;
            }
            else
            {
                _addResult = null;
            }
            return result;
        }

        public override FilmPersonDto AddResult()
        {
            return _addResult;
        }

        public override async Task<OperationStatus> DeleteAsync(string key)
        {
            var response = await _client.DeleteAsync($"{_route}/{key}");
            return StatusFromResponse(response);
        }

        public override async Task<List<FilmPersonDto>> GetAllAsync()
        {
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var rawFilmPeople = JsonConvert.DeserializeObject<List<KeyedFilmPersonDto>>(stringResponse);
            var result = new List<FilmPersonDto>();
            foreach (var k in rawFilmPeople)
            {
                var dto = new FilmPersonDto(k.Title,
                                            k.Year,
                                            k.LastName,
                                            k.Birthdate,
                                            k.Role);
                dto.Key = _keyService.ConstructFilmPersonKey(k.Title,
                                                             k.Year,
                                                             k.LastName,
                                                             k.Birthdate,
                                                             k.Role);
                result.Add(dto);
            }
            return result;
        }

        public override async Task<OperationStatus> GetByKeyAsync(string key)
        {
            var response = await _client.GetAsync($"{_route}/{key}");
            var result = StatusFromResponse(response);
            if (result == OperationStatus.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                _getResults[key] = JsonConvert.DeserializeObject<FilmPersonDto>(stringResponse);
                _getResults[key].Key = key;
            }
            else
            {
                _getResults[key] = null;
            }
            return result;
        }

        public override FilmPersonDto GetByKeyResult(string key)
        {
            if (_getResults.ContainsKey(key))
            {
                return _getResults[key];
            }
            else
            {
                return null;
            }
        }

        public override async Task<OperationStatus> UpdateAsync(FilmPersonDto dto)
        {
            var b = new BaseFilmPersonDto(dto.Title,
                                           dto.Year,
                                           dto.LastName,
                                           dto.Birthdate,
                                           dto.Role);
            var response = await _client.PutAsync(_route, jsonContent);
            return StatusFromResponse(response);
        }

        public async Task CascadeDeleteForFilmAsync(string title, short year)
        {
            var filmPeople = await GetAllAsync();
            var toBeDeleted = filmPeople.Where(fp => fp.Title == title && fp.Year == year);
            await DeleteRangeAsync(toBeDeleted);

        }

        private async Task DeleteRangeAsync(IEnumerable<FilmPersonDto> toBeDeleted)
        {
            foreach (var dto in toBeDeleted)
            {
                var key = _keyService.ConstructFilmPersonKey(dto.Title, dto.Year, dto.LastName, dto.Birthdate, dto.Role);
                var s = await DeleteAsync(key);
            }
        }


        public async Task<int> RelationCountForPersonAsync(string lastName, string birthdate)
        {
            return (await RelationsForPersonAsync(lastName, birthdate)).Count;
        }

        public async Task<List<FilmPersonDto>> RelationsForPersonAsync(string lastName, string birthdate)
        {
            var filmPeople = await GetAllAsync();
            var spec = new FilmPersonDtoByLastNameAndBirthdate(lastName, birthdate);
            return filmPeople.Where(spec.Predicate).ToList();
        }

        public async Task<OperationStatus> DeleteRelationsForPersonAsync(string lastName, string birthdate)
        {
            var relationsToDelete = await RelationsForPersonAsync(lastName, birthdate);
            return await DeleteRangeAsync(relationsToDelete);
        }

        public async Task<int> RelationCountForFilmAsync(string title, short year)
        {
            return (await RelationsForFilmAsync(title, year)).Count;
        }

        public async Task<List<FilmPersonDto>> RelationsForFilmAsync(string title, short year)
        {
            var filmPeople = await GetAllAsync();
            var spec = new FilmPersonDtoByTitleAndYear(title, year);
            return filmPeople.Where(spec.Predicate).ToList();
        }

        public async Task<OperationStatus> DeleteRelationsForFilmAsync(string title, short year)
        {
            var relationsToDelete = await RelationsForFilmAsync(title, year);
            return  await DeleteRangeAsync(relationsToDelete);
        }

        public async Task<OperationStatus> DeleteRangeAsync(List<FilmPersonDto> filmPeopleToDete)
        {
            var result = OperationStatus.OK;
            foreach (var fp in filmPeopleToDete)
            {                
                var s = await DeleteAsync(fp.Key);
                if (s != OperationStatus.OK)
                {
                    result = s;
                    break;
                }
            }
            return result;
        }
    }
}
