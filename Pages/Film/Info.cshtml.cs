using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Constants;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Film
{
    public class InfoModel : BasePageModel
    {
        private readonly IPersonService _personService;
        private readonly IFilmPersonService _filmPersonService;        
        public InfoModel(IPersonService pservice,
                         IFilmPersonService fpservice,
                         IErrorService eservice) : base(eservice)
        {
            _personService = pservice;
            _filmPersonService = fpservice;
            _keyService = new KeyService();
        }
        public string Title { get; set; }
        public short Year { get; set; }
        public List<(string Name, string Birthdate)> Actors { get; set; }
        public List<(string Name, string Birthdate)> Composers { get; set; }
        public List<(string Name, string Birthdate)> Directors { get; set; }

        public List<(string Name, string Birthdate)> Scriptwriters { get; set; }
        private PersonDto _contributor;

        public async Task<IActionResult> OnGetAsync(string key)
        {
            var data = _keyService.DeconstructFilmKey(key);
            Title = data.title;
            Year = data.year;
            Actors = await GetContributorsAsync(key, FilmConstants.Role_Actor);
            Composers = await GetContributorsAsync(key, FilmConstants.Role_Composer);
            Directors = await GetContributorsAsync(key, FilmConstants.Role_Director);
            Scriptwriters = await GetContributorsAsync(key, FilmConstants.Role_Writer);
            return Page();
        }

        private async Task<List<(string Name, string Birthdate)>> GetContributorsAsync(string key, string role)
        {
            List<(string Name, string Birthdate)> result = new List<(string Name, string Birthdate)>();
            var filmPeople = await _filmPersonService.GetAllAsync();
            foreach (var fp in filmPeople)
            {
                if (fp.Title == Title && fp.Year == Year && fp.Role == role)
                {
                    var s = await GetPersonAsync(fp.LastName, fp.Birthdate);
                    if (s == OperationStatus.OK)
                    {
                        result.Add((_contributor.FullName, _contributor.BirthdateString));
                    }                   
                }
            }
            return result;
        }

        private async Task<OperationStatus> GetPersonAsync(string lastName, string birthdate)
        {
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await _personService.GetByKeyAsync(key);
            if (res.Status == OperationStatus.OK)
            {
                _contributor = (PersonDto)res.ResultValue.Single();
            }
            else
            {
                _contributor = null;
            }
            return res.Status;
        }
    }
}