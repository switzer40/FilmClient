﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var s = await _service.GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                FilmToEdit = _service.GetByKeyResult(key);
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
            var s = await _service.UpdateAsync(FilmToEdit);
            
            if (s == OperationStatus.OK)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return HandleError(s, _action);
            }            
        }
    }
}