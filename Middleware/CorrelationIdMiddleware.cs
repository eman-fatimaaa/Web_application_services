using System.Diagnostics;

namespace WebApplication1.Middleware;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Get existing id from headers or create a new one
        if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString("N");
        }

        // Store it for downstream components
        context.Items[HeaderName] = correlationId.ToString();
        context.Response.Headers[HeaderName] = correlationId.ToString();

        using (_logger.BeginScope(new Dictionary<string, object> { [HeaderName] = correlationId.ToString() }))
        {
            await _next(context);
        }
    }
}