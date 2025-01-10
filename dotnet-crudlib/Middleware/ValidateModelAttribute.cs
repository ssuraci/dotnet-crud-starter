using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebapiTemplate.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace NetCrudStarter.Filter;

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(getErrorDictionary(modelState))
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }

    static Dictionary<string, List<string>> getErrorDictionary(ModelStateDictionary modelState)
    {
        Dictionary<string, List<string>> tmpres =
            modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToList());
        Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
        foreach (string key in tmpres.Keys)
        {
            if (tmpres[key] != null && tmpres[key].Count > 0)
            {
                res.Add(key, tmpres[key]);
            }
        }

        return res;
    }
}

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new ValidationFailedResult(context.ModelState);
        }
    }

    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        return base.OnActionExecutionAsync(context, next);
    }
}