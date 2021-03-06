﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Constants;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Person
{
    public class InfoModel : PageModel
    {
        private readonly IPersonService _personService;
        private readonly IFilmPersonService _filmPersonService;
        private readonly IKeyService _keyService;
        public InfoModel(IPersonService pservice,
                         IFilmPersonService fpservice)
        {
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
            var data = _keyService.DeconstructPersonKey(key);
            LastName = data.lastName;
            Birthdate = data.birthdate;
            AsActor = await ExtractContributionsAsync(FilmConstants.Role_Actor);
            AsComposer = await ExtractContributionsAsync(FilmConstants.Role_Composer);
            AsDirector = await ExtractContributionsAsync(FilmConstants.Role_Director);
            AsScreenWriter = await ExtractContributionsAsync(FilmConstants.Role_Writer);
            return Page();
        }

        private async Task<PersonDto> GetPersonAsync(string key)
        {
            var s = await _personService.GetByKeyAsync(key);
            if (s == OperationStatus.OK)
            {
                return _personService.GetByKeyResult(key);
            }
            else
            {
                return null;
            }
        }

        private async Task<List<(string Title, short Year)>> ExtractContributionsAsync(string role)
        {
            List<(string Title, short Year)> result = new List<(string Title, short Year)>();
            var filmPeople = await _filmPersonService.GetAllAsync();
            foreach (var fp in filmPeople)
            {
                if (fp.LastName == LastName && fp.Birthdate == Birthdate && fp.Role == role)
                {
                    result.Add((fp.Title, fp.Year));
                }
            }
            return result;
        }
    }
}