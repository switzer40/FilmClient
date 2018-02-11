using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.Person
{
    public class CreateModel : BasePageModel
    {
        private readonly IPersonService _service;        
        public CreateModel(IPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _mainProperty = nameof(PersonToAdd);
            InitializeCreateReasons();
            _keyService = new KeyService();
        }
        [BindProperty]
        public PersonDto PersonToAdd { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var res = await _service.AddAsync(PersonToAdd);
            var s = res.Status;
            if (s != OperationStatus.OK)
            {
                return HandleError(s, _action);
            }

            return RedirectToPage("./Index");
        }
    }
}