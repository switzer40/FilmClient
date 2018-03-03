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

        public IList<FilmDto> Films { get; set; }

        public async Task OnGetAsync()
        {
            await InitDataAsync();
            var res = await _service.GetAllAsync(_pageNumber, PageSize);
            List<IKeyedDto> films = default;
            if (res.Status == OperationStatus.OK)
            {
                films = res.Value;
            }
            Films = new List<FilmDto>();
            foreach (var f in films)
            {
                var k = (KeyedFilmDto)f;
                var val = new FilmDto(k.Title, k.Year, k.Length);
                Films.Add(val);
            }
        }

        private async Task InitDataAsync()
        {
            if(_totalRows > 0)
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
