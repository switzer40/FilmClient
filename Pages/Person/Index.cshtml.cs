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
            People = new List<PersonDto>();
            _totalRows = 0;
        }
        public List<PersonDto> People { get; set; }
        public async Task OnGetAsync()
        {
            await InitDataAsync();
            var res = await _service.GetAllAsync(_pageNumber, PageSize);
            List<IKeyedDto> people = default;
            if (res.Status == OperationStatus.OK)
            {
                people = res.Value;
                foreach (var p in people)
                {
                    var k= (KeyedPersonDto)p;
                    var val = new PersonDto(k.LastName, k.Birthdate, k.FirstMidName);
                    val.ShortBirthdate = DateTime.Parse(k.Birthdate).ToShortDateString();
                    val.Key = _keyService.ConstructPersonKey(k.LastName, k.Birthdate);
                    People.Add(val);
                }
            }            
        }

        private async Task InitDataAsync()
        {
            if (_totalRows > 0)
            {
                return; //initialize only once
            }
            var res = await _service.CountAsync();
            if (res.Status == OperationStatus.OK)
            {
                _totalRows = res.Value;
                CalculateRowData(_totalRows);
            }           
        }
    }
}