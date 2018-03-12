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
            var rawList = await _service.GetAllAsync(pageIndex.Value, PageSize);
            foreach (var k in rawList)
            {
                var fp = (KeyedFilmPersonDto)k;
                var p = await GetPersonAsync(fp.LastName, fp.Birthdate);
                var m = new FilmPersonModel(fp.Title, fp.Year, fp.LastName, p.FirstMidName, fp.Birthdate, fp.Role);
                items.Add(m);
            }
            
            FilmPeople = new PaginatedList<FilmPersonModel>(items, _totalRows, pageIndex.Value, PageSize);
        }

        private async Task InitDataAsync()
        {
            if (_totalRows > 0)
            {
                return; //initialize only once
            }
            _totalRows = await _service.CountAsync();
            CalculateRowData(_totalRows);
        }
        
        private async Task<PersonDto> GetPersonAsync(string lastName, string birthdate)
        {
            PersonDto result = null;
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var k = await _personService.GetByKeyAsync(key);
            var p = (KeyedPersonDto)k;

            if (p != null)
            {                
                result = new PersonDto(p.LastName,p.Birthdate, p.FirstMidName);
            }
            return result;
        }
    }
}