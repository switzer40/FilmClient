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
    public class ValidateFilmNotDuplicateAttribute : TypeFilterAttribute
    {
        public ValidateFilmNotDuplicateAttribute() : base(typeof(ValidateFilmNotDuplicateFilterImpl))
        {
        }
        private class ValidateFilmNotDuplicateFilterImpl : IAsyncActionFilter
        {
            private readonly IFilmService _service;
            private readonly IKeyService _keyService;
            public ValidateFilmNotDuplicateFilterImpl(IFilmService service)
            {
                _service = service;
                _keyService = new KeyService();
            }



            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("dto"))
                {
                    var dto = (FilmDto)context.ActionArguments["dto"];
                    var key = _keyService.ConstructFilmKey(dto.Title, dto.Year);
                    var res = await _service.GetByKeyAsync(key);
                    var s = res.Status;
                    if (s == OperationStatus.OK)
                    {
                        context.Result = new BadRequestObjectResult($"A film {dto.Title}({dto.Year}) exists already.");
                        return;
                    }
                }
                await next();
            }
        }
    }
}
