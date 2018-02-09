using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;

namespace FilmClient.Pages.FilmPerson
{
    public class CreateModel : BasePageModel
    {
        private readonly IFilmPersonService _service;
        public CreateModel(IFilmPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _action = "Create";
            _mainProperty = nameof(FilmPersonToAdd);
            InitializeCreateReasons();
        }
        [BindProperty]
        public FilmPersonDto FilmPersonToAdd { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            FilmPersonToAdd = await _service.GetLastEntryAsync();                                           
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var res = await _service.AddAsync(FilmPersonToAdd);
            var s = res.Status;
            if (s != OperationStatus.OK)
            {
                return HandleError(s, _action);
            }

            return RedirectToPage("./Index");
        }
    }
}