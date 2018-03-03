﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Person;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.FilmPerson
{
    public class IndexModel : BasePageModel
    {
        private readonly IFilmPersonService _service;
        private readonly IPersonService _personService;
        
        
        public IndexModel(IFilmPersonService service, IPersonService pservice, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _personService = pservice;
            _keyService = new KeyService();
            FilmPeople = new List<FilmPersonModel>();
            _totalRows = 0;
            
        }
        [BindProperty]
        public List<FilmPersonModel> FilmPeople { get; set; }


        public async Task OnGetAsync()
        {
            await InitDataAsync();
            var res = await _service.GetAllAsync(_pageNumber, PageSize);
            List<IKeyedDto> list = (res.Status == OperationStatus.OK) ? res.Value : default;
            List<KeyedFilmPersonDto> filmPeople = new List<KeyedFilmPersonDto>();
            foreach (var item in list)
            {
                var val = (KeyedFilmPersonDto)item;
                filmPeople.Add(val);
            }
            foreach (var fp in filmPeople)
            {
                PersonDto p = await GetPersonAsync(fp.LastName, fp.Birthdate);
                p.FullName = $"{p.FirstMidName} {p.LastName}";                
                var model = new FilmPersonModel(fp.Title, fp.Year, p.LastName, p.FirstMidName, fp.Birthdate, fp.Role);                
                model.Key = _keyService.ConstructFilmPersonKey(fp.Title,
                                                               fp.Year,
                                                               fp.LastName,
                                                               fp.Birthdate,
                                                               fp.Role);
                FilmPeople.Add(model);
            }
        }
        private async Task InitDataAsync()
        {
            if (_totalRows > 0)
            {
                return; //initialize only once
            }
            var res = await _service.CountAsync();
            _totalRows = (res.Status == OperationStatus.OK) ? res.Value : 0;
            CalculateRowData(_totalRows);
        }
        
        private async Task<PersonDto> GetPersonAsync(string lastName, string birthdate)
        {
            PersonDto result = null;
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var res = await _personService.GetByKeyAsync(key);
            var s = res.Status;

            if (s == OperationStatus.OK)
            {
                var p = (KeyedPersonDto) res.Value;
                result = new PersonDto(p.LastName,p.Birthdate, p.FirstMidName);
            }
            return result;
        }
    }
}