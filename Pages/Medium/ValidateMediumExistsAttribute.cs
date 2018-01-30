using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmClient.Pages.Shared;
using FilmAPI.Common.Utilities;

namespace FilmClient.Pages.Medium
{
    public class ValidateMediumExistsAttribute : TypeFilterAttribute
    {
        public ValidateMediumExistsAttribute() : base(typeof(ValidateMediumExistsFilterImpl))
        {
        }
        private class ValidateMediumExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IMediumService _service;
            private readonly IKeyService _keyService;
            public ValidateMediumExistsFilterImpl(IMediumService service)
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
                    if (s == OperationStatus.BadRequest)
                    {
                        context.Result = new BadRequestObjectResult($"The key {key} is malformed");
                        return;
                    }
                    else
                    if (s == OperationStatus.NotFound)
                    {
                        context.Result = new NotFoundObjectResult($"There is no medium with key {key}");
                        return;
                    }
                }
                await next();
            }
        }
    }
}
