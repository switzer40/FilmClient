using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmClient.Pages.Error;
using FilmClient.Pages.Film;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Medium;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IFilmService fservice,
                          IPersonService pservice,
                          IMediumService mservice,
                          IFilmPersonService fpservice,
                          IErrorService eservice) : base(eservice)
        {
            _filmService = fservice;
            _personService = pservice;
            _mediumService = mservice;
            _filmPersonService = fpservice;
        }
        private readonly IFilmService _filmService;
        private readonly IPersonService _personService;
        private readonly IMediumService _mediumService;
        private readonly IFilmPersonService _filmPersonService;
        public int FilmCount { get; set; }
        public int PersonCount { get; set; }
        public int MediumCount { get; set; }
        public int RelationCount { get; set; }
        public async Task OnGetAsync()
        {
            FilmCount = (await _filmService.CountAsync());
            PersonCount = (await _personService.CountAsync());
            MediumCount = (await _mediumService.CountAsync());
            RelationCount = (await _filmPersonService.CountAsync());
        }
    }
}
