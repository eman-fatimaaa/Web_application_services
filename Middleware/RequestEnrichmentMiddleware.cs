// Middleware/RequestEnrichmentMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Middleware;

public class RequestEnrichmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestEnrichmentMiddleware> _logger;

    public RequestEnrichmentMiddleware(RequestDelegate next, ILogger<RequestEnrichmentMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Resolve the scoped service from the request scope (this is safe)
        var enricher = context.RequestServices.GetRequiredService<IRequestEnricher>();

        await enricher.EnrichAsync(context);

        // Surface values on the response so you can see them
        context.Response.OnStarting(() =>
        {
            if (context.Items.TryGetValue("UA", out var v) && v is string s && !string.IsNullOrWhiteSpace(s))
                context.Response.Headers["X-UA"] = s;

            if (context.Items.TryGetValue("X-Correlation-Id", out var cid) && cid is string c && !string.IsNullOrWhiteSpace(c))
                context.Response.Headers["X-Correlation-Id"] = c;

            return Task.CompletedTask;
        });

        await _next(context);
    }
}