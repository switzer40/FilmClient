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
        public PaginatedList<PersonDto> People { get; set; }
        public async Task OnGetAsync(int? pageIndex =0)
        {
            await InitDataAsync();
            var items = new List<PersonDto>();
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
;                    items.Add(val);
                }
            }
            else
            {
                HandleError(NotFoundStatus, _action);
            }         
            People = new PaginatedList<PersonDto>(items, _totalRows, pageIndex.Value, PageSize);
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