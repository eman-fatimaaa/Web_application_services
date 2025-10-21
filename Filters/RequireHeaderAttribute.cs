using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequireHeaderAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _header;
    public RequireHeaderAttribute(string header) => _header = header;

    public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
    {
        if (!ctx.HttpContext.Request.Headers.TryGetValue(_header, out var v) || string.IsNullOrWhiteSpace(v))
        {
            ctx.Result = new BadRequestObjectResult(new ProblemDetails {
                Title = "Bad Request",
                Detail = $"Missing required header: {_header}",
                Status = 400
            });
            return;
        }
        await next();
    }
}