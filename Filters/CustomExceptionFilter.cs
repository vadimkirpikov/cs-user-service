using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CsApi.Filters;

[AttributeUsage(AttributeTargets.All)]
public class CustomExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        context.Result = new BadRequestObjectResult(new { Error = exception.Message });
        context.ExceptionHandled = true;
    }
}
