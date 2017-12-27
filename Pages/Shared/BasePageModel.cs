using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        private readonly IErrorService _errorService;
        protected Dictionary<string, List<string>> Reasons;
        protected string _action;
        protected string _mainProperty;
        public BasePageModel(IErrorService eservice)
        {
            _errorService = eservice;
            Reasons = new Dictionary<string, List<string>>
            {
                {"Create", new List<string>() },
                {"Edit", new List<string>() },
                {"Delete", new List<string>() },
                {"Info", new List<string>() }
            };
        }
        protected void InitializeCreateReasons()
        {
            if (Reasons[_action] == null)
            {
                Reasons[_action] = new List<string>();
            }
            Reasons[_action].Add("Everything great!");
            Reasons[_action].Add($"{_mainProperty} was malformed");
            Reasons[_action].Add("Ooops! That should not happen");
            Reasons[_action].Add("The server blew it!");
        }
        protected void InitializeEditReasons()
        {
            if (Reasons[_action] == null)
            {
                Reasons[_action] = new List<string>();
            }
            Reasons[_action].Add("Everything great!");
            Reasons[_action].Add($"{_mainProperty} was malformed");
            Reasons[_action].Add("Are you trying to edit a nonexistent entity?");
            Reasons[_action].Add("The server blew it!");
        }
        protected void InitializeDeleteReasons()
        {
            if (Reasons[_action] == null)
            {
                Reasons[_action] = new List<string>();
            }
            Reasons[_action].Add("Everything great!");
            Reasons[_action].Add($"{_mainProperty} was malformed");
            Reasons[_action].Add("Are you trying to edelete a nonexistent entity?");
            Reasons[_action].Add("The server blew it!");
        }
        protected IActionResult HandleError(OperationStatus status, string action)
        {
            _errorService.ErrorStatus = status;
            status.ReasonForFailure = Reasons[action][status.Value];
            return RedirectToPage("/Error");
        }


    }
}
