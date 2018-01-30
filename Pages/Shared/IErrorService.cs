using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IErrorService
    {
        OperationStatus ErrorStatus { get; set; }
    }
}
