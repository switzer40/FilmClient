using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using FilmClient.Pages.Shared;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.Film
{
    public class CreateModel : BasePageModel
    {
        private readonly IFilmService _service;


        public CreateModel(IFilmService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _mainProperty = nameof(FilmToAdd);
            InitializeCreateReasons();
        }

        

        public IActionResult OnGet()
        {
            FilmToAdd = new FilmDto();           
            return Page();
        }

        [BindProperty]
        public FilmDto FilmToAdd { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var f =  (KeyedFilmDto)await _service.AddAsync(FilmToAdd);
            if (f != null)
            {
                FilmToAdd = new FilmDto(f.Title, f.Year, f.Length);
            }
            else
            {
                HandleError(OperationStatus.NotFound, "Add");
            }

            return RedirectToPage("./Index");
        }
    }
}