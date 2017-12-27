using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Medium
{
    public class EditModel : BasePageModel
    {
        private readonly IMediumService _service;
        private readonly IKeyService _keyService;
        public EditModel(IMediumService service, IErrorService eservice) :base(eservice)
        {
            _service = service;
            _action = "Edit";
            _mainProperty = nameof(MediumToEdit);
            InitializeEditReasons();
            _keyService = new KeyService();
        }
        public MediumDto MediumToEdit { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var s = await _service.GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                MediumToEdit = _service.GetByKeyResult(key);
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {            
            var s = await _service.UpdateAsync(MediumToEdit);
            if (s == OperationStatus.OK)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return HandleError(s, _action);
            }
        }
    }
}