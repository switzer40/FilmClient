﻿using System;
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
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Film
{
    public class InfoModel : BasePageModel
    {
        private readonly IPersonService _personService;
        private readonly IPersonMapper _personMapper;
        private readonly IFilmPersonService _filmPersonService;        
        public InfoModel(IFilmService service,
                         IPersonService pservice,
                         IPersonMapper pMapper,
                         IFilmPersonService fpservice,
                         IErrorService eservice) : base(eservice)
        {           
            _personService = pservice;
            _personMapper = pMapper;
            _filmPersonService = fpservice;
            _keyService = new KeyService();
        }
        public string Title { get; set; }
        public short Year { get; set; }
        public List<(string Name, string Birthdate)> Actors { get; set; }
        public List<(string Name, string Birthdate)> Composers { get; set; }
        public List<(string Name, string Birthdate)> Directors { get; set; }
        public List<(string Name, string Birthdate)> Scriptwriters { get; set; }        

        public async Task<IActionResult> OnGetAsync(string key)
        {
            var (title, year) = _keyService.DeconstructFilmKey(key);
            Title = title;
            Year = year;
            Actors = await GetContributorsAsync(title, year, FilmConstants.Role_Actor);
            Composers = await GetContributorsAsync(title, year, FilmConstants.Role_Composer);
            Directors = await GetContributorsAsync(title, year, FilmConstants.Role_Director);
            Scriptwriters = await GetContributorsAsync(title, year, FilmConstants.Role_Writer);
            return Page();
        }

        private async Task<List<(string Name, string Birthdate)>> GetContributorsAsync(string title, short year, string role)
        {
            List<(string Name, string Birthdate)> result = new List<(string Name, string Birthdate)>();
            var res = await _filmPersonService.GetByTitleYearAndRoleAsync(title, year, role);
            var filmPeople = res.Value;
            foreach (var item in filmPeople)
            {
                var fp = (KeyedFilmPersonDto)item;
                var res1 = await _personService.GetByLastNameAndBirthdateAsync(fp.LastName, fp.Birthdate);
                var p = res1.Value;
                var name = p.FullName;
                var birthdate = fp.Birthdate;
                result.Add((name, birthdate));
            }
            return result;
        }
    }
}