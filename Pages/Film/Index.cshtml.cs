using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace FilmClient.Pages.Film
{
    public class IndexModel : BasePageModel
    {
        private readonly IFilmService _service;

        public IndexModel(IFilmService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _totalRows = 0;
        }
        
        public PaginatedList<FilmDto> Films { get; set; }

        public async Task OnGetAsync(int? pageIndex = 0, string searchString = "")
        {
            await InitDataAsync();
            
            var items = new List<FilmDto>();
            var rawList = await _service.GetAllAsync(pageIndex.Value, PageSize);
            foreach (var k in rawList)
            {
                var f = (KeyedFilmDto)k;
                var val = new FilmDto(f.Title, f.Year, f.Length);
                items.Add(val);
            }
            Films = new PaginatedList<FilmDto>(items, _totalRows, pageIndex.Value, PageSize);
            Films.PageIndex = pageIndex.Value;
        }

        private async Task InitDataAsync()
        {
            if(_totalRows > 0)
            {
                return; //initialize only once
            }
            _totalRows = await _service.CountAsync();
            CalculateRowData(_totalRows);                     
        }
    }
}
