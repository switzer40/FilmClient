using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;

namespace FilmClient.Pages.Person
{
    public class EditModel : BasePageModel
    {
        private readonly IPersonService _service;                
        public EditModel(IPersonService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            
            _keyService = new KeyService();
        }
        [BindProperty]
        public PersonDto PersonToEdit { get; set; }


        public async Task<IActionResult> OnGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new BadRequestObjectResult("key may not be null");
            }
            var s = await _service.GetByKeyAsync(key);
            
            if (s == OperationStatus.OK)
            {
                PersonToEdit = _service.GetByKeyResult(key);
                return Page();
            }
            else
            {
                return HandleError(s, _action);
            }            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var s = await _service.UpdateAsync(PersonToEdit);
            if (s == OperationStatus.OK)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return HandleError(s, _action);
            }
        }
    }
}