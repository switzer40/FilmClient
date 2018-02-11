using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
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
            var res = await _service.GetByKeyAsync(key);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var p = (KeyedPersonDto)res.ResultValue.Single();
                PersonToDelete = new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
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
           
            var res = await _service.DeleteAsync(key);
            var s = res.Status;
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