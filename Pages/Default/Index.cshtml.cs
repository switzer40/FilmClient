using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Default
{
    public class IndexModel : PageModel
    {
        private readonly IDefaultService _service;
        public IndexModel(IDefaultService service)
        {
            _service = service;
        }
        [BindProperty]
        public DefaultDto CurrentDefaultValues { get; set; }
        public void OnGet()
        {
            CurrentDefaultValues = _service.GetCurrentDefaultValues();
        }
    }
}