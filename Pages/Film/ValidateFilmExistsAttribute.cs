using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class ValidateFilmExistsAttribute : TypeFilterAttribute
    {
        public ValidateFilmExistsAttribute() : base(typeof(ValidateFilmExistsFilterImpl))
        {
        }
        private class ValidateFilmExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IFilmService _service;
            private readonly IKeyService _keyService;
            public ValidateFilmExistsFilterImpl(IFilmService service)
            {
                _service = service;
                _keyService = new KeyService();
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("key"))
                {
                    var key = (string)context.ActionArguments["key"];
                    var s = await _service.GetByKeyAsync(key);
                    if (s != OperationStatus.OK)
                    {
                        var data = _keyService.DeconstructFilmKey(key);
                        context.Result = new NotFoundObjectResult($"There is no film {data.title}({data.year})");
                        return;
                    }
                }
                await next();
            }
        }
    }
}
