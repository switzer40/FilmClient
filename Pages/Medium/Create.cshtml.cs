using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Default;
using FilmClient.Pages.Error;
using FilmClient.Pages.Film;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Medium
{
    public class CreateModel : BasePageModel
    {
        private readonly IMediumService _service;
        private readonly IFilmService _filmService;
        private readonly IDefaultService _defaultervice;

        public CreateModel(IMediumService service,
                           IFilmService fservice,
                           IDefaultService dservice,
                           IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _filmService = fservice;
            _defaultervice = dservice;
            _mainProperty = nameof(MediumToAdd);
            InitializeCreateReasons();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var res = await _filmService.GetLastEntryAsync();
            KeyedFilmDto f = (res.Status == OperationStatus.OK) ? (KeyedFilmDto)res.Value : default;
            var d = _defaultervice.GetCurrentDefaultValues();
            if (f != null)
            {
                MediumToAdd = new MediumDto(f.Title, f.Year, d.MediumType, d.Location, true);
            }
            else
            {
                MediumToAdd = new MediumDto
                {
                    MediumType = d.MediumType,
                    Location = d.Location,
                    GermanSubtitles = true
                };
            }
            
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
            var res = await _service.AddAsync(MediumToAdd);
            var s = res.Status;
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