using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;
using FilmAPI.Common.DTOs;
using FilmClient.Pages.Error;

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
            var res = await _service.GetLastEntryAsync();
            if (res != null)
            {
                var dto  =(KeyedFilmPersonDto)res;
                FilmPersonToAdd = new FilmPersonDto(dto.Title, dto.Year, dto.LastName, dto.Birthdate, dto.Role);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var fp = (KeyedFilmPersonDto)await _service.AddAsync(FilmPersonToAdd);
            if (fp != null)
            {
                FilmPersonToAdd = new FilmPersonDto(fp.Title, fp.Year, fp.LastName, fp.Birthdate, fp.Role);
            }
            else
            {
                return HandleError(NotFoundStatus, _action);
            }
            return RedirectToPage("./Index");
        }
    }
}