using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Film
{
    public class EditModel : BasePageModel
    {
        private readonly IFilmService _service;
        
        public EditModel(IFilmService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Edit";
            _mainProperty = nameof(FilmToEdit);
            InitializeEditReasons();
        }
        [BindProperty]
        public FilmDto FilmToEdit { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var f = (KeyedFilmDto)await _service.GetByKeyAsync(key);
            if (f != null)
            {
                FilmToEdit = new FilmDto(f.Title, f.Year, f.Length);                
                return Page();
            }
            else
            {
                var s = OperationStatus.NotFound;
                return HandleError(s, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _service.UpdateAsync(FilmToEdit);
            return RedirectToPage("./Index");
        }
    }
}