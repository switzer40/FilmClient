using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Services;
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
