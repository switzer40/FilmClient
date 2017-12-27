using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmClient.Pages.Shared;
using FilmClient.Pages.Medium;
using FilmClient.Pages.FilmPerson;

namespace FilmClient.Pages.Film
{
    public class DeleteModel : BasePageModel
    {
        private readonly IFilmService _service;
        private readonly IMediumService _mediumService;
        private readonly IFilmPersonService _filmPersonService;

        public DeleteModel(IFilmService service, IMediumService mservice, IFilmPersonService fservice, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _mediumService = mservice;
            _filmPersonService = fservice;
            _action = "Delete";
            _mainProperty = nameof(FilmToDelete);
            InitializeDeleteReasons();
        }

        [BindProperty]
        public FilmDto FilmToDelete { get; set; }
        [BindProperty]
        public bool HasMedia { get; set; }
        [BindProperty]
        public List<MediumDto> AffectedMedia { get; set; }
        [BindProperty]
        public int RelationCount { get; set; }
        [BindProperty]
        public List<FilmPersonDto> AffectedRelations { get; set; }

        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            var s = await _service.GetByKeyAsync(key);

            if (s == OperationStatus.OK)
            {
                FilmToDelete = _service.GetByKeyResult(key);
                HasMedia = await _mediumService.HasMediumForFilmAsync(FilmToDelete.Title, FilmToDelete.Year);
                AffectedMedia = await _mediumService.MediaForFilmAsync(FilmToDelete.Title, FilmToDelete.Year);
                RelationCount = await _filmPersonService.RelationCountForFilmAsync(FilmToDelete.Title, FilmToDelete.Year);
                AffectedRelations = await _filmPersonService.RelationsForFilmAsync(FilmToDelete.Title, FilmToDelete.Year);
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }            
        }

        public async Task<IActionResult> OnPostAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

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
