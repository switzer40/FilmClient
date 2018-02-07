using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _totalRows = _service.Count();
            CalculateRowData(_totalRows);
        }

        public IList<FilmDto> Films { get; set; }

        public async Task OnGetAsync()
        {
            var films = await _service.GetAllAsync(_pageNumber, PageSize);
            Films = new List<FilmDto>();
            foreach (var f in films)
            {
                f.Key = _keyService.ConstructFilmKey(f.Title, f.Year);
                Films.Add(f);
            }
        }
        public IActionResult PreviousPage()
        {
            if (_pageNumber > 0)
            {
                _pageNumber--;
            }
            return Page();
        }
        public IActionResult NextPage()
        {
            if (_pageNumber < _numberOfPages)
            {
                _pageNumber++;
            }
            return Page();
        }
    }
}
