using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Constants;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Person
{
    public class InfoModel : BasePageModel
    {
        private readonly IPersonService _personService;
        private readonly IFilmPersonService _filmPersonService;       
        public InfoModel(IPersonService pservice,
                         IFilmPersonService fpservice,
                         IErrorService eservice) : base(eservice)
        {
            pservice.SetController("Person");
            _personService = pservice;
            _filmPersonService = fpservice;
            _keyService = new KeyService();
        }
        [BindProperty]
        public string ContributorName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Birthdate { get; set; }
        [BindProperty]
        public List<(string Title, short Year)> AsActor { get; set; }
        [BindProperty]
        public List<(string Title, short Year)> AsComposer { get; set; }
        [BindProperty]
        public List<(string Title, short Year)> AsDirector { get; set; }
        [BindProperty]
        public List<(string Title, short Year)> AsScreenWriter { get; set; }

        public async Task<IActionResult> OnGetAsync(string key)
        {
            PersonDto p = await GetPersonAsync(key);
            ContributorName = p.FullName;
            var (lastName, birthdate) = _keyService.DeconstructPersonKey(key);
            LastName = lastName;
            Birthdate = birthdate;
            AsActor = await ExtractContributionsAsync(FilmConstants.Role_Actor);
            AsComposer = await ExtractContributionsAsync(FilmConstants.Role_Composer);
            AsDirector = await ExtractContributionsAsync(FilmConstants.Role_Director);
            AsScreenWriter = await ExtractContributionsAsync(FilmConstants.Role_Writer);
            return Page();
        }

        private async Task<PersonDto> GetPersonAsync(string key)
        {
            var res = await _personService.GetByKeyAsync(key);
            var s = res.Status;
            if (s == OperationStatus.OK)
            {
                var p = (KeyedPersonDto)res.Value;
                return new PersonDto(p.LastName, p.Birthdate, p.FirstMidName);
            }
            else
            {
                return null;
            }
        }

        private async Task<List<(string Title, short Year)>> ExtractContributionsAsync(string role)
        {
            List<(string Title, short Year)> result = new List<(string Title, short Year)>();
            var res = await _filmPersonService.GetByLastNameBirthdateAndRoleAsync(LastName, Birthdate, role);            
            foreach (var item in res.Value)
            {
                var fp = (KeyedFilmPersonDto)item;
                result.Add((fp.Title, fp.Year));
            }            
            return result;
        }
    }
}