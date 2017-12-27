using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using FilmClient.Pages.Shared;

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

            var s = await _service.AddAsync(FilmToAdd);

            if (s != OperationStatus.OK)
            {
                return HandleError(s, _action);
            }

            return RedirectToPage("./Index");
        }
    }
}