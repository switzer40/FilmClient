using System.Diagnostics;
using System.Net.NetworkInformation;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages
{
    public class ErrorModel : PageModel
    {
        public ErrorModel(IErrorService eservice)
        {
            ErrorStatus = eservice.ErrorStatus;
        }
        [BindProperty]
        public OperationStatus ErrorStatus
        { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
