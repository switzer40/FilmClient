using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmClient.Pages.Shared;
using FilmClient.Pages.Medium;
using FilmClient.Pages.FilmPerson;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.Film
{
    public class DeleteModel : BasePageModel
    {
        private readonly IFilmService _service;
        private readonly IMediumService _mediumService;
        private readonly IFilmPersonService _filmPersonService;
        
        public DeleteModel(IFilmService service, IMediumService mservice, IFilmPersonService fservice, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _mediumService = mservice;
            _filmPersonService = fservice;
            _action = "Delete";
            _mainProperty = nameof(FilmToDelete);
            InitializeDeleteReasons();
        }

        [BindProperty]
        public FilmDto FilmToDelete { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            var f = (KeyedFilmDto)await _service.GetByKeyAsync(key);
            if (f != null)
            {
                FilmToDelete = new FilmDto(f.Title, f.Year, f.Length);               
                return Page();
            }
            else
            {
                var s = OperationStatus.NotFound;
                return HandleError(s, _action);
            }            
        }

        public async Task<IActionResult> OnPostAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            await _service.DeleteAsync(key);
            return RedirectToPage("../Index");
        }
    }
}
