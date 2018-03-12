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
            KeyedMediumDto m = default;
            try
            {
                m = (KeyedMediumDto)await _service.GetLastEntryAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            var d = _defaultervice.GetCurrentDefaultValues();
            if (m != null)
            { 
                MediumToAdd = new MediumDto(m.Title, m.Year, m.MediumType, m.Location, m.HasGermanSubtitles);
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
            var m =(KeyedMediumDto)await _service.AddAsync(MediumToAdd);
            if (m != null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                var s = OperationStatus.NotFound;
                return HandleError(s, _action);
            }
        }
    }
}