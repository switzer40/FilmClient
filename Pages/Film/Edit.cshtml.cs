using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
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
            var res = await _service.GetByKeyAsync(key);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var f = (KeyedFilmDto) res.ResultValue.Single();
                FilmToEdit.Title = f.Title;
                FilmToEdit.Year = f.Year;
                FilmToEdit.Length = f.Length;
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var res = await _service.UpdateAsync(FilmToEdit);
            
            if (res.Status == OperationStatus.OK)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return HandleError(res.Status, _action);
            }            
        }
    }
}