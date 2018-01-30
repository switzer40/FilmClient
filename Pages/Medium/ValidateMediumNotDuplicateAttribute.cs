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

namespace FilmClient.Pages.Medium
{
    public class ValidateMediumNotDuplicateAttribute : TypeFilterAttribute
    {
        public ValidateMediumNotDuplicateAttribute() : base(typeof(ValidateMediumNotDuplicateFilterImpl))
        {
        }
        private class ValidateMediumNotDuplicateFilterImpl : IAsyncActionFilter
        {
            private readonly IMediumService _service;
            private readonly IKeyService _keyService;
            public ValidateMediumNotDuplicateFilterImpl(IMediumService service)
            {
                _service = service;
                _keyService = new KeyService();
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("dto"))
                {
                    var dto = (MediumDto)context.ActionArguments["dto"];
                    var key = _keyService.ConstructMediumKey(dto.Title, dto.Year, dto.MediumType);
                    var s = await _service.GetByKeyAsync(key);
                    if (s == OperationStatus.OK)
                    {
                        context.Result = new BadRequestObjectResult($"A Medium with key {key} already exists");
                        return;
                    }
                }
                await next();
            }
        }
    }
}
