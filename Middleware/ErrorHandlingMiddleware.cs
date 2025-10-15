using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Middleware; // <— must match your project namespace

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (WebApplication1.Exceptions.AppNotFoundException ex)
        {
            await WriteProblem(ctx, 404, "Resource not found", ex.Message);
        }
        catch (WebApplication1.Exceptions.InvalidForeignKeyException ex)
        {
            await WriteProblem(ctx, 400, "Invalid relationship", ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblem(ctx, 500, "Server error", ex.Message);
        }
    }

    private static async Task WriteProblem(HttpContext ctx, int status, string title, string? detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = status;
        await ctx.Response.WriteAsJsonAsync(problem);
    }
}