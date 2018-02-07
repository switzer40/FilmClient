using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Medium;
using FilmClient.Pages.Shared;

namespace FilmClient.Pages.Medium
{
    public class IndexModel : BasePageModel
    {
        private readonly IMediumService _service;        
        public IndexModel(IMediumService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
            Media = new List<MediumDto>();
            _totalRows = _service.Count();
            CalculateRowData(_totalRows);
        }
        [BindProperty]
        public List<MediumDto> Media { get; set; }
        public async Task OnGetAsync()
        {
            var media = await _service.GetAllAsync(_pageNumber, PageSize);
            foreach (var m in media)
            {
                m.Key = _keyService.ConstructMediumKey(m.Title, m.Year, m.MediumType);
                Media.Add(m);
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