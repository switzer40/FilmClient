using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.FilmPerson
{
    public class EditModel : BasePageModel
    {
        private readonly IFilmPersonService _service;
        public EditModel(IFilmPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Edit";
            _mainProperty = nameof(FilmPersonToEdit);
            InitializeEditReasons();
        }
        [BindProperty]
        public FilmPersonDto FilmPersonToEdit { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var fp = (KeyedFilmPersonDto) await _service.GetByKeyAsync(key);
            if (fp != null)
            {
                FilmPersonToEdit = new FilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
                return Page();
            }
            else
            {
                return HandleError(NotFoundStatus, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _service.UpdateAsync(FilmPersonToEdit);
            return RedirectToPage("./Index");
        }
    }
}