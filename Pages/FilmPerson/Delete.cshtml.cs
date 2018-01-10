using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.FilmPerson
{
    public class DeleteModel : BasePageModel
    {
        private readonly IFilmPersonService _service;        
        public DeleteModel(IFilmPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
        }
        [BindProperty]
        public FilmPersonDto FilmPersonToDelete { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }
            var s = await _service.GetByKeyAsync(key);

            if (s == OperationStatus.OK)
            {
                FilmPersonToDelete = _service.GetByKeyResult(key);
                return Page();
            }
            else
            {
                return HandleError(s, "Get");
            }
        }
        public async Task<IActionResult> OnPostAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            var s = await _service.DeleteAsync(key);

            if (s == OperationStatus.OK)
            {
                return RedirectToPage("./index");
            }

            return RedirectToPage("/Error");
        }
    }
}