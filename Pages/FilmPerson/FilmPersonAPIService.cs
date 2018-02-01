using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonAPIService : BaseService<FilmPersonDto>, IFilmPersonService
    {
        public FilmPersonAPIService(IErrorService eservice) : base(eservice)
        {
        }
        public override async Task<OperationResult> AddAsync(FilmPersonDto dto)
        {
            _action = "Add";
            var route = ComputeRoute();
            HttpContent jsonContent = ContentFromDto(dto);
            var response = await _client.PostAsync(route, jsonContent);
            return ResultFromResponse(response);
        }

        public override async Task<int> CountAsync()
        {
            var filmPeople = await GetAllAsync();
            return filmPeople.Count;
        }

        public override async Task<OperationResult> DeleteAsync(string key)
        {
            _action = "Delete";
            var route = ComputeRoute();
            var response = await _client.DeleteAsync(route);
            return ResultFromResponse(response);
        }

        public override async Task<List<FilmPersonDto>> GetAllAsync()
        {
            _action = "GetAll";
            var route = ComputeRoute();
            var result = new List<FilmPersonDto>();
            var response = await _client.GetAsync(route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<List<FilmPersonDto>>(stringResponse);
            return result;
        }

        public override async Task<OperationResult> GetByKeyAsync(string key)
        {
            _action = "GetByKey";
            var route = ComputeRoute(key);
            var list = new List<IKeyedDto>();
            var response = await _client.GetAsync(route);
            var result = ResultFromResponse(response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var retVal = JsonConvert.DeserializeObject<KeyedFilmPersonDto>(stringResponse);                
                list.Add(retVal);
            }            
            result.ResultValue = list;
            return result;

        }

        public override string KeyFrom(FilmPersonDto dto)
        {
            return _keyService.ConstructFilmPersonKey(dto.Title,
                                                      dto.Year,
                                                      dto.LastName,
                                                      dto.Birthdate,
                                                      dto.Role);
        }

        public override async Task<OperationResult> UpdateAsync(FilmPersonDto dto)
        {
            _action = "Edit";
            var route = ComputeRoute();
            var jsonContent = ContentFromDto(dto);
            var response = await _client.PutAsync(route, jsonContent);
            return ResultFromResponse(response);
        }

        protected override IBaseDto ArgFromDto(BaseDto dto)
        {
            
            var b = (FilmPersonDto)dto;
             return new BaseFilmPersonDto(b.Title, b.Year, b.LastName, b.Birthdate, b.Role);            
        }
    }
}
