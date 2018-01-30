using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Medium
{
    public class CreateModel : BasePageModel
    {
        private readonly IMediumService _service;


        public CreateModel(IMediumService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _mainProperty = nameof(MediumToAdd);
            InitializeCreateReasons();
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public MediumDto MediumToAdd { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var s = await _service.AddAsync(MediumToAdd);

            if (s == OperationStatus.OK)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return HandleError(s, _action);
            }
        }
    }
}