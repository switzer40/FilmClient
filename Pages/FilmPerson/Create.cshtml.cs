using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Services;

namespace FilmClient.Pages.FilmPerson
{
    public class CreateModel : BasePageModel
    {
        private readonly IFilmPersonService _service;
        public CreateModel(IFilmPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _mainProperty = nameof(FilmPersonToAdd);
            InitializeCreateReasons();
        }
        [BindProperty]
        public FilmPersonDto FilmPersonToAdd { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            FilmPersonToAdd = (await _service.GetAllAsync()).LastOrDefault();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var s = await _service.AddAsync(FilmPersonToAdd);
            if (s != OperationStatus.OK)
            {
                return HandleError(s, _action);
            }

            return RedirectToPage("./Index");
        }
    }
}