using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Error
{
    public class IndexModel : PageModel
    {
        private readonly IErrorService _service;
        public IndexModel(IErrorService eservice)
        {
            _service = eservice;
        }
        [BindProperty]
        public ErrorDto Error { get; set; }
        public void OnGet()
        {

        }
    }
}