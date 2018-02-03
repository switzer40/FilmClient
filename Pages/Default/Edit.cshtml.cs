using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Default
{
    public class EditModel : PageModel
    {
        private readonly IDefaultService _service;
        public EditModel(IDefaultService service)
        {
            _service = service;
        }
        [BindProperty]
        public DefaultDto DefaultValuesToEdit { get; set; }
        public void OnGet()
        {
            DefaultValuesToEdit = _service.GetCurrentDefaultValues();
        }
        public void OnPost(string key)
        {
            var key1 = DefaultDto.ConstructKey(DefaultValuesToEdit);
            _service.UpdateDefaultValues(key1);
        }
    }
}