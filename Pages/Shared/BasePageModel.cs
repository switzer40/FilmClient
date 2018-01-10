using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
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
        public BasePageModel(IErrorService errorService)
        {
            ErrorService = errorService;
            _keyService = new KeyService();
        }
        protected OperationStatus ErrorStatus;
        protected Dictionary<int, string> CreateReasons;
        protected Dictionary<int, string> DeleteReasons;
        protected Dictionary<int, string> EditReasons;       
        protected string _action;
        protected string _mainProperty;
        protected IKeyService _keyService;
        public IErrorService ErrorService { get; }
        public void InitializeCreateReasons()
        {
            CreateReasons = new Dictionary<int, string>();
            CreateReasons[OperationStatus.BadRequest.Value] = "Malformed argument";
            CreateReasons[OperationStatus.NotFound.Value] = "Unknown entity";
        }
        public void InitializeDeleteReasons()
        {
            DeleteReasons = new Dictionary<int, string>();
            DeleteReasons[OperationStatus.BadRequest.Value] = "Malformed key";
            DeleteReasons[OperationStatus.NotFound.Value] = "Unknown entity";

        }
        public void InitializeEditReasons()
        {
            EditReasons = new Dictionary<int, string>();
            EditReasons[OperationStatus.BadRequest.Value] = "Malformed argument";
            EditReasons[OperationStatus.NotFound.Value] = "Unknown entity";
        }
        public IActionResult HandleError(OperationStatus status, string action)
        {
            IActionResult result = null;
            ErrorStatus = status;
            if (status.ReasonForFailure == "")
            {
                ErrorStatus.ReasonForFailure = getReason(status, _action);
            }
            switch (status.Name)
            {
                case "BadRequest":
                    result = new BadRequestResult();
                    break;
                case "NotFound":
                    result = new NotFoundResult();
                    break;
                default:
                    throw new Exception($"Unknownoperation status: {status.Name}");
            }
            return result;
        }

        private string getReason(OperationStatus status, string action)
        {
            switch (action)
            {
                case "Create":
                    return CreateReasons[status.Value];
                case "Delete":
                    return DeleteReasons[status.Value];
                case "Edit":
                    return EditReasons[status.Value];
                default:
                    throw new Exception($"Unknown action");
            }
        }
    }
}
