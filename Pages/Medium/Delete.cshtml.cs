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
using FilmClient.Pages.Error;

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
                var m = (KeyedMediumDto)res.Value;
                MediumToDelete = new MediumDto(m.Title, m.Year, m.MediumType, m.Location, m.HasGermanSubtitles);
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
            return RedirectToPage("../Error/Index");
        }
    }
}