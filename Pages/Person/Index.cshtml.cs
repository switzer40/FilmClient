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
            var res = await _service.GetAllAsync(pageIndex.Value, PageSize);
            if (res.Status == OperationStatus.OK)
            {
                var rawList = res.Value;
                foreach (var k in rawList)
                {
                    var p = (KeyedPersonDto)k;
                    var dto = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
                    items.Add(dto);
                }
            }
            People = new PaginatedList<PersonDto>(items, _totalRows, pageIndex.Value, PageSize);
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