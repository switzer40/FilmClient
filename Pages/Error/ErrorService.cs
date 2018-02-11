using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public class ErrorService : IErrorService
    {
        public ErrorService()
        {
        }
        public ErrorService(OperationStatus status)
        {
            _errorStatus = status;
        }
        private OperationStatus _errorStatus;
        public OperationStatus ErrorStatus { get => _errorStatus; set => _errorStatus = value; }
    }
}
