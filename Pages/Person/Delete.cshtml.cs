using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Person
{
    public class DeleteModel : BasePageModel
    {
        private readonly IPersonService _service;
        private readonly IFilmPersonService _filmPersonService;        
        public DeleteModel(IPersonService service, IFilmPersonService fservice, IErrorService eservice) :base(eservice)
        {
            _service = service;
            _filmPersonService = fservice;
            _keyService = new KeyService();
            _action = "Delete";
            _mainProperty = nameof(PersonToDelete);
            InitializeDeleteReasons();
        }
        public PersonDto PersonToDelete { get; set; }
        
        public bool DoDeleteRelations { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var s = await _service.GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                PersonToDelete = _service.GetByKeyResult(key);
                var data = _keyService.DeconstructPersonKey(key);                
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
            if (s== OperationStatus.OK)
            {
                return RedirectToPage("../Index");
            }
            else
            {
                return HandleError(s, _action);
            }
        }        
    }
}