using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;

namespace FilmClient.Pages.Person
{
    public class IndexModel : PageModel
    {
        private readonly IPersonService _service;
        private readonly IKeyService _keyService;
        public IndexModel(IPersonService service)
        {
            _service = service;
            _keyService = new KeyService();
            People = new List<PersonDto>();
        }
        public List<PersonDto> People { get; set; }
        public async Task OnGetAsync()
        {
            var people = await _service.GetAllAsync();
            foreach (var p in people)
            {
                p.ShortBirthdate = DateTime.Parse(p.BirthdateString).ToShortDateString();
                p.Key = _keyService.ConstructPersonKey(p.LastName, p.BirthdateString);
                People.Add(p);
            }
        }
    }
}