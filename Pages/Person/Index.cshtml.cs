using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using FilmClient.Pages.Error;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.FilmPerson;
using FilmAPI.Common.DTOs;

namespace FilmClient.Pages.Person
{
    public class IndexModel : BasePageModel
    {
        private readonly IPersonService _service;

        public IndexModel(IPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
            _totalRows = 0;
        }
       [BindProperty]
        public string Filter { get; set; }
        public PaginatedList<PersonDto> People { get; set; }
        public async Task OnGetAsync( string searchString, string filter, int? pageIndex =0)
        {
            
            var items = new List<PersonDto>();
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewData["Filter"] = searchString;
            }
            var pageSize = PageSize;
            if (!string.IsNullOrEmpty(searchString))
            {
                var rawList = await _service.GetAbsolutelyAllAsync();
                foreach (var k in rawList)
                {
                    var p = (KeyedPersonDto)k;
                    if (p.LastName.Contains(searchString) || p.FirstMidName.Contains(searchString))
                    {
                        var val = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
                        val.ShortBirthdate = DateTime.Parse(p.Birthdate).ToShortDateString();
                        items.Add(val);
                        _totalRows = items.Count;
                        pageSize = _totalRows;
                    }
                }
            }
            else
            {
                await InitDataAsync();
                var rawList = await _service.GetAllAsync(pageIndex.Value, PageSize);
                if (rawList != null)
                {
                    foreach (var k in rawList)
                    {
                        var p = (KeyedPersonDto)k;
                        var val = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
                        val.Key = _keyService.ConstructPersonKey(p.LastName, p.Birthdate);
                        DateTime birth = DateTime.Parse(val.BirthdateString);
                        val.ShortBirthdate = birth.ToShortDateString();
                        ; items.Add(val);
                    }
                }
            }
            People = new PaginatedList<PersonDto>(items, _totalRows, pageIndex.Value, pageSize);
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
    }
}