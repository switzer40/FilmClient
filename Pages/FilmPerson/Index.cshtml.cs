using System;
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

namespace FilmClient.Pages.FilmPerson
{
    public class IndexModel : PageModel
    {
        private readonly IFilmPersonService _service;
        private readonly IPersonService _personService;
        private readonly IKeyService _keyService;
        public IndexModel(IFilmPersonService service, IPersonService pservice)
        {
            _service = service;
            _personService = pservice;
            _keyService = new KeyService();
            FilmPeople = new List<FilmPersonModel>();
        }
        [BindProperty]
        public List<FilmPersonModel> FilmPeople { get; set; }


        public async Task OnGetAsync()
        {
            var filmPeople = await _service.GetAllAsync();
            foreach (var fp in filmPeople)
            {
                PersonDto p = await GetPersonAsync(fp.LastName, fp.Birthdate);
                p.FullName = $"{p.FirstMidName} {p.LastName}";
                var model = new FilmPersonModel();
                model.Title = fp.Title;
                model.Year = fp.Year;
                model.Contributor = p.FullName;
                model.ContributorBirthdate = fp.Birthdate;
                model.Role = fp.Role;
                model.Key = _keyService.ConstructFilmPersonKey(fp.Title,
                                                               fp.Year,
                                                               fp.LastName,
                                                               fp.Birthdate,
                                                               fp.Role);
                FilmPeople.Add(model);
            }
        }

        private async Task<PersonDto> GetPersonAsync(string lastName, string birthdate)
        {
            var key = _keyService.ConstructPersonKey(lastName, birthdate);
            var s = await _personService.GetByKeyAsync(key);
            return (s == OperationStatus.OK) ? _personService.GetByKeyResult(key) : null;
        }
    }
}