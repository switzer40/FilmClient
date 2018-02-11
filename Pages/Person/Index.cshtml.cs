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

namespace FilmClient.Pages.Person
{
    public class IndexModel : BasePageModel
    {
        private readonly IPersonService _service;
        
        public IndexModel(IPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
            People = new List<PersonDto>();
            _totalRows = 0;
        }
        public List<PersonDto> People { get; set; }
        public async Task OnGetAsync()
        {
            await InitDataAsync();
            var people = await _service.GetAllAsync(_pageNumber, PageSize);
            foreach (var p in people)
            {
                p.ShortBirthdate = DateTime.Parse(p.BirthdateString).ToShortDateString();
                p.Key = _keyService.ConstructPersonKey(p.LastName, p.BirthdateString);
                People.Add(p);
            }
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