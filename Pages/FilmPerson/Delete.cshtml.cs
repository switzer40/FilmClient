using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.FilmPerson
{
    public class DeleteModel : BasePageModel
    {
        private readonly IFilmPersonService _service;        
        public DeleteModel(IFilmPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
        }
        [BindProperty]
        public FilmPersonDto FilmPersonToDelete { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }
            var res = await _service.GetByKeyAsync(key);
            if (res != null)
            {
                var fp = (KeyedFilmPersonDto)res;
                FilmPersonToDelete = new FilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
                return Page();
            }
            else
            {
                return HandleError(OperationStatus.NotFound, "Get");
            }
        }
        public async Task<IActionResult> OnPostAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            await _service.DeleteAsync(key);
            

            return RedirectToPage("../Error/Index");
        }
    }
}