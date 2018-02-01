using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Medium
{
    public class EditModel : BasePageModel
    {
        private readonly IMediumService _service;
        
        public EditModel(IMediumService service, IErrorService eservice) :base(eservice)
        {
            _service = service;
            _action = "Edit";
            _mainProperty = nameof(MediumToEdit);
            InitializeEditReasons();            
        }
        public MediumDto MediumToEdit { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var res = await _service.GetByKeyAsync(key);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var m = (KeyedMediumDto)res.ResultValue.Single();
                MediumToEdit = new MediumDto(m.Title, m.Year, m.MediumType, m.Location);
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var res = await _service.UpdateAsync(MediumToEdit);
            var s = res.Status;
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