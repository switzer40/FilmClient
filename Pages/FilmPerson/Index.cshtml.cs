using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.FilmPerson
{
    public class IndexModel : BasePageModel
    {
        private readonly IFilmPersonService _service;
        private readonly IPersonService _personService;
        
        
        public IndexModel(IFilmPersonService service, IPersonService pservice, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _personService = pservice;
            _keyService = new KeyService();
            _totalRows = 0;
            
        }
        
        public PaginatedList<FilmPersonModel> FilmPeople { get; set; }


        public async Task OnGetAsync(int? pageIndex = 0)
        {
            await InitDataAsync();
            var items = new List<FilmPersonModel>();
            var res = await _service.GetAllAsync(pageIndex.Value, PageSize);
            if (res.Status == OperationStatus.OK)
            {
                var rawList = res.Value;
                foreach (var k in rawList)
                {
                    var fp = (KeyedFilmPersonDto)k;
                    var p = await GetPersonAsync(fp.LastName, fp.Birthdate);
                    var model = new FilmPersonModel(fp.Title,
                                                    fp.Year, fp.LastName,
                                                    p.FirstMidName,
                                                    fp.Birthdate,
                                                    fp.Role);
                    items.Add(model);
                }
            }
            FilmPeople = new PaginatedList<FilmPersonModel>(items, _totalRows, pageIndex.Value, PageSize);
        }

        private async Task InitDataAsync()
        {
            if (_totalRows > 0)
            {
                return; //initialize only once
            }
            var res = await _service.CountAsync();
            _totalRows = (res.Status == OperationStatus.OK) ? res.Value : 0;
            CalculateRowData(_totalRows);
        }
        
        private async Task<PersonDto> GetPersonAsync(string lastName, string birthdate)
        {
            PersonDto result = null;
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await _personService.GetByKeyAsync(key);
            var s = res.Status;

            if (s == OperationStatus.OK)
            {
                var p = (KeyedPersonDto) res.Value;
                result = new PersonDto(p.LastName,p.Birthdate, p.FirstMidName);
            }
            return result;
        }
    }
}