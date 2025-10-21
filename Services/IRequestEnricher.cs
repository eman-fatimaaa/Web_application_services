namespace WebApplication1.Services;

public interface IRequestEnricher
{
    Task EnrichAsync(HttpContext context);
}