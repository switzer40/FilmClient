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
using FilmAPI.Common.DTOs;

namespace FilmClient.Pages.Medium
{
    public class DeleteModel : BasePageModel
    {
        private readonly IMediumService _service;        
        public DeleteModel(IMediumService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Delete";
            _mainProperty = nameof(MediumToDelete);
            InitializeDeleteReasons();
            _keyService = new KeyService();
        }
        [BindProperty]
        public MediumDto MediumToDelete { get; set; }
        public async Task<IActionResult> OnGetAsync(string key)
        {
            var lookupKey = (string.IsNullOrEmpty(key)) ? MediumToDelete.Key : key;
            var res = await _service.GetByKeyAsync(lookupKey);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var m = (KeyedMediumDto)res.ResultValue.Single();
                MediumToDelete.Title = m.Title;
                MediumToDelete.Year = m.Year;
                MediumToDelete.MediumType = m.MediumType;
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
            if (s == OperationStatus.OK)
            {
                return RedirectToPage("./index");
            }
            return RedirectToPage("/Error");
        }
    }
}