using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using Microsoft.AspNetCore.Http;
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
        protected const int PageSize = 10;
        protected OperationStatus ErrorStatus;
        protected Dictionary<int, string> CreateReasons;
        protected Dictionary<int, string> DeleteReasons;
        protected Dictionary<int, string> EditReasons;       
        protected string _action;
        protected string _controller;
        protected string _mainProperty;
        protected IKeyService _keyService;
        protected int _totalRows;
        protected int _numberOfPages;
        protected int _pageNumber;
        protected int _lastPage;
        public IErrorService ErrorService { get; }
        public int PageNumber => _pageNumber;
        public int NumberOfPages => _numberOfPages;
        public int LastPage => _lastPage;
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
                ErrorStatus.ReasonForFailure = GetReason(status, _action);
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

        private string GetReason(OperationStatus status, string action)
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
        protected void CalculateRowData(int totalRows)
        {
            _lastPage = (int)Math.Floor(totalRows / (double)PageSize);

        }
    }
}
