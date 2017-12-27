using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmClient.Pages.Shared;
using FilmClient.Pages.FilmPerson;

namespace FilmClient.Pages.Person
{
    public class DeleteModel : BasePageModel
    {
        private readonly IPersonService _service;
        private readonly IFilmPersonService _filmPersonService;
        public DeleteModel(IPersonService service,IFilmPersonService fservice,  IErrorService eservice) : base(eservice)
        {
            _service = service;
            _filmPersonService = fservice;
        }
        [BindProperty]
        public PersonDto PersonToDelete { get; set; }
        [BindProperty]
        public int RelationCount { get; set; }
        [BindProperty]
        public List<FilmPersonDto> AffectedRelations { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new BadRequestObjectResult("null key not allowed");
            }
            var s = await _service.GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                PersonToDelete = _service.GetByKeyResult(key);
                RelationCount = await _filmPersonService.RelationCountForPersonAsync(PersonToDelete.LastName, PersonToDelete.BirthdateString);
                AffectedRelations = await _filmPersonService.RelationsForPersonAsync(PersonToDelete.LastName, PersonToDelete.BirthdateString);
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync(string key)
        {
            var s = await _service.DeleteAsync(key);
            if (s == OperationStatus.OK)
            {
                return RedirectToPage("./index");
            }
            else
            {
                return HandleError(s, _action);
            }
        }
    }
}