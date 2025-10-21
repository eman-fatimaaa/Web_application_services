namespace WebApplication1.Services;

public class RequestEnricher : IRequestEnricher
{
    private readonly ILogger<RequestEnricher> _logger;

    public RequestEnricher(ILogger<RequestEnricher> logger)
    {
        _logger = logger;
    }

    public async Task EnrichAsync(HttpContext context)
    {
        // Example: simulate async I/O (db/cache) and add data to the request
        await Task.Delay(5); // replace with real async work (e.g., db lookup)

        var userAgent = context.Request.Headers.UserAgent.ToString();
        context.Items["UA"] = string.IsNullOrWhiteSpace(userAgent) ? "unknown" : userAgent;

        // Example: read (safe) correlation id set by previous middleware
        var corr = context.Items.TryGetValue("X-Correlation-Id", out var id) ? id?.ToString() : null;

        _logger.LogInformation("Request enriched (Path: {Path}, UA: {UA}, CorrelationId: {CorrelationId})",
            context.Request.Path, context.Items["UA"], corr);
    }
}